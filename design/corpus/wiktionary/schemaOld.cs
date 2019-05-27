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


//public static void dumps() {

//  var root = Corpus.Dirs.wiktDbnary + @"graphDBExport\scheme\dump-";

//  bool isClass(string url) { var parts = url.Split(':'); return parts.Length == 2 && char.IsUpper(parts[1][0]); }

//  void writeList(string fn, IEnumerable<string> l) => File.WriteAllText(
//    root + fn + ".txt",
//    string.Join(",\r\n", l.Distinct().OrderBy(s => s).Select(s => s)));
//  //string.Join(",\r\n", l.Distinct().OrderBy(s => s).Select(s => $"\"{s}\"")));
//  //string.Join(",\r\n", l.Distinct().OrderBy(s => s).Select(s => $"{{\"{s}\", 1}}")));

//  var rawTrs = parse().ToArray();
//  File.WriteAllLines(root + "value-objs.txt", valueObjects(rawTrs));
//  var trs = filterUriValueObjects(rawTrs).ToArray();

//  var classes = trs.SelectMany(t => Linq.Items(t.s, t.o)).Where(s => isClass(s)).Distinct().ToHashSet();

//  var valueTypes = Linq.Items("rdf:langString", "xsd:int", "xsd:string").ToHashSet();
//  var blank = "\"blank\"";
//  var pNyms = Linq.Items("dbnary:antonym", "dbnary:approximateSynonym", "dbnary:holonym", "dbnary:hypernym", "dbnary:hyponym", "dbnary:meronym", "dbnary:synonym", "dbnary:troponym").ToHashSet();
//  var cNotNyms = Linq.Items("dbnary:Translation", "dbnary:Gloss", "ontolex:Form").ToHashSet();
//  var cNotNymsOnly = Linq.Items("dbnary:Page", "ontolex:LexicalSense", "rdf:Statement").Concat(cNotNyms).ToHashSet();

//  writeList("t-all", trs.Select(t => $"{t.s} - {t.p} - {t.o}"));
//  writeList("so-all", trs.SelectMany(t => Linq.Items(t.s, t.o)));
//  writeList("c-all", classes.OrderBy(s => s));
//  writeList("p-all", trs.Select(t => t.p));
//  writeList("s-p-all", trs.Select(t => $"{t.s} - {t.p}"));
//  writeList("s-p-o-all", trs.Select(t => $"{t.s} - {t.p} - {t.o}"));
//  writeList("o-p-s-all", trs.Select(t => $"{t.o} - {t.p} - {t.s}"));
//  writeList("p-s-o-all", trs.Select(t => $"{t.p} - {t.s} - {t.o}"));

//  var vals = trs.Where(t => valueTypes.Contains(t.o)).ToArray();
//  writeList("s-p-value", vals.Select(t => $"{t.s} - {t.p}"));
//  writeList("p-s-value", vals.Select(t => $"{t.p} - {t.s}"));
//  writeList("p-value", vals.Select(t => $"{t.p}"));

//  var rels = trs.Where(t => classes.Contains(t.s) && classes.Contains(t.o)).ToArray();
//  writeList("p-o-class", rels.Select(t => $"{t.p} - {t.o}"));
//  writeList("o-p-class", rels.Select(t => $"{t.o} - {t.p}"));
//  writeList("o-p-s-class", rels.Select(t => $"{t.o} - {t.p} - {t.s}"));
//  writeList("p-class", rels.Select(t => t.p));
//  writeList("s-p-class", rels.Select(t => $"{t.s} - {t.p}"));
//  writeList("s-p-o-class", rels.Select(t => $"{t.s} - {t.p} - {t.o}"));
//  writeList("p-s-class", rels.Select(t => $"{t.p} - {t.s}"));

//  var blanks = trs.Where(t => t.o == blank).ToArray();
//  writeList("s-p-blank", blanks.Select(t => $"{t.s} - {t.p}"));
//  writeList("s-blank", blanks.Select(t => $"{t.s}"));
//  writeList("p-blank", blanks.Select(t => $"{t.p}"));

//  var nyms = trs.Where(t => pNyms.Contains(t.p)).ToArray();
//  writeList("s-nyms", nyms.Where(t => !cNotNyms.Contains(t.s)).Select(t => t.s));
//  writeList("o-nyms", nyms.Where(t => !cNotNyms.Contains(t.o)).Select(t => t.o));
//  writeList("s-nyms-o", nyms.Where(t => !cNotNyms.Contains(t.o)).Select(t => $"{t.s} => {t.o}"));

//  var uriObj = trs.Where(t => t.o[0] == '@').ToArray();
//  writeList("s-p-uriobj", uriObj.Select(t => $"{t.s} - {t.p}"));
//  writeList("s-uriobj", uriObj.Select(t => $"{t.s}"));
//  writeList("p-uriobj", uriObj.Select(t => $"{t.p}"));
//  writeList("o-uriobj", uriObj.Select(t => $"{t.o}"));

//  //*** all values
//  var valueProps = trs.Where(t => t.o[0] == '@' || t.o == blank || ValueProps.ContainsKey(t.p)).ToArray();
//  writeList("s-p-o-vals-all", valueProps.Select(t => $"{t.s} - {t.p} - {t.o}"));
//  writeList("s-p-vals-all", valueProps.Select(t => $"{t.s} - {t.p}"));

//  //*** NYMS
//  writeList("c-nymsOnly", classes.Where(c => !cNotNymsOnly.Contains(c)));
//  writeList("c-notNymsOnly", classes.Where(c => cNotNymsOnly.Contains(c)));

//  // join NYMs
//  //var joinNyms = trs.Select(t => {
//  //  string toNym(string s) => cNotNymsOnly.Contains(s) || !classes.Contains(s) ? s : "l:Nym";
//  //  var res = new triple { s = toNym(t.s), p = t.p, o = toNym(t.o) };
//  //  return res.s == t.s && res.o == t.o ? null : res;
//  //}).Where(t => t != null).ToArray();

//  //writeList("s-p-o-nyms", joinNyms.Select(t => $"{t.s} - {t.p} - {t.o}"));
//  //writeList("o-p-s-nyms", joinNyms.Select(t => $"{t.o} - {t.p} - {t.s}"));
//  //writeList("p-s-o-nyms", joinNyms.Select(t => $"{t.p} - {t.s} - {t.o}"));

//  //var valuePropsNyms = joinNyms.Where(t => t.o[0] == '@' || t.o == blank || ValueProps.ContainsKey(t.p)).ToArray();
//  //writeList("s-p-o-vals-nyms", valuePropsNyms.Select(t => $"{t.s} - {t.p} - {t.o}"));
//  //writeList("s-p-vals-nyms", valuePropsNyms.Select(t => $"{t.s} - {t.p}"));

//}

//public class PredicateInfo {
//  static PredicateInfo() {
//    void fill(Dictionary<string, byte> data, WiktConsts.PredicateType type) {
//      foreach (var kv in data) {
//        infos[kv.Value] = new PredicateInfo { predicateUri = kv.Key, type = type };
//      }
//    }
//    fill(ValueProps, WiktConsts.PredicateType.ValueProps); fill(BlankProps, WiktConsts.PredicateType.ValueProps); fill(BlankPropsInner, WiktConsts.PredicateType.ValueProps);
//    fill(NymRelProps, WiktConsts.PredicateType.NymRelProps); fill(NotNymRelProps, WiktConsts.PredicateType.NotNymRelProps);
//    fill(null /*TODO*/, WiktConsts.PredicateType.UriValuesProps);

//    //fill(BlankProps, "BlankProps"); fill(BlankPropsInner, "BlankPropsInner"); fill(UriValuesProps, ValueType.UriValuesProps);
//    uriToInfo = infos.Where(u => u!=null).ToDictionary(u => u.predicateUri); 
//  }
//  public static PredicateInfo[] infos = Enumerable.Range(0, 255).Select(i => (PredicateInfo)null).ToArray();
//  public static Dictionary<string, PredicateInfo> uriToInfo;
//  public string predicateUri; // e.g. dbnary:partOfSpeech 
//  public WiktConsts.PredicateType type; // e.g ValueProps

//}

////static byte cNymClasses = 1;
////public static Dictionary<string, byte> Types = new Dictionary<string, byte> {
////  {"lexinfo:Adjective",cNymClasses++},
////  {"lexinfo:Adverb",cNymClasses++},
////  {"lexinfo:Interjection",cNymClasses++},
////  {"lexinfo:Noun",cNymClasses++},
////  {"lexinfo:Particle",cNymClasses++},
////  {"lexinfo:Prefix",cNymClasses++},
////  {"lexinfo:Preposition",cNymClasses++},
////  {"lexinfo:Pronoun",cNymClasses++},
////  {"lexinfo:ProperNoun",cNymClasses++},
////  {"lexinfo:Suffix",cNymClasses++},
////  {"lexinfo:Verb",cNymClasses++},
////  {"ontolex:Affix",cNymClasses++},
////  {"ontolex:LexicalEntry",cNymClasses++},
////  {"ontolex:MultiWordExpression",cNymClasses++},
////  {"ontolex:Word",cNymClasses++},
////  //{"lexinfo:AbbreviatedForm",cNymClasses++},
////  //{"lexinfo:Adposition",cNymClasses++},
////  //{"lexinfo:Affix",cNymClasses++},
////  //{"lexinfo:Article",cNymClasses++},
////  //{"lexinfo:Conjunction",cNymClasses++},
////  //{"lexinfo:Determiner",cNymClasses++},
////  //{"lexinfo:Infix",cNymClasses++},
////  //{"lexinfo:Number",cNymClasses++},
////  //{"lexinfo:Numeral",cNymClasses++},
////  //{"lexinfo:Postposition",cNymClasses++},
////  //{"lexinfo:Symbol",cNymClasses++},
////  //{"olia:MainVerb",cNymClasses++},
////  //{"olia:ModalVerb",cNymClasses++},
////};
//// lexinfo:Adjective lexinfo:Adverb lexinfo:Interjection lexinfo:Noun lexinfo:Particle lexinfo:Prefix lexinfo:Preposition lexinfo:Pronoun lexinfo:ProperNoun lexinfo:Suffix lexinfo:Verb ontolex:Affix ontolex:LexicalEntry ontolex:MultiWordExpression ontolex:Word 

////public static Dictionary<string, byte> NymClasses = new Dictionary<string, byte> {
////  {"ontolex:LexicalEntry",1},
////};

////public static Dictionary<string, byte> NotNymClasses = new Dictionary<string, byte> {
////  {"dbnary:Gloss", NodeTypes.Gloss},
////  {"dbnary:Page", NodeTypes.Page},
////  {"dbnary:Translation", NodeTypes.Translation},
////  {"ontolex:Form", NodeTypes.Form},
////  {"ontolex:LexicalSense", NodeTypes.LexicalSense },
////  {"rdf:Statement", NodeTypes.Statement },
////};
//// dbnary:Gloss dbnary:Page dbnary:Translation ontolex:Form ontolex:LexicalSense rdf:Statement
//public static HashSet<string> isNodeTypes = new string[] { NodeTypes.Gloss, NodeTypes.Form, NodeTypes.LexicalSense,
//  NodeTypes.LexicalÈntry, NodeTypes.Page, NodeTypes.Statement, NodeTypes.Translation }.ToHashSet();

//public static class NodeTypes {
//  public const string Gloss = "dbnary:Gloss";
//  public const string Page = "dbnary:Page";
//  public const string Translation = "dbnary:Translation";
//  public const string Form = "ontolex:Form";
//  public const string Statement = "rdf:Statement";
//  public const string LexicalSense = "ontolex:LexicalSense";
//  public const string LexicalÈntry = "ontolex:LexicalEntry";
//}
//public const int NodeTypesLen = 6;

//public static Dictionary<string, byte> ValueProps = new Dictionary<string, byte> {
//  //{"dbnary:partOfSpeech", 150},
//  {"dbnary:rank", 151},
//  {"dbnary:senseNumber", 152},
//  {"dbnary:targetLanguageCode", 153},
//  {"dbnary:usage", 154},
//  {"dbnary:writtenForm", 155},
//  {"lexinfo:abbreviationFor", 156},
//  {"lexinfo:pronunciation", 157},
//  {"lime:language", 158},
//  {"ontolex:phoneticRep", 159},
//  {"ontolex:writtenRep", 160},
//  {"skos:note", 161}
//};

////public static Dictionary<string, Dictionary<string, byte>> UriValuesProps = WiktConsts.ConstMan.enumValueMap;
////new Dictionary<string, byte> {
////  // {"dbnary:partOfSpeech", 181}, replaced by lexinfo:partOfSpeech
////  {"dbnary:targetLanguage", 182},
////  {"lexinfo:animacy", 183},
////  {"lexinfo:gender", 184},
////  {"lexinfo:number", 185},
////  {"lexinfo:partOfSpeech", 186},
////  {"lexinfo:person", 187},
////  {"lexinfo:tense", 188},
////  {"lexinfo:verbFormMood", 189},
////  {"olia:hasCase", 190},
////  {"olia:hasCountability", 191},
////  {"olia:hasDefiniteness", 192},
////  {"olia:hasDegree", 193},
////  {"olia:hasGender", 194},
////  //{"olia:hasInflectionType", 195},
////  {"olia:hasMood", 194},
////  {"olia:hasNumber", 197},
////  {"olia:hasPerson", 198},
////  //{"olia:hasSeparability", 199},
////  {"olia:hasTense", 200},
////  //{"olia:hasValency", 201},
////  //{"olia:hasVoice", 202},
////  {"terms:language", 203},
////  {"rdf:predicate", 204},
////};

//public static Dictionary<string, byte> BlankProps = new Dictionary<string, byte> {
//  {"skos:definition", 170},
//  {"skos:example", 171},
//  {"vartrans:lexicalRel", 172}
//};

//public static Dictionary<string, byte> BlankPropsInner = new Dictionary<string, byte> {
//  {"rdf:value", 175},
//};

//public static Dictionary<string, byte> NymRelProps = new Dictionary<string, byte> {
//  {"dbnary:antonym", 50},
//  {"dbnary:hypernym", 53},
//  {"dbnary:hyponym", 54},
//  {"dbnary:synonym", 56},
//  //{"dbnary:troponym", 57},
//  //{"dbnary:approximateSynonym", 51},
//  //{"dbnary:holonym", 52},
//  //{"dbnary:meronym", 55},
//};
////dbnary:antonym dbnary:hypernym dbnary:hyponym dbnary:synonym 
////dbnary:troponym dbnary:meronym dbnary:approximateSynonym dbnary:holonym



//public static Dictionary<string, byte> NotNymRelProps = new Dictionary<string, byte> {
//  {"dbnary:describes", 70},
//  {"dbnary:gloss", 71},
//  {"dbnary:isTranslationOf", 72},
//  {"ontolex:canonicalForm", 73},
//  {"ontolex:otherForm", 74},
//  {"ontolex:sense", 75},
//  {"rdf:object", 76},
//  {"rdf:subject", 77},
//};

//static byte cUriValues = 1;
//public static Dictionary<string, byte> UriValues = new Dictionary<string, byte> {
//  {"lexinfo:abbreviation",cUriValues++},
//  {"lexinfo:acronym",cUriValues++},
//  {"lexinfo:adjective",cUriValues++},
//  {"lexinfo:adposition",cUriValues++},
//  {"lexinfo:adverb",cUriValues++},
//  {"lexinfo:adverbialPronoun",cUriValues++},
//  {"lexinfo:affix",cUriValues++},
//  {"lexinfo:article",cUriValues++},
//  {"lexinfo:baseElement",cUriValues++},
//  {"lexinfo:cardinalNumeral",cUriValues++},
//  {"lexinfo:circumposition",cUriValues++},
//  {"lexinfo:collective",cUriValues++},
//  {"lexinfo:conditional",cUriValues++},
//  {"lexinfo:conjunction",cUriValues++},
//  {"lexinfo:contraction",cUriValues++},
//  {"lexinfo:definite",cUriValues++},
//  {"lexinfo:demonstrativePronoun",cUriValues++},
//  {"lexinfo:determiner",cUriValues++},
//  {"lexinfo:exclamativePronoun",cUriValues++},
//  {"lexinfo:expression",cUriValues++},
//  {"lexinfo:feminine",cUriValues++},
//  {"lexinfo:firstPerson",cUriValues++},
//  {"lexinfo:future",cUriValues++},
//  {"lexinfo:idiom",cUriValues++},
//  {"lexinfo:imperative",cUriValues++},
//  {"lexinfo:imperfect",cUriValues++},
//  {"lexinfo:indefinite",cUriValues++},
//  {"lexinfo:indefiniteCardinalNumeral",cUriValues++},
//  {"lexinfo:indefiniteOrdinalNumeral",cUriValues++},
//  {"lexinfo:indefinitePronoun",cUriValues++},
//  {"lexinfo:indicative",cUriValues++},
//  {"lexinfo:infinitive",cUriValues++},
//  {"lexinfo:infix",cUriValues++},
//  {"lexinfo:interjection",cUriValues++},
//  {"lexinfo:interrogativeCardinalNumeral",cUriValues++},
//  {"lexinfo:interrogativePronoun",cUriValues++},
//  {"lexinfo:letter",cUriValues++},
//  {"lexinfo:masculine",cUriValues++},
//  {"lexinfo:modal",cUriValues++},
//  {"lexinfo:multiplicativeNumeral",cUriValues++},
//  {"lexinfo:neuter",cUriValues++},
//  {"lexinfo:noun",cUriValues++},
//  {"lexinfo:number",cUriValues++},
//  {"lexinfo:numeral",cUriValues++},
//  {"lexinfo:numeralFraction",cUriValues++},
//  {"lexinfo:ordinalAdjective",cUriValues++},
//  {"lexinfo:participle",cUriValues++},
//  {"lexinfo:participleAdjective",cUriValues++},
//  {"lexinfo:particle",cUriValues++},
//  {"lexinfo:past",cUriValues++},
//  {"lexinfo:pastParticipleAdjective",cUriValues++},
//  {"lexinfo:perfective",cUriValues++},
//  {"lexinfo:personalPronoun",cUriValues++},
//  {"lexinfo:phraseologicalUnit",cUriValues++},
//  {"lexinfo:plural",cUriValues++},
//  {"lexinfo:possessiveAdjective",cUriValues++},
//  {"lexinfo:possessivePronoun",cUriValues++},
//  {"lexinfo:postposition",cUriValues++},
//  {"lexinfo:prefix",cUriValues++},
//  {"lexinfo:preposition",cUriValues++},
//  {"lexinfo:present",cUriValues++},
//  {"lexinfo:pronominalAdverb",cUriValues++},
//  {"lexinfo:pronoun",cUriValues++},
//  {"lexinfo:properNoun",cUriValues++},
//  {"lexinfo:proverb",cUriValues++},
//  {"lexinfo:radical",cUriValues++},
//  {"lexinfo:reciprocalPronoun",cUriValues++},
//  {"lexinfo:reflexivePersonalPronoun",cUriValues++},
//  {"lexinfo:relativePronoun",cUriValues++},
//  {"lexinfo:secondPerson",cUriValues++},
//  {"lexinfo:singular",cUriValues++},
//  {"lexinfo:subjunctive",cUriValues++},
//  {"lexinfo:suffix",cUriValues++},
//  {"lexinfo:symbol",cUriValues++},
//  {"lexinfo:thirdPerson",cUriValues++},
//  {"lexinfo:verb",cUriValues++},
//  {"olia:Accusative",cUriValues++},
//  {"olia:ActiveVoice",cUriValues++},
//  {"olia:AdverbialParticiple",cUriValues++},
//  {"olia:Animate",cUriValues++},
//  {"olia:Comparative",cUriValues++},
//  {"olia:Countable",cUriValues++},
//  {"olia:DativeCase",cUriValues++},
//  {"olia:Feminine",cUriValues++},
//  {"olia:First",cUriValues++},
//  {"olia:Future",cUriValues++},
//  {"olia:FuturePerfect",cUriValues++},
//  {"olia:GenitiveCase",cUriValues++},
//  {"olia:ImperativeMood",cUriValues++},
//  {"olia:Inanimate",cUriValues++},
//  {"olia:IndicativeMood",cUriValues++},
//  {"olia:Infinitive",cUriValues++},
//  {"olia:InstrumentalCase",cUriValues++},
//  {"olia:Intransitive",cUriValues++},
//  {"olia:LocativeCase",cUriValues++},
//  {"olia:Masculine",cUriValues++},
//  {"olia:MixedInflection",cUriValues++},
//  {"olia:Neuter",cUriValues++},
//  {"olia:Nominative",cUriValues++},
//  {"olia:NonSeparable",cUriValues++},
//  {"olia:Participle",cUriValues++},
//  {"olia:PassiveVoice",cUriValues++},
//  {"olia:Past",cUriValues++},
//  {"olia:PastPerfectTense",cUriValues++},
//  {"olia:Perfect",cUriValues++},
//  {"olia:Plural",cUriValues++},
//  {"olia:Positive",cUriValues++},
//  {"olia:Present",cUriValues++},
//  {"olia:QuotativeMood",cUriValues++},
//  {"olia:ReflexiveVoice",cUriValues++},
//  {"olia:Second",cUriValues++},
//  {"olia:SecondPolite",cUriValues++},
//  {"olia:Separable",cUriValues++},
//  {"olia:Singular",cUriValues++},
//  {"olia:StrongInflection",cUriValues++},
//  {"olia:SubjunctiveMood",cUriValues++},
//  {"olia:Superlative",cUriValues++},
//  {"olia:Third",cUriValues++},
//  {"olia:Transitive",cUriValues++},
//  {"olia:Uncountable",cUriValues++},
//  {"olia:Uninflected",cUriValues++},
//  {"olia:VocativeCase",cUriValues++},
//  {"olia:WeakInflection",cUriValues++},
//  // by hand, from english and german: SELECT DISTINCT ?o WHERE { ?s rdf:predicate ?o . ?s a rdf:Statement . }
//  {"dbnary:synonym", cUriValues++},
//  {"dbnary:hyponym",cUriValues++},
//  {"dbnary:antonym",cUriValues++},
//  {"dbnary:meronym",cUriValues++},
//  {"dbnary:hypernym",cUriValues++},
//  {"dbnary:holonym",cUriValues++},
//  {"dbnary:troponym",cUriValues++},
//};

//public static Dictionary<string, byte> ValidPartOfspeach =
//  new[] { "lexinfo:adjective", "lexinfo:adverb", "lexinfo:cardinalNumeral", "lexinfo:interjection",
//    "lexinfo:noun", "lexinfo:preposition", "lexinfo:pronoun", "lexinfo:verb" }.
//  ToDictionary(k => k, k => UriValues[k]);
//// lexinfo:adjective lexinfo:adverb lexinfo:cardinalNumeral lexinfo:interjection lexinfo:noun lexinfo:preposition lexinfo:pronoun lexinfo:verb 
//// lexinfo:adjective, lexinfo:adverb, lexinfo:cardinalNumeral, lexinfo:interjection, lexinfo:noun, lexinfo:preposition, lexinfo:pronoun, lexinfo:verb 

/*******************************************************
 * PRIVATE
 ******************************************************/
//static string[] types = new[] { "rdf:langString", "xsd:int", "xsd:string" };

//static IEnumerable<triple> filterUriValueObjects(IEnumerable<triple> src) {
//  var subjs = src.Select(s => s.s).ToHashSet();
//  return src.Select(s => {
//    if (!types.Contains(s.o) && !s.o.StartsWith("\"") && !subjs.Contains(s.o)) s.o = "@" + s.o.Split(':')[0];
//    return s;
//  });
//}
//static IEnumerable<string> valueObjects(IEnumerable<triple> src) {
//  var subjs = src.Select(s => s.s).ToHashSet();
//  return src.Select(s => s.o).Where(s => !types.Contains(s) && !s.StartsWith("\"") && !subjs.Contains(s)).Distinct().OrderBy(s => s);
//}

//static IEnumerable<triple> parse() {
//  var txt = File.ReadAllText(Directory.GetCurrentDirectory() + @"\wiktionary\schema.txt");
//  var subjs = txt.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim(' ', '\n', '\r')).Where(s => !string.IsNullOrEmpty(s)).ToArray();
//  foreach (var subj in subjs) {
//    var sparts = subj.Split(new[] { ' ' }, 2);
//    var ss = sparts[0];
//    var props = sparts[1].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim(' ', '\n', '\r')).Where(s => !string.IsNullOrEmpty(s)).ToArray();
//    foreach (var prop in props) {
//      var pparts = prop.Split(new[] { ' ' }, 2);
//      var pp = pparts[0];
//      var objs = pparts[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim(' ', '\n', '\r')).Where(s => !string.IsNullOrEmpty(s)).ToArray();
//      foreach (var oo in objs) yield return new triple { s = ss, p = pp, o = oo };
//    }
//  }
//}
//public class triple {
//  public string s;
//  public string p;
//  public string o;
//}

