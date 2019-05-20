﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

// http://docs.rdf4j.org/rest-api/
// https://www.keycdn.com/support/popular-curl-examples
// import from h:\Users\pavel\graphdb-import\*.ttl 

public static class WiktQueries {

  /************************************************
   * PUBLIC
   ************************************************ */

  public static string[] allLangs = new string[] {
    //"bg",
    "en",
    "bg","de","el","es","fi","fr","id","it","ja","la","lt","mg","nl","no","pl","pt","ru","sh","sv","tr",
  };

  public static void exports() => parallelLangWithDirs((lang, langDir, schemeDirLang) => {

    IEnumerable<string> sparglExportArgs() {
      foreach (var q in propsQueries())
        yield return sparqlArg(lang, langDir + q.file.ToLower(), namespaces + q.query);
      foreach (var q in relQueries())
        yield return sparqlArg(lang, langDir + q.file.ToLower(), namespaces + q.query);
      foreach (var cls in classMap.Values)
        yield return sparqlArg(lang, langDir + "ids_" + clsToName[cls.Split(':')[1].ToLower()], namespaces + idsQuery(cls));
    }

    foreach (var arg in sparglExportArgs()) {
      var output = runCurl(arg);
    }
  });

  public static void imports() {
    parallelLang(async lang => {
      var name = $"dbnary_{ lang}";
      Console.WriteLine(name);
      // remove DB
      using (var client = new HttpClient()) {
        var res = client.DeleteAsync($"http://localhost:7200/rest/repositories/{name}").Result.Content as StreamContent;
        Console.WriteLine(await res.ReadAsStringAsync());
      }
      // create new DB => MultipartFormDataContent
      using (var client = new HttpClient()) {
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        using (var content = new MultipartFormDataContent()) {
          var fn = Directory.GetCurrentDirectory() + @"\wiktionary\createRepo.ttl";
          var txt = File.ReadAllText(fn).Replace("XXIDXX", name);
          content.Add(new StringContent(txt, Encoding.UTF8, "application/x-turtle"), "config", "config");
          var res = client.PostAsync($"http://localhost:7200/rest/repositories", content).Result.Content as StreamContent;
          Console.WriteLine(await res.ReadAsStringAsync());
        }
      }
      // import
      using (var client = new HttpClient()) {
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        var json = string.Format("{{'fileNames':['dbnary/{0}/{0}_dbnary_ontolex.ttl','dbnary/{0}/{0}_dbnary_morpho.ttl']}}", lang).Replace('\'', '"');
        using (var content = new StringContent(json, Encoding.UTF8, "application/json")) {
          var res = client.PostAsync($"http://localhost:7200/rest/data/import/server/{name}", content).Result.Content as StreamContent;
          Console.WriteLine(await res.ReadAsStringAsync());
        }
      }

      // wait import
      using (var client = new HttpClient()) {
        int time = 0;
        while (true) {
          Thread.Sleep(1000 * 10);
          time++;
          Console.WriteLine($"{name}: {time * 10}sec");
          var res = client.GetAsync($"http://localhost:7200/rest/data/import/active/{name}").Result.Content as StreamContent;
          var count = await res.ReadAsStringAsync();
          if (count == "0") break;
        }
        Console.WriteLine($"{name}: DONE");
      }


    });
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
  }

  public static void metaInfos() {
    parallelLangWithDirs(async (lang, langDir, schemeDirLang) => {
      Console.WriteLine($"{lang} start");
      var name = $"dbnary_{ lang}";
      var resultFn = schemeDirLang + "allProps";
      var query = namespaces + metaQuery;
      using (var client = new HttpClient()) {
        client.DefaultRequestHeaders.Add("Accept", "text/turtle");
        using (var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("query", query) })) {
          var res = client.PostAsync($"http://localhost:7200/repositories/{name}", content).Result.Content as StreamContent;
          var ttl = await res.ReadAsStringAsync();
          File.WriteAllText(resultFn, ttl);
          Console.WriteLine($"{lang} end");
        }
      }
      //var arg = sparqlArg(lang, schemeDirLang + "allProps", namespaces + metaQuery);
      //var output = runCurl(arg);
    });
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
  }

  /************************************************
   * PRIVATE
   ************************************************ */
  static string sparqlArg(string lang, string outFile, string query) {
    outFile = outFile == null ? "" : $"-o '{outFile}.ttl'";
    return $"-G {dbnaryUrl(lang)} {outFile} -H 'Accept:text/turtle' -d query={HttpUtility.UrlEncode(query)}";
  }

  static string dbnaryUrl(string lang) => $"http://localhost:7200/repositories/dbnary_{lang}";


  static void parallelLang(Action<string> doExport) {
    Parallel.ForEach(allLangs, new ParallelOptions { MaxDegreeOfParallelism = 2 }, doExport);
  }

  static void parallelLangWithDirs(Action<string, string, string> doExport) {
    parallelLang(lang => {
      var langDir = Corpus.Dirs.wiktDbnary + @"graphDBExport\" + lang + "\\";
      var schemeDir = Corpus.Dirs.wiktDbnary + @"graphDBExport\scheme\";
      var schemePrefix = schemeDir + lang + "_";
      if (!Directory.Exists(langDir)) Directory.CreateDirectory(langDir);
      if (!Directory.Exists(schemeDir)) Directory.CreateDirectory(schemeDir);
      doExport(lang, langDir, schemePrefix);
    });
  }

  static string runCurl(string args) {
    var process = new Process() {
      StartInfo = new ProcessStartInfo("curl.exe", args) {
        UseShellExecute = false,
        RedirectStandardOutput = true,
      }
    };
    var output = new StringBuilder();
    process.OutputDataReceived += (sender, a) => {
      Console.WriteLine(a.Data);
      output.AppendLine(a.Data);
    };
    process.Start();
    process.BeginOutputReadLine();
    process.WaitForExit();
    return output.ToString();
  }


  /*****************************************************************
   * CLASSED
   *****************************************************************/

  public static Dictionary<string, string> classes = new Dictionary<string, string> {
    { "dbnary:Translation", "t" },
    { "g", "dbnary:Gloss"},
    { "p", "dbnary:Page"},
    { "e", "ontolex:LexicalEntry"},
    { "s", "ontolex:LexicalSense"},
    { "f", "ontolex:Form"},
    { "m", "ontolex:MultiWordExpression"},
  };


  public static Dictionary<string, string> classMap = new Dictionary<string, string> {
    { "t", "dbnary:Translation" },
    { "g", "dbnary:Gloss"},
    { "p", "dbnary:Page"},
    { "e", "ontolex:LexicalEntry"},
    { "s", "ontolex:LexicalSense"},
    { "f", "ontolex:Form"},
    { "m", "ontolex:MultiWordExpression"},
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


  static string namespaces = @"
PREFIX ontolex: <http://www.w3.org/ns/lemon/ontolex#>
PREFIX dbnary: <http://kaiko.getalp.org/dbnary#>
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
  const string metaQuery = @"
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
  FILTER(isLiteral(?o))
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
    
    BIND( URI( CONCAT(""{1}{1}:"", SUBSTR( STR(?s), 36))) as ?st)
    BIND( URI( CONCAT(""{3}{3}:"", SUBSTR( STR(?o), 36))) as ?so)
    VALUES ?p {{ {4} }} 
  }}
}}
", classMap[sfrom], sfrom, classMap[sto], sto, preds);

  const string nyms = "dbnary:antonym dbnary:holonym dbnary:hypernym dbnary:hyponym dbnary:meronym dbnary:synonym dbnary:troponym";

  static IEnumerable<relQueryFile> relQueries() {
    // Page
    yield return new relQueryFile {
      file = "rel_Page_Synonyms_Page",
      query = relQuery("p", "p", nyms)
    };
    yield return new relQueryFile {
      file = "rel_Page_Descr_Entry",
      query = relQuery("p", "e", "dbnary:describes")
    };
    yield return new relQueryFile {
      file = "rel_Page_Descr_Multi",
      query = relQuery("p", "m", "dbnary:describes")
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
      query = relQuery("e", "f", "ontolex:canonicalForm")
    };
    yield return new relQueryFile {
      file = "rel_Entry_Otherform_Form",
      query = relQuery("e", "f", "ontolex:otherForm")
    };
    yield return new relQueryFile {
      file = "rel_Entry_Sense_Sense",
      query = relQuery("e", "s", "ontolex:sense")
    };

    // Translation
    yield return new relQueryFile {
      file = "rel_Trans_Gloss_Gloss",
      query = relQuery("t", "g", "dbnary:gloss")
    };
    yield return new relQueryFile {
      file = "rel_Trans_Trans_Entry",
      query = relQuery("t", "e", "dbnary:isTranslationOf")
    };
    yield return new relQueryFile {
      file = "rel_Trans_Trans_Sense",
      query = relQuery("t", "s", "dbnary:isTranslationOf")
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
", classMap[sfrom], sfrom, preds);

  static IEnumerable<relQueryFile> propsQueries() {
    // Trans
    yield return new relQueryFile {
      file = "prop_Trans_TargetLanguageCode",
      query = propsQuery("t", "dbnary:targetLanguageCode")
    };
    yield return new relQueryFile {
      file = "prop_Trans_Usage",
      query = propsQuery("t", "dbnary:usage")
    };
    yield return new relQueryFile {
      file = "prop_Trans_WrittenForm",
      query = propsQuery("t", "dbnary:writtenForm")
    };

    // Entry
    yield return new relQueryFile {
      file = "prop_Entry_PartOfSpeech",
      query = propsQuery("e", "dbnary:partOfSpeech")
    };
    yield return new relQueryFile {
      file = "prop_Entry_Language",
      query = propsQuery("e", "li:language")
    };

    // Sense
    yield return new relQueryFile {
      file = "prop_Sense_Number",
      query = propsQuery("s", "dbnary:senseNumber")
    };

    // Gloss
    yield return new relQueryFile {
      file = "prop_Gloss_Rank",
      query = propsQuery("g", "dbnary:rank")
    };
    yield return new relQueryFile {
      file = "prop_Gloss_SenseNumber",
      query = propsQuery("g", "dbnary:senseNumber")
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
      query = propsQuery("f", "ontolex:phoneticRep")
    };
    yield return new relQueryFile {
      file = "prop_Form_WrittenRep",
      query = propsQuery("f", "ontolex:writtenRep")
    };
  }
}


/*
 *********************** 1:n RELATION
PREFIX ontolex: <http://www.w3.org/ns/lemon/ontolex#>
PREFIX dbnary: <http://kaiko.getalp.org/dbnary#>

CONSTRUCT {?s ontolex:sense ?c}
WHERE {
SELECT ?s (COUNT(?o) as ?c) 
WHERE {
    ?s ontolex:sense ?o
}
GROUP BY ?s
HAVING (COUNT(?o) > 1)
LIMIT 10
} 

# WHERE's:
# ?o dbnary:describes ?s
# NO ?s ontolex:sense ?o
# ?s ontolex:canonicalForm ?o
# ?s dbnary:gloss ?o . ?s a dbnary:Translation .
# ?s dbnary:isTranslationOf ?o . ?s a dbnary:Translation

 *  ************************ EXPORT SCHEME
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
