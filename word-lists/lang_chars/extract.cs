using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;

// "C:\Program Files\WinHTTrack\WinHTTrack.exe" capture http://download.mimer.com/pub/developer/charts/
// result to d:\rewise\word-lists\lang_chars\appdata\

namespace lang_chars {
  public static class Root {
    public static string root = AppDomain.CurrentDomain.BaseDirectory[0] + @":\rewise\word-lists\lang_chars\appdata\";
    public static string source = root + @"mimer-site\";
  }


  public static class Extract {

    public static void run() {
      var res = getNames();
    }


    static List<string> getNames() {
      var files = Directory.GetFiles(Root.source, "*.html").Select(f => Path.GetFileNameWithoutExtension(f)).Where(fn => !fn.StartsWith("UCA_") && fn != "index.html").ToArray();
      var full = CultureInfo.GetCultures(CultureTypes.AllCultures).Select(cu => string.Format("{0} - {1} - {2} - {3}", cu.EnglishName.ToLower(), cu.LCID, cu.Name, cu.NativeName)).Distinct().OrderBy(c => c).ToArray();
      var notFound = files.Where(f => {
        if (!replace.TryGetValue(f, out string fn)) fn = f;
        return full.All(n => !n.StartsWith(fn));
      }).ToArray();
      //File.WriteAllLines(@"d:\temp\files.txt", files);
      File.WriteAllLines(@"d:\temp\full.txt", full);
      File.WriteAllLines(@"d:\temp\notFound.txt", notFound);
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
OK irish_gaelic
kirghiz
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
