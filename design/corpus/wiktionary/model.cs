// inheritance: https://weblogs.asp.net/manavi/inheritance-mapping-strategies-with-entity-framework-code-first-ctp5-part-1-table-per-hierarchy-tph

//************* TODO
// GLOSS: t.setIntValue(ctx, this, predicates.ref rank) || // 14384x DUPL !!!! 
// Statement: what are type of subject and object
// for pl: TranslationD: t.setRefValue(ctx, this, predicates.dbnary_isTranslationOf, ref isTranslationOf) : 479919x DUPL

using System.Collections.Generic;
using static WiktConsts;

namespace WiktModel {

  // Page
  public class Page : Helper, ITranslation {
    // ? ontolex_canonicalForm, ontolex_otherForm, ontolex_sense, lexinfo_partOfSpeech, olia_hasCountability, lime_language
    public string title;
    public List<NymRel> nyms;
    public List<NymRel> nymsOf;
    public Entry[] entries;
    public List<TranslationData> translations { get; set; }
  }

  // Entry
  public class Entry : Helper, ITranslation {
    // ? vartrans_lexicalRel, dbnary_describes, lime_language
    public FormData canonicalForm;
    public FormData[] otherForm;
    public lexinfo_partOfSpeech partOfSpeech;
    public List<lexinfo_partOfSpeechEx> partOfSpeechEx;
    public string writtenRep;
    public List<NymRel> nyms;
    public FormInfos infos;
    //public List<Gloss> translationGlosses;
    public List<TranslationData> translations { get; set; }
    public SenseData[] senses;
  }

  public class Gloss : Helper {
    public GlossData gloss;
  }

  public class Form : Helper {
    public FormData form;
  }

  // Translation
  public class Translation : Helper {
    public TranslationData trans;
  }

  // Sense
  public class Sense : Helper {
    public SenseData sense;
  }

  public class Statement : Helper {
    // ? dbnary_usage, 
    public GlossData gloss;
    public int? subjectId;
    public int? objectId;
    public rdf_predicate predicate;
  }

  //***************** HELPERs
   
  public struct GlossData {
    public string value; // rdf_value,
    public int? rank; // dbnary_rank - xsd:int
    public string senseNumber; //dbnary:senseNumber - xsd:string
  }

  public struct FormData {
    public string note;
    public string writtenRep;
    public FormInfos infos;
  }

  public struct SenseData: ITranslation {
    public List<NymRel> nyms;
    public string senseNumber;
    public string definition;
    public string example;
    public List<TranslationData> translations { get; set; }
  }

  public struct TranslationData {
    public string writtenForm;
    public string usage;
    public string targetLanguage;
  }

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

  public interface ITranslation {
    List<TranslationData> translations { get; set; }
  }

  public partial class Helper {
    public int id;
  }



}
