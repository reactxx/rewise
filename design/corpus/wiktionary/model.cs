// inheritance: https://weblogs.asp.net/manavi/inheritance-mapping-strategies-with-entity-framework-code-first-ctp5-part-1-table-per-hierarchy-tph

namespace WiktModel {

  // Page
  public partial class Page {
    public int Id { get; set; }
    public string Title { get; set; }
  }

  // Entry
  public partial class Entry {
    public int Id { get; set; }
    public int PageId { get; set; }
    public byte NymType { get; set; }

    // direct infos
    public string PartOfSpeech { get; set; }
    public string AbbreviationFor { get; set; }
    public string WrittenRep { get; set; }
    public string LexicalRel { get; set; }
  }

  // Translation
  public partial class Translation {
    public int Id { get; set; }
    public int OfEntry { get; set; }
    public int OfPage { get; set; }
    public int OfSense { get; set; }

    public string WrittenForm { get; set; }
    public string Usage { get; set; }
    public string TargetLanguageCode { get; set; }
  }

  // Sense
  public partial class Sense {
    public int Id { get; set; }

    public int SenseNumber { get; set; }
    public string Definition { get; set; }
    public string Example { get; set; }
  }

  //************** M:N
  public class Entry_Sense {
    public int EntryId { get; set; }
    public int SenseId { get; set; }

    public byte EntryType { get; set; }
  }

  public class Entry_Nym {
    public int EntryId { get; set; }
    public int ToPageId { get; set; }

    public byte EntryType { get; set; }
    public byte NymType { get; set; }
  }

  public class Sense_Nym {
    public int SenseId { get; set; }
    public int ToPageId { get; set; }

    public byte NymType { get; set; }
  }

  public class Page_Nym {
    public int PageId { get; set; }
    public int ToPageId { get; set; }

    public byte NymType { get; set; }
  }

}
