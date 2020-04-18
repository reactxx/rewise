using System.IO;
using System.Linq;

public static class CorpusIndex {
  public static void run() {
    // sources in c:\Users\pavel\graphdb-import\dbnary, copyied to d:\rewise\data\wiktionary\dbnary\source-ttls
    // parses TTLs files and create IDS in d:\rewise\data\wiktionary\dbnary\db\??\*.txt
    WiktTtlParser.parseTtlsFirstRun();

    // parses TTLs files again, take IDS and create d:\rewise\data\wiktionary\dbnary\db\??\dbnary_Page.json
    WiktTtlParser.parseTtlsSecondRun(); // save to d:\rewise\data\wiktionary\dbnary\db\

    // dumps info from .JSONs to d:\rewise\data\wiktionary\dbnary\dumps\??.txt
    dumpAll();

    // ****************** HELPER

    // load parsed Wikt data (JSON fro parseTtlsSecondRun) to memory for dump etc. 
    WiktDB.loadData();
  }

  public static void dumpAll() {
    WiktDB.loadData();
    foreach (var lang in WiktConsts.AllLangs) {
      File.WriteAllLines(@"d:\rewise\data\wiktionary\dbnary\dumps\" + lang + ".txt",
        WiktDB.getObjsStr<WiktModel.Entry>(lang).Select(f => f.toString()));
    }
  }

}

