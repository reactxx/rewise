using System;
using System.Collections.Generic;
using System.Linq;

public static class WiktConsts {

  public static HashSet<string> Ignored = new HashSet<string> {
    { "dbnary:troponym" },
    { "dbnary:partOfSpeech" },
    { "terms:language" },
    { "lexinfo:relatedTerm"},
    { "lexinfo:root"},
    { "lexinfo:abbreviationFor"},
    { "olia:hasValency"},
    { "olia:hasSeparability"},
    { "olia:hasDefiniteness"},
    { "owl:inverseOf"},
    { "protonsys:transitiveOver"},
    { "rdfs:subPropertyOf"},
    { "rdfs:subClassOf"},
    { "rdfs:domain"},
    { "rdfs:range"},
    { "terms:description"},
  };

  public enum predicates {
    no,
    rdf_type, rdf_predicate, rdf_value, rdf_object, rdf_subject,
    dbnary_antonym, dbnary_approximateSynonym, dbnary_describes, dbnary_gloss, dbnary_holonym, dbnary_hypernym, dbnary_hyponym, dbnary_isTranslationOf, dbnary_meronym,
    dbnary_rank, dbnary_senseNumber, dbnary_synonym, dbnary_targetLanguage, dbnary_targetLanguageCode, dbnary_usage, dbnary_writtenForm, lexinfo_animacy,
    lexinfo_gender, lexinfo_number, lexinfo_partOfSpeech, lexinfo_person, lexinfo_pronunciation, lexinfo_tense, lexinfo_verbFormMood, lime_language, olia_hasCase,
    olia_hasCountability, olia_hasDegree, olia_hasGender, olia_hasInflectionType, olia_hasMood, olia_hasNumber, olia_hasPerson, olia_hasTense, olia_hasVoice, ontolex_canonicalForm,
    ontolex_otherForm, ontolex_phoneticRep, ontolex_sense, ontolex_writtenRep, skos_definition, skos_example, skos_note, vartrans_lexicalRel,
  }

  public enum PredicateType {
    no,
    a,
    ValueProps, UriValuesProps, BlankProps, BlankPropsInner, NymRelProps, NotNymRelProps
  }

  public static bool parsePredicate(string url, out predicates pred, out PredicateType type) {
    pred = predicates.no; type = PredicateType.no;
    if (!string2Pred.TryGetValue(url, out pred)) return false;
    type = predicateTypes[pred];
    return true;
  }
  static Dictionary<string, predicates> string2Pred = Enum.GetValues(typeof(predicates)).Cast<predicates>().ToDictionary(p => Enum.GetName(typeof(predicates), p).Replace('_', ':'));

  internal static Dictionary<predicates, PredicateType> predicateTypes = new Dictionary<predicates, PredicateType> {
    { predicates.rdf_type, PredicateType.a},
    { predicates.rdf_value, PredicateType.BlankPropsInner},
    { predicates.skos_definition, PredicateType.BlankProps},
    { predicates.skos_example, PredicateType.BlankProps},
    { predicates.vartrans_lexicalRel, PredicateType.BlankProps},
    { predicates.dbnary_describes, PredicateType.NotNymRelProps},
    { predicates.dbnary_gloss, PredicateType.NotNymRelProps},
    { predicates.dbnary_isTranslationOf, PredicateType.NotNymRelProps},
    { predicates.ontolex_canonicalForm, PredicateType.NotNymRelProps},
    { predicates.ontolex_otherForm, PredicateType.NotNymRelProps},
    { predicates.ontolex_sense, PredicateType.NotNymRelProps},
    { predicates.rdf_object, PredicateType.NotNymRelProps},
    { predicates.rdf_subject, PredicateType.NotNymRelProps},
    { predicates.dbnary_antonym, PredicateType.NymRelProps},
    { predicates.dbnary_approximateSynonym, PredicateType.NymRelProps},
    { predicates.dbnary_holonym, PredicateType.NymRelProps},
    { predicates.dbnary_hypernym, PredicateType.NymRelProps},
    { predicates.dbnary_hyponym, PredicateType.NymRelProps},
    { predicates.dbnary_meronym, PredicateType.NymRelProps},
    { predicates.dbnary_synonym, PredicateType.NymRelProps},
    { predicates.lexinfo_animacy, PredicateType.UriValuesProps},
    { predicates.lexinfo_gender, PredicateType.UriValuesProps},
    { predicates.lexinfo_number, PredicateType.UriValuesProps},
    { predicates.lexinfo_partOfSpeech, PredicateType.UriValuesProps},
    { predicates.lexinfo_person, PredicateType.UriValuesProps},
    { predicates.lexinfo_tense, PredicateType.UriValuesProps},
    { predicates.lexinfo_verbFormMood, PredicateType.UriValuesProps},
    { predicates.olia_hasCase, PredicateType.UriValuesProps},
    { predicates.olia_hasCountability, PredicateType.UriValuesProps},
    { predicates.olia_hasDegree, PredicateType.UriValuesProps},
    { predicates.olia_hasGender, PredicateType.UriValuesProps},
    { predicates.olia_hasInflectionType, PredicateType.UriValuesProps},
    { predicates.olia_hasMood, PredicateType.UriValuesProps},
    { predicates.olia_hasNumber, PredicateType.UriValuesProps},
    { predicates.olia_hasPerson, PredicateType.UriValuesProps},
    { predicates.olia_hasTense, PredicateType.UriValuesProps},
    { predicates.olia_hasVoice, PredicateType.UriValuesProps},
    { predicates.rdf_predicate, PredicateType.UriValuesProps},
    { predicates.dbnary_rank, PredicateType.ValueProps},
    { predicates.dbnary_senseNumber, PredicateType.ValueProps},
    { predicates.dbnary_targetLanguage, PredicateType.ValueProps},
    { predicates.dbnary_targetLanguageCode, PredicateType.ValueProps},
    { predicates.dbnary_usage, PredicateType.ValueProps},
    { predicates.dbnary_writtenForm, PredicateType.ValueProps},
    { predicates.lexinfo_pronunciation, PredicateType.ValueProps},
    { predicates.lime_language, PredicateType.ValueProps},
    { predicates.ontolex_phoneticRep, PredicateType.ValueProps},
    { predicates.ontolex_writtenRep, PredicateType.ValueProps},
    { predicates.skos_note, PredicateType.ValueProps},
  };

  public enum lexinfo_partOfSpeech : byte {
    no,
    lexinfo_adjective, lexinfo_adverb, lexinfo_cardinalNumeral, lexinfo_interjection,
    lexinfo_noun, lexinfo_preposition, lexinfo_pronoun, lexinfo_verb
  }
  public enum olia_hasCase : byte {
    no,
    olia_Accusative,
    olia_DativeCase,
    olia_GenitiveCase,
    olia_InstrumentalCase,
    olia_LocativeCase,
    olia_Nominative,
    olia_VocativeCase,
  }
  public enum olia_hasCountability : byte {
    no,
    olia_Countable,
    olia_Uncountable,
  }
  public enum olia_hasDegree : byte {
    no,
    olia_Comparative,
    olia_Positive,
    olia_Superlative,
  }
  public enum olia_hasInflectionType : byte {
    no,
    olia_MixedInflection,
    olia_StrongInflection,
    olia_WeakInflection,
  }
  public enum olia_hasMood : byte {
    no,
    olia_AdverbialParticiple,
    olia_ImperativeMood,
    olia_IndicativeMood,
    olia_Infinitive,
    olia_Participle,
    olia_QuotativeMood,
    olia_SubjunctiveMood,
  }
  public enum olia_hasVoice : byte {
    no,
    olia_ActiveVoice,
    olia_PassiveVoice,
    olia_ReflexiveVoice,
  }
  public enum rdf_predicate : byte {
    no,
    dbnary_antonym,
    dbnary_approximateSynonym,
    dbnary_holonym,
    dbnary_hypernym,
    dbnary_hyponym,
    dbnary_meronym,
  }
  public enum lexinfo_animacy : byte {
    no,
    olia_Animate,
    olia_Inanimate,
  }
  public enum lexinfo_verbFormMood : byte {
    no,
    lexinfo_conditional,
    lexinfo_imperative,
    lexinfo_indicative,
    lexinfo_infinitive,
    lexinfo_participle,
    lexinfo_perfective,
    lexinfo_subjunctive,
  }

  enum olia_hasTense : byte {
    no,
    olia_Future,
    olia_Past,
    olia_Present,
    olia_FuturePerfect,
    olia_PastPerfectTense,
    olia_Perfect,
  }
  enum lexinfo_tense : byte {
    no,
    lexinfo_future,
    lexinfo_past,
    lexinfo_present,
    lexinfo_imperfect,
  }
  public enum tense : byte {
    no,
    future,
    past,
    present,
    imperfect,
    futurePerfect,
    pastPerfectTense,
    perfect,
  }
  static Dictionary<string, string> tenseDict = new Dictionary<string, string> {
    {"olia:Future","future"},
    {"olia:Past","past"},
    {"olia:Present","present"},
    {"olia:FuturePerfect","futurePerfect"},
    {"olia:PastPerfectTense","pastPerfectTense"},
    {"olia:Perfect","perfect"},
    {"lexinfo:future","future"},
    {"lexinfo:past","past"},
    {"lexinfo:present","present"},
    {"lexinfo:imperfect","imperfect"},
  };

  enum olia_hasGender : byte {
    no,
    olia_Feminine,
    olia_Masculine,
    olia_Neuter,
  }
  enum lexinfo_gender : byte {
    no,
    lexinfo_feminine,
    lexinfo_masculine,
  }
  public enum gender : byte {
    no,
    feminine,
    masculine,
    neuter,
  }
  static Dictionary<string, string> genderDict = new Dictionary<string, string> {
    {"olia:Feminine","feminine"},
    {"olia:Masculine","masculine"},
    {"olia:Neuter","neuter"},
    {"lexinfo:feminine","feminine"},
    {"lexinfo:masculine","masculine"},
  };

  enum olia_hasPerson : byte {
    no,
    olia_First,
    olia_Second,
    olia_Third,
    olia_SecondPolite,
  }
  enum lexinfo_person : byte {
    no,
    lexinfo_firstPerson,
    lexinfo_secondPerson,
    lexinfo_thirdPerson,
  }
  public enum person : byte {
    no,
    first,
    second,
    third,
    secondPolite,
  }
  static Dictionary<string, string> personDict = new Dictionary<string, string> {
    {"olia:First","first"},
    {"olia:Second","second"},
    {"olia:Third","third"},
    {"olia:SecondPolite","secondPolite"},
    {"lexinfo:firstPerson","first"},
    {"lexinfo:secondPerson","second"},
    {"lexinfo:thirdPerson","third"},
  };

  enum olia_hasNumber : byte {
    no,
    olia_Plural,
    olia_Singular,
  }
  enum lexinfo_number : byte {
    no,
    lexinfo_plural,
    lexinfo_singular,
  }
  public enum number : byte {
    no,
    plural,
    singular,
  }
  static Dictionary<string, string> numberDict = new Dictionary<string, string> {
    {"olia:Plural","plural"},
    {"olia:Singular","singular"},
    {"lexinfo:plural","plural"},
    {"lexinfo:singular","singular"},
  };

  public static class ConstMan {

    static ConstMan() {
      foreach (var t in all) {
        var name = t.Name;
        var prop = name.Replace('_', ':');
        var values = Enum.GetNames(t).Select(n => n.Replace('_', ':')).ToArray();
        enumValueMap[prop] = Enum.GetValues(t).Cast<object>().ToDictionary(i => Enum.GetName(t, i).Replace('_', ':'), i => (byte)i);
      }
    }

    public static T enumValue<T>(string propNameUri, string valueUri) where T : Enum {
      if (enumValueTransform.TryGetValue(valueUri, out string v)) valueUri = v;
      if (enumNameTransform.TryGetValue(propNameUri, out string vv)) propNameUri = vv;
      return (T)(object)enumValueMap[propNameUri][valueUri];
    }

    //static Type[] all = new[] { typeof(lexinfo_partOfSpeech), typeof(olia_hasCase), typeof(olia_hasDegree), typeof(olia_hasInflectionType),
    static Type[] all = new[] { typeof(lexinfo_partOfSpeech), typeof(olia_hasCase), typeof(olia_hasDegree), typeof(olia_hasInflectionType),
      typeof(olia_hasMood), typeof(olia_hasVoice), typeof(lexinfo_animacy), typeof(lexinfo_verbFormMood),
      typeof(olia_hasTense), typeof(lexinfo_tense), typeof(olia_hasGender), typeof(lexinfo_gender),
      typeof(olia_hasPerson), typeof(lexinfo_person), typeof(olia_hasNumber), typeof(lexinfo_number), typeof(olia_hasCountability), typeof(rdf_predicate) };

    internal static Dictionary<string, Dictionary<string, byte>> enumValueMap = new Dictionary<string, Dictionary<string, byte>>();
    static Dictionary<string, string> enumValueTransform = numberDict.Concat(personDict).Concat(genderDict).Concat(tenseDict).ToDictionary(kv => kv.Key, kv => kv.Value);
    static Dictionary<string, string> enumNameTransform = new Dictionary<string, string> {
      {"olia_hasTense","tense" },
      {"lexinfo_tense","tense" },
      {"olia_hasGender","gender" },
      {"lexinfo_gender","gender" },
      {"lexinfo_person","person" },
      {"olia_hasPerson","person" },
      {"olia_hasNumber","number" },
      {"lexinfo_number","number" },
    };
  }

}


