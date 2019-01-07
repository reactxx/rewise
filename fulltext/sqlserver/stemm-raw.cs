using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Globalization;
using System.Collections;

namespace fulltext {

  public static class StemmingRaw {

    public static void getAllStemms(RawData res, string[] words, LangsLib.langs lang, int batchSize = int.MaxValue) {

      MD5 md5 = MD5.Create();
      var langId = new LangsLib.Metas().Items[lang].Id;

      Stemming.getStemms(words, lang, dbStems => {

        Console.Write(string.Format("\r{3} attempt: {0}/{1}, batches: {2}      ", res.attemptCount, res.attemptNum, ++res.batchCount * batchSize, langId));

        var stems = new List<Tuple<Guid, string[]>>();
        foreach (var stem in dbStems) {
          var arr = stem.stemms.Split(',');
          if (arr.Length == 1)
            continue;
          var hash = new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes(stem.stemms)));
          stems.Add(Tuple.Create(hash, arr));

          if (res.attemptCount==1) {
            // assign ID to word (first words => lower ID)
            string actWord = stem.word.ToLower();
            if (Array.IndexOf(arr, actWord) >= 0) { // source word is in stemms
              if (!res.wordsIdx.TryGetValue(actWord, out int actId)) res.wordsIdx[actWord] = actId = res.wordAutoIncrement++;
              res.done[actId] = true;
            }
          }
          
        }

        foreach (var stemms in stems) {

          // adjust stemms word IDs and add them to TODO
          var ids = stemms.Item2.Select(w => {
            if (!res.wordsIdx.TryGetValue(w, out int wid)) res.wordsIdx[w] = wid = res.wordAutoIncrement++;
            res.todo.Add(Tuple.Create(w, wid));
            return wid;
          }).ToArray();
          Array.Sort(ids);

          // stemm group already exists => continue
          if (res.groups.ContainsKey(stemms.Item1)) continue;

          // create stemm group
          res.groups[stemms.Item1] = new Group() {
            id = ++res.groupIdAutoIncrement,
            wordIds = ids
          };
        }
      }, batchSize);
    }

    public static string[] getTodoWords(RawData res) {
      var todo = res.todo;
      res.todo.Clear();
      return todo.Where(t => !res.done[t.Item2]).Select(t => {
        res.done[t.Item2] = true;
        return t.Item1;
      }).ToArray(); ;
    }

    public static void getAllStemms(RawData res, int attempts, string[] initialWords, LangsLib.langs lang, int batchSize = int.MaxValue) {
      if (attempts <= 0) attempts = 32;
      res.start = DateTime.Now;
      for (var i = 0; i < attempts; i++) {
        var words = i == 0 ? initialWords : getTodoWords(res);
        res.attemptCount++;
        res.attemptNum = words.Length;
        res.batchCount = 0;
        if (words.Length == 0)
          break;
        getAllStemms(res, words, lang, batchSize);
        if (res.wordsCount == res.wordsIdx.Count)
          break;
        res.wordsCount = res.wordsIdx.Count;
      }
    }

    public static void getAllStemms(string root) {
      var metas = new LangsLib.Metas();
      var srcDir = root + @"dicts_source\";
      var dumpDir = root + @"fulltext\sqlserver\dumps\";
      foreach (var lc in metas.Items.Values.Select(it => it.lc)) {
        var srcFn = srcDir + lc.Name + ".txt";
        if (!File.Exists(srcFn)) continue;
        var dumpFn = dumpDir + lc.Name + ".xml";
        if (File.Exists(dumpFn)) continue;
        try {
          var words = File.ReadAllLines(srcFn);
          var res = new RawData();
          getAllStemms(res, 0, words, (LangsLib.langs)lc.LCID, 5000);
          dumpAllStemmsResult(res, dumpFn);
        } catch (Exception e) {
          File.WriteAllText(dumpDir + lc.Name + ".log", e.Message + "\r\n" + e.StackTrace);
        }
      }

    }

    public static void dumpAllStemmsResult(RawData res, string fn) {
      res.wordsCount = res.wordsIdx.Count;
      res.groupsCount = res.groups.Count;
      res.end = DateTime.Now;
      //res.wordsIdx.Values.Aggregate((r, i) => {
      //  if (i[0] == 0) res.firstGroupZeroCount++;
      //  if (i.Count <= 1) return null;
      //  res.moreGroupsCount++;
      //  res.moreGroupsSum += i.Count - 1;
      //  return null;
      //});
      if (File.Exists(fn)) File.Delete(fn);
      var ser = new XmlSerializer(typeof(RawData));
      using (var fs = File.OpenWrite(fn))
        ser.Serialize(fs, res);
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

  public struct Group {
    public int id;
    public int[] wordIds;
  }

  public class Buff {
    public string[] words = new string[0];
    public int[][] groups = new int[0][];
  }

  public class RawData {
    [XmlIgnore]
    public Dictionary<string, int> wordsIdx = new Dictionary<string, int>();
    [XmlIgnore]
    public Dictionary<Guid, Group> groups = new Dictionary<Guid, Group>(new MD5Comparer2());
    [XmlIgnore]
    public BitArray done = new BitArray(32000000);
    [XmlIgnore]
    public List<Tuple<string, int>> todo = new List<Tuple<string, int>>();

    public int groupIdAutoIncrement;
    public int wordAutoIncrement;
    public int wordsCount;
    public int attemptCount;

    public int groupsCount;
    public int attemptNum;
    public int batchCount;
    public DateTime start;
    public DateTime end;

    //[XmlIgnore]
    //public Dictionary<string, List<int>> words2 = new Dictionary<string, List<int>>(); // first item in the list is primary stemms set
    //[XmlIgnore]
    //public Dictionary<Guid, int> groups2 = new Dictionary<Guid, int>(new MD5Comparer2());
    //[XmlIgnore]
    //public Dictionary<string, ulong?> words = new Dictionary<string, ulong?>();
    //[XmlIgnore]
    //public HashSet<ulong> groups = new HashSet<ulong>();
    //public int moreGroupsCount;
    //public int singleWordStemmsCount;
    //public List<string> wordNotInStemmsLog = new List<string>();
    //public static ulong singleStemmsHashValue = 1234567890;
    public int groupsCount;
    //public int moreGroupsCount;
    //public int moreGroupsSum;
    //public int firstGroupZeroCount;
  }


}
