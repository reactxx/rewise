using System;

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

// compress data in Dart
//public static class CompressData {
//  public static void run(DartDom.BookLang book) {
//    book.texts = new DartDom.CompressMap {
//      dataString = book.factsDir.Select(f => f.text).ToArray()
//    };
//    book.suffixes = new DartDom.CompressMap {
//      dataString = book.tries.prefixes.SelectMany(t => t.suffixes.Select(n => n.suffix)).ToArray()
//    };
//    book.suffixes = new DartDom.CompressMap {
//      dataInt = book.factsDir.Select(f => {
//        if (f.breaks.Length < 2) return null;
//        if (f.breaks.Length == 2 && f.breaks[0] == 0 && f.breaks[1] == f.text.Length) return null;
//        if (f.breaks[0] == 0) return f.breaks.Skip(1).ToArray();
//        return f.breaks;
//      }).ToArray()
//    };
//  }
//}