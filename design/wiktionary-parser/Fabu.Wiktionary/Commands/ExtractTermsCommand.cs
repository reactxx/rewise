﻿using CommandLine;
using Fabu.Wiktionary.FuzzySearch;
using Fabu.Wiktionary.TermProcessing;
using Fabu.Wiktionary.TextConverters.Wiki;
using Fabu.Wiktionary.TextConverters.Wiki.Templates;
using Fabu.Wiktionary.Transform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Fabu.Wiktionary.Commands
{
    class WordCreator : IWordCreator
    {
        void IWordCreator.Create(List<Term> word)
        {
            this.word = word;
        }
        List<Term> word;
    }
    class ExtractTermsCommand : BaseCommand<ExtractTermsCommand.Args>
    {
        [Verb("extract", HelpText = "Extract language names")]
        public class Args : BaseArgs
        {
            [Option("term", Required = false, HelpText = "A single term to process.")]
            public string Term { get; set; }
        }

        // extract --in enwiktionary-20180120-pages-articles.xml
        protected override void RunCommand(Args args, Func<int, BaseArgs, bool> onProgress)
        {
            var str = "<TD>3</TD><TD>2</TD><TD>1</TD></TR></TABLE>}}";
            var regex = new Regex(@"\G(\n)");
            var match = regex.Match(str);
            Console.WriteLine(match.Value);
            var sections = DumpTool.LoadDump<List<SectionName>>(args.DumpDir, DumpTool.SectionsDictDump);
            var sectionsSearch = new ReverseLevenshteinSearch(sections);
            var languageNames = DumpTool.LoadDump<List<SectionName>>(args.DumpDir, DumpTool.LanguagesDump);
            var languageSearch = new IgnoreCaseSearch<SectionName>(languageNames, _ => _.Name, new SectionNameComparer());
            var lagnuageCodes = DumpTool.LoadLanguageCodes(args.DumpDir);
            var ignoredTemplates = new List<string>(); //DumpTool.LoadDump<List<string>>(args.DumpDir, DumpTool.IgnoredTemplatesDump);

            var transform = new FixTyposSectionName(languageSearch, sectionsSearch, true);

            var wiktionaryDump = DumpTool.LoadWikimediaDump(args.DumpDir, args.WiktionaryDumpFile);
            var processor = new PageGraphProcessor(transform);
            var textProcessorFactory = new WikitextConverterFactory(lagnuageCodes, ignoredTemplates, false);
            var indexWriter = new AWSCloudSearchHtmlWriter(Config, Config.GetSection("WordIndex")["DomainName"]);
            var htmlWriter = new HtmlWordCreator(textProcessorFactory, indexWriter);
            var extractor = new WiktionaryTermExtractor(processor, args.Term, htmlWriter);
            var analyzer = new WiktionaryAnalyzer(extractor, wiktionaryDump);
            var pagesProcessed = 0;
            if (onProgress != null)
                analyzer.PageProcessed += (sender, e) => { pagesProcessed = e.Index; e.Abort = onProgress(e.Index, args); };
            analyzer.Compute();

            DumpTool.SaveDump(args.DumpDir, "empty-pages.json", extractor.EmptyResults);
            DumpTool.SaveDump(args.DumpDir, "templates.json", TemplateConverter.ConvertedTemplates.OrderByDescending(kvp => kvp.Value));
            DumpTool.SaveDump(args.DumpDir, "template-examples.json", TemplateConverter.TemplatesExamples.Select(kvp => new KeyValuePair<string,IEnumerable<string>>(kvp.Key, kvp.Value.Take(5))));
            DumpTool.SaveDump(args.DumpDir, "nodes.json", BaseNodeConverter.ConvertedNodes.OrderByDescending(kvp => kvp.Value));
            DumpTool.SaveDump(args.DumpDir, "parserTags.json", ParserTagConverter.ConvertedParserTags.OrderByDescending(kvp => kvp.Value));

            Console.WriteLine();
            Console.WriteLine($"Pages processed: {pagesProcessed}");
            Console.WriteLine($"Words defined: {extractor.WordsCount}");
            Console.WriteLine($"Total terms: {extractor.TermsCount}");
        }
    }
}
