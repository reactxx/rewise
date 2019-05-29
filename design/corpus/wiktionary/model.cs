// inheritance: https://weblogs.asp.net/manavi/inheritance-mapping-strategies-with-entity-framework-code-first-ctp5-part-1-table-per-hierarchy-tph

//************* TODO
// GLOSS: t.setIntValue(ctx, this, predicates.ref rank) || // 14384x DUPL !!!! 
// Statement: what are type of subject and object
// for pl: TranslationD: t.setRefValue(ctx, this, predicates.dbnary_isTranslationOf, ref isTranslationOf) : 479919x DUPL

using System.Collections.Generic;
using static WiktConsts;

namespace WiktModel {

  public struct NymRel {
    public rdf_predicate type;
    public int relId;
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

  // Page
  public class Page : Helper {
    // ? ontolex_canonicalForm, ontolex_otherForm, ontolex_sense, lexinfo_partOfSpeech, olia_hasCountability, lime_language
    public string title;
    public List<int> describes;
    public List<NymRel> nyms;
    public List<NymRel> nymsOf;
  }

  // Entry
  public class Entry : Helper {
    // ? vartrans_lexicalRel, dbnary_describes, lime_language
    public int? pageId; // fill from page
    public int? canonicalFormId;
    public List<int> otherFormIds;
    public List<int> senseIds;
    public List<int> isTranslationOf;
    public lexinfo_partOfSpeech partOfSpeech;
    public List<lexinfo_partOfSpeechEx> partOfSpeechEx;
    public string writtenRep;
    public List<NymRel> nyms;
    public FormInfos infos;
  }

  public class Gloss : Helper {
    public string value; // rdf_value,
    public int? rank; // dbnary_rank - xsd:int
    public string senseNumber; //dbnary:senseNumber - xsd:string
    // back refs
    public int? translationOf;
    public int? statementOf;
  }

  public class Form : Helper {
    public string note;
    public string writtenRep;
    public FormInfos infos;
    // back refs
    public int? canonicalOf;
    public int? otherOf;
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
    public List<NymRel> nyms;
    public List<int> senseIdsOf;
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
