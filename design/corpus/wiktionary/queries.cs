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
    foreach (var q in relQueries())
      yield return curlCmd(drootDir + q.file, dataPrefixes + q.query);
    foreach (var cls in classMap.Values)
      yield return curlCmd(drootDir + "ids" + cls.Split(':')[1] + ".", dataPrefixes + idsQuery(cls));

    //yield return curlCmd(drootDir + "allInstancePropsWithType", dataPrefixes + allInstancePropsWithType);
    //yield return curlCmd(drootDir + "allInstanceProps", dataPrefixes + allInstanceProps);
  }

  const string rewiseUrl = "http://localhost:7200/repositories/rewisse";

  static string curlCmd(string outFile, string query) =>
    //string.Format("-G {0} -o \"{1}\" -H \"Accept:application/x-trig\" -d query=", rewiseUrl, outFile) +
    string.Format("-G {0} -o \"{1}\" -H \"Accept:{2}\" -d query=", rewiseUrl, outFile + ".ttl", "text/turtle") +
      HttpUtility.UrlEncode(query); //.Replace("%","%%");

  /*****************************************************************
   * CLASSED
   *****************************************************************/

  static Dictionary<string, string> classMap = new Dictionary<string, string> {
    { "t", "db:Translation" },
    { "g", "db:Gloss"},
    { "p", "db:Page"},
    { "e", "on:LexicalEntry"},
    { "s", "on:LexicalSense"},
    { "f", "on:Form"},
    { "m", "on:MultiWordExpression"},
  };

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
PREFIX m: <m:> # MultiWordExpression
";

  /*****************************************************************
   * SELECT META INFOS
   *****************************************************************/

  const string allInstanceProps = @"
CONSTRUCT {?type ?property ?dataType}
WHERE {
	SELECT DISTINCT ?type ?property ?dataType
		WHERE 
		{ 	
			?obj a ?type .
			?obj ?property ?valueObj . 
		VALUES ?type { 
			db:Translation
			on:LexicalEntry
			on:LexicalSense
			db:Gloss
			on:Form
			db:Page
		}
    BIND(datatype(?valueObj) as ?dataType)
	}
}
";

  const string allInstancePropsWithType = @"
CONSTRUCT {?type ?property ?valueType}
WHERE {
	SELECT DISTINCT ?type ?property ?valueType
		WHERE 
		{ 	
			?obj a ?type .
			?obj ?property ?valueObj . 
			?valueObj a ?valueType .
		VALUES ?type { 
			db:Translation
			on:LexicalEntry
			on:LexicalSense
			db:Gloss
			on:Form
			db:Page
		}
	}
}
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
", className);

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
    
    BIND( URI( CONCAT(""{1}:"", SUBSTR( STR(?s), 36))) as ?st)
    BIND( URI( CONCAT(""{3}:"", SUBSTR( STR(?o), 36))) as ?so)
    VALUES ?p {{ {4} }} 
  }}
}}
LIMIT 1000
", classMap[sfrom], sfrom, classMap[sto], sto, preds);

  const string nyms = "db:antonym db:holonym db:hypernym db:hyponym db:meronym db:synonym db:troponym";

  static IEnumerable<relQueryFile> relQueries() {
    // Page
    yield return new relQueryFile {
      file = "relPageSynonymsPage",
      query = relQuery("p", "p", nyms)
    };
    yield return new relQueryFile {
      file = "relPageDescrEntry",
      query = relQuery("p", "e", "db:describes")
    };
    yield return new relQueryFile {
      file = "relPageDescrMulti",
      query = relQuery("p", "m", "db:describes")
    };

    // LexicalSense
    yield return new relQueryFile {
      file = "relSenseSynonymsPage",
      query = relQuery("s", "p", nyms)
    };

    // LexicalEntry
    yield return new relQueryFile {
      file = "relEntrySynonymsPage",
      query = relQuery("p", "p", nyms)
    };
    yield return new relQueryFile {
      file = "relEntryCanformForm",
      query = relQuery("e", "f", "on:canonicalForm")
    };
    yield return new relQueryFile {
      file = "relEntryOtherformForm",
      query = relQuery("e", "f", "on:otherForm")
    };
    yield return new relQueryFile {
      file = "relEntrySenseSense",
      query = relQuery("e", "s", "on:sense")
    };

    // Translation
    yield return new relQueryFile {
      file = "relTransGloss",
      query = relQuery("t", "g", "db:gloss")
    };
    yield return new relQueryFile {
      file = "relTransTransEntry",
      query = relQuery("t", "e", "db:isTranslationOf")
    };
    yield return new relQueryFile {
      file = "relTransTransSense",
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
    ?o a {2} .
   	?s ?p ?o .
    
    BIND( URI( CONCAT(""{1}:"", SUBSTR( STR(?s), 36))) as ?st)
    BIND( URI( CONCAT(""{3}:"", SUBSTR( STR(?o), 36))) as ?so)
    VALUES ?p {{ {4} }} 
  }}
}}
# LIMIT 1000
", classMap[sfrom], sfrom, classMap[sto], sto, preds);


}
