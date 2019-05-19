using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

// http://docs.rdf4j.org/rest-api/
// https://www.keycdn.com/support/popular-curl-examples
// import from h:\Users\pavel\graphdb-import\*.ttl 

public static class WiktQueries {

  public static string[] allLangs = new string[] {
    "bg","de","el","en","es","fi","fr","id","it","ja","la","lt","mg","nl","no","pl","pt","ru","sh","sv","tr",
  };

  const string limit = "";
  //const string limit = "LIMIT 1000";

  public static void runQueriess() {
    foreach (var lang in allLangs) runQueries(lang);
  }

  public static void runMetaQueriess() {
    Parallel.ForEach(allLangs, new ParallelOptions { MaxDegreeOfParallelism = 2 }, lang => {
      forLang(lang, (langDir, schemePrefix) => {
        var cmd = curlCmd(lang, schemePrefix + "allProps", dataPrefixes + allProps);
        Process.Start("curl.exe", cmd).WaitForExit();
      });
    });
  }

  static void forLang(string lang, Action<string, string> doAction) {
    var langDir = Corpus.Dirs.wiktDbnary + @"graphDBExport\" + lang + "\\";
    var sd = Corpus.Dirs.wiktDbnary + @"graphDBExport\scheme\";
    var schemePrefix = sd + lang + "_";
    if (!Directory.Exists(langDir)) Directory.CreateDirectory(langDir);
    if (!Directory.Exists(sd)) Directory.CreateDirectory(sd);
    doAction(langDir, schemePrefix);
  }

  public static void runQueries(string lang) {
    forLang(lang, (langDir, schemePrefix) => {
      Parallel.ForEach(commands(lang, langDir, schemePrefix), new ParallelOptions { MaxDegreeOfParallelism = 2 }, args =>
         Process.Start("curl.exe", args).WaitForExit()
      );
    });
  }

  static IEnumerable<string> commands(string lang, string langDir, string schemePrefix) {
    foreach (var q in propsQueries())
      yield return curlCmd(lang, langDir + q.file.ToLower(), dataPrefixes + q.query);
    foreach (var q in relQueries())
      yield return curlCmd(lang, langDir + q.file.ToLower(), dataPrefixes + q.query);
    foreach (var cls in classMap.Values)
      yield return curlCmd(lang, langDir + "ids_" + clsToName[cls.Split(':')[1].ToLower()], dataPrefixes + idsQuery(cls));
  }

  static string dbnaryUrl(string lang) => "http://localhost:7200/repositories/dbnary_" + lang;

  static string curlCmd(string lang, string outFile, string query) =>
    string.Format("-G {0} -o '{1}.ttl' -H 'Accept:text/turtle' -d query={2}", dbnaryUrl(lang), outFile, HttpUtility.UrlEncode(query));

  /*****************************************************************
   * CLASSED
   *****************************************************************/

  public static Dictionary<string, string> classMap = new Dictionary<string, string> {
    { "t", "db:Translation" },
    { "g", "db:Gloss"},
    { "p", "db:Page"},
    { "e", "on:LexicalEntry"},
    { "s", "on:LexicalSense"},
    { "f", "on:Form"},
    { "m", "on:MultiWordExpression"},
  };
  public static Dictionary<string, string> clsToName = new Dictionary<string, string> {
    { "translation", "trans" },
    { "gloss", "gloss" },
    { "page", "page" },
    { "lexicalentry", "entry" },
    { "lexicalsense", "sense" },
    { "form", "form" },
    { "multiwordexpression", "multi" },
  };
  public static Dictionary<string, string> nameToId = clsToName.Values.ToDictionary(n => n, n => n[0].ToString());


  static string dataPrefixes = @"
PREFIX on: <http://www.w3.org/ns/lemon/ontolex#>
PREFIX db: <http://kaiko.getalp.org/dbnary#>
PREFIX lexinfo: <http://www.lexinfo.net/ontology/2.0/lexinfo#>
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX olia: <http://purl.org/olia/olia.owl#>
PREFIX skos: <http://www.w3.org/2004/02/skos/core#>
PREFIX terms:   <http://purl.org/dc/terms/>
PREFIX lime: <http://www.w3.org/ns/lemon/lime#>
PREFIX var:   <http://www.w3.org/ns/lemon/vartrans#>
PREFIX prot:   <http://proton.semanticweb.org/protonsys#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
PREFIX : <ll:>
";

  /*****************************************************************
   * SELECT META INFOS
   *****************************************************************/
  const string allProps = @"
CONSTRUCT {?t ?p ?to}
WHERE {
SELECT DISTINCT ?t ?p ?to 
WHERE {{
  ?s ?p ?o .
  ?s a ?t .
  ?o a ?to .
  FILTER(isUri(?o))
  } UNION {
  ?s ?p ?o .
  ?s a ?t .
  BIND (DATATYPE(?o) as ?to)
  FILTER(!isLiteral(?o))
  } UNION {
  ?s ?p ?o .
  ?s a ?t .
  FILTER(isBlank(?o))
  BIND(COALESCE(?x, 'blank') as ?to)
}}}
";

  /*****************************************************************
   * IDS queries
   *****************************************************************/

  static string idsQuery(string className) => string.Format(@"
CONSTRUCT {{:e :e ?obj}}
WHERE {{
	SELECT ?obj
		WHERE 
		{{ 	
			?obj2 a {0} .
      BIND( SUBSTR( STR(?obj2), 36) as ?obj)
		}}
}}
" + limit, className);

  /*****************************************************************
   * CLASS RELATIONS queries
   *****************************************************************/

  static string relQuery(string sfrom, string sto, string preds) => string.Format(@"
CONSTRUCT {{?st ?p ?so}}
WHERE {{
	SELECT ?st ?p ?so
	WHERE {{

    ?s a {0} .
    ?o a {2} .
   	?s ?p ?o .
    
    BIND( URI( CONCAT(""{1}{1}:"", SUBSTR( STR(?s), 36))) as ?st)
    BIND( URI( CONCAT(""{3}{3}:"", SUBSTR( STR(?o), 36))) as ?so)
    VALUES ?p {{ {4} }} 
  }}
}}
" + limit, classMap[sfrom], sfrom, classMap[sto], sto, preds);

  const string nyms = "db:antonym db:holonym db:hypernym db:hyponym db:meronym db:synonym db:troponym";

  static IEnumerable<relQueryFile> relQueries() {
    // Page
    yield return new relQueryFile {
      file = "rel_Page_Synonyms_Page",
      query = relQuery("p", "p", nyms)
    };
    yield return new relQueryFile {
      file = "rel_Page_Descr_Entry",
      query = relQuery("p", "e", "db:describes")
    };
    yield return new relQueryFile {
      file = "rel_Page_Descr_Multi",
      query = relQuery("p", "m", "db:describes")
    };

    // LexicalSense
    yield return new relQueryFile {
      file = "rel_Sense_Synonyms_Page",
      query = relQuery("s", "p", nyms)
    };

    // LexicalEntry
    yield return new relQueryFile {
      file = "rel_Entry_Synonyms_Page",
      query = relQuery("e", "p", nyms)
    };
    yield return new relQueryFile {
      file = "rel_Entry_Canform_Form",
      query = relQuery("e", "f", "on:canonicalForm")
    };
    yield return new relQueryFile {
      file = "rel_Entry_Otherform_Form",
      query = relQuery("e", "f", "on:otherForm")
    };
    yield return new relQueryFile {
      file = "rel_Entry_Sense_Sense",
      query = relQuery("e", "s", "on:sense")
    };

    // Translation
    yield return new relQueryFile {
      file = "rel_Trans_Gloss_Gloss",
      query = relQuery("t", "g", "db:gloss")
    };
    yield return new relQueryFile {
      file = "rel_Trans_Trans_Entry",
      query = relQuery("t", "e", "db:isTranslationOf")
    };
    yield return new relQueryFile {
      file = "rel_Trans_Trans_Sense",
      query = relQuery("t", "s", "db:isTranslationOf")
    };
  }

  class relQueryFile {
    public string query;
    public string file;
  }

  /*****************************************************************
   * CLASS PROPS queries
   *****************************************************************/

  static string propsQuery(string sfrom, string preds) => string.Format(@"
CONSTRUCT {{?st :p ?o}}
WHERE {{
	SELECT ?st ?o
	WHERE {{

    ?s a {0} .
   	?s {2} ?o .
    
    BIND( URI( CONCAT(""{1}{1}:"", SUBSTR( STR(?s), 36))) as ?st)
  }}
}}
" + limit, classMap[sfrom], sfrom, preds);

  static IEnumerable<relQueryFile> propsQueries() {
    // Trans
    yield return new relQueryFile {
      file = "prop_Trans_TargetLanguageCode",
      query = propsQuery("t", "db:targetLanguageCode")
    };
    yield return new relQueryFile {
      file = "prop_Trans_Usage",
      query = propsQuery("t", "db:usage")
    };
    yield return new relQueryFile {
      file = "prop_Trans_WrittenForm",
      query = propsQuery("t", "db:writtenForm")
    };

    // Entry
    yield return new relQueryFile {
      file = "prop_Entry_PartOfSpeech",
      query = propsQuery("e", "db:partOfSpeech")
    };
    yield return new relQueryFile {
      file = "prop_Entry_Language",
      query = propsQuery("e", "li:language")
    };

    // Sense
    yield return new relQueryFile {
      file = "prop_Sense_Number",
      query = propsQuery("s", "db:senseNumber")
    };

    // Gloss
    yield return new relQueryFile {
      file = "prop_Gloss_Rank",
      query = propsQuery("g", "db:rank")
    };
    yield return new relQueryFile {
      file = "prop_Gloss_SenseNumber",
      query = propsQuery("g", "db:senseNumber")
    };
    yield return new relQueryFile {
      file = "prop_Gloss_Value",
      query = propsQuery("g", "rd:value")
    };

    // Form
    yield return new relQueryFile {
      file = "prop_Form_Note",
      query = propsQuery("f", "sk:note")
    };
    yield return new relQueryFile {
      file = "prop_Form_PhoneticRep",
      query = propsQuery("f", "on:phoneticRep")
    };
    yield return new relQueryFile {
      file = "prop_Form_WrittenRep",
      query = propsQuery("f", "on:writtenRep")
    };
  }

}


/*
 * BLANK NODE TEST
SELECT DISTINCT ?ts ?p
WHERE {
  ?s ?p ?o;
     a ?ts
  FILTER (isBlank(?o)) 
}

 * IMPORT FILE TO DB
curl -X POST --header 'Content-Type: application/json' --header 'Accept: application/json' -d '{
 "fileNames": [
 "resources/family-data.ttl"
 ]
 }' 'http://localhost:7200/rest/data/import/server/test'

 * EXPORT SCHEME
PREFIX lexinfo: <http://www.lexinfo.net/ontology/2.0/lexinfo#>
PREFIX dbnary: <http://kaiko.getalp.org/dbnary#>
PREFIX ontolex: <http://www.w3.org/ns/lemon/ontolex#>
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX olia: <http://purl.org/olia/olia.owl#>
PREFIX skos: <http://www.w3.org/2004/02/skos/core#>
PREFIX terms:   <http://purl.org/dc/terms/>
PREFIX lime: <http://www.w3.org/ns/lemon/lime#>
PREFIX var:   <http://www.w3.org/ns/lemon/vartrans#>
PREFIX prot:   <http://proton.semanticweb.org/protonsys#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
prefix rd: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
prefix owl: <http://www.w3.org/2002/07/owl#>

CONSTRUCT {?s ?p ?o} WHERE {
SELECT DISTINCT ?s ?p ?o
WHERE {
  ?s ?p ?o .
  FILTER(!STRSTARTS(LCASE(STR(?p)),"http://www.w3.org/2002/07/owl"))
  FILTER(!STRSTARTS(LCASE(STR(?p)),"http://www.w3.org/1999/02/22-rdf-syntax-ns"))
  FILTER(!STRSTARTS(LCASE(STR(?p)),"http://www.w3.org/2000/01/rdf-schema"))
  FILTER(!STRSTARTS(LCASE(STR(?p)),"http://proton.semanticweb.org/protonsys"))
  FILTER(!STRSTARTS(LCASE(STR(?s)),"http://www.w3.org/1999/02/22-rdf-syntax-ns"))
}
ORDER BY ?s ?p
}*/
