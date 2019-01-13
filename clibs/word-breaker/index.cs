/*
      StemmerBreaker.Services.init(@"c:\rewise\", true);
      var service = StemmerBreaker.Services.getService(LangsLib.langs.cs_cz);
      var words = service.wordBreak("1 2 3");
      var stemms = service.stemm("123");
 * */
using LangsLib;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StemmerBreaker {

  public static class Root {
    public static string root = AppDomain.CurrentDomain.BaseDirectory[0] + @":\rewise\clibs\word-breaker\";
  }

  public enum PutTypes { put, alt, EOW, EOS, EOP, EOC, phraseStart, phraseEnd, phrase, phraseSmall, error }

  public struct StemItem { public PutTypes type; public string word; }

  public struct TPosLen {
    public int Pos;
    public int Len; //could be negative when SpellChecker error
  }

  // for every thread single Runners
  public class Services {

    public static void testCreation() {
      instance = new Services();
      foreach (var runner in instance.runners.Values)
        runner.testCreation();
    }

    public static Service getService(langs lang) {
      if (instance == null) instance = new Services();
      return instance.runners[lang];
    }
    Services() {
      runners = LangsLib.Metas.Items.ToDictionary(kv => kv.Key, kv => new Service((langs)kv.Value.LCID));
    }
    public static Service getDefaultCOM() {
      return instance.runners[langs._];
    }
    [ThreadStatic] static Services instance;
    Dictionary<langs, Service> runners;
  }

  public class Service {

    internal Service(langs lang) {
      this.lang = lang;
      com = LibManager.getCOM(lang);
    }

    internal void testCreation() {
      var br = wordBreak("1 2 3");
      var st = stemm("123");
    }

    langs lang;
    COM com;

    IStemmer stemmer; IWordBreaker breaker;

    public List<string> wordBreakLargeWordList(string text, int lineCount) {
      var res = new List<string>();
      Parallel.ForEach(SplitLines.Run(text, lineCount), chunk => {
        wordBreak(chunk, getWordBreaker(), (type, pos, len) => {
          if (type != PutTypes.put) return;
          lock(res)
            res.Add(chunk.Substring(pos, len));
        });
      });
      return res;
    }

    public List<string> wordBreakWords(string text) {
      var res = new List<string>();
      wordBreak(text, (type, pos, len) => {
        if (type != PutTypes.put) return;
        res.Add(text.Substring(pos, len));
      });
      return res;
    }

    public List<TPosLen> wordBreak(string text) {
      var res = new List<TPosLen>();
      wordBreak(text, (type, pos, len) => {
        if (type != PutTypes.put) return;
        res.Add(new TPosLen { Pos = pos, Len = len });
      });
      return res;
    }

    public IWordBreaker getWordBreaker() {
      IWordBreaker res = com.getWordBreaker();
      if (res == null)
        res = Services.getDefaultCOM().com.getWordBreaker(); //neutral word breaker
      if (res == null)
        throw new Exception("breaker == null");
      return res;
    }

    public static void wordBreak(string text, IWordBreaker breaker, Action<PutTypes, int, int> onPutWord) {
      if (string.IsNullOrEmpty(text)) return;
      BreakSink cws = new BreakSink(onPutWord);
      TEXT_SOURCE pTextSource = new TEXT_SOURCE();
      pTextSource.pfnFillTextBuffer += fillTextBuffer;
      //pTextSource.awcBuffer = text;
      pTextSource.awcBuffer = text;
      pTextSource.iCur = 0;
      pTextSource.iEnd = text.Length;
      breaker.BreakText(ref pTextSource, cws, /*cps*/null);
    }


    public void wordBreak(string text, Action<PutTypes, int, int> onPutWord) {
      if (string.IsNullOrEmpty(text)) return;
      if (breaker == null)
        breaker = getWordBreaker();
      wordBreak(text, breaker, onPutWord);
    }

    public List<string> stemm(string word, List<string> res = null) {
      stemm(word, (type, w) => {
        //if (type != PutTypes.put) return;
        if (res == null) res = new List<string>();
        res.Add(w);
      });
      return res;
    }

    public void stemm(string word, Action<PutTypes, string> onPutWord) {
      if (stemmer == null) stemmer = com.getStemmer();
      if (stemmer == null) { onPutWord(PutTypes.put, word); return; }
      StemSink sink = new StemSink(onPutWord);
      stemmer.GenerateWordForms(word, word.Length, sink);
    }

    //**************** private
    static uint fillTextBuffer(ref TEXT_SOURCE ts) {
      return WBREAK_E_END_OF_TEXT;
    }

    const uint WBREAK_E_END_OF_TEXT = 0x80041780;

    class StemSink : IWordFormSink {
      public StemSink(Action<PutTypes, string> onPutWord) { this.onPutWord = onPutWord; }
      Action<PutTypes, string> onPutWord;
      public void PutAltWord(string pwcInBuf, int cwc) { onPutWord(PutTypes.alt, pwcInBuf.Substring(0, cwc)); }
      public void PutWord(string pwcInBuf, int cwc) { onPutWord(PutTypes.put, pwcInBuf.Substring(0, cwc)); }
    }

    class BreakSink : IWordSink {
      public BreakSink(Action<PutTypes, int, int> onPutWord) { this.onPutWord = onPutWord; }
      Action<PutTypes, int, int> onPutWord;
      //int count = 0;
      public void PutWord(int cwc, string pwcInBuf, int cwcSrcLen, int cwcSrcPos) {
        //if (cwcSrcPos > 255 || cwcSrcLen > 127) throw new Exception("class BreakSink : IWordSink: cwcSrcPos > 255 || cwcSrcLen > 127");
        onPutWord(PutTypes.put, cwcSrcPos, cwcSrcLen);
        //count++;
      }
      //https://tlzprgmr.wordpress.com/2008/02/19/ifilters-part-2-using-word-breakers/: "Date: 2/19/2008" results in word: Date, alt: 2/19/2008, word: DD20080219
      public void PutAltWord(int cwc, string pwcInBuf, int cwcSrcLen, int cwcSrcPos) { onPutWord(PutTypes.alt, (byte)cwcSrcPos, (sbyte)cwcSrcLen); }
      public void StartAltPhrase() { onPutWord(PutTypes.phraseStart, 0, -1); }
      public void EndAltPhrase() { onPutWord(PutTypes.phraseEnd, 0, -1); }
      public void PutBreak(WORDREP_BREAK_TYPE breakType) {
        switch (breakType) {
          case WORDREP_BREAK_TYPE.WORDREP_BREAK_EOC: onPutWord(PutTypes.EOC, 0, -1); break;
          case WORDREP_BREAK_TYPE.WORDREP_BREAK_EOP: onPutWord(PutTypes.EOP, 0, -1); break;
          case WORDREP_BREAK_TYPE.WORDREP_BREAK_EOS: onPutWord(PutTypes.EOS, 0, -1); break;
          case WORDREP_BREAK_TYPE.WORDREP_BREAK_EOW: onPutWord(PutTypes.EOW, 0, -1); break;
          default: onPutWord(PutTypes.error, 0, -1); break;
        }
      }
    }

  }
}
