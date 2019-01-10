using System;
using System.Collections.Generic;
using System.IO;

namespace indexer {

  //****** Source Book data


  public interface FactText {
    int id { get; }
    string text { get; }
    Word[] words { set; }
  }
  public class Word {
    public int id;
    public int start;
    public int len;
    public int[] stemmGroups;
  }

  //****** Word breaking and assigning global ID to words. Creating prefix index.
  //public class BookFactWords {
  //  public FactWords[] facts;
  //  public byte[] prefixIdx;
  //}

  ////****** assigning stemmGroups to every word from all facts
  //public class BookFactStems {
  //  public FactStems[] facts;
  //}
  //public class FactStems: FactWords {
  //  public int[] stemmGroups;
  //}

  public static class BookIndex {
    static void FactWords (IEnumerable<FactText> facts, Dictionary<string, int> globalWordsIdx) {
    }
    //static BookFactStems getBookFactStems(FactWords[] facts) {     return null;
    //}
  }

}
