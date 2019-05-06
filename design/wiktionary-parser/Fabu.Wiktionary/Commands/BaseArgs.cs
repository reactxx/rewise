using CommandLine;
using System;

namespace Fabu.Wiktionary.Commands
{
    public class BaseArgs
    {
        public static string DefaultDumpDir = AppDomain.CurrentDomain.BaseDirectory[0].ToString() + @":\rewise\data\wiktionary\";
        public static string DefaultWiktFile = AppDomain.CurrentDomain.BaseDirectory[0].ToString() + @":\rewise\data\wiktionary\enwiktionary-20190501-pages-articles.xml";

        //[Option("in", Required = false, HelpText = "Wiktionary dump to be processed.")]
        public string WiktionaryDumpFile = DefaultWiktFile;
        //[Option("dumps", Required = false, Default = DefaultDumpDir, HelpText = "Output directory name")]
        public string DumpDir = DefaultDumpDir;
        [Option("limit", Required = false, HelpText = "Limit number of processed pages.", Default = -1)]
        public int LimitPages { get; internal set; }

        //[Option('r', "read", Required = true, HelpText = "Input files to be processed.")]
        //public IEnumerable<string> InputFiles { get; set; }

        //// Omitting long name, defaults to name of property, ie "--verbose"
        //[Option(Default = false, HelpText = "Prints all messages to standard output.")]
        //public bool Verbose { get; set; }

        //[Option("stdin", Default = false, HelpText = "Read from stdin")]
        //public bool stdin { get; set; }

        //[Value(0, MetaName = "offset", HelpText = "File offset.")]
        //public long? Offset { get; set; }
    }
}
