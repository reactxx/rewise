// inheritance: https://weblogs.asp.net/manavi/inheritance-mapping-strategies-with-entity-framework-code-first-ctp5-part-1-table-per-hierarchy-tph

namespace WiktModel {

  public abstract partial class Helper {
    public int Id { get; set; }
  }

  // Page
  public partial class Page: Helper {
    public string Title { get; set; }
  }

  // Entry
  public partial class Entry : Helper {
    public int PageId { get; set; }
    public byte NymType { get; set; }

    // direct infos
    public string PartOfSpeech { get; set; } // dbnary:partOfSpeech - xsd:string, l:Nym - lexinfo:partOfSpeech - @lexinfo
    public string Languages { get; set; } // lime:language - xsd:string, terms:language - @lexvo
    public string AbbreviationFor { get; set; } // lexinfo:abbreviationFor - xsd:string
    public string WrittenRep { get; set; } // ontolex:writtenRep - rdf:langString
    public string LexicalRel { get; set; } // vartrans:lexicalRel - "blank"

    // Uri values
    public byte Gender { get; set; } // lexinfo:gender - @lexinfo,
    public byte HasCountability { get; set; } // olia:hasCountability - @olia,
    public byte HasInflectionType { get; set; } // olia:hasInflectionType - @olia,
    public byte HasSeparability { get; set; } // olia:hasSeparability - @olia,
    public byte HasValency { get; set; } // olia:hasValency - @olia,
    public byte HasVoice { get; set; } // olia:hasVoice - @olia,
  }

  // Translation
  public partial class Translation : Helper {
    public int OfEntry { get; set; }
    public int OfPage { get; set; }
    public int OfSense { get; set; }

    public string WrittenForm { get; set; } // dbnary:writtenForm - rdf:langString, dbnary:writtenForm - xsd:string
    public string Usage { get; set; } // dbnary:usage - xsd:string
    public string TargetLanguageCode { get; set; } // dbnary:targetLanguage - @lexvo, dbnary:targetLanguageCode - xsd:string
  }

  // Sense
  public partial class Sense : Helper {
    public int SenseNumber { get; set; } // dbnary:senseNumber - xsd:string
    public string Definition { get; set; } // skos:definition - "blank"
    public string Example { get; set; } // skos:example - "blank"
  }

  public partial class Statement : Helper {
    public int SubjectId { get; set; } // Page or Entry id
    public int PageObjectId { get; set; }
    public byte NymType { get; set; }
    public string Usage { get; set; } // SV only, 180 cases only
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
