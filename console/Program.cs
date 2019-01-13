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
        CreateFrekventWords.run();
        HunspellLib.extractWordLists();
        First_64k.run();
        //StemmingRaw.processLangs(First_64k.Root.wordLists);

        //****** basic tests
        //var metas = new LangsLib.Metas();
        //StemmerBreaker.Services.testCreation();
      }
    }
  }
}
