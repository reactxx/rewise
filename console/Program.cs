using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;

namespace fulltext {
  class Program {

    static string[] wordLists = new string[] {
      @"word-lists\frekvent\appdata\words\",
      @"hunspell\hunspell\appdata\words\",
    };
    
    static void Main(string[] args) {

      //****** basic utils
      //LangsLib.Metas.designTimeRebuild();
      CreateFrekventWords.run();
      HunspellLib.extractWordLists();
      StemmingRaw.processLangs(wordLists);

      //****** basic tests
      //var metas = new LangsLib.Metas();
      //StemmerBreaker.Services.testCreation();

    }
  }
}
