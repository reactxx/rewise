using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using System.Xml.Linq;
using System.Text;
using System.Xml.Serialization;

// "C:\Program Files\WinHTTrack\WinHTTrack.exe" capture http://download.mimer.com/pub/developer/charts/
// result to d:\rewise\word-lists\lang_chars\appdata\

namespace lang_chars {

  public class Alphabet {
    public int LCID;
    public string script;
    public string name;
    public string[] alphas;
    public char[] alphabet;
  }

  public static class extractFromMimer {

    public static Dictionary<string, Alphabet> loadAlhabets() {
      var ser = new XmlSerializer(typeof(Alphabet[]));
      var fnName = Root.unicode + "mimerAlphabets.xml";
      Alphabet[] alphas;
      using (var fs = File.OpenRead(fnName))
        alphas = ser.Deserialize(fs) as Alphabet[];
      return alphas.ToDictionary(a => a.name);
    }

    //public static void useAlphabets() {

    //  Dictionary<string, Alphabet> alphas = loadAlhabets();
    //  var wrongsLog = new List<string>();
    //  foreach (var lang in LangsLib.Metas.Items) {
    //    if (!CultureInfoTexts.charsSample.TryGetValue(lang.Value.Id, out char[] sample)) continue;
    //    var parentName = lang.Value.lc.Parent.Name;
    //    if (alphas.TryGetValue(parentName, out Alphabet alpha)) {
    //      var wrongs = sample.Except(alpha.alphabet).Distinct().ToArray();
    //      if (wrongs.Length > 0) {
    //        wrongsLog.Add(string.Format("Name={0}, EnglishName={1}, parentName={2}, {3}", lang.Value.lc.Name, lang.Value.lc.EnglishName, parentName, new string(wrongs)));
    //      }
    //    }
    //  }
    //  File.WriteAllLines(@"c:\temp\wrongsLog.txt", wrongsLog);
    //}

    public static void extractAlphabets() {
      var txts =
        Directory.GetFiles(Root.mimerSite, "*.txt").
        Select(fn => {
          var txt = File.ReadAllText(fn);
          var m = nameCodeRx.Match(txt);
          if (m == null || !m.Success) return new { lc = null as CultureInfo, name = null as string, script = null as string, fn = Path.GetFileNameWithoutExtension(fn) };
          CultureInfo msLc;
          try {
            msLc = CultureInfo.GetCultureInfo(m.Groups[1].Value);
            if (msLc.EnglishName.StartsWith("Unknown Language")) msLc = null;
          } catch {
            msLc = null;
          }
          return new { lc = msLc, name = msLc == null ? null as string : m.Groups[1].Value + "/" + msLc.LCID + "/" + msLc.EnglishName, script = m.Groups.Count > 1 ? m.Groups[2].Value : null, fn = Path.GetFileNameWithoutExtension(fn) };
        }).
        Where(f => f.lc != null).
        ToArray();

      // parse .HTML's
      var alphas = new List<Alphabet>();
      foreach (var file in txts) {
        if (
          file.fn == "german_phonebook" || // ??
          file.fn == "lao_traditional" || // ??
          file.fn == "moldavian" || // the same as romanian
          file.fn == "greeklatin" || // use greek, greeklatin is greek + latin
          file.fn == "kurdish" || // is latin, not in .net. Use soroni (persian)
          file.lc.Name == "zh"
          ) continue;
        var htmlFn = Root.mimerSite + file.fn + ".html";
        var html = "<?xml version=\"1.0\"?>\n<table " + File.ReadAllText(htmlFn).Split(new string[] { "<table ", "</table>" }, StringSplitOptions.None)[1].Replace("&nbsp;", " ").Replace("<br>", "<br/>") + "</table>";
        var xml = XElement.Parse(html);
        var res = new List<StringBuilder>();
        var sb = new StringBuilder();
        var rows = xml.Descendants("tr").ToArray();
        foreach (var nd in rows) {
          sb.Clear();
          var textsNode = nd.DescendantNodes().Where(n => n.NodeType == System.Xml.XmlNodeType.Text).OfType<XText>().SelectMany(t => t.Value.Split(' ')).ToArray();
          foreach (var hex in textsNode) {
            if (hex.Length != 4) continue;
            int code;
            try {
              code = int.Parse(hex, NumberStyles.HexNumber);
            } catch {
              continue;
            }
            var ch = Convert.ToChar(code);
            if (!LangsLib.UnicodeBlockNames.isLetter(ch))
              ch = ch;
            else
              sb.Append(Convert.ToChar(code));
          }
          var texts = sb.ToString();
          var first = nd.Nodes().First() as XElement;
          var isContinue = first.Nodes().Count() == 0;
          if (isContinue) res[res.Count - 1].Append(texts);
          else res.Add(new StringBuilder(texts));
        }

        alphas.Add(new Alphabet {
          LCID = file.lc.LCID,
          script = file.script,
          name = file.lc.Name,
          alphas = res.Select(r => r.ToString()).ToArray(),
          alphabet = res.SelectMany(r => r.ToString().ToCharArray()).Distinct().OrderBy(ch => ch).ToArray(),
        });
      }

      var wrong = alphas.GroupBy(a => a.name).Where(g => g.Count() > 1).Select(g => g.Key).ToArray();
      //var ku = txts.Where(g => g.lc.Name=="ku").ToArray();

      var ser = new XmlSerializer(typeof(Alphabet[]));
      var fnName = Root.unicode + "mimerAlphabets.xml";
      if (File.Exists(fnName)) File.Delete(fnName);
      using (var fs = File.OpenWrite(fnName))
        ser.Serialize(fs, alphas.ToArray());
    }

    static Regex nameCodeRx = new Regex("'\\[(.*?)\\](\\[(.*?)\\])?", RegexOptions.Multiline);
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
