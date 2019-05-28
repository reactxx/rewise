// inheritance: https://weblogs.asp.net/manavi/inheritance-mapping-strategies-with-entity-framework-code-first-ctp5-part-1-table-per-hierarchy-tph

using static WiktSchema;

namespace WiktModel {

  public abstract class Helper {
    public int Id;
    public virtual void acceptProp(ParsedTriple t, WiktCtx ctx) { }
  }

  public abstract class FormLike: Helper {
    public string writtenRep;
    public NymRels nyms;
    public FormInfos infos;
  }
  // Page
  public class Page : Helper {
    public string Title;
  }

  // Entry
  public class Entry : Helper {

    public int PageId;

    // values ValidPartOfspeach[lexinfo:noun, ...]
    // Q: more-than-single-partOfSpeach, all-partOfSpeach
    public byte PartOfSpeech; // lexinfo:partOfSpeech (excluded lexinfo:partOfSpeech=ontolex:LexicalEntry), ignored dbnary:partOfSpeech

    // Q: canonical-form-sh, canonical-form, entry-writtenRep
    public string WrittenRep; // for SH: ontolex:writtenRep. Else ontolex:canonicalForm.ontolex:writtenRep

  }

  public class Gloss : Helper {
    public string value; // rdf_value,
    public int? rank; // dbnary_rank - xsd:int
    public string senseNumber; //dbnary:senseNumber - xsd:string
  }

  public class Form : Helper {
  }

  // Translation
  public class Translation : Helper {
    public int? isTranslationOf;
    public int? gloss;
    public string writtenForm;
    public string usage;
    public string targetLanguage;
    //public string targetLanguageCode;
  }

  // Sense
  public class Sense : Helper {
    public int SenseNumber; // dbnary:senseNumber - xsd:string
    public string Definition; // skos:definition - "blank"
    public string Example; // skos:example - "blank"
  }

  public class Statement : Helper {
    public int SubjectId; // Page or Entry id
    public int PageObjectId;
    public byte NymType;
    public string Usage; // SV only, 180 cases only
  }

}
