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

      StemmingRaw.processLangs(wordLists);

      //CreateFrekventWords.run();
      //HunspellLib.extractWordLists();

      //LangsLib.Metas.designTimeRebuild();

      //var metas = new LangsLib.Metas();

      //StemmerBreaker.Services.testCreation();

    }
  }
}
