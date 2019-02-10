using System.Collections.Generic;
using Sepia.Globalization;
using System.IO;
using System.Linq;
using System.Text;

public class LangMatrix {

  public string[][] data;
  public string[] langs;
  public string[] values; // can be null

  public class Values {
    public string lang;
    public string[] values;
  }

  public LangMatrix() { }

  public LangMatrix(IEnumerable<Values> values) {
    var vals = values.NotNulls().OrderBy(v => v.lang).ToArray();
    langs = vals.Select(v => v.lang).ToArray();
    data = vals.Select(v => v.values).ToArray();
  }

  public LangMatrix(int valuesCount, IEnumerable<string> _langs = null) {
    langs = (_langs != null ? _langs : Langs.meta.Select(l => l.id)).OrderBy(s => s).ToArray();
    data = new string[langs.Length][];
    for (var i = 0; i < data.Length; i++) data[i] = new string[valuesCount];
  }

  public LangMatrix(string[] values, IEnumerable<string> langs = null) : this(values.Length, langs) {
    this.values = values;
  }

  // ******* indexers
  public string[] this[string lang] { get { return data[langsDir[lang]]; } }
  public string this[string lang, int valueIdx] { get { return data[langsDir[lang]][valueIdx]; } }
  public string this[string lang, string value] { get { return data[langsDir[lang]][valuesDir[value]]; } } // exception when values is null
  public string[] this[LocaleIdentifier lang] { get { return data[locsDir[lang]]; } }
  public string this[LocaleIdentifier lang, int valueIdx] { get { return data[locsDir[lang]][valueIdx]; } }
  public string this[LocaleIdentifier lang, string value] { get { return data[locsDir[lang]][valuesDir[value]]; } } // exception when values is null

  public Dictionary<string /*lang <id>*/, int /*lang's values*/> langsDir { get { var idx = 0; return _langsDir ?? (_langsDir = langs.ToDictionary(s => s, s => idx++)); } }
  public Dictionary<string, int> valuesDir { get { var idx = 0; return _valuesDir ?? (_valuesDir = values.ToDictionary(s => s, s => idx++)); } }
  public Dictionary<LocaleIdentifier, int> locsDir { get { var idx = 0; return _locsDir ?? (_locsDir = langs.ToDictionary(s => LocaleIdentifier.Parse(s), s => idx++, Comparer)); } }

  Dictionary<string, int> _langsDir;
  Dictionary<string, int> _valuesDir;
  public Dictionary<LocaleIdentifier, int> _locsDir;


  // ******* checkTexts
  public Dictionary<string, Dictionary<string, string>> checkTexts(int skip = 0, int take = int.MaxValue) {
    var res = new Dictionary<string, Dictionary<string, string>>();
    langs.ForEach(lang => {
      var id = LocaleIdentifier.Parse(lang);
      var wrongs = LangsLib.UnicodeBlockNames.checkBlockNames(this[lang].Skip(skip).Take(take), id.Script);
      if (wrongs == null) return;
      res[lang] = wrongs;
    });
    return res;
  }

  // ******* save x load
  public void save(string path, bool groupDuplicity = false) {
    using (var wr = new StreamWriter(path, false, Encoding.UTF8))
      save(wr, groupDuplicity);
  }

  public void save(StreamWriter wr, bool groupDuplicity = false) {
    var sb = new StringBuilder();
    wr.WriteCsvRow(
      string.Format("{0}/{1}", groupDuplicity ? "langs" : "lang", values == null ? "" : "values"),
      values == null ? Enumerable.Range(0, data[0].Length).Select(v => v.ToString()) : values);
    if (groupDuplicity) {
      data.
        Select((arr, idx) => new { lang = langs[idx], value = arr.JoinStrings(";", sb), row = arr }).
        GroupBy(g => g.value).
        ForEach(g => {
          wr.WriteCsvRow(
            g.Select(it => it.lang).JoinStrings(",", sb),
            g.First().row);
        });
    } else {
      data.ForEach((row, idx) => wr.WriteCsvRow(
        langs[idx],
        row));
    }
  }

  public static LangMatrix load(string path) {
    using (var rdr = new StreamReader(path))
      return load(rdr);
  }
  public static LangMatrix load(StreamReader rdr) {
    var lines = rdr.ReadAllLines().Select(r => r.Split(new char[] { ';' }, 2)).Select(r => new { col0 = r[0], row = r[1].Split(';') }).ToArray();
    var cell00 = lines[0].col0.Split('/');
    var groupDuplicity = cell00[0] == "langs";
    var res = new LangMatrix();
    var dataList = new List<string[]>();
    var langsList = new List<string>();
    if (cell00[1] == "values") res.values = lines[0].row;
    lines.Skip(1).ForEach(l => {
      foreach (var lang in groupDuplicity ? l.col0.Split(',') : Linq.Items(l.col0)) {
        langsList.Add(lang);
        dataList.Add(l.row);
      }
    });
    res.langs = langsList.ToArray();
    res.data = dataList.ToArray();
    return res;
  }

  // ******** Comparer

  public static LocaleIdentifierEqualityComparer Comparer = new LocaleIdentifierEqualityComparer();

  public class LocaleIdentifierEqualityComparer : IEqualityComparer<LocaleIdentifier>, IComparer<LocaleIdentifier> {
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

}

