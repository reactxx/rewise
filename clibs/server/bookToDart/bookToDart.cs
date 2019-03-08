public static class BookToDart {
  public class FactSrc {
    public string left;
    public string right;
    public int lesson;
  }
  public class BookSrc {
    public string id; // guid ID
    public DartDom.Meta meta;
    public string srcLang;
    public string destLang;
  }
  public static DartDom.Book import(BookSrc meta, FactSrc[] facts) {
    var parsed = FactSrcParser.parse(meta.srcLang, meta.destLang, facts);
    var srcStemm = FactStemming.stemming(meta.srcLang, parsed.Item1);
    var destStemm = FactStemming.stemming(meta.destLang, parsed.Item2);
    var res = new DartDom.Book {
      id = meta.id,
      meta = meta.meta,
      src = new DartDom.BookLang {
        lang = meta.srcLang,
        factsDir = parsed.Item1,
        tries = srcStemm.Item1,
        groupsFactIds = srcStemm.Item2,
      },
      dest = new DartDom.BookLang {
        lang = meta.destLang,
        factsDir = parsed.Item2,
        tries = destStemm.Item1,
        groupsFactIds = destStemm.Item2,
      },
    };
    return res;
  }
}
