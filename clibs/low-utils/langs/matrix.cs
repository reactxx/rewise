using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public class LangMatrix {
  public LangMatrix() { }
  public LangMatrix(int valuesCount, IEnumerable<string> _langs = null) {
    langs = (_langs != null ? _langs : Langs.meta.Select(l => l.id)).OrderBy(s => s).ToArray();
    data = new string[langs.Length][];
    for (var i = 0; i < data.Length; i++) data[i] = new string[valuesCount];
    loaded();
  }
  public LangMatrix(string[] _values): this(_values.Length) {
    values = _values;
    loaded();
  }
  public LangMatrix loaded() {
    if (langsDir == null) {
      var idx = 0;
      langsDir = langs.ToDictionary(s => s, s => idx++);
    }
    if (valuesIdx == null && values!=null) {
      var idx = 0;
      valuesIdx = values.ToDictionary(s => s, s => idx++);
    }
    return this;
  } 
  public string[][] data;
  public string[] langs;
  public string[] values; // can be null
  // indexers
  public string[] this[string idx] { get { return data[langsDir[idx]]; } }
  public string this[string idx, int idx2] { get { return data[langsDir[idx]][idx2]; } }
  public string this[string idx, string idx2] { get { return data[langsDir[idx]][valuesIdx[idx2]]; } } // exception when values is null
  // dirs
  [JsonIgnore]
  public Dictionary<string /*lang <id>*/, int /*lang's values*/> langsDir;
  [JsonIgnore]
  public Dictionary<string, int> valuesIdx; // can be null

  }


