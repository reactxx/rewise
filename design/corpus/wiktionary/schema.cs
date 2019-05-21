using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class WiktSchema {

  public static Dictionary<string, byte> NymClasses = new Dictionary<string, byte> {
    {"lexinfo:AbbreviatedForm",1},
    {"lexinfo:Adjective",2},
    {"lexinfo:Adposition",3},
    {"lexinfo:Adverb",4},
    {"lexinfo:Affix",5},
    {"lexinfo:Article",6},
    {"lexinfo:Conjunction",7},
    {"lexinfo:Determiner",8},
    {"lexinfo:Infix",9},
    {"lexinfo:Interjection",10},
    {"lexinfo:Noun",11},
    {"lexinfo:Number",12},
    {"lexinfo:Numeral",13},
    {"lexinfo:Particle",14},
    {"lexinfo:Postposition",15},
    {"lexinfo:Prefix",16},
    {"lexinfo:Preposition",17},
    {"lexinfo:Pronoun",18},
    {"lexinfo:ProperNoun",19},
    {"lexinfo:Suffix",20},
    {"lexinfo:Symbol",21},
    {"lexinfo:Verb",22},
    {"olia:MainVerb",23},
    {"olia:ModalVerb",24},
    {"ontolex:Affix",25},
    {"ontolex:LexicalEntry",26},
    {"ontolex:MultiWordExpression",27},
    {"ontolex:Word",28},
  };

  public static Dictionary<string, byte> NotNymClasses = new Dictionary<string, byte> {
    {"dbnary:Gloss", NodeTypes.Gloss},
    {"dbnary:Page", NodeTypes.Page},
    {"dbnary:Translation", NodeTypes.Translation},
    {"ontolex:Form", NodeTypes.Form},
    {"ontolex:LexicalSense", NodeTypes.LexicalSense },
  };

  public static class NodeTypes {
    public const byte EntryCounter = 100;

    public const byte Gloss = 101;
    public const byte Page = 102;
    public const byte Translation = 103;
    public const byte Form = 104;
    public const byte LexicalSense = 105;
  }
  public const int NodeTypesLen = 6;

  public static Dictionary<string, byte> Classes = NymClasses.Concat(NotNymClasses).ToDictionary(kv => kv.Key, kv => kv.Value);

  public static Dictionary<string, byte> ValueProps = new Dictionary<string, byte> {
    {"dbnary:partOfSpeech", 150},
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

  public static Dictionary<string, byte> BlankProps = new Dictionary<string, byte> {
    {"skos:definition", 170},
    {"skos:example", 171},
    {"var:lexicalRel", 172}
  };

  public static Dictionary<string, byte> NymProps = new Dictionary<string, byte> {
    {"dbnary:antonym", 50},
    {"dbnary:approximateSynonym", 51},
    {"dbnary:holonym", 52},
    {"dbnary:hypernym", 53},
    {"dbnary:hyponym", 54},
    {"dbnary:meronym", 55},
    {"dbnary:synonym", 56},
    {"dbnary:troponym", 57},
  };

  public static Dictionary<string, byte> NotNymRelProps = new Dictionary<string, byte> {
    {"dbnary:describes", 70},
    {"dbnary:gloss", 71},
    {"dbnary:isTranslationOf", 72},
    {"ontolex:canonicalForm", 73},
    {"ontolex:otherForm", 74},
    {"ontolex:sense", 75}
  };

  public static Dictionary<string, byte> Props = NymProps.Concat(NotNymRelProps).Concat(ValueProps).Concat(BlankProps).ToDictionary(kv => kv.Key, kv => kv.Value);

  public static Dictionary<string, byte> ClassesProps = Props.Concat(Classes).ToDictionary(kv => kv.Key, kv => kv.Value);

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
    {"terms", "http://purl.org/dc/terms/"},
    {"vartrans", "http://www.w3.org/ns/lemon/vartrans#"},
    {"xsd", "http://www.w3.org/2001/XMLSchema#"},
  };

  public static void dumps() {

    var root = Corpus.Dirs.wiktDbnary + @"graphDBExport\scheme\dump-";

    bool isClass(string url) { var parts = url.Split(':'); return parts.Length == 2 && char.IsUpper(parts[1][0]); }

    void writeList(string fn, IEnumerable<string> l) => File.WriteAllText(
      root + fn + ".txt",
      //string.Join(",\r\n", l.Distinct().OrderBy(s => s).Select(s => $"\"{s}\"")));
      string.Join(",\r\n", l.Distinct().OrderBy(s => s).Select(s => $"{{\"{s}\", 1}}")));

    var trs = WiktSchema.parse().ToArray();

    var classes = trs.SelectMany(t => Linq.Items(t.s, t.o)).Where(s => isClass(s)).Distinct().ToHashSet();

    var valueTypes = Linq.Items("rdf:langString", "xsd:int", "xsd:string").ToHashSet();
    var blank = "\"blank\"";
    var pNyms = Linq.Items("dbnary:antonym", "dbnary:approximateSynonym", "dbnary:holonym", "dbnary:hypernym", "dbnary:hyponym", "dbnary:meronym", "dbnary:synonym", "dbnary:troponym").ToHashSet();
    var cNotNyms = Linq.Items("dbnary:Translation", "dbnary:Gloss", "ontolex:Form").ToHashSet();
    var cNotNymsOnly = Linq.Items("dbnary:Page", "ontolex:LexicalSense").Concat(cNotNyms).ToHashSet();

    writeList("t-all", trs.Select(t => $"{t.s} - {t.p} - {t.o}"));
    writeList("so-all", trs.SelectMany(t => Linq.Items(t.s, t.o)));
    writeList("c-all", classes.OrderBy(s => s));
    writeList("p-all", trs.Select(t => t.p));
    writeList("s-p-all", trs.Select(t => $"{t.s} - {t.p}"));
    writeList("s-p-o-all", trs.Select(t => $"{t.s} - {t.p} - {t.o}"));
    writeList("o-p-s-all", trs.Select(t => $"{t.o} - {t.p} - {t.s}"));
    writeList("p-s-o-all", trs.Select(t => $"{t.p} - {t.s} - {t.o}"));

    var vals = trs.Where(t => valueTypes.Contains(t.o));
    writeList("s-p-value", vals.Select(t => $"{t.s} - {t.p}"));
    writeList("p-s-value", vals.Select(t => $"{t.p} - {t.s}"));
    writeList("p-value", vals.Select(t => $"{t.p}"));

    var rels = trs.Where(t => t.o != blank && !valueTypes.Contains(t.o));
    writeList("p-o-class", rels.Select(t => $"{t.p} => {t.o}"));
    writeList("o-p-class", rels.Select(t => $"{t.o} => {t.p}"));
    writeList("p-class", rels.Select(t => t.p));
    writeList("s-p-class", rels.Select(t => $"{t.s} => {t.p}"));
    writeList("p-s-class", rels.Select(t => $"{t.p} => {t.s}"));

    var blanks = trs.Where(t => t.o == blank);
    writeList("s-p-blank", blanks.Select(t => $"{t.s} - {t.p}"));
    writeList("s-blank", blanks.Select(t => $"{t.s}"));
    writeList("p-blank", blanks.Select(t => $"{t.p}"));

    var nyms = trs.Where(t => pNyms.Contains(t.p));
    writeList("s-nyms", nyms.Where(t => !cNotNyms.Contains(t.s)).Select(t => t.s));
    writeList("o-nyms", nyms.Where(t => !cNotNyms.Contains(t.o)).Select(t => t.o));
    writeList("s-nyms-o", nyms.Where(t => !cNotNyms.Contains(t.o)).Select(t => $"{t.s} => {t.o}"));

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
  }

  /*******************************************************
   * PRIVATE
   ******************************************************/

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
