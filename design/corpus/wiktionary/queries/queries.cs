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
    //yield return curlCmd(drootDir + "allInstancePropsWithType.", allInstancePropsWithType);
    //yield return curlCmd(drootDir + "allInstanceProps.", allInstanceProps);
    foreach (var cls in classes)
      yield return curlCmd(drootDir + cls.Split(':')[1] + ".", classIds(cls));
  }

  const string rewiseUrl = "http://localhost:7200/repositories/rewisse";

  static string curlCmd(string outFile, string query, bool isTurtle = true) =>
    //string.Format("-G {0} -o \"{1}\" -H \"Accept:application/x-trig\" -d query=", rewiseUrl, outFile) +
    string.Format("-G {0} -o \"{1}\" -H \"Accept:{2}\" -d query=", rewiseUrl, outFile + (isTurtle ? "ttl" : "json"), isTurtle ? "text/turtle" : "application/rdf+json") +
      HttpUtility.UrlEncode(query); //.Replace("%","%%");
    
  //const string allInstanceProps2 = @"CONSTRUCT {?type ?property ?dataType} WHERE {?type ?property ?dataType}";

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
	SELECT DISTINCT ?obj
		WHERE 
		{{ 	
			?obj2 a {0} .
      BIND( SUBSTR( STR(?obj2), 36) as ?obj)
		}}
}}
", className);

  static string[] classes = new string[] {
      "dbnary:Translation",
      "ontolex:LexicalEntry",
      "ontolex:LexicalSense",
      "dbnary:Gloss",
      "ontolex:Form",
      "dbnary:Page",
  };

}
