using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using System.Xml.Linq;
using System.Text;

// "C:\Program Files\WinHTTrack\WinHTTrack.exe" capture http://download.mimer.com/pub/developer/charts/
// result to d:\rewise\word-lists\lang_chars\appdata\

namespace lang_chars {

  public static class extractFromMimer {

    public static void run() {
      var res = getNames();
    }
    static Regex nameCodeRx = new Regex("'\\[(.*?)\\](\\[(.*?)\\])?", RegexOptions.Multiline);

    static List<string> getNames() {
      // extract CultureInfo name and scrips from .TXT's
      var txts =
        Directory.GetFiles(Root.mimerSite, "*.txt").
        Select(fn => {
          var txt = File.ReadAllText(fn);
          var m = nameCodeRx.Match(txt);
          if (m == null || !m.Success) return new { lc = null as CultureInfo, name = null as string, code = null as string, fn = Path.GetFileNameWithoutExtension(fn) };
          CultureInfo msLc;
          try {
            msLc = CultureInfo.GetCultureInfo(m.Groups[1].Value);
            if (msLc.EnglishName.StartsWith("Unknown Language")) msLc = null;
          } catch {
            msLc = null;
          }
          return new { lc = msLc, name = msLc == null ? null as string : m.Groups[1].Value + "/" + msLc.LCID + "/" + msLc.EnglishName, code = m.Groups.Count > 1 ? m.Groups[2].Value : null, fn = Path.GetFileNameWithoutExtension(fn) };
        }).
        Where(f => f.lc != null).
        ToArray();

      // parse .HTML's
      foreach (var file in txts) {
        var htmlFn = Root.mimerSite + file.fn + ".html";
        var html = "<?xml version=\"1.0\"?>\n<table " + File.ReadAllText(htmlFn).Split(new string[] { "<table ", "</table>" }, StringSplitOptions.None)[1].Replace("&nbsp;", " ").Replace("<br>", "<br/>") + "</table>";
        var xml = XElement.Parse(html);
        var res = new List<StringBuilder>();
        var sb = new StringBuilder();
        var rows = xml.Descendants("tr").ToArray();
        foreach (var nd in rows) {
          sb.Clear();
          var textsNode = nd.DescendantNodes().Where(n => n.NodeType == System.Xml.XmlNodeType.Text).OfType<XText>().Select(t => t.Value).ToArray();
          foreach (var hex in textsNode) {
            if (hex.Length != 4) continue;
            int code;
            try {
              code = int.Parse(hex, NumberStyles.HexNumber);
            } catch {
              continue;
            }
            sb.Append(Convert.ToChar(code));
          }
          var texts = sb.ToString();
          var first = nd.Nodes().First() as XElement;
          var isContinue = first.Nodes().Count() == 0;
          if (isContinue) res[res.Count - 1].Append(texts);
          else res.Add(new StringBuilder(texts));
        }

      }

      var files = Directory.GetFiles(Root.mimerSite, "*.html").Select(f => Path.GetFileNameWithoutExtension(f)).Where(fn => !fn.StartsWith("UCA_") && fn != "index.html").ToArray();
      var full = CultureInfo.GetCultures(CultureTypes.AllCultures).Select(cu => new { lc = cu, title = string.Format("{0} - {1} - {2} - {3}", cu.EnglishName.ToLower(), cu.LCID, cu.Name, cu.NativeName) }).ToArray();
      var trace = files.Select(fnMimer => {
        if (!replace.TryGetValue(fnMimer, out string fn)) fn = fnMimer;
        var okTitle = full.Where(f => f.title.StartsWith(fn)).ToArray();
        if (okTitle.Length == 0) return new { wrong = true, lc = null as CultureInfo, fn };
        var reslc = okTitle.Select(t => {
          return t.lc;
        }).SingleOrDefault();// LangsLib.Metas.Items.ContainsKey((LangsLib.langs)t.lc.LCID));
        return new { wrong = false, lc = reslc, fn };
      });
      var notFound = trace.Where(t => t.wrong).Select(t => t.fn).ToArray();
      //File.WriteAllLines(@"d:\temp\files.txt", files);
      File.WriteAllLines(Root.driver + @":\temp\full.txt", full.Select(f => f.title));
      File.WriteAllLines(Root.driver + @":\temp\notFound.txt", notFound);
      return null;
    }

    static Dictionary<string, string> replace = new Dictionary<string, string>() {
      {"bengali", "bangla" },
      { "scottish_gaelic","scottish gaelic"},
      { "kirghiz","kyrgyz"},
      { "moldavian","romanian (moldova)"},
      { "romanian","romanian (romania)"},
      { "romansch","romansh"},
      { "swahili","kiswahili"},
      { "swati","siswati"},
      { "tamazight","standard moroccan tamazight"},
      //{ "tamazight","central atlas tamazight"},
      { "tswana","setswana"},
      { "xhosa","isixhosa"},
      { "zulu","isizulu"},
    };
  }

}
/*
?? arumanian, cast rumunstiny
OK bengali
charts_help
chinese_pinyin
chinese_pinyin_name
chinese_wubihua
chinese_zhuyin
X chinese_zhuyin_name
elfdalian, 3000 lidi ve svedsku
X frisian, kus holandska
german_phonebook
greeklatin
X index
X irish_gaelic
OK kirghiz
lao_traditional
OK moldavian
X moore
myanmar
ndebele
oriya
OK romansch
X scots, kousek sktska
OK scottish_gaelic
sepedi
sorani
sorbian_lower
sorbian_upper
spanish_traditional
OK swahili
OK swati
OK tamazight, neporadek
OK tswana
vietnamese_traditional
OK xhosa
OK zulu

 */
