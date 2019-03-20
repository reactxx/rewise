/*
var words = Service.wordBreak("cs-CZ", "1 2 3");
var stemms = service.getWordStemms("cs-CZ", "123");
*/
using System;
using System.Linq;
using System.Collections.Generic;

namespace StemmerBreakerNew {

  public static class Root {
    public static string root = AppDomain.CurrentDomain.BaseDirectory[0] + @":\rewise\clibs\breaking\appdata\";
  }

  public struct TPosLen {
    public int Pos;
    public int Len;
  }

  public static class Service {

    public static List<TPosLen>[] wordBreak(string lang, IList<string> texts) {
      var breaker = Creators.createBreaker(lang);
      var res = new List<TPosLen>[texts.Count];
      for (var i = 0; i < texts.Count; i++) {
        var l = res[i] = new List<TPosLen>();
        wordBreak(texts[i], breaker, (type, pos, len) => {
          if (type != PutTypes.put) return;
          l.Add(new TPosLen { Pos = pos, Len = len });
        });
      }
      return res;
    }

    public static List<string> getWordStemms(string lang, string word, List<string> res = null) {
      stemm(lang, word, (type, w) => {
        //if (type != PutTypes.put) return;
        if (res == null) res = new List<string>();
        res.Add(w);
      });
      return res;
    }

    //**************** private
    enum PutTypes { put, alt, EOW, EOS, EOP, EOC, phraseStart, phraseEnd, phrase, phraseSmall, error }
    struct StemItem { public PutTypes type; public string word; }

    static void wordBreak(string text, IWordBreaker breaker, Action<PutTypes, int, int> onPutWord) {
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

    static void stemm(string lang, string word, Action<PutTypes, string> onPutWord) {
      var stemmer = Creators.createStemmer(lang);
      if (stemmer == null) { onPutWord(PutTypes.put, word); return; }
      StemSink sink = new StemSink(onPutWord);
      stemmer.GenerateWordForms(word, word.Length, sink);
    }

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
