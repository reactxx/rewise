// inheritance: https://weblogs.asp.net/manavi/inheritance-mapping-strategies-with-entity-framework-code-first-ctp5-part-1-table-per-hierarchy-tph

using System.Collections.Generic;
using static WiktConsts;
using static WiktSchema;

namespace WiktModel {

  public struct NymRels {
    public List<int> antonym;
    public List<int> approximateSynonym;
    public List<int> holonym;
    public List<int> hypernym;
    public List<int> hyponym;
    public List<int> meronym;
    public List<int> synonym;
  }

  public struct FormInfos {
    public olia_hasCase hasCase;
    public olia_hasDegree hasDegree;
    public olia_hasInflectionType hasInflectionType;
    public olia_hasCountability hasCountability;
    public olia_hasMood hasMood;
    public olia_hasVoice hasVoice;
    public lexinfo_animacy animacy;
    public lexinfo_verbFormMood verbFormMood;
    public number number;
    public person person;
    public gender gender;
    public tense tense;
  }

  public partial class Helper {
    public int id;
  }

  public abstract class FormLike: Helper {
    public string writtenRep;
    public NymRels nyms;
    public FormInfos infos;
  }
  // Page
  public class Page : Helper {
    // ? ontolex_canonicalForm, ontolex_otherForm, ontolex_sense, lexinfo_partOfSpeech, olia_hasCountability, lime_language
    public string title;
  }

  // Entry
  public class Entry : FormLike {
    // ? vartrans_lexicalRel, dbnary_describes, lime_language
    public int? pageId; // fill from page
    public int? canonicalFormId;
    public List<int> otherFormIds;
    public List<int> senseIds;
    public lexinfo_partOfSpeech partOfSpeech;
    public List<lexinfo_partOfSpeechEx> partOfSpeechEx;
    //public int? sense;
  }

  public class Gloss : Helper {
    public string value; // rdf_value,
    public int? rank; // dbnary_rank - xsd:int
    public string senseNumber; //dbnary:senseNumber - xsd:string
  }

  public class Form : FormLike {
    public string note;
    // ? lexinfo_pronunciation, ontolex_phoneticRep
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
    public NymRels nyms;
    public string senseNumber;
    public string definition;
    public string example;
  }

  public class Statement : Helper {
    // ? dbnary_usage, 
    public int? subjectId;
    public int? objectId;
    public rdf_predicate predicate;
    public int? gloss;
  }

}
