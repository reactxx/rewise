using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace Corpus {

  public static class DbpediaParser {

    static string[] langs = new string[] { "en", "fr", "de", "ja", "ru", "pt", "es", "nl" };

    public class MyGraph : Graph, IDisposable {
      public MyGraph(string destFn) : base() {
        wr = new StreamWriter(destFn);
      }
      int count = 0;
      StreamWriter wr;
      public override bool Assert(Triple t) {
        if (t.Predicate is UriNode && t.Object is LiteralNode) {
          var predicate = t.Predicate as UriNode;
          if (predicate.Uri.AbsoluteUri != "http://purl.org/dc/elements/1.1/source") return true;
          var obj = t.Object as LiteralNode;
          withoutMarkup(obj.Value.ToCharArray(), wr);

          //obj.Value;
          count++;
          if ((count & 0xffff) == 0) Console.WriteLine(count.ToString());
        }
        return true;
      }
      public override void Dispose() {
        base.Dispose();
        wr.Close();
      }

      void withoutMarkup(char[] body, StreamWriter wr) {
        bool inBr = false, isBlank = true;
        for (var i = 0; i < body.Length; i++) {
          var ch = body[i];
          if (ch == '<')
            inBr = true;
          else if (ch == '>') {
            inBr = false;
            wr.Write(' ');
            isBlank = true;
          } else if (!inBr) {
            var bl = ch == ' ' || ch == '\r' || ch == '\n';
            if (bl) {
              if (!isBlank) { wr.Write(' '); isBlank = true; }
            } else {
              isBlank = false;
              wr.Write(ch);
            }
          }
        }
      }

    }

    // srcFn stazeno z https://wiki.dbpedia.org/downloads-2016-10, Raw html tables
    public static void parseTTL() {
      void parseTTLLow(string lang) {
        var srcFn = $"{Dirs.wikiesDbpedia}raw_tables_{lang}.ttl";
        if (!File.Exists(srcFn)) return;
        Console.WriteLine(srcFn);
        var destFn = $"{Dirs.root}dbpedia\\{lang}.txt";
        var ttlparser = new NTriplesParser();
        using (var graph = new MyGraph(destFn))
        using (var rdr = new StreamReader(srcFn))
          ttlparser.Load(graph, rdr);
      }

      //foreach (var lang in langs) parseTTLLow(lang);
      Parallel.ForEach(langs, new ParallelOptions { MaxDegreeOfParallelism = 4 }, parseTTLLow);
    }

  }

  public static class WikiParser_deprecated {

    public static IEnumerable<string> parseXml(string fn, string lang) {

      return wordBreak(extractPageText().SelectMany(pageText => withoutMarkup(pageText)));

      IEnumerable<string> extractPageText() {
        XmlReaderSettings settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse };
        var sb = new StringBuilder();
        using (var sr = new StreamReader(fn))
        using (var rdr = XmlReader.Create(sr, settings)) {
          while (true) {
            if (!rdr.ReadToFollowing("page")) break;
            rdr.ReadToFollowing("id");
            var id = rdr.ReadElementContentAsInt();
            rdr.ReadToFollowing("text");
            var txt = rdr.ReadElementContentAsString();
            yield return txt;
          }
        }
      }

      IEnumerable<string> withoutMarkup(string body) {
        int pos = 0, brDeep = 0;
        for (var i = 0; i < body.Length; i++) {
          var ch = body[i];
          if (ch == '{') {
            if (brDeep == 0) yield return body.Substring(pos, i - pos);
            brDeep++;
          } else if (ch == '}') {
            if (brDeep == 0) throw new Exception();
            brDeep--;
            if (brDeep == 0) pos = i + 1;
          }
        }
        if (pos < body.Length) yield return body.Substring(pos, body.Length - pos);
      }

      IEnumerable<string> wordBreak(IEnumerable<string> pars) =>
        StemmerBreakerNew.Service.wordBreak(lang, pars);

    }
  }
}