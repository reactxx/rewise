using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF;

namespace WiktModel {
  public class Page {
    public int Id { get; set; }
    public string Title { get; set; }
  }
  public class Entry {
    public int Id { get; set; }
    public int PageId { get; set; } // unique?
    public int SenseId { get; set; } // unique?
    public string PartOfSpeech { get; set; }
    public string Title { get; set; } // title;note. ?? Is CanonicalForm unique
    public string[] Other { get; set; } // [title;note]
    // public string Language { get; set; } - prop_entry_language.ttl, is singletone? => export all uniques
  }
  public class Trans {
    public int Id { get; set; }
    public int EntryId { get; set; } // unique?
    public int SenseId { get; set; } // unique?
    public ushort Lang { get; set; }
    public string Usage { get; set; }
    // gloss ?? is unique to Id ?
    public string GlossRank { get; set; }
    public string Gloss { get; set; }
  }
  public class Sense {
    public int Id { get; set; }
  }
}
