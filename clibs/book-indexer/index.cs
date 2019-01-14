using System;
using System.Collections.Generic;
using System.IO;

namespace indexer {

  //****** Source Book data
  public interface Book {
    IEnumerable<FactText> facts { get; }
    LangsLib.langs lang { get; }

  }

  public interface FactText {
    int id { get; } // internal book fact ID
    string text { get; } // fact text
  }
  public class Word {
    public int id; // global word ID
    public int start; public int len; // SOURCE word in range in FactText.text
    public int[] groups; // stemm groups
  }
  public class PrefixWord {
    public int id; // global word ID
    public int start; public int len; // SOURCE word in range in FactText.text
    public int[] groups; // stemm groups
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
    static void FactWords(IEnumerable<FactText> facts, Dictionary<string, int> globalWordsIdx) {
    }
    //static BookFactStems getBookFactStems(FactWords[] facts) {     return null;
    //}
  }

}
