namespace WiktModel {
  public interface IObj {
    int Id { get; set; }
  }
  public class Page : IObj {
    public int Id { get; set; }
    public string Title { get; set; }
  }
  public class Entry : IObj {
    public int Id { get; set; }
    // uniq ?o dbnary:describes ?s
    public int PageId { get; set; }
    // NOUNIQUE ?s ontolex:sense ?o
    public int SenseId { get; set; }
    public string PartOfSpeech { get; set; }
    // uniq ?s ontolex:canonicalForm ?o
    public string Title { get; set; } // title;note. ?? Is CanonicalForm unique
    public string[] Other { get; set; } // [title;note]
    // public string Language { get; set; } - prop_entry_language.ttl, is singletone? => export all uniques
  }
  public class Trans : IObj {
    public int Id { get; set; }
    // uniq ?s dbnary:isTranslationOf ?o . ?s a dbnary:Translation
    public int IsTranslationOf { get; set; } // unique?
    public ushort Lang { get; set; }
    public string Usage { get; set; }
    // uniq # ?s dbnary:gloss ?o . ?s a dbnary:Translation .
    public string GlossRank { get; set; }
    public string Gloss { get; set; }
  }
  public class Sense : IObj {
    public int Id { get; set; }
    public int Number { get; set; }
  }

  public class SenseObj {
    public int ObjId { get; set; }
    public int SenseId { get; set; }
  }

}
