namespace Dom {
  // BOOK
  public class Book {
    public string id; // guid ID
    public string srcLang; // null for Goethe and Eurotalk
    public Meta meta;
    public Lang[] langs;
  }
  public class Meta {
  }
  // LANG
  public class Lang {
    public string lang;
    public FactRoot[] facts;
    public Trie tries;
    public CompressMap texts;
    public CompressMap suffixes;
    public CompressMap breaks;
  }
  // FACT
  public class FactRoot: LangFact {
    public int lesson;
    public int csvId; // number of line in .cvs file
  }
  public class LangFact {
    public int id; // unique id in book's lang
    public string text; // null iff childs!=null
    public int[] breaks; // word-breaking result. [pos1, len1, pos2, len2, ...]
    public LangFact[] childs;
    public int[] ftxGroupIds;
    public ChildTypes childType;
  }
  public enum ChildTypes { none, wclass, meaning, synonym }
  // TRIE
  public class Trie {
    public TriePrefix[] prefixes;
  }
  public class TriePrefix {
    public string prefix;
    public TrieSuffix[] suffixes;
  }
  public class TrieSuffix {
    public string suffix;
    public int[] ftxGroupIds;
    public int[] factIds;
  }
  // COMPRESS
  public class CompressMap {
    public int count;
    public CompressMapItem[] items;
  }
  public class CompressMapItem {
    public int key;
    public int count;
  }
}