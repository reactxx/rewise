using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace fulltext {

  public class Dump {
    public int groupIdAutoIncrement;
    public int wordAutoIncrement;
    public DateTime start;
    public int duration;
    public int attemptNo;
  }

  public class StemmingRaw: Dump {

    int attemptCount;
    int attemptLen;
    CultureInfo lc;
    int batchSize;
    string root;
    Dictionary<string, int> wordsIdx = new Dictionary<string, int>();
    Dictionary<Guid, Group> groups = new Dictionary<Guid, Group>(new MD5Comparer2());
    BitArray done = new BitArray(32000000);
    HashSet<Todo> todo = new HashSet<Todo>(new TodoComparer());
    int lastWordsCount;

    public StemmingRaw(string root, LangsLib.langs lang, int batchSize = 5000) : this(root, new LangsLib.Metas().Items[lang].lc, batchSize) {

    }

    StemmingRaw(string root, CultureInfo lc, int batchSize = 5000) {
      this.lc = lc;
      this.batchSize = batchSize;
      this.root = root;
    }

    public static void processLangs(string root, int batchSize = 5000) {
      var metas = new LangsLib.Metas();
      var srcDir = root + @"dicts_source\";
      var dumpDir = root + @"fulltext\sqlserver\dumps-raw\";
      foreach (var lc in metas.Items.Values.Select(it => it.lc)) {
        var srcFn = srcDir + lc.Name + ".txt";
        if (!File.Exists(srcFn)) continue;
        var dumpFn = dumpDir + lc.Name + ".xml";
        if (File.Exists(dumpFn)) continue;
        new StemmingRaw(root, lc, batchSize).processLang(batchSize);
      }
    }

    public void processLang(int batchSize = 5000) {
      var dumpFn = root + @"fulltext\sqlserver\dumps-raw\" + lc.Name;
      try {
        var words = File.ReadAllLines(root + @"dicts_source\" + lc.Name + ".txt");
        getLangStemms(words);
        dumpLangStemms(dumpFn + ".xml");
        saveLangStemms(dumpFn);
      } catch (Exception e) {
        File.WriteAllText(dumpFn + ".log", e.Message + "\r\n" + e.StackTrace);
      }
    }

    void saveLangStemms(string fn) {
      using (var wordTxt = new StreamWriter(fn + "-word.txt"))
      using (var wordBinf = File.Create(fn + "-word.bin"))
      using (var wordBin = new BinaryWriter(wordBinf)) {
        wordTxt.Write(wordAutoIncrement); wordTxt.Write('|'); ; wordTxt.Write(lc.Name); wordTxt.Write('|'); ; wordTxt.Write('|');
        wordBin.Write(wordAutoIncrement);

      }
    }

    void getAllStemms(string[] words) {

      MD5 md5 = MD5.Create();

      Stemming.getStemms(words, (LangsLib.langs)lc.LCID, dbStems => {

        Console.Write(string.Format("\r{3} attempt: {0}/{1}, batches: {2}      ", attemptNo, attemptLen, ++attemptCount * batchSize, lc.EnglishName));

        var stems = new List<Tuple<Guid, string[]>>();
        foreach (var stem in dbStems) {
          if (stem == null || stem.stemms == null) continue;
          var arr = stem.stemms.Split(',');
          if (arr.Length == 1)
            continue;
          var hash = new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes(stem.stemms)));
          stems.Add(Tuple.Create(hash, arr));

          if (attemptNo == 1) {
            // assign ID to word (first words => lower ID)
            string actWord = stem.word.ToLower();
            if (Array.IndexOf(arr, actWord) >= 0) { // source word is in stemms
              if (!wordsIdx.TryGetValue(actWord, out int actId)) wordsIdx[actWord] = actId = ++wordAutoIncrement;
              done[actId] = true;
            }
          }

        }

        foreach (var stemms in stems) {

          // adjust stemms word IDs and add them to TODO
          var ids = stemms.Item2.Select(w => {
            if (!wordsIdx.TryGetValue(w, out int wid)) wordsIdx[w] = wid = ++wordAutoIncrement;
            todo.Add(new Todo() { word = w, id = wid });
            return wid;
          }).ToArray();
          Array.Sort(ids);

          // stemm group already exists => continue
          if (groups.ContainsKey(stemms.Item1)) continue;

          // create stemm group
          groups[stemms.Item1] = new Group() {
            id = ++groupIdAutoIncrement,
            wordIds = ids
          };
        }
      }, batchSize);
    }

    string[] getTodoWords() {
      var td = todo.Where(t => !done[t.id]).Select(t => {
        done[t.id] = true;
        return t.word;
      }).ToArray(); ;
      todo.Clear();
      return td;
    }

    void getLangStemms(string[] initialWords) {
      start = DateTime.Now;
      while (true) {
        var words = i == 0 ? initialWords : getTodoWords();
        attemptNo++;
        attemptLen = words.Length;
        attemptCount = 0;
        if (words.Length == 0)
          break;
        if (attemptNo == 1 && words.Length > 30000) {
          getAllStemms(words.Take(30000).ToArray());
          getAllStemms(words.Skip(30000).ToArray());
        } else
          getAllStemms(words);
        if (lastWordsCount == wordsIdx.Count)
          break;
        lastWordsCount = wordsIdx.Count;
      }
    }

    void dumpLangStemms(string fn) {
      duration = (int)Math.Round((DateTime.Now - start).TotalSeconds);
      if (File.Exists(fn)) File.Delete(fn);
      var ser = new XmlSerializer(typeof(Dump));
      using (var fs = File.OpenWrite(fn))
        ser.Serialize(fs, this);
    }

  }

  public class MD5Comparer2 : IEqualityComparer<Guid> {
    public bool Equals(Guid a, Guid b) {
      return a.Equals(b);
    }

    public int GetHashCode(Guid a) {
      return a.GetHashCode();
    }
  }

  public class TodoComparer : IEqualityComparer<Todo> {
    public bool Equals(Todo a, Todo b) {
      return a.id == b.id;
    }

    public int GetHashCode(Todo a) {
      return a.id;
    }
  }


  public struct Group {
    public int id;
    public int[] wordIds;
  }

  public struct Todo {
    public int id;
    public string word;
  }

  public class Buff {
    public string[] words = new string[0];
    public int[][] groups = new int[0][];
  }

}
