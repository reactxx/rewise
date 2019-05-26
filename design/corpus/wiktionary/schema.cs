using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VDS.RDF;
using static WiktTtlParser;

//http://kaiko.getalp.org/static/lemon/dbnary-doc/
public static class WiktSchema {

  public class ParsedTriple {

    public ParsedTriple(Context ctx, Triple t) {
      var items = new[] { TripleItem.Create(t.Subject, ctx, 0), TripleItem.Create(t.Predicate, ctx, 1), TripleItem.Create(t.Object, ctx, 2) };
      foreach (var it in items) if (predType!=WiktConsts.PredicateType.Ignore) parsedItem(ctx, it);
    }

    void parsedItem(Context ctx, TripleItem item) {
      var sb = new StringBuilder();
      switch (item.type) {
        case 0: //Uri
          var isData = item.Scheme == ctx.lang;
          var url = item.Scheme + ":" + item.Path;
          switch (item.triplePart) {
            case 0: // subject
              if (!isData) ctx.addError("!isData", url); else subjDataId = item.Path;
              return;
            case 1: // predicate
              predicateUri = url;
              if (WiktConsts.IgnoredProps.Contains(url)) { predType = WiktConsts.PredicateType.Ignore; return; }
              if (WiktConsts.parsePredicate(url, out predicate, out predType)) return;
              ctx.addError("wrong prop", url);
              return;
            case 2: // object
              if (isData) { objDataId = item.Path; return; }
              if (predType==WiktConsts.PredicateType.a) {
                if (WiktConsts.IgnoredClasses.Contains(url)) { predType = WiktConsts.PredicateType.Ignore; return; }
                objDataType = isNodeTypes.Contains(url) ? url : null;
                if (objDataType == null)
                  ctx.addError("classType != null", $"{subjDataId} {predicateUri} {url}");
                return;
              }
              if (predType == WiktConsts.PredicateType.UriValuesProps) {
                objUri = url;
                try { WiktConsts.ConstMan.enumValue(predicateUri, objUri); } catch {
                  ctx.addError("wrong uri value", $"{predicateUri}:{objUri}");
                  return;
                }
                return;
                //WiktConsts.ConstMan
              }
              if (item.Scheme == "lexvo") {
                objLang = item.Path; return;
              }
              ctx.addError("wrong value", $"{subjDataId} {predicateUri} {url}");
              return;
          }
          break;
        case 1: //blank
          switch (item.triplePart) {
            case 0: subjBlankId = item.InternalID; return;
            case 2: objBlankId = item.InternalID; return;
            case 1: ctx.addError("blank in prop", item.InternalID); return;
          }
          break;
        case 2: //literal
          switch (item.triplePart) {
            case 2: objLang = item.Language; objValue = item.Value; return;
            default: ctx.addError("literal not in object", item.Value); return;
          }
      }
    }

    public void dumpForAcceptProp(string className, string lang, Dictionary<string, int[]> res) {
      //if (predSchemeInfo == null) return;
      var sb = new StringBuilder();
      var langIdx = WiktQueries.allLangsIdx[lang];
      var allIdx = WiktQueries.allLangsIdx.Count;

      void fmt(string l, string r, bool cond = true) { if (!cond) return; sb.Append(r); sb.Append('('); sb.Append(l); sb.Append(')');  }


      sb.Append(className); sb.Append(": ");

      fmt(predType.ToString(), predicateUri); sb.Append('=');

      fmt("dataId", "", objDataId != null);
      fmt("value", ""/*objLang*/, objValue != null);
      fmt("lang", "" /*objLang*/, objValue == null && objLang != null);
      fmt("uriValue", objUri!=null ? objUri.ToString() : "", objUri!=null);

      var key = sb.ToString();
      res[key] = res.AddEx(key, arr => { arr[langIdx]++; arr[allIdx]++; return arr; }, () => new int[allIdx+1]);
    }

    // **** Processed in ttlParser.parseTtls:
    public string subjDataId;  // e.g. eng:<subjDataId>
    public string subjBlankId; // e.g. .:<blankId>


    //WiktConsts.PredicateType = a

    public string objDataType; // objDataType contains className, "ontolex:Form"
    public string objBlankId; // evaluated to objValue. e.g. .:<blankId>. 

    // **** Processed in node.acceptProp:
    //public PredicateInfo predInfo;
    public string predicateUri;
    public WiktConsts.PredicateType predType;
    public WiktConsts.predicates predicate;

    public string objDataId;  // Data id for relation target. e.g. eng:<objDataId>
    public string objValue; // string value or objBlankId's value
    public string objLang; // iso-3 lang code from lexvo:<objLang>
    public string objUri;
    // public string objUriValues; // UriValues, e.g. "olia:hasGender"
  }

  public class TripleItem {
    public static TripleItem Create(INode node, Context ctx, int triplePart) {
      var s = node as UriNode;
      if (s != null) {
        var res = ctx.decodePath(s.Uri);
        res.triplePart = triplePart; res.type = 0;
        return res;
      }
      var b = node as BlankNode;
      if (b != null) return new TripleItem { InternalID = b.InternalID, triplePart = triplePart, type = 1 };
      var l = node as LiteralNode;
      if (l != null) return new TripleItem { Language = l.Language, Value = l.Value, triplePart = triplePart, type = 2 };
      throw new Exception();
    }

    public int type;
    public int triplePart;
    // url - 0
    public string Scheme;
    public string Path;
    // blank - 1
    public string InternalID;
    // literal - 2
    public string Language;
    public string Value;
  }

  public class PredicateInfo {
    static PredicateInfo() {
      void fill(Dictionary<string, byte> data, WiktConsts.PredicateType type) {
        foreach (var kv in data) {
          infos[kv.Value] = new PredicateInfo { predicateUri = kv.Key, type = type };
        }
      }
      fill(ValueProps, WiktConsts.PredicateType.ValueProps); fill(BlankProps, WiktConsts.PredicateType.ValueProps); fill(BlankPropsInner, WiktConsts.PredicateType.ValueProps);
      fill(NymRelProps, WiktConsts.PredicateType.NymRelProps); fill(NotNymRelProps, WiktConsts.PredicateType.NotNymRelProps);
      fill(null /*TODO*/, WiktConsts.PredicateType.UriValuesProps);

      //fill(BlankProps, "BlankProps"); fill(BlankPropsInner, "BlankPropsInner"); fill(UriValuesProps, ValueType.UriValuesProps);
      uriToInfo = infos.Where(u => u!=null).ToDictionary(u => u.predicateUri); 
    }
    public static PredicateInfo[] infos = Enumerable.Range(0, 255).Select(i => (PredicateInfo)null).ToArray();
    public static Dictionary<string, PredicateInfo> uriToInfo;
    public string predicateUri; // e.g. dbnary:partOfSpeech 
    public WiktConsts.PredicateType type; // e.g ValueProps

    //public T getEnumValue<T>(string uri) where T : Enum {
    //  T o = 0;
    //  //return 0 as T;
    //}


    //public byte id; // e.g. 150
  }

  //static byte cNymClasses = 1;
  //public static Dictionary<string, byte> Types = new Dictionary<string, byte> {
  //  {"lexinfo:Adjective",cNymClasses++},
  //  {"lexinfo:Adverb",cNymClasses++},
  //  {"lexinfo:Interjection",cNymClasses++},
  //  {"lexinfo:Noun",cNymClasses++},
  //  {"lexinfo:Particle",cNymClasses++},
  //  {"lexinfo:Prefix",cNymClasses++},
  //  {"lexinfo:Preposition",cNymClasses++},
  //  {"lexinfo:Pronoun",cNymClasses++},
  //  {"lexinfo:ProperNoun",cNymClasses++},
  //  {"lexinfo:Suffix",cNymClasses++},
  //  {"lexinfo:Verb",cNymClasses++},
  //  {"ontolex:Affix",cNymClasses++},
  //  {"ontolex:LexicalEntry",cNymClasses++},
  //  {"ontolex:MultiWordExpression",cNymClasses++},
  //  {"ontolex:Word",cNymClasses++},
  //  //{"lexinfo:AbbreviatedForm",cNymClasses++},
  //  //{"lexinfo:Adposition",cNymClasses++},
  //  //{"lexinfo:Affix",cNymClasses++},
  //  //{"lexinfo:Article",cNymClasses++},
  //  //{"lexinfo:Conjunction",cNymClasses++},
  //  //{"lexinfo:Determiner",cNymClasses++},
  //  //{"lexinfo:Infix",cNymClasses++},
  //  //{"lexinfo:Number",cNymClasses++},
  //  //{"lexinfo:Numeral",cNymClasses++},
  //  //{"lexinfo:Postposition",cNymClasses++},
  //  //{"lexinfo:Symbol",cNymClasses++},
  //  //{"olia:MainVerb",cNymClasses++},
  //  //{"olia:ModalVerb",cNymClasses++},
  //};
  // lexinfo:Adjective lexinfo:Adverb lexinfo:Interjection lexinfo:Noun lexinfo:Particle lexinfo:Prefix lexinfo:Preposition lexinfo:Pronoun lexinfo:ProperNoun lexinfo:Suffix lexinfo:Verb ontolex:Affix ontolex:LexicalEntry ontolex:MultiWordExpression ontolex:Word 

  //public static Dictionary<string, byte> NymClasses = new Dictionary<string, byte> {
  //  {"ontolex:LexicalEntry",1},
  //};

  //public static Dictionary<string, byte> NotNymClasses = new Dictionary<string, byte> {
  //  {"dbnary:Gloss", NodeTypes.Gloss},
  //  {"dbnary:Page", NodeTypes.Page},
  //  {"dbnary:Translation", NodeTypes.Translation},
  //  {"ontolex:Form", NodeTypes.Form},
  //  {"ontolex:LexicalSense", NodeTypes.LexicalSense },
  //  {"rdf:Statement", NodeTypes.Statement },
  //};
  // dbnary:Gloss dbnary:Page dbnary:Translation ontolex:Form ontolex:LexicalSense rdf:Statement
  public static HashSet<string> isNodeTypes = new string[] { NodeTypes.Gloss, NodeTypes.Form, NodeTypes.LexicalSense,
    NodeTypes.LexicalÈntry, NodeTypes.Page, NodeTypes.Statement, NodeTypes.Translation }.ToHashSet();

  public static class NodeTypes {
    public const string Gloss = "dbnary:Gloss";
    public const string Page = "dbnary:Page";
    public const string Translation = "dbnary:Translation";
    public const string Form = "ontolex:Form";
    public const string Statement = "rdf:Statement";
    public const string LexicalSense = "ontolex:LexicalSense";
    public const string LexicalÈntry = "ontolex:LexicalEntry";
  }
  public const int NodeTypesLen = 6;

  public static Dictionary<string, byte> ValueProps = new Dictionary<string, byte> {
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

  //public static Dictionary<string, Dictionary<string, byte>> UriValuesProps = WiktConsts.ConstMan.enumValueMap;
  //new Dictionary<string, byte> {
  //  // {"dbnary:partOfSpeech", 181}, replaced by lexinfo:partOfSpeech
  //  {"dbnary:targetLanguage", 182},
  //  {"lexinfo:animacy", 183},
  //  {"lexinfo:gender", 184},
  //  {"lexinfo:number", 185},
  //  {"lexinfo:partOfSpeech", 186},
  //  {"lexinfo:person", 187},
  //  {"lexinfo:tense", 188},
  //  {"lexinfo:verbFormMood", 189},
  //  {"olia:hasCase", 190},
  //  {"olia:hasCountability", 191},
  //  {"olia:hasDefiniteness", 192},
  //  {"olia:hasDegree", 193},
  //  {"olia:hasGender", 194},
  //  //{"olia:hasInflectionType", 195},
  //  {"olia:hasMood", 194},
  //  {"olia:hasNumber", 197},
  //  {"olia:hasPerson", 198},
  //  //{"olia:hasSeparability", 199},
  //  {"olia:hasTense", 200},
  //  //{"olia:hasValency", 201},
  //  //{"olia:hasVoice", 202},
  //  {"terms:language", 203},
  //  {"rdf:predicate", 204},
  //};

  public static Dictionary<string, byte> BlankProps = new Dictionary<string, byte> {
    {"skos:definition", 170},
    {"skos:example", 171},
    {"vartrans:lexicalRel", 172}
  };

  public static Dictionary<string, byte> BlankPropsInner = new Dictionary<string, byte> {
    {"rdf:value", 175},
  };

  public static Dictionary<string, byte> NymRelProps = new Dictionary<string, byte> {
    {"dbnary:antonym", 50},
    {"dbnary:hypernym", 53},
    {"dbnary:hyponym", 54},
    {"dbnary:synonym", 56},
    //{"dbnary:troponym", 57},
    //{"dbnary:approximateSynonym", 51},
    //{"dbnary:holonym", 52},
    //{"dbnary:meronym", 55},
  };
  //dbnary:antonym dbnary:hypernym dbnary:hyponym dbnary:synonym 
  //dbnary:troponym dbnary:meronym dbnary:approximateSynonym dbnary:holonym



  public static Dictionary<string, byte> NotNymRelProps = new Dictionary<string, byte> {
    {"dbnary:describes", 70},
    {"dbnary:gloss", 71},
    {"dbnary:isTranslationOf", 72},
    {"ontolex:canonicalForm", 73},
    {"ontolex:otherForm", 74},
    {"ontolex:sense", 75},
    {"rdf:object", 76},
    {"rdf:subject", 77},
  };

  static byte cUriValues = 1;
  public static Dictionary<string, byte> UriValues = new Dictionary<string, byte> {
    {"lexinfo:abbreviation",cUriValues++},
    {"lexinfo:acronym",cUriValues++},
    {"lexinfo:adjective",cUriValues++},
    {"lexinfo:adposition",cUriValues++},
    {"lexinfo:adverb",cUriValues++},
    {"lexinfo:adverbialPronoun",cUriValues++},
    {"lexinfo:affix",cUriValues++},
    {"lexinfo:article",cUriValues++},
    {"lexinfo:baseElement",cUriValues++},
    {"lexinfo:cardinalNumeral",cUriValues++},
    {"lexinfo:circumposition",cUriValues++},
    {"lexinfo:collective",cUriValues++},
    {"lexinfo:conditional",cUriValues++},
    {"lexinfo:conjunction",cUriValues++},
    {"lexinfo:contraction",cUriValues++},
    {"lexinfo:definite",cUriValues++},
    {"lexinfo:demonstrativePronoun",cUriValues++},
    {"lexinfo:determiner",cUriValues++},
    {"lexinfo:exclamativePronoun",cUriValues++},
    {"lexinfo:expression",cUriValues++},
    {"lexinfo:feminine",cUriValues++},
    {"lexinfo:firstPerson",cUriValues++},
    {"lexinfo:future",cUriValues++},
    {"lexinfo:idiom",cUriValues++},
    {"lexinfo:imperative",cUriValues++},
    {"lexinfo:imperfect",cUriValues++},
    {"lexinfo:indefinite",cUriValues++},
    {"lexinfo:indefiniteCardinalNumeral",cUriValues++},
    {"lexinfo:indefiniteOrdinalNumeral",cUriValues++},
    {"lexinfo:indefinitePronoun",cUriValues++},
    {"lexinfo:indicative",cUriValues++},
    {"lexinfo:infinitive",cUriValues++},
    {"lexinfo:infix",cUriValues++},
    {"lexinfo:interjection",cUriValues++},
    {"lexinfo:interrogativeCardinalNumeral",cUriValues++},
    {"lexinfo:interrogativePronoun",cUriValues++},
    {"lexinfo:letter",cUriValues++},
    {"lexinfo:masculine",cUriValues++},
    {"lexinfo:modal",cUriValues++},
    {"lexinfo:multiplicativeNumeral",cUriValues++},
    {"lexinfo:neuter",cUriValues++},
    {"lexinfo:noun",cUriValues++},
    {"lexinfo:number",cUriValues++},
    {"lexinfo:numeral",cUriValues++},
    {"lexinfo:numeralFraction",cUriValues++},
    {"lexinfo:ordinalAdjective",cUriValues++},
    {"lexinfo:participle",cUriValues++},
    {"lexinfo:participleAdjective",cUriValues++},
    {"lexinfo:particle",cUriValues++},
    {"lexinfo:past",cUriValues++},
    {"lexinfo:pastParticipleAdjective",cUriValues++},
    {"lexinfo:perfective",cUriValues++},
    {"lexinfo:personalPronoun",cUriValues++},
    {"lexinfo:phraseologicalUnit",cUriValues++},
    {"lexinfo:plural",cUriValues++},
    {"lexinfo:possessiveAdjective",cUriValues++},
    {"lexinfo:possessivePronoun",cUriValues++},
    {"lexinfo:postposition",cUriValues++},
    {"lexinfo:prefix",cUriValues++},
    {"lexinfo:preposition",cUriValues++},
    {"lexinfo:present",cUriValues++},
    {"lexinfo:pronominalAdverb",cUriValues++},
    {"lexinfo:pronoun",cUriValues++},
    {"lexinfo:properNoun",cUriValues++},
    {"lexinfo:proverb",cUriValues++},
    {"lexinfo:radical",cUriValues++},
    {"lexinfo:reciprocalPronoun",cUriValues++},
    {"lexinfo:reflexivePersonalPronoun",cUriValues++},
    {"lexinfo:relativePronoun",cUriValues++},
    {"lexinfo:secondPerson",cUriValues++},
    {"lexinfo:singular",cUriValues++},
    {"lexinfo:subjunctive",cUriValues++},
    {"lexinfo:suffix",cUriValues++},
    {"lexinfo:symbol",cUriValues++},
    {"lexinfo:thirdPerson",cUriValues++},
    {"lexinfo:verb",cUriValues++},
    {"olia:Accusative",cUriValues++},
    {"olia:ActiveVoice",cUriValues++},
    {"olia:AdverbialParticiple",cUriValues++},
    {"olia:Animate",cUriValues++},
    {"olia:Comparative",cUriValues++},
    {"olia:Countable",cUriValues++},
    {"olia:DativeCase",cUriValues++},
    {"olia:Feminine",cUriValues++},
    {"olia:First",cUriValues++},
    {"olia:Future",cUriValues++},
    {"olia:FuturePerfect",cUriValues++},
    {"olia:GenitiveCase",cUriValues++},
    {"olia:ImperativeMood",cUriValues++},
    {"olia:Inanimate",cUriValues++},
    {"olia:IndicativeMood",cUriValues++},
    {"olia:Infinitive",cUriValues++},
    {"olia:InstrumentalCase",cUriValues++},
    {"olia:Intransitive",cUriValues++},
    {"olia:LocativeCase",cUriValues++},
    {"olia:Masculine",cUriValues++},
    {"olia:MixedInflection",cUriValues++},
    {"olia:Neuter",cUriValues++},
    {"olia:Nominative",cUriValues++},
    {"olia:NonSeparable",cUriValues++},
    {"olia:Participle",cUriValues++},
    {"olia:PassiveVoice",cUriValues++},
    {"olia:Past",cUriValues++},
    {"olia:PastPerfectTense",cUriValues++},
    {"olia:Perfect",cUriValues++},
    {"olia:Plural",cUriValues++},
    {"olia:Positive",cUriValues++},
    {"olia:Present",cUriValues++},
    {"olia:QuotativeMood",cUriValues++},
    {"olia:ReflexiveVoice",cUriValues++},
    {"olia:Second",cUriValues++},
    {"olia:SecondPolite",cUriValues++},
    {"olia:Separable",cUriValues++},
    {"olia:Singular",cUriValues++},
    {"olia:StrongInflection",cUriValues++},
    {"olia:SubjunctiveMood",cUriValues++},
    {"olia:Superlative",cUriValues++},
    {"olia:Third",cUriValues++},
    {"olia:Transitive",cUriValues++},
    {"olia:Uncountable",cUriValues++},
    {"olia:Uninflected",cUriValues++},
    {"olia:VocativeCase",cUriValues++},
    {"olia:WeakInflection",cUriValues++},
    // by hand, from english and german: SELECT DISTINCT ?o WHERE { ?s rdf:predicate ?o . ?s a rdf:Statement . }
    {"dbnary:synonym", cUriValues++},
    {"dbnary:hyponym",cUriValues++},
    {"dbnary:antonym",cUriValues++},
    {"dbnary:meronym",cUriValues++},
    {"dbnary:hypernym",cUriValues++},
    {"dbnary:holonym",cUriValues++},
    {"dbnary:troponym",cUriValues++},
  };

  public static Dictionary<string, byte> ValidPartOfspeach =
    new[] { "lexinfo:adjective", "lexinfo:adverb", "lexinfo:cardinalNumeral", "lexinfo:interjection",
      "lexinfo:noun", "lexinfo:preposition", "lexinfo:pronoun", "lexinfo:verb" }.
    ToDictionary(k => k, k => UriValues[k]);
  // lexinfo:adjective lexinfo:adverb lexinfo:cardinalNumeral lexinfo:interjection lexinfo:noun lexinfo:preposition lexinfo:pronoun lexinfo:verb 
  // lexinfo:adjective, lexinfo:adverb, lexinfo:cardinalNumeral, lexinfo:interjection, lexinfo:noun, lexinfo:preposition, lexinfo:pronoun, lexinfo:verb 



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

  public static void dumps() {

    var root = Corpus.Dirs.wiktDbnary + @"graphDBExport\scheme\dump-";

    bool isClass(string url) { var parts = url.Split(':'); return parts.Length == 2 && char.IsUpper(parts[1][0]); }

    void writeList(string fn, IEnumerable<string> l) => File.WriteAllText(
      root + fn + ".txt",
      string.Join(",\r\n", l.Distinct().OrderBy(s => s).Select(s => s)));
    //string.Join(",\r\n", l.Distinct().OrderBy(s => s).Select(s => $"\"{s}\"")));
    //string.Join(",\r\n", l.Distinct().OrderBy(s => s).Select(s => $"{{\"{s}\", 1}}")));

    var rawTrs = parse().ToArray();
    File.WriteAllLines(root + "value-objs.txt", valueObjects(rawTrs));
    var trs = filterUriValueObjects(rawTrs).ToArray();

    var classes = trs.SelectMany(t => Linq.Items(t.s, t.o)).Where(s => isClass(s)).Distinct().ToHashSet();

    var valueTypes = Linq.Items("rdf:langString", "xsd:int", "xsd:string").ToHashSet();
    var blank = "\"blank\"";
    var pNyms = Linq.Items("dbnary:antonym", "dbnary:approximateSynonym", "dbnary:holonym", "dbnary:hypernym", "dbnary:hyponym", "dbnary:meronym", "dbnary:synonym", "dbnary:troponym").ToHashSet();
    var cNotNyms = Linq.Items("dbnary:Translation", "dbnary:Gloss", "ontolex:Form").ToHashSet();
    var cNotNymsOnly = Linq.Items("dbnary:Page", "ontolex:LexicalSense", "rdf:Statement").Concat(cNotNyms).ToHashSet();

    writeList("t-all", trs.Select(t => $"{t.s} - {t.p} - {t.o}"));
    writeList("so-all", trs.SelectMany(t => Linq.Items(t.s, t.o)));
    writeList("c-all", classes.OrderBy(s => s));
    writeList("p-all", trs.Select(t => t.p));
    writeList("s-p-all", trs.Select(t => $"{t.s} - {t.p}"));
    writeList("s-p-o-all", trs.Select(t => $"{t.s} - {t.p} - {t.o}"));
    writeList("o-p-s-all", trs.Select(t => $"{t.o} - {t.p} - {t.s}"));
    writeList("p-s-o-all", trs.Select(t => $"{t.p} - {t.s} - {t.o}"));

    var vals = trs.Where(t => valueTypes.Contains(t.o)).ToArray();
    writeList("s-p-value", vals.Select(t => $"{t.s} - {t.p}"));
    writeList("p-s-value", vals.Select(t => $"{t.p} - {t.s}"));
    writeList("p-value", vals.Select(t => $"{t.p}"));

    var rels = trs.Where(t => classes.Contains(t.s) && classes.Contains(t.o)).ToArray();
    writeList("p-o-class", rels.Select(t => $"{t.p} - {t.o}"));
    writeList("o-p-class", rels.Select(t => $"{t.o} - {t.p}"));
    writeList("o-p-s-class", rels.Select(t => $"{t.o} - {t.p} - {t.s}"));
    writeList("p-class", rels.Select(t => t.p));
    writeList("s-p-class", rels.Select(t => $"{t.s} - {t.p}"));
    writeList("s-p-o-class", rels.Select(t => $"{t.s} - {t.p} - {t.o}"));
    writeList("p-s-class", rels.Select(t => $"{t.p} - {t.s}"));

    var blanks = trs.Where(t => t.o == blank).ToArray();
    writeList("s-p-blank", blanks.Select(t => $"{t.s} - {t.p}"));
    writeList("s-blank", blanks.Select(t => $"{t.s}"));
    writeList("p-blank", blanks.Select(t => $"{t.p}"));

    var nyms = trs.Where(t => pNyms.Contains(t.p)).ToArray();
    writeList("s-nyms", nyms.Where(t => !cNotNyms.Contains(t.s)).Select(t => t.s));
    writeList("o-nyms", nyms.Where(t => !cNotNyms.Contains(t.o)).Select(t => t.o));
    writeList("s-nyms-o", nyms.Where(t => !cNotNyms.Contains(t.o)).Select(t => $"{t.s} => {t.o}"));

    var uriObj = trs.Where(t => t.o[0] == '@').ToArray();
    writeList("s-p-uriobj", uriObj.Select(t => $"{t.s} - {t.p}"));
    writeList("s-uriobj", uriObj.Select(t => $"{t.s}"));
    writeList("p-uriobj", uriObj.Select(t => $"{t.p}"));
    writeList("o-uriobj", uriObj.Select(t => $"{t.o}"));

    //*** all values
    var valueProps = trs.Where(t => t.o[0] == '@' || t.o == blank || ValueProps.ContainsKey(t.p)).ToArray();
    writeList("s-p-o-vals-all", valueProps.Select(t => $"{t.s} - {t.p} - {t.o}"));
    writeList("s-p-vals-all", valueProps.Select(t => $"{t.s} - {t.p}"));

    //*** NYMS
    writeList("c-nymsOnly", classes.Where(c => !cNotNymsOnly.Contains(c)));
    writeList("c-notNymsOnly", classes.Where(c => cNotNymsOnly.Contains(c)));

    // join NYMs
    var joinNyms = trs.Select(t => {
      string toNym(string s) => cNotNymsOnly.Contains(s) || !classes.Contains(s) ? s : "l:Nym";
      var res = new triple { s = toNym(t.s), p = t.p, o = toNym(t.o) };
      return res.s == t.s && res.o == t.o ? null : res;
    }).Where(t => t != null).ToArray();

    writeList("s-p-o-nyms", joinNyms.Select(t => $"{t.s} - {t.p} - {t.o}"));
    writeList("o-p-s-nyms", joinNyms.Select(t => $"{t.o} - {t.p} - {t.s}"));
    writeList("p-s-o-nyms", joinNyms.Select(t => $"{t.p} - {t.s} - {t.o}"));

    var valuePropsNyms = joinNyms.Where(t => t.o[0] == '@' || t.o == blank || ValueProps.ContainsKey(t.p)).ToArray();
    writeList("s-p-o-vals-nyms", valuePropsNyms.Select(t => $"{t.s} - {t.p} - {t.o}"));
    writeList("s-p-vals-nyms", valuePropsNyms.Select(t => $"{t.s} - {t.p}"));

  }

  /*******************************************************
   * PRIVATE
   ******************************************************/
  static string[] types = new[] { "rdf:langString", "xsd:int", "xsd:string" };

  static IEnumerable<triple> filterUriValueObjects(IEnumerable<triple> src) {
    var subjs = src.Select(s => s.s).ToHashSet();
    return src.Select(s => {
      if (!types.Contains(s.o) && !s.o.StartsWith("\"") && !subjs.Contains(s.o)) s.o = "@" + s.o.Split(':')[0];
      return s;
    });
  }
  static IEnumerable<string> valueObjects(IEnumerable<triple> src) {
    var subjs = src.Select(s => s.s).ToHashSet();
    return src.Select(s => s.o).Where(s => !types.Contains(s) && !s.StartsWith("\"") && !subjs.Contains(s)).Distinct().OrderBy(s => s);
  }

  static IEnumerable<triple> parse() {
    var txt = File.ReadAllText(Directory.GetCurrentDirectory() + @"\wiktionary\schema.txt");
    var subjs = txt.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim(' ', '\n', '\r')).Where(s => !string.IsNullOrEmpty(s)).ToArray();
    foreach (var subj in subjs) {
      var sparts = subj.Split(new[] { ' ' }, 2);
      var ss = sparts[0];
      var props = sparts[1].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim(' ', '\n', '\r')).Where(s => !string.IsNullOrEmpty(s)).ToArray();
      foreach (var prop in props) {
        var pparts = prop.Split(new[] { ' ' }, 2);
        var pp = pparts[0];
        var objs = pparts[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim(' ', '\n', '\r')).Where(s => !string.IsNullOrEmpty(s)).ToArray();
        foreach (var oo in objs) yield return new triple { s = ss, p = pp, o = oo };
      }
    }
  }
  public class triple {
    public string s;
    public string p;
    public string o;
  }

}
