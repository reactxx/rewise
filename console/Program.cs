using StemmerBreaker;
using System.Linq;
using System;
using System.Text;
using System.Collections.Generic;
using Sepia.Globalization;
using System.IO;

namespace fulltext {
  class Program {

    static void Main(string[] args) {
      using (var imp = new Impersonator.Impersonator("pavel", "LANGMaster", "zvahov88_")) {

        foreach (var fn in Directory.EnumerateFiles(@"c:\rewise\data\01_csv\", "*.csv", SearchOption.AllDirectories)) {
          var txt = File.ReadAllText(fn);
          File.WriteAllText(fn, txt, Encoding.UTF8);
        }



        var oldLangs = "".Split(',');
        var newLangs = oldLangs.Select(old => Langs.oldToNew(old)).JoinStrings(",");
        newLangs = null;

        //var txt = "по́щенска ма́рка";
        //var len1 = txt.Normalize(NormalizationForm.FormC).Length;
        //var len2 = txt.Normalize(NormalizationForm.FormD).Length;
        //var len3 = txt.Normalize(NormalizationForm.FormKC).Length;
        //var len4 = txt.Normalize(NormalizationForm.FormKD).Length;
        //txt = null;

        //var lang = "de-DE";
        //foreach (var txt in new string[] { "heißen"  }) {
        //  var res = StemmerBreakerNew.Service.wordBreak(lang, new List<String>() { txt });
        //  var words = res[0].Select(p => txt.Substring(p.Pos, p.Len)).ToArray();
        //  foreach (var w in words) {
        //    var res2 = StemmerBreakerNew.Service.getWordStemms(lang, w).JoinStrings(">");
        //    res2 = null;
        //  }
        //}
        //lang = null;

        //************** IMPORT FROM RJ
        //ImportFromRJ.Import();

        //****** basic utils
        //CreateFrekventWords.run();
        //HunspellLib.extractWordLists();
        //StemmingRaw.processLangs(WordLists.wordLists);

        //************** LANGS START

        //LangsDesignLib.Build(); //ALL

        //UnicodeDesignLib.getUnicodeBlockNames();
        //CldrDesignLib.RefreshCldrDataSource();
        //CldrDesignLib.RefreshNetSuportedCultures();
        //CldrDesignLib.RefreshOldToNew();
        //CldrDesignLib.RefreshTexts();
        //CldrDesignLib.Build();
        //CldrTrans.Build();
        //LangsDesignLib.MergeOldToCldr();
        //CldrDesignLib.RefreshCldrStatistics();
        //CldrDesignLib.BuildDart();
        //CldrDesignLib.UnicodeDart();
        //************** LANGS END

        //****** basic tests
        //var resp = Client.makeRequest(client => client.SayHello(new RewiseDom.HelloRequest { CsharpId = 1234, NoRecursion = true }));
        //Diff.Test();
        //CldrLangMatrix.load().save();

        //var lang = "en-GB";
        //foreach (var txt in new string[] { "flowers'", "flowers’" }) {
        //  var res = StemmerBreakerNew.Service.wordBreak(lang, new List<String>() { txt });
        //  var words = res[0].Select(p => txt.Substring(p.Pos, p.Len)).ToArray();
        //  foreach (var w in words) {
        //    var res2 = StemmerBreakerNew.Service.getWordStemms(lang, w).JoinStrings(">");
        //    res2 = null;
        //  }
        //}
        //lang = null;

        //Unicode.dumpNetUncLettersDiff();
        //var idxs = LangsLib.UnicodeBlockNames.blockIdxs("abcABC123-");

        //StemmingRaw.processLang(LangsLib.Metas.get(LangsLib.langs.de_de).lc, WordLists.wordLists, true, false);
        //var metas = new LangsLib.Metas();
        //StemmerBreaker.Services.testCreation();

        //****** trash

        //var reps = Sepia.Globalization.Cldr.Instance.Repositories; //C:\Users\pavel\AppData\Local\UnicodeCLDR\core
        //reps = null;

        //CultureInfoTexts.dumpCldrRbnfTypes();
        //lang_chars.extractFromMimer.extractAlphabets();
        //lang_chars.extractFromMimer.useAlphabets();

        //var config = new RewiseDom.Config();
        //config.WorkSpaces["localhost"] = new RewiseDom.WorkSpace { CsharpServer = new RewiseDom.Connection { Host = "localhost", Port = 1234 } };
        //var ser = Protobuf.ToJson(config);
        //ser = null;

        //Huffman_Encoding.Program.Main();
        //BitsProgram.Main();
        //var buf = MatrixToDartTask.import(@"c:\rewise\design\importFromRJ\appdata\source\all\GoetheVerlag.csv", null, null);
      }
    }
  }
}
