using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;

// http://docs.rdf4j.org/rest-api/
// https://www.keycdn.com/support/popular-curl-examples
// import from h:\Users\pavel\graphdb-import\*.ttl 

public static class WiktQueries {

  public static void generateCmd() {
    var rootDir = Corpus.Dirs.wikiesDbnary + @"graphDBExport\";
    var drootDir = rootDir.ToLower().Replace("c:\\", "d:\\");
    Parallel.ForEach(commands(drootDir), new ParallelOptions { MaxDegreeOfParallelism = 2 }, args =>
       Process.Start("curl.exe", args).WaitForExit()
    );
  }

  static IEnumerable<string> commands(string drootDir) {
    foreach (var q in dataQueries())
      yield return curlCmd(drootDir + q.file, dataPrefixes + q.query);
    foreach (var cls in classes)
      yield return curlCmd(drootDir + "ids" + cls.Split(':')[1] + ".", classIds(cls));

    //yield return curlCmd(drootDir + "allInstancePropsWithType", allInstancePropsWithType);
    //yield return curlCmd(drootDir + "allInstanceProps", allInstanceProps);
  }

  const string rewiseUrl = "http://localhost:7200/repositories/rewisse";

  static string curlCmd(string outFile, string query) =>
    //string.Format("-G {0} -o \"{1}\" -H \"Accept:application/x-trig\" -d query=", rewiseUrl, outFile) +
    string.Format("-G {0} -o \"{1}\" -H \"Accept:{2}\" -d query=", rewiseUrl, outFile + ".ttl", "text/turtle") +
      HttpUtility.UrlEncode(query); //.Replace("%","%%");

  const string allInstanceProps = @"
PREFIX dbnary: <http://kaiko.getalp.org/dbnary#>
PREFIX ontolex: <http://www.w3.org/ns/lemon/ontolex#>

CONSTRUCT {?type ?property ?dataType}
WHERE {
	SELECT DISTINCT ?type ?property ?dataType
		WHERE 
		{ 	
			?obj a ?type .
			?obj ?property ?valueObj . 
		VALUES ?type { 
			dbnary:Translation
			ontolex:LexicalEntry
			ontolex:LexicalSense
			dbnary:Gloss
			ontolex:Form
			dbnary:Page
		}
    BIND(datatype(?valueObj) as ?dataType)
	}
}
";

  const string allInstancePropsWithType = @"
PREFIX dbnary: <http://kaiko.getalp.org/dbnary#>
PREFIX ontolex: <http://www.w3.org/ns/lemon/ontolex#>
PREFIX lexinfo: <http://www.lexinfo.net/ontology/2.0/lexinfo#>

CONSTRUCT {?type ?property ?valueType}
WHERE {
	SELECT DISTINCT ?type ?property ?valueType
		WHERE 
		{ 	
			?obj a ?type .
			?obj ?property ?valueObj . 
			?valueObj a ?valueType .
		VALUES ?type { 
			dbnary:Translation
			ontolex:LexicalEntry
			ontolex:LexicalSense
			dbnary:Gloss
			ontolex:Form
			dbnary:Page
		}
	}
}
";
  static string classIds(string className) => string.Format(@"
PREFIX ontolex: <http://www.w3.org/ns/lemon/ontolex#>
PREFIX dbnary: <http://kaiko.getalp.org/dbnary#>
PREFIX : <l:>
CONSTRUCT {{:e :e ?obj}}
WHERE {{
	SELECT ?obj
		WHERE 
		{{ 	
			?obj2 a {0} .
      BIND( SUBSTR( STR(?obj2), 36) as ?obj)
		}}
}}
", className);

  static string dataPrefixes = @"
PREFIX on: <http://www.w3.org/ns/lemon/ontolex#>
PREFIX db: <http://kaiko.getalp.org/dbnary#>
PREFIX : <l:>
PREFIX t: <t:> # Translation
PREFIX e: <e:> # LexicalEntry
PREFIX g: <g:> # Gloss
PREFIX p: <p:> # Page
PREFIX s: <s:> # LexicalSense
PREFIX f: <f:> # Form
PREFIX m: <m:> # Form
";

  static string dataQuery(string from, string sfrom, string to, string sto, string preds) => string.Format(@"
CONSTRUCT {{?st ?p ?so}}
WHERE {{
	SELECT ?st ?p ?so
	WHERE {{

    ?s a {0} .
    ?o a {2} .
   	?s ?p ?o .
    
    BIND( URI( CONCAT(""{1}:"", SUBSTR( STR(?s), 36))) as ?st)
    BIND( URI( CONCAT(""{3}:"", SUBSTR( STR(?o), 36))) as ?so)
    VALUES ?p {{ {4} }} 
  }}
}}
# LIMIT 1000
", from, sfrom, to, sto, preds);

  const string nyms = "db:antonym db:holonym db:hypernym db:hyponym db:meronym db:synonym db:troponym";

  static IEnumerable<queryFile> dataQueries() {
    // Page
    yield return new queryFile {
      file = "relPageSynonymsPage",
      query = dataQuery("db:Page", "p", "db:Page", "p", nyms)
    };
    yield return new queryFile {
      file = "relPageDescrEntry",
      query = dataQuery("db:Page", "p", "on:LexicalEntry", "e", "db:describes")
    };
    yield return new queryFile {
      file = "relPageDescrMulti",
      query = dataQuery("db:Page", "p", "on:MultiWordExpression", "m", "db:describes")
    };

    // LexicalSense
    yield return new queryFile {
      file = "relSenseSynonymsPage",
      query = dataQuery("on:LexicalSense", "s", "db:Page", "p", nyms)
    };

    // LexicalEntry
    yield return new queryFile {
      file = "relEntrySynonymsPage",
      query = dataQuery("on:LexicalEntry", "e", "db:Page", "p", nyms)
    };
    yield return new queryFile {
      file = "relEntryCanformForm",
      query = dataQuery("on:LexicalEntry", "e", "on:Form", "f", "on:canonicalForm")
    };
    yield return new queryFile {
      file = "relEntryOtherformForm",
      query = dataQuery("on:LexicalEntry", "e", "on:Form", "f", "on:otherForm")
    };
    yield return new queryFile {
      file = "relEntrySenseSense",
      query = dataQuery("on:LexicalEntry", "e", "on:LexicalSense", "s", "on:sense")
    };

    // Translation
    yield return new queryFile {
      file = "relTransGloss",
      query = dataQuery("db:Translation", "t", "db:Gloss", "g", "db:gloss")
    };
    yield return new queryFile {
      file = "relTransTransEntry",
      query = dataQuery("db:Translation", "t", "on:LexicalEntry", "e", "db:isTranslationOf")
    };
    yield return new queryFile {
      file = "relTransTransSense",
      query = dataQuery("db:Translation", "t", "on:LexicalSense", "s", "db:isTranslationOf")
    };
  }

  class queryFile {
    public string query;
    public string file;
  }

  static string[] classes = new string[] {
      "dbnary:Translation",
      "ontolex:LexicalEntry",
      "ontolex:LexicalSense",
      "dbnary:Gloss",
      "ontolex:Form",
      "dbnary:Page",
  };

}
