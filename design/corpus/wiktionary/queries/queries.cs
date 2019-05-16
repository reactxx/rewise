using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

public static class WiktQueries {

  public static void generateCmd() {
    var rootDir = Corpus.Dirs.wikiesDbnary + @"graphDBExport\";
    var drootDir = rootDir.ToLower().Replace("c:\\", "d:\\");
    using (var wr = new StreamWriter(rootDir + "export.cmd", false)) {
      System.Diagnostics.Process.Start("CMD.exe", curlCmd(drootDir + "allInstanceProps.ttl", allInstanceProps));
      return;
      wr.WriteLine(curlCmd(drootDir + "allInstanceProps.ttl", allInstanceProps));
      wr.WriteLine(curlCmd(drootDir + "allInstancePropsWithType.ttl", allInstancePropsWithType));
      foreach (var cls in classes)
        wr.WriteLine(curlCmd(drootDir + cls.Split(':')[1] + ".ttl", classIds(cls)));
    }
  }

  const string rewiseUrl = "http://localhost:7200/repositories/rewisse";

  static string curlCmd(string outFile, string query) =>
    string.Format("call curl -G {0} -o \"{1}\" -H \"Accept:application/x-trig\" -d query=", rewiseUrl, outFile) +
      HttpUtility.UrlEncode(query); //.Replace("%","%%");

  const string allInstanceProps2 = @"CONSTRUCT {?type ?property ?dataType} WHERE {?type ?property ?dataType}";

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

CONSTRUCT {?type ?property ?type}
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

  static string[] classes = new string[] {
      "dbnary:Translation",
      "ontolex:LexicalEntry",
      "ontolex:LexicalSense",
      "dbnary:Gloss",
      "ontolex:Form",
      "dbnary:Page",
  };

}
  