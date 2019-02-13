namespace fulltext {
  class Program {

    static void Main(string[] args) {
      using (var imp = new Impersonator.Impersonator("pavel", "LANGMaster", "zvahov88_")) {

        //****** basic utils
        //CreateFrekventWords.run();
        //HunspellLib.extractWordLists();
        //StemmingRaw.processLangs(WordLists.wordLists);

        //UnicodeDesignLib.getUnicodeBlockNames();

        //CldrDesignLib.RefreshCldrDataSource();
        //CldrDesignLib.RefreshNetSuportedCultures();
        //CldrDesignLib.RefreshOldToNew();
        //CldrDesignLib.RefreshTexts();
        //CldrDesignLib.Build(); CldrDesignLib.RefreshCldrStatistics();
        //CldrTrans.Build();

        //LangsDesignLib.RefreshOldVersionInfo();

        LangsDesignLib.Build(); //ALL

        //****** basic tests
        //CldrLangMatrix.load().save();

        //Unicode.dumpNetUncLettersDiff();
        //var idxs = LangsLib.UnicodeBlockNames.blockIdxs("abcABC123-");

        //StemmingRaw.processLang(LangsLib.Metas.get(LangsLib.langs.de_de).lc, WordLists.wordLists, true, false);
        //var metas = new LangsLib.Metas();
        //StemmerBreaker.Services.testCreation();

        //****** trash
        //CultureInfoTexts.dumpCldrRbnfTypes();
        //lang_chars.extractFromMimer.extractAlphabets();
        //lang_chars.extractFromMimer.useAlphabets();

      }
    }
  }
}
