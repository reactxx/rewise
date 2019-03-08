using System.IO;
using System.Linq;

public static class MatrixToDart {
  public class FactIn {
    public FactSideIn[] sides;
    public int lesson;
  }
  public class FactSideIn {
    public string lang;
    public string text;
  }
  public class BookIn {
    public string name;
    public Meta meta;
    public string srcLang;
  }
  public class Meta {
  }
  public class BookOut: BookIn {
    public string[] destLangs;
    public FactOut[] façts;
  }
  public class FactOut {
    public FactSideOut[] sides;
    public WordClass wordClass; // word class
    public int lessonId;
  }
  public class FactSideOut {
    public string lang;
    public string text;
    public int[] breaks;
    public string toStemm;
  }
  public enum WordClass { }

  public static BookOut import(string matrixFn, string metaFn, string srcLang /*null => all langs*/) {
    var matrix = new LangMatrix(matrixFn);
    matrix.save(@"c:\temp\test.csv");
    return;
    var meta = metaFn == null ? null : Json.Deserialize<Meta>(metaFn);
    var bookIn = new BookIn { meta = meta, srcLang = srcLang, name = Path.GetFileNameWithoutExtension(matrixFn).ToLower() };
    var destLangs = matrix.langs.Where(l => l!="Lesson").Select(l => Langs.oldToNew(l)).ToArray();
    
    return null;
  }

    public static BookOut import(BookIn meta, string[] destLangs, FactIn[] facts) {
    return new MatrixToDart.BookOut {
      name = meta.name,
      meta = meta.meta,
      srcLang = meta.srcLang,
      destLangs = destLangs,
      façts = FactInParser.parse(meta, facts),
    };
  }
}

public static class FactInParser {
  public static MatrixToDart.FactOut[] parse(MatrixToDart.BookIn meta, MatrixToDart.FactIn[] facts) {
    return null;
  }
}
