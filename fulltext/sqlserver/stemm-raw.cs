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

      Stemming.getStemms(words, lang, stems => {

        Console.Write(string.Format("\rAttempt: {0}/{1}, batches: {2}      ", res.attemptCounts.Count, res.attemptCounts[res.attemptCounts.Count - 1], ++res.batchCount * batchSize));

        var stems2 = new List<Tuple<Guid, string[]>>();
        foreach (var stem in stems) {
          var arr = stem.stemms.Split(',');
          if (arr.Length == 1)
            continue;
          var hash = new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes(stem.stemms)));
          stems2.Add(Tuple.Create(hash, arr));

          // assign id to word (first words => lower id)
          if (!res.srcDone) {
            var lw = stem.word.ToLower();
            if (Array.IndexOf(arr, lw) >= 0) // source word is in stemms
              if (!res.wordsIdx.ContainsKey(lw)) res.wordsIdx[lw] = res.wordAutoIncrement++;
          }
        }

        foreach (var stemms in stems2) {

          // adjust stemms word Ids
          var ids = stemms.Item2.Select(w => {
            if (!res.wordsIdx.TryGetValue(w, out int wid)) res.wordsIdx[w] = wid = res.wordAutoIncrement++;
            return wid;
          }).ToArray();
          Array.Sort(ids);

          // stemm group already exists => continue
          if (res.groups2.ContainsKey(stemms.Item1)) continue;

          // create stemm group
          res.groups[stemms.Item1] = new Group() {
            id = ++res.groupIdAutoIncrement,
            wordIds = ids
          };
        }
      }, batchSize);
    }

    public static string[] getTodoWords(RawData res) {
      return res.words2.Where(nv => {
        if (nv.Value[0] != 0) return false;
        nv.Value[0] = -1;
        return true;
      }).Select(nv => nv.Key).ToArray();
    }

    public static void getAllStemms(RawData res, int attempts, string[] initialWords, LangsLib.langs lang, int batchSize = int.MaxValue) {
      if (attempts <= 0) attempts = 32;
      res.start = DateTime.Now;
      for (var i = 0; i < attempts; i++) {
        var words = i == 0 ? initialWords : getTodoWords(res);
        res.attemptCounts.Add(words.Length);
        res.batchCount = 0;
        if (words.Length == 0)
          break;
        getAllStemms(res, words, lang, batchSize);
        if (res.wordsCount == res.words2.Count)
          break;
        res.wordsCount = res.words2.Count;
      }
    }

    public static void dumpAllStemmsResult(RawData res, string fn) {
      res.wordsCount = res.words2.Count;
      res.groupsCount = res.groups2.Count;
      res.end = DateTime.Now;
      res.words2.Values.Aggregate((r, i) => {
        if (i[0] == 0) res.firstGroupZeroCount++;
        if (i.Count <= 1) return null;
        res.moreGroupsCount++;
        res.moreGroupsSum += i.Count - 1;
        return null;
      });
      if (File.Exists(fn)) File.Delete(fn);
      var ser = new XmlSerializer(typeof(RawData));
      using (var fs = File.OpenWrite(fn))
        ser.Serialize(fs, res);
    }

    //static byte[] getHash(string str) {
    //  using (MD5 md5 = MD5.Create())
    //    return md5.ComputeHash(Encoding.UTF8.GetBytes(str));
    //}
  }

  public class MD5Comparer2 : IEqualityComparer<Guid> {
    public bool Equals(Guid a, Guid b) {
      return a.Equals(b);
      //for (var i = 0; i < a.Length; i++)
      //  if (a[i] != b[i]) return false;
      //return true;
    }

    public int GetHashCode(Guid a) {
      return a.GetHashCode();
      //byte b = 0;
      //for (var i = 0; i < a.Length; i++) b += a[i];
      //return b;
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
    public Dictionary<string, int> wordsIdx = new Dictionary<string, int>();
    //public string[] words = new string[0];
    public Dictionary<Guid, Group> groups = new Dictionary<Guid, Group>(new MD5Comparer2());
    public BitArray stemmed = new BitArray(32000000);

    public bool srcDone;
    public int groupIdAutoIncrement;
    public int wordAutoIncrement;

    public List<int> attemptCounts = new List<int>();
    public int batchCount;
    public DateTime start;
    public DateTime end;

    [XmlIgnore]
    public Dictionary<string, List<int>> words2 = new Dictionary<string, List<int>>(); // first item in the list is primary stemms set
    [XmlIgnore]
    public Dictionary<Guid, int> groups2 = new Dictionary<Guid, int>(new MD5Comparer2());
    //[XmlIgnore]
    //public Dictionary<string, ulong?> words = new Dictionary<string, ulong?>();
    //[XmlIgnore]
    //public HashSet<ulong> groups = new HashSet<ulong>();
    //public int moreGroupsCount;
    public int singleWordStemmsCount;
    public List<string> wordNotInStemmsLog = new List<string>();
    public static ulong singleStemmsHashValue = 1234567890;
    public int wordsCount;
    public int groupsCount;
    public int moreGroupsCount;
    public int moreGroupsSum;
    public int firstGroupZeroCount;
  }


}
