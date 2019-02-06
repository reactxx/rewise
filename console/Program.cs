namespace fulltext {
  class Program {

    static void Main(string[] args) {
      using (var imp = new Impersonator.Impersonator("pavel", "LANGMaster", "zvahov88_")) {

        //****** basic utils
        //LangsLib.Metas.designTimeRebuild(); 
        //CreateFrekventWords.run();
        //HunspellLib.extractWordLists();
        //StemmingRaw.processLangs(WordLists.wordLists);
        //Unicode.getUnicodeBlockNames();
        CldrLib.Build(true);

        //****** basic tests

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
