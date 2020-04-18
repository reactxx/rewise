
// source data: d:\rewise\data\wordnet\download\all+xml.zip
// from ZIP: 
// - wns\cldr\ + wns\wikt\ => d:\rewise\data\wordnet\wn-wikt\
// - wns\???\wn-???-lmf.xml => d:\rewise\data\wordnet\
public static class WordNetIndex {
  public static void run() {
    wordNet.Parser.xmlToDBSecondPhase();
    wordNet.Parser.dbStat();
    wordNet.Dumps.dumps();
    wordNet.Dumps.lemmas();
    wordNet.Dumps.langLemmas();
  }

  public static void runDoc() {
    // parse wn-???-lmf.xml, get d:\rewise\data\wordnet\ids.txt
    wordNet.Parser.xmlToDBFirstPhase();

    // parse wn-???-lmf.xml again, create and fill "wordnetDB" SQL database
    // at the end, call WnWikt.run
    wordNet.Parser.xmlToDBSecondPhase();

    // dump "dbStat.txt"
    wordNet.Parser.dbStat();
    // dump/???.txt
    wordNet.Dumps.dumps();
    // dump/eng_lemmas*.txt
    wordNet.Dumps.lemmas();
    // dump-words/???.txt
    wordNet.Dumps.langLemmas();

    // ================== HELPERS, called in xmlToDBSecondPhase
    // call createNewSource(), dump i to wn-wikt.json and insert new entries, langs and translations to "wordnetDB"
    WnWikt.run();

    // imports wn-wikt\.* to memory and merge it with "wordnetDB"
    WnWikt.createNewSource();
  }
}

