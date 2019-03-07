namespace DartDom {
  // BOOK
  public class Book {
    public string id; // guid ID
    public Meta meta;
    public BookLang src;
    public BookLang dest;
  }
  public class Meta {
  }
  // BOOK LANG
  public class BookLang {
    public string lang;
    //*** large data
    public Fact[] factsDir;
    public DiffHistory[] factsHistoryDir;
    public TrieDir tries;
    public int[][] groupsFactIds; // for every groupId: its factId's
    //--------------
    ////*** integer sizes (num of bits in single byte)
    //public int groupIdSize; // for TrieNode.groupIds
    //public int factIdSize; // for Fact.id, Fact.rightIds, Fact.importId, TrieNode.factIds, BookLang.groupsFactIds
    //public int lessonIdSize; // for Fact.lessonId
    //public int wordClassSize; // for Fact.wordClass
    ////*** filled after compilation
    //public int factsDirSize; // size of length of compiled BookLang.factsDir
    //public int factsHistoryDirSize; // size of length of compiled BookLang.factsHistoryDir
    //public int trieDirSize;  // size of length of compiled BookLang.TrieDir
    ////--------------
    ////*** string compression
    //public CompressMap texts; // for Fact.text
    //public CompressMap suffixes; // for TrieNode.suffix
    //public CompressMap diffValues; // for DiffEntry.value
    ////*** int compression
    //public CompressMap breaks; // for Fact.breaks, 
  }
  // FACT
  public class Fact {
    //public int id; // unique id in BookLang. Is filled in loadFact(int factId) method
    public string text; 
    public int[] breaks; // word-breaking result. [pos1, len1, pos2, len2, ...]. First zero is ommited. If null => whole text;
    public int[] rightIds; // seznam pravych stran
    // size flags:
    // in Dart public int sizeFlags; // exists: wordClass, lessonId, breaks
    public WordClass wordClass; // word class
    public int lessonId;
    //public int importId; // number of line in .cvs file
  }
  public enum WordClass { }
  // TRIE
  public class TrieDir {
    public Trie[] prefixes;
  }
  public class Trie {
    public string prefix;
    public TrieNode[] suffixes; // dart-compiles to byte[]
    public int triePos; // after compiation: position of compiled data
  }
  public class TrieNode {
    public string suffix;
    public int[] groupIds;
    public int[] factIds;
  }
  // COMPRESS
  //public class CompressMap {
  //  public string[] dataString;
  //  public int[][] dataInt;
  //}
  // DIFF ???? TODO C# x Dart ????
  public class DiffHistory {
    //public int factId; // unique fact id in book's lang. Is filled in loadHistory(int factId) method
    public DiffHistoryItem[] items;
  }
  public class DiffHistoryItem {
    public DiffEntry[] diffs;
    public DiffInfo info;
  }
  public class DiffInfo {
    // public int idx; DiffInfo order, filled during decompression
    public int authorId; // ??
    public int date; // ??
    public int stars;
  }
  public enum DiffEntryType {
    remove,
    add,
    equal
  };
  public class DiffEntry {
    //public DiffEntryType entryType; computed from flags
    public int flags; // 1 bit: 0-equal, 1-remove
    public string value;
    public int count; // count len: 0 (=> add), 1 2 3: 3, 5 or 10 bits)
  }
}
