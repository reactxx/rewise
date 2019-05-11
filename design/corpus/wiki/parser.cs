using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml;

namespace Corpus {

  public static class Parser {

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