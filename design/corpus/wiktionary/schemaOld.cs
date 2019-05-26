using System;
using System.Collections.Generic;
using System.Linq;

public static class WiktSchemaOld {

  static Dictionary<string, string> genCode(string name, Dictionary<string, byte> data) =>
    data.Where(kv => !WiktConsts.IgnoredProps.Contains(kv.Key)).
    ToDictionary(kv => name + "š" + kv.Key, kv => $"{{ predicates.{kv.Key.Replace(':', '_')}, PredicateType.{name}}},");

  public static void test() {
    var m1 = WiktConsts.predicateTypes.Keys; var m2 = Enum.GetValues(typeof(WiktConsts.predicates)).Cast<WiktConsts.predicates>();
    var d1 = m1.Except(m2).ToArray();
    var d2 = m2.Except(m1).ToArray();

    var mm1 = WiktConsts.ConstMan.enumValueMap.Select(kv => kv.Key.Replace(':', '_')).ToArray();
    var mm2 = WiktConsts.predicateTypes.Where(pt => pt.Value == WiktConsts.PredicateType.UriValuesProps).Select(kv => kv.Key.ToString()).ToArray();
    var dd1 = mm1.Except(mm2).ToArray();
    var dd2 = mm2.Except(mm1).ToArray();

    d2 = null;
  }

  public static void run() {
    var dicts = new[] { ValueProps, UriValuesProps, BlankProps, BlankPropsInner, NymRelProps, NotNymRelProps };
    var all = dicts.SelectMany(d => d.Select(kv => kv.Key)).Except(WiktConsts.IgnoredProps).Select(s => s.Replace(':', '_')).ToHashSet();
    var inEnum = Enum.GetNames(typeof(WiktConsts.predicates)).Cast<string>().ToArray();
    var d1 = inEnum.Except(all).ToArray();
    var d2 = string.Join(",", all.Except(inEnum));
    inEnum = null;
    var kvsDict = new[] {
      genCode("UriValuesProps", UriValuesProps),
      genCode("BlankProps", BlankProps),
      genCode("BlankPropsInner", BlankPropsInner),
      genCode("NymRelProps", NymRelProps),
      genCode("NotNymRelProps", NotNymRelProps),
      genCode("ValueProps", ValueProps),
    };
    var code = string.Join("\r\n", kvsDict.SelectMany(d => d).OrderBy(kv => kv.Key).Select(kv => kv.Value));
    code = null;
  }

  static Dictionary<string, byte> ValueProps = new Dictionary<string, byte> {
    //{"dbnary:partOfSpeech", 150},
    {"dbnary:rank", 151},
    {"dbnary:senseNumber", 152},
    {"dbnary:targetLanguageCode", 153},
    {"dbnary:usage", 154},
    {"dbnary:writtenForm", 155},
    {"lexinfo:abbreviationFor", 156},
    {"lexinfo:pronunciation", 157},
    {"lime:language", 158},
    {"ontolex:phoneticRep", 159},
    {"ontolex:writtenRep", 160},
    {"skos:note", 161}
  };

  static Dictionary<string, byte> UriValuesProps = new Dictionary<string, byte> {
    {"dbnary:partOfSpeech", 181},
    {"dbnary:targetLanguage", 182},
    {"lexinfo:animacy", 183},
    {"lexinfo:gender", 184},
    {"lexinfo:number", 185},
    {"lexinfo:partOfSpeech", 186},
    {"lexinfo:person", 187},
    {"lexinfo:tense", 188},
    {"lexinfo:verbFormMood", 189},
    {"olia:hasCase", 190},
    {"olia:hasCountability", 191},
    {"olia:hasDefiniteness", 192},
    {"olia:hasDegree", 193},
    {"olia:hasGender", 194},
    {"olia:hasInflectionType", 195},
    {"olia:hasMood", 194},
    {"olia:hasNumber", 197},
    {"olia:hasPerson", 198},
    {"olia:hasSeparability", 199},
    {"olia:hasTense", 200},
    {"olia:hasValency", 201},
    {"olia:hasVoice", 202},
    {"terms:language", 203},
    {"rdf:predicate", 204},
    {"rdf:type", 204},
  };

  static Dictionary<string, byte> BlankProps = new Dictionary<string, byte> {
    {"skos:definition", 170},
    {"skos:example", 171},
    {"vartrans:lexicalRel", 172}
  };

  static Dictionary<string, byte> BlankPropsInner = new Dictionary<string, byte> {
    {"rdf:value", 175},
  };

  static Dictionary<string, byte> NymRelProps = new Dictionary<string, byte> {
    {"dbnary:antonym", 50},
    {"dbnary:approximateSynonym", 51},
    {"dbnary:holonym", 52},
    {"dbnary:hypernym", 53},
    {"dbnary:hyponym", 54},
    {"dbnary:meronym", 55},
    {"dbnary:synonym", 56},
    {"dbnary:troponym", 57},
  };

  static Dictionary<string, byte> NotNymRelProps = new Dictionary<string, byte> {
    {"dbnary:describes", 70},
    {"dbnary:gloss", 71},
    {"dbnary:isTranslationOf", 72},
    {"ontolex:canonicalForm", 73},
    {"ontolex:otherForm", 74},
    {"ontolex:sense", 75},
    {"rdf:object", 76},
    {"rdf:subject", 77},
  };

  const string missing =
@"letter
idiom
abbreviation
proverb
imperative
prefix
conjunction
suffix
adposition
properNoun
expression
affix
proverb
article
conjunction
postposition
particle
properNoun
acronym
proverb
particle
possessiveAdjective
idiom
prefix
interrogativePronoun
conjunction
abbreviation
suffix
indefinitePronoun
modal
numeralFraction
personalPronoun
numeral
indefiniteCardinalNumeral
symbol
participleAdjective
collective
ordinalAdjective
multiplicativeNumeral
pastParticipleAdjective
properNoun
properNoun
suffix
phraseologicalUnit
proverb
prefix
determiner
numeral
particle
conjunction
symbol
infix
postposition
article
affix
idiom
properNoun
participle
properNoun
abbreviation
indefinitePronoun
suffix
pronominalAdverb
numeral
prefix
letter
particle
postposition
Adverb
demonstrativePronoun
article
affix";

}


