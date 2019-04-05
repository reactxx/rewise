using System.Collections.Generic;
using Sepia.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System;

public class LangMatrixRow {
  public string lang;
  public string[] row;
  public string[] columnNames;

  public bool isEmpty() {
    return this == null || row == null || row.All(r => r == null);
  }

  public void checkTexts(Dictionary<string, Dictionary<string, string>> protocol) {
    var locId = LocaleIdentifier.Parse(lang);
    var wrongs = UnicodeBlocks.checkBlockNames(row, locId.Script);
    if (wrongs == null) return;
    protocol[lang] = wrongs;
  }
}

// cell[0][0]: <langs>or<lang>/<colNames>or<>
// langs=> first column is comma delimited list of langs (else single lang)
// colNames=> cel[0][1..] are column names (LangMatrix.colNames) 
// if cell[0][0] does starts with "lang[s]/" => RJ dictionary primary data (inverted row x column, lang value could be "Lesson"). 
public class LangMatrix {

  public string[][] data;
  public string[] langs;
  public string[] colNames; // can be null

  public LangMatrix() { }

  public LangMatrix(IEnumerable<LangMatrixRow> rows, Dictionary<string, Dictionary<string, string>> protocol = null, bool testEmpty = false) {
    var vals = rows.NotNulls(t => testEmpty ? t.isEmpty() : false).OrderBy(v => v.lang).ToArray();
    // columnNames
    var rowsWithColumnNames = vals.Select(v => v.columnNames != null ? 1 : 0).Sum();
    if (rowsWithColumnNames == vals.Length) { // column name mode
      if (!vals.All(v => v.columnNames.Length == v.row.Length))
        throw new Exception();
      colNames = vals.SelectMany(v => v.columnNames).Distinct().OrderBy(s => s).ToArray();
      var cidx = 0;
      var colIdx = colNames.ToDictionary(s => s, s => cidx++);
      data = vals.Select(v => {
        var row = new string[colNames.Length];
        for (var i = 0; i < v.columnNames.Length; i++) {
          var name = v.columnNames[i]; var value = v.row[i];
          row[colIdx[name]] = value;
        }
        return row;
      }).ToArray();
    } else if (rowsWithColumnNames > 0) { // wrong column names
      throw new Exception();
    } else { // no columnNames
      var rowLen = vals[0].row.Length;
      if (!vals.All(v => v.row.Length == rowLen))
        throw new Exception();
      data = vals.Select(v => v.row).ToArray();
    }
    langs = vals.Select(v => v.lang).ToArray();
    if (protocol != null) vals.ForEach(v => v.checkTexts(protocol));
  }

  public LangMatrix(string path) : this(new StreamReader(path)) { }

  public LangMatrix(StreamReader rdr) : this() {
    try {
      var lines = rdr.ReadAllLines().ToArray();
      var cell00 = lines[0].Split(new char[] { ';' }, 2)[0].Split('/');
      LangMatrixRow[] rawLines = null;
      var data = new List<string[]>();
      if (cell00.Length != 2) { // => RJ import format: change COLS and ROWS
        var matxOld = lines.Select(l => l.Normalize().Split(';')).ToArray();
        var matxNew = new List<string[]>();
        for (var i = 0; i < matxOld[0].Length; i++) matxNew.Add(new string[matxOld.Length]);
        for (var i = 0; i < matxOld.Length; i++)
          for (var j = 0; j < matxOld[0].Length; j++)
            matxNew[j][i] = matxOld[i][j];

        rawLines = readRaw(matxNew);
        for (var i = 0; i < rawLines.Length; i++) rawLines[i].lang = Langs.oldToNew(rawLines[i].lang);

        rawLines = new LangMatrixRow[] { null }.Concat(rawLines).ToArray();
        cell00 = new string[] { "lang", "" };

        //var colsNum = rawLines.Length - 1;
        //var rowsNum = rawLines[0].row.Length + 1; // adds 1 for rawLines[].lang
        //langs = new string[rawLines[0].row.Length + 1];
        //for (var rowIdx = 0; rowIdx < rowsNum; rowIdx++) {
        //  if (rowIdx == 0) langs[0] = rawLines[0].lang;
        //  else langs[rowIdx] = rawLines[0].row[rowIdx - 1];
        //}
        //for (var i = 0; i < langs.Length; i++) langs[i] = Langs.oldToNew(langs[i]);
        //for (var rowIdx = 0; rowIdx < rowsNum; rowIdx++) {
        //  var row = new string[colsNum];
        //  for (var colIdx = 0; colIdx < colsNum; colIdx++) {
        //    if (rowIdx == 0) row[colIdx] = rawLines[colIdx + 1].lang;
        //    else row[colIdx] = rawLines[colIdx + 1].row[rowIdx - 1];
        //  }
        //  data.Add(row);
        //}
      } else {
        rawLines = readRaw(lines);
      }
      var langs = new List<string>();
      var groupTheSameRows = cell00[0] == "langs"; // group by rows
      if (cell00[1] == "colNames") colNames = rawLines[0].row; // save column names
      rawLines.Skip(1).ForEach(l => {
        foreach (var lang in groupTheSameRows ? l.lang.Split(',') : Linq.Items(l.lang)) {
          langs.Add(lang);
          data.Add(l.row);
        }
      });
      this.langs = langs.ToArray();

      this.data = data.ToArray();
    } finally {
      rdr.Close();
    }
  }

  // ******* indexers
  public string[] this[string lang] { get { return tryData(lang, out string[] res) ? res : null; } }
  public string this[string lang, int valueIdx] { get { return tryData(lang, out string[] res) ? res[valueIdx] : null; } }
  //public string this[string lang, string value] { get { return this[lang][valuesDir[value]]; } } // exception when values is null
  //public string[] this[LocaleIdentifier lang] { get { return data[locsDir[lang]]; } }
  //public string this[LocaleIdentifier lang, int valueIdx] { get { return this[lang][valueIdx]; } }
  //public string this[LocaleIdentifier lang, string value] { get { return this[lang][valuesDir[value]]; } } // exception when values is null

  public Dictionary<string /*lang <id>*/, int /*lang's values*/> langsDir { get { var idx = 0; return _langsDir ?? (_langsDir = langs.ToDictionary(s => s, s => idx++)); } }
  public Dictionary<string, int> valuesDir { get { var idx = 0; return _valuesDir ?? (_valuesDir = colNames.ToDictionary(s => s, s => idx++)); } }
  public Dictionary<LocaleIdentifier, int> locsDir { get { var idx = 0; return _locsDir ?? (_locsDir = langs.ToDictionary(s => LocaleIdentifier.Parse(s), s => idx++, LangMatrixComparer.Comparer)); } }
  public bool tryData(string lang, out string[] d) {
    if (langsDir.TryGetValue(lang, out int idx)) { d = data[idx]; return true; } else { d = null; return false; }
  }

  Dictionary<string, int> _langsDir;
  Dictionary<string, int> _valuesDir;
  public Dictionary<LocaleIdentifier, int> _locsDir;


  // ******* save x load
  public static LangMatrixRow[] readRaw(List<string[]> lines) {
    return lines.Select(r => new LangMatrixRow { lang = r[0], row = r.Skip(1).Select(c => c == "" ? null : c).ToArray() }).ToArray();
  }

  public static LangMatrixRow[] readRaw(string[] lines) {
    return lines.Select(r => r.Split(new char[] { ';' }, 2)).Select(r => new LangMatrixRow { lang = r[0], row = r[1].Split(';').Select(c => c == "" ? null : c).ToArray() }).ToArray();
  }
  public static string[] readLangs(StreamReader rdr) {
    return rdr.ReadAllLines().Skip(1).Select(r => r.Split(new char[] { ';' }, 2)).Select(r => r[0]).ToArray();
  }
  public static string[] readLangs(string path) {
    using (var rdr = new StreamReader(path))
      return readLangs(rdr);
  }
  public struct RawLine { public string col0; public string[] row; }

  public void save(string path, bool groupTheSameRows = false) {
    using (var wr = new StreamWriter(path, false, Encoding.UTF8))
      save(wr, groupTheSameRows);
  }

  public void saveRaw(StreamWriter wr) {
    var sb = new StringBuilder();
    data.ForEach((row, idx) => WriteCsvRow(wr,
      langs[idx],
      row,
      sb));
  }
  public void save(StreamWriter wr, bool groupTheSameRows = false) {
    var sb = new StringBuilder();
    WriteCsvRow(wr,
      string.Format("{0}/{1}", groupTheSameRows ? "langs" : "lang", colNames == null ? "" : "colNames"),
      colNames == null ? Enumerable.Range(0, data[0].Length).Select(v => v.ToString()) : colNames,
      sb);
    if (groupTheSameRows) {
      data.
        Select((arr, idx) => new { lang = langs[idx], rowText = arr.JoinStrings(";", sb) }).
        GroupBy(g => g.rowText).
        ForEach(g => {
          WriteCsvRow(wr,
            g.Select(it => it.lang).JoinStrings(",", sb),
            g.First().rowText);
        });
    } else {
      data.ForEach((row, idx) => WriteCsvRow(wr,
        langs[idx],
        row,
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

