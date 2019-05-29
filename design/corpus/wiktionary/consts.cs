using System;
using System.Collections.Generic;
using System.Linq;

public static class WiktConsts {

  public static HashSet<string> IgnoredProps = new HashSet<string> {
    // < 1000
    { "dbnary:troponym" },
    { "dbnary:partOfSpeech" },
    { "terms:language" },
    { "lexinfo:relatedTerm"},
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
    // not for us
    { "terms:bibliographicCitation"}, // 360168x
    { "lexinfo:pronunciation"}, //1516042x
    { "ontolex:phoneticRep" }, // 696696x
    { "lime:language"}, //5117808 x
    // ???
    { "vartrans:lexicalRel"}, // 42570x
    { "lexinfo:root"}, // 16177x in LT, contains lexinfo:radical  lexinfo:root 16177x<words>, ...
  };

  public static HashSet<string> IgnoredClasses = new HashSet<string> {
    {"lexinfo:Adjective"},
    {"lexinfo:Adverb"},
    {"lexinfo:Interjection"},
    {"lexinfo:Noun"},
    {"lexinfo:Particle"},
    {"lexinfo:Prefix"},
    {"lexinfo:Preposition"},
    {"lexinfo:Pronoun"},
    {"lexinfo:ProperNoun"},
    {"lexinfo:Suffix"},
    {"lexinfo:Verb"},
    {"ontolex:Affix"},
    {"ontolex:MultiWordExpression"},
    {"ontolex:Word"},
    {"lexinfo:AbbreviatedForm"},
    {"lexinfo:Adposition"},
    {"lexinfo:Affix"},
    {"lexinfo:Article"},
    {"lexinfo:Conjunction"},
    {"lexinfo:Determiner"},
    {"lexinfo:Infix"},
    {"lexinfo:Number"},
    {"lexinfo:Numeral"},
    {"lexinfo:Postposition"},
    {"lexinfo:Symbol"},
    {"olia:MainVerb"},
    {"olia:ModalVerb"},
    {"lime:Lexicon"},
  };

  public static string[] AllLangs = new string[] { "bg", "en", "fi", "pl", "de", "fr", "ru", "sh", "mg", "el", "nl", "lt", "sv", "es", "pt", "tr", "it", "ja", "id", "la", "no" };
  public static Dictionary<string, byte> AllLangsIdMask = AllLangs.Select((lang, idx) => new { lang, idx }).ToDictionary(li => li.lang, li => (byte)li.idx);

  public static class NodeTypeNames {
    public const string Gloss = "dbnary:Gloss";
    public const string Page = "dbnary:Page";
    public const string Translation = "dbnary:Translation";
    public const string Form = "ontolex:Form";
    public const string Statement = "rdf:Statement";
    public const string LexicalSense = "ontolex:LexicalSense";
    public const string LexicalÈntry = "ontolex:LexicalEntry";
  }

  public static string[] ClassIds = new string[] { NodeTypeNames.Translation, NodeTypeNames.Form, NodeTypeNames.Gloss, NodeTypeNames.LexicalSense, NodeTypeNames.LexicalÈntry, NodeTypeNames.Page, NodeTypeNames.Statement };
  public static Dictionary<string, byte> ClassIdMask = ClassIds.Select((lang, idx) => new { lang, idx }).ToDictionary(li => li.lang, li => (byte)li.idx);
  public static HashSet<string> NodeTypes = ClassIds.ToHashSet();


  public enum predicates {
    no,
    rdf_type, rdf_predicate, rdf_value, rdf_object, rdf_subject,
    dbnary_antonym, dbnary_approximateSynonym, dbnary_describes, dbnary_gloss, dbnary_holonym, dbnary_hypernym, dbnary_hyponym, dbnary_isTranslationOf, dbnary_meronym,
    dbnary_rank, dbnary_senseNumber, dbnary_synonym, dbnary_targetLanguage, dbnary_targetLanguageCode, dbnary_usage, dbnary_writtenForm, lexinfo_animacy,
    lexinfo_pronunciation, lexinfo_verbFormMood, lime_language, olia_hasCase,
    olia_hasCountability, olia_hasDegree, olia_hasInflectionType, olia_hasMood, olia_hasVoice, ontolex_canonicalForm,
    ontolex_otherForm, ontolex_phoneticRep, ontolex_sense, ontolex_writtenRep, skos_definition, skos_example, skos_note, vartrans_lexicalRel,
    lexinfo_partOfSpeech, lexinfo_partOfSpeechEx,
    // LM props
    gender, number, tense, person,
    // as replacement for. Following items must be defined.
    lexinfo_gender, lexinfo_number, lexinfo_tense, lexinfo_person, olia_hasNumber, olia_hasPerson, olia_hasTense, olia_hasGender,
  }

  public enum PredicateType {
    no,
    a,
    Ignore, // prop from Ignored
    ValueProps, UriValuesProps, BlankProps, BlankPropsInner, NymRelProps, NotNymRelProps
  }

  public static bool parsePredicate(string url, out predicates pred, out PredicateType type) { //, out Dictionary<string, byte> urlValues) {
    pred = predicates.no; type = PredicateType.no;
    if (!string2Pred.TryGetValue(url, out pred)) return false;
    type = predicateTypes[pred];
    //if (type == PredicateType.UriValuesProps) urlValues = ConstMan.enumValueMap[url];
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
    { predicates.lexinfo_partOfSpeech, PredicateType.UriValuesProps},
    { predicates.lexinfo_partOfSpeechEx, PredicateType.UriValuesProps},
    { predicates.lexinfo_verbFormMood, PredicateType.UriValuesProps},
    { predicates.olia_hasCase, PredicateType.UriValuesProps},
    { predicates.olia_hasCountability, PredicateType.UriValuesProps},
    { predicates.olia_hasDegree, PredicateType.UriValuesProps},
    { predicates.olia_hasInflectionType, PredicateType.UriValuesProps},
    { predicates.olia_hasMood, PredicateType.UriValuesProps},
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

    { predicates.gender, PredicateType.UriValuesProps},
    { predicates.number, PredicateType.UriValuesProps},
    { predicates.person, PredicateType.UriValuesProps},
    { predicates.tense, PredicateType.UriValuesProps},
    // must me defined
    { predicates.lexinfo_gender, PredicateType.UriValuesProps},
    { predicates.lexinfo_number, PredicateType.UriValuesProps},
    { predicates.lexinfo_person, PredicateType.UriValuesProps},
    { predicates.lexinfo_tense, PredicateType.UriValuesProps},
    { predicates.olia_hasGender, PredicateType.UriValuesProps},
    { predicates.olia_hasNumber, PredicateType.UriValuesProps},
    { predicates.olia_hasPerson, PredicateType.UriValuesProps},
    { predicates.olia_hasTense, PredicateType.UriValuesProps},
  };

  public enum lexinfo_partOfSpeech : byte {
    no,
    lexinfo_adjective, lexinfo_adverb, lexinfo_cardinalNumeral, lexinfo_interjection,
    lexinfo_noun, lexinfo_preposition, lexinfo_pronoun, lexinfo_verb
  }
  public enum lexinfo_partOfSpeechEx : byte {
    no,
    lexinfo_abbreviation, lexinfo_acronym, lexinfo_adposition, lexinfo_Adverb, lexinfo_affix, lexinfo_article, lexinfo_collective, lexinfo_conjunction, lexinfo_demonstrativePronoun, lexinfo_determiner,
    lexinfo_expression, lexinfo_idiom, lexinfo_imperative, lexinfo_indefiniteCardinalNumeral, lexinfo_indefinitePronoun, lexinfo_infix, lexinfo_interrogativePronoun, lexinfo_letter, lexinfo_modal,
    lexinfo_multiplicativeNumeral, lexinfo_numeral, lexinfo_numeralFraction, lexinfo_ordinalAdjective, lexinfo_participle, lexinfo_participleAdjective, lexinfo_particle, lexinfo_pastParticipleAdjective,
    lexinfo_personalPronoun, lexinfo_phraseologicalUnit, lexinfo_possessiveAdjective, lexinfo_postposition, lexinfo_prefix, lexinfo_pronominalAdverb, lexinfo_properNoun, lexinfo_proverb, lexinfo_suffix,
    lexinfo_symbol, lexinfo_adverbialPronoun, lexinfo_reciprocalPronoun, lexinfo_exclamativePronoun, lexinfo_possessivePronoun, lexinfo_reflexivePersonalPronoun,
    lexinfo_relativePronoun, lexinfo_indefiniteOrdinalNumeral, lexinfo_circumposition, lexinfo_interrogativeCardinalNumeral, lexinfo_radical,
    lexinfo_number, lexinfo_contraction, lexinfo_baseElement,
  }
  public static HashSet<string> partOfSpeechDir = Enum.GetNames(typeof(lexinfo_partOfSpeech)).Cast<string>().Select(s => s.Replace('_', ':')).ToHashSet();

  public enum olia_hasCase : byte {
    no = 0,
    olia_Accusative = 0x1,
    olia_DativeCase = 0x2,
    olia_GenitiveCase = 0x4,
    olia_InstrumentalCase = 0x8,
    olia_LocativeCase = 0x10,
    olia_Nominative = 0x20,
    olia_VocativeCase = 0x40,
  }
  public enum olia_hasCountability : byte {
    no = 0,
    olia_Countable = 0x1,
    olia_Uncountable = 0x2,
  }
  public enum olia_hasDegree : byte {
    no = 0,
    olia_Comparative = 0x1,
    olia_Positive = 0x2,
    olia_Superlative = 0x4,
  }
  public enum olia_hasInflectionType : byte {
    no = 0,
    olia_MixedInflection = 0x1,
    olia_StrongInflection = 0x2,
    olia_WeakInflection = 0x4,
    olia_Uninflected = 0x8,
  }
  public enum olia_hasMood : byte {
    no = 0,
    olia_AdverbialParticiple = 0x1,
    olia_ImperativeMood = 0x2,
    olia_IndicativeMood = 0x4,
    olia_Infinitive = 0x8,
    olia_Participle = 0x10,
    olia_QuotativeMood = 0x20,
    olia_SubjunctiveMood = 0x40,
  }
  public enum olia_hasVoice : byte {
    no = 0,
    olia_ActiveVoice = 0x1,
    olia_PassiveVoice = 0x2,
    olia_ReflexiveVoice = 0x4,
  }
  public enum rdf_predicate : byte {
    no = 0,
    dbnary_antonym = 0x1,
    dbnary_approximateSynonym = 0x2,
    dbnary_holonym = 0x4,
    dbnary_hypernym = 0x8,
    dbnary_hyponym = 0x10,
    dbnary_meronym = 0x20,
    dbnary_troponym = 0x40,
    dbnary_synonym = 0x80,
  }
  public enum lexinfo_animacy : byte {
    no = 0,
    olia_Animate = 0x1,
    olia_Inanimate = 0x2,
  }
  public enum lexinfo_verbFormMood : byte {
    no = 0,
    lexinfo_conditional = 0x1,
    lexinfo_imperative = 0x2,
    lexinfo_indicative = 0x4,
    lexinfo_infinitive = 0x8,
    lexinfo_participle = 0x10,
    lexinfo_perfective = 0x20,
    lexinfo_subjunctive = 0x40,
  }

  enum olia_hasTense : byte {
    no = 0,
    olia_Future = 0x1,
    olia_Past = 0x2,
    olia_Present = 0x4,
    olia_FuturePerfect = 0x8,
    olia_PastPerfectTense = 0x10,
    olia_Perfect = 0x20,
  }
  enum lexinfo_tense : byte {
    no = 0,
    lexinfo_future = 0x1,
    lexinfo_past = 0x2,
    lexinfo_present = 0x4,
    lexinfo_imperfect = 0x8,
  }
  public enum tense : byte {
    no = 0,
    future = 0x1,
    past = 0x2,
    present = 0x4,
    imperfect = 0x8,
    futurePerfect = 0x10,
    pastPerfectTense = 0x20,
    perfect = 0x40,
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
    lexinfo_neuter
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
    {"lexinfo:neuter","neuter"},

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

    public static byte enumValue(string propNameUri, string valueUri) {
      if (enumValueTransform.TryGetValue(valueUri, out string v)) valueUri = v;
      if (enumNameTransform.TryGetValue(propNameUri, out string vv)) propNameUri = vv;
      var res = !enumValueMap.TryGetValue(propNameUri, out Dictionary<string, byte> vals) ? 0 : (!vals.TryGetValue(valueUri, out byte val) ? 0 : val);
      return (byte)res;
    }

    public static T enumValue<T>(string valueUri) where T : Enum {
      var name = typeof(T).Name.Replace('_', ':');
      return (T)(object)enumValue(name, valueUri);
    }

    static Type[] all = new[] {
      // Spec
      typeof(lexinfo_partOfSpeech), typeof(rdf_predicate), typeof(lexinfo_partOfSpeechEx),

      // Form infos 
      typeof(olia_hasCase), typeof(olia_hasDegree), typeof(olia_hasInflectionType), typeof(olia_hasCountability),
      typeof(olia_hasMood), typeof(olia_hasVoice), typeof(lexinfo_animacy), typeof(lexinfo_verbFormMood), 
      // LM enums...
      typeof(number),typeof(person),typeof(gender),typeof(tense),
      // as replacement of
      typeof(olia_hasNumber), typeof(lexinfo_number), typeof(olia_hasTense), typeof(lexinfo_tense), typeof(olia_hasGender), typeof(lexinfo_gender), typeof(olia_hasPerson), typeof(lexinfo_person),
    };

    internal static Dictionary<string, Dictionary<string, byte>> enumValueMap = new Dictionary<string, Dictionary<string, byte>>();
    static Dictionary<string, string> enumValueTransform = numberDict.Concat(personDict).Concat(genderDict).Concat(tenseDict).ToDictionary(kv => kv.Key, kv => kv.Value);
    static Dictionary<string, string> enumNameTransform = new Dictionary<string, string> {
      {"olia:hasTense","tense" },
      {"lexinfo:tense","tense" },
      {"olia:hasGender","gender" },
      {"lexinfo:gender","gender" },
      {"lexinfo:person","person" },
      {"olia:hasPerson","person" },
      {"olia:hasNumber","number" },
      {"lexinfo:number","number" },
    };
  }

  public static Dictionary<string, string> Namespaces = new Dictionary<string, string> {
    {"dbnary", "http://kaiko.getalp.org/dbnary#"},
    {"fn", "http://www.w3.org/2005/xpath-functions#"},
    {"lexinfo", "http://www.lexinfo.net/ontology/2.0/lexinfo#"},
    {"lime", "http://www.w3.org/ns/lemon/lime#"},
    {"olia", "http://purl.org/olia/olia.owl#"},
    {"ontolex", "http://www.w3.org/ns/lemon/ontolex#"},
    {"owl", "http://www.w3.org/2002/07/owl#"},
    {"prot", "http://proton.semanticweb.org/protonsys#"},
    {"rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#"},
    {"rdfs", "http://www.w3.org/2000/01/rdf-schema#"},
    {"sesame", "http://www.openrdf.org/schema/sesame#"},
    {"skos", "http://www.w3.org/2004/02/skos/core#"},
    {"vartrans", "http://www.w3.org/ns/lemon/vartrans#"},
    {"xsd", "http://www.w3.org/2001/XMLSchema#"},

    {"lexvo", "http://lexvo.org/id/iso639-3/"},
    {"terms", "http://purl.org/dc/terms/"},
  };

}


