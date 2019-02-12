using System.Collections.Generic;
using Sepia.Globalization;
using System.IO;
using System.Linq;
using System.Text;


public class LangMatrixWrapper {
  public string[] values;
}

public class LangMatrixValues<T> where T : LangMatrixWrapper {
  public string lang;
  public T wrapper;
}

public abstract class LangMatrixLow<T> where T : LangMatrixWrapper {

  public T[] data;
  public string[] langs;
  public string[] values; // can be null

  abstract protected T wrapp(string[] values);

  public LangMatrixLow() { }

  public LangMatrixLow(IEnumerable<LangMatrixValues<T>> values) {
    var vals = values.NotNulls().OrderBy(v => v.lang).ToArray();
    langs = vals.Select(v => v.lang).ToArray();
    data = vals.Select(v => v.wrapper).ToArray();
  }

  public LangMatrixLow(string path) : this(new StreamReader(path)) { }

  public LangMatrixLow(StreamReader rdr) : this() {
    try {
      var lines = readRaw(rdr);
      var cell00 = lines[0].lang.Split('/');
      var groupDuplicity = cell00[0] == "langs";
      var dataList = new List<string[]>();
      var langsList = new List<string>();
      if (cell00[1] == "values") values = lines[0].wrapper.values;
      lines.Skip(1).ForEach(l => {
        foreach (var lang in groupDuplicity ? l.lang.Split(',') : Linq.Items(l.lang)) {
          langsList.Add(lang);
          dataList.Add(l.wrapper.values);
        }
      });
      langs = langsList.ToArray();
      data = dataList.Select(arr => wrapp(arr)).ToArray();
    } finally { rdr.Close(); }
  }

  // ******* indexers
  public T this[string lang] { get { return data[langsDir[lang]] as T; } }
  public string this[string lang, int valueIdx] { get { return this[lang].values[valueIdx]; } }
  public string this[string lang, string value] { get { return this[lang].values[valuesDir[value]]; } } // exception when values is null
  public T this[LocaleIdentifier lang] { get { return data[locsDir[lang]] as T; } }
  public string this[LocaleIdentifier lang, int valueIdx] { get { return this[lang].values[valueIdx]; } }
  public string this[LocaleIdentifier lang, string value] { get { return this[lang].values[valuesDir[value]]; } } // exception when values is null

  public Dictionary<string /*lang <id>*/, int /*lang's values*/> langsDir { get { var idx = 0; return _langsDir ?? (_langsDir = langs.ToDictionary(s => s, s => idx++)); } }
  public Dictionary<string, int> valuesDir { get { var idx = 0; return _valuesDir ?? (_valuesDir = values.ToDictionary(s => s, s => idx++)); } }
  public Dictionary<LocaleIdentifier, int> locsDir { get { var idx = 0; return _locsDir ?? (_locsDir = langs.ToDictionary(s => LocaleIdentifier.Parse(s), s => idx++, LangMatrixComparer.Comparer)); } }

  Dictionary<string, int> _langsDir;
  Dictionary<string, int> _valuesDir;
  public Dictionary<LocaleIdentifier, int> _locsDir;

  // ******* save x load

  public LangMatrixValues<T>[] readRaw(StreamReader rdr) {
    return rdr.ReadAllLines().Select(r => r.Split(new char[] { ';' }, 2)).Select(r => new LangMatrixValues<T> { lang = r[0], wrapper = wrapp(r[1].Split(';')) }).ToArray();
  }
  public string[] readLangs(StreamReader rdr) {
    return rdr.ReadAllLines().Skip(1).Select(r => r.Split(new char[] { ';' }, 2)).Select(r => r[0]).ToArray();
  }
  public struct RawLine { public string col0; public string[] row; }

  public void save(string path, bool groupDuplicity = false) {
    using (var wr = new StreamWriter(path, false, Encoding.UTF8))
      save(wr, groupDuplicity);
  }

  public void save(StreamWriter wr, bool groupDuplicity = false) {
    var sb = new StringBuilder();
    WriteCsvRow(wr,
      string.Format("{0}/{1}", groupDuplicity ? "langs" : "lang", values == null ? "" : "values"),
      values == null ? Enumerable.Range(0, data[0].values.Length).Select(v => v.ToString()) : values,
      sb);
    if (groupDuplicity) {
      data.
        Select((arr, idx) => new { lang = langs[idx], rowText = arr.values.JoinStrings(";", sb) }).
        GroupBy(g => g.rowText).
        ForEach(g => {
          WriteCsvRow(wr,
            g.Select(it => it.lang).JoinStrings(",", sb),
            g.First().rowText);
        });
    } else {
      data.ForEach((row, idx) => WriteCsvRow(wr,
        langs[idx],
        row.values,
        sb));
    }
  }

  static void WriteCsvRow(StreamWriter wr, string header, string row) {
    wr.Write(header);
    wr.Write(';');
    wr.Write(row);
    wr.WriteLine();
  }
  static void WriteCsvRow(StreamWriter wr, string header, IEnumerable<string> row, StringBuilder sb) {
    WriteCsvRow(wr, header, row.JoinStrings(";", sb));
  }

}

// ******** Comparer


public class LangMatrixComparer : IEqualityComparer<LocaleIdentifier>, IComparer<LocaleIdentifier> {

  public static LangMatrixComparer Comparer = new LangMatrixComparer();

  public bool Equals(LocaleIdentifier x, LocaleIdentifier y) {
    return x.ToString().Equals(y.ToString());
  }

  public int GetHashCode(LocaleIdentifier obj) {
    return obj.ToString().GetHashCode();
  }

  public int Compare(LocaleIdentifier x, LocaleIdentifier y) {
    return x.ToString().CompareTo(y.ToString());
  }

}

