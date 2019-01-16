using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;

namespace fulltext {
  class Program {

    static void Main(string[] args) {
      using (var imp = new Impersonator.Impersonator("pavel", "LANGMaster", "zvahov88_")) {

        //****** basic utils
        //LangsLib.Metas.designTimeRebuild();
        //CreateFrekventWords.run();
        //HunspellLib.extractWordLists();
        //First_64k.run();
        //StemmingRaw.processLangs(WordLists.wordLists);

        var src = platformsLib.lib.CreateTypeSyntax();
        var dart = src.Item1;
        var ts = src.Item2;
        ts = null;

        //****** basic tests
        //StemmingRaw.processLang(LangsLib.Metas.get(LangsLib.langs.de_de).lc, WordLists.wordLists, true, false);
        //var metas = new LangsLib.Metas();
        //StemmerBreaker.Services.testCreation();
      }
    }
  }
}
