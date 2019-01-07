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

namespace fulltext {

  public static class StemmingAll {

    public static void getAllStemms(GetAllStemmsResult res, string[] words, LangsLib.langs lang, int batchSize = int.MaxValue) {
      MD5 md5 = MD5.Create();
      var langId = new LangsLib.Metas().Items[lang].Id;
      //var comparer = new Comparer(new LangsLib.Metas().Items[lang].lc);
      Stemming.getStemms(words, lang, stems => {
        Console.Write(string.Format("\r{3} attempt: {0}/{1}, batches: {2}      ", res.attemptCounts.Count, res.attemptCounts[res.attemptCounts.Count-1], ++res.batchCount * batchSize, langId));
        foreach (var stem in stems) {
          if (stem == null || stem.stemms==null) continue;
          var stemms = stem.stemms.Split(',');
          if (stemms.Length == 1) {
            res.singleWordStemmsCount++;
            continue;
          }
          var wordLower = stem.word.ToLower();
          var word = stemms.FirstOrDefault(s => wordLower == s);

          var hash = new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes(stem.stemms)));
          int id;
          if (!res.groups.TryGetValue(hash, out id))
            res.groups[hash] = id = ++res.groupAutoIncrement;

          // save value
          foreach (var st in stemms) {
            List<int> oldList;
            var found = res.words.TryGetValue(st, out oldList);
            if (st == word) {
              if (found) {
                var idIdx = oldList.IndexOf(id);
                if (idIdx > 0)
                  oldList.RemoveAt(idIdx); // maybe remove noticed secondary stemms
                oldList[0] = id;
              } else
                res.words[st] = new List<int>() { id }; // set primary stemms to newly created word
            } else {
              if (found) {
                if (oldList[0] == id)
                  continue;
                var idIdx = oldList.IndexOf(id);
                if (idIdx < 0)
                  oldList.Add(id); // add secondary stemms
              } else
                res.words[st] = new List<int>() { 0, id }; // create todo Word, notice secondary stemms
            }
          }
        }
      }, batchSize);
    }

    public static string[] getTodoWords(GetAllStemmsResult res) {
      return res.words.Where(nv => {
        if (nv.Value[0] != 0) return false;
        nv.Value[0] = -1;
        return true;
      }).Select(nv => nv.Key).ToArray();
    }

    public static void getAllStemms(GetAllStemmsResult res, int attempts, string[] initialWords, LangsLib.langs lang, int batchSize = int.MaxValue) {
      if (attempts <= 0) attempts = 32;
      res.start = DateTime.Now;
      for (var i = 0; i < attempts; i++) {
        var words = i == 0 ? initialWords : getTodoWords(res);
        res.attemptCounts.Add(words.Length);
        res.batchCount = 0;
        if (words.Length == 0)
          break;
        getAllStemms(res, words, lang, batchSize);
        if (res.wordsCount == res.words.Count)
          break;
        res.wordsCount = res.words.Count;
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
          var res = new GetAllStemmsResult();
          getAllStemms(res, 0, words, (LangsLib.langs)lc.LCID, 5000);
          dumpAllStemmsResult(res, dumpFn);
        } catch (Exception e) {
          File.WriteAllText(dumpDir + lc.Name + ".log", e.Message + "\r\n" + e.StackTrace);
        }
      }

    }


    public static void dumpAllStemmsResult(GetAllStemmsResult res, string fn) {
      res.wordsCount = res.words.Count;
      res.groupsCount = res.groups.Count;
      res.end = DateTime.Now;
      res.words.Values.Aggregate((r, i) => {
        if (i[0] == 0) res.firstGroupZeroCount++;
        if (i.Count <= 1) return null;
        res.moreGroupsCount++;
        res.moreGroupsSum += i.Count - 1;
        return null;
      });
      if (File.Exists(fn)) File.Delete(fn);
      var ser = new XmlSerializer(typeof(GetAllStemmsResult));
      using (var fs = File.OpenWrite(fn))
        ser.Serialize(fs, res);
    }

    //static byte[] getHash(string str) {
    //  using (MD5 md5 = MD5.Create())
    //    return md5.ComputeHash(Encoding.UTF8.GetBytes(str));
    //}
  }

  public class Comparer : IComparer<string> {
    public Comparer(CultureInfo ci) {
      this.ci = ci;
    }
    CultureInfo ci;
    public int Compare(string x, string y) { return ci.CompareInfo.Compare(x, y); }
  }

  public class MD5Comparer : IEqualityComparer<Guid> {
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

  //public class WordStemms {
  //  public byte[] main;
  //  public List<byte[]> other;
  //}

  public class GetAllStemmsResult {
    public int groupAutoIncrement;
    [XmlIgnore]
    public Dictionary<string, List<int>> words = new Dictionary<string, List<int>>(); // first item in the list is primary stemms set
    [XmlIgnore]
    public Dictionary<Guid, int> groups = new Dictionary<Guid, int>(new MD5Comparer());
    //[XmlIgnore]
    //public Dictionary<string, ulong?> words = new Dictionary<string, ulong?>();
    //[XmlIgnore]
    //public HashSet<ulong> groups = new HashSet<ulong>();
    //public int moreGroupsCount;
    public int singleWordStemmsCount;
    public List<string> wordNotInStemmsLog = new List<string>();
    public static ulong singleStemmsHashValue = 1234567890;
    public List<int> attemptCounts = new List<int>();
    public int wordsCount;
    public int groupsCount;
    public int moreGroupsCount;
    public int moreGroupsSum;
    public int firstGroupZeroCount;
    public int batchCount;
    public DateTime start;
    public DateTime end;
  }


}
