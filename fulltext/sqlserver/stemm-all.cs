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

    public static void getAllStemms(GetAllStemmsResult res, string[] words, LangsLib.langs lang, int maxCount = int.MaxValue) {
      MD5 md5 = MD5.Create();
      var comparer = new Comparer(new LangsLib.Metas().Items[lang].lc);
      Stemming.getStemms(words, lang, stem => {
        var stemms = stem.stemms.Split(',').OrderBy(s => s, comparer).ToArray();
        var wordLower = stem.word.ToLower();
        var word = stemms.FirstOrDefault(s => wordLower == s);
        if (stemms.Length == 1)
          res.singleWordStemmsCount++;
        if (word == null && res.wordNotInStemmsLog.Count < 100)
          res.wordNotInStemmsLog.Add(wordLower + " not-in " + stem.stemms);

        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(stem.stemms));
        int id;
        if (!res.groups2.TryGetValue(hash, out id))
          res.groups2[hash] = id = ++res.groupAutoIncrement;

        // save value
        foreach (var st in stemms) {
          List<int> oldList;
          var found = res.words2.TryGetValue(st, out oldList);
          if (st == word) {
            if (found) {
              if (oldList[0] != 0) {
                oldList[0] = 0;
              }
              var idIdx = oldList.IndexOf(id);
              if (idIdx > 0)
                oldList.RemoveAt(idIdx); // maybe remove noticed secondary stemms
              oldList[0] = id;
            } else
              res.words2[st] = new List<int>() { id }; // set primary stemms to newly created word
          } else {
            if (found) {
              if (oldList[0] == id)
                continue;
              var idIdx = oldList.IndexOf(id);
              if (idIdx < 0)
                oldList.Add(id); // add secondary stemms
            } else
              res.words2[st] = new List<int>() { 0, id }; // create todo Word, notice secondary stemms
          }
        }
      }, maxCount);
    }

    public static string[] getTodoWords(GetAllStemmsResult res) {
      return res.words2.Where(nv => nv.Value[0] == 0).Select(nv => nv.Key).ToArray();
    }

    public static void getAllStemms(GetAllStemmsResult res, int attempts, string[] initialWords, LangsLib.langs lang, int maxCount = int.MaxValue) {
      if (attempts <= 0) attempts = int.MaxValue;
      for (var i = 0; i < attempts; i++) {
        var words = i == 0 ? initialWords : getTodoWords(res);
        res.attemptCounts.Add(words.Length);
        if (words.Length == 0)
          break;
        getAllStemms(res, words, lang, maxCount);
        if (res.wordsCount == res.words2.Count)
          break;
        res.wordsCount = res.words2.Count;
      }
    }

    public static void dumpAllStemmsResult(GetAllStemmsResult res, string fn) {
      res.wordsCount = res.words2.Count;
      res.groupsCount = res.groups2.Count;
      res.words2.Values.Aggregate((r, i) => {
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

    static byte[] getHash(string str) {
      using (MD5 md5 = MD5.Create())
        return md5.ComputeHash(Encoding.UTF8.GetBytes(str));
    }
  }

  public class Comparer : IComparer<string> {
    public Comparer(CultureInfo ci) {
      this.ci = ci;
    }
    CultureInfo ci;
    public int Compare(string x, string y) { return ci.CompareInfo.Compare(x, y); }
  }

  public class MD5Comparer : IEqualityComparer<byte[]> {
    public bool Equals(byte[] a, byte[] b) {
      return a.SequenceEqual(b);
    }

    public int GetHashCode(byte[] key) {
      return key.Sum(b => b);
    }
  }

  public class WordStemms {
    public byte[] main;
    public List<byte[]> other;
  }

  public class GetAllStemmsResult {
    public int groupAutoIncrement;
    [XmlIgnore]
    public Dictionary<string, List<int>> words2 = new Dictionary<string, List<int>>(); // first item in the list is primary stemms set
    [XmlIgnore]
    public Dictionary<byte[], int> groups2 = new Dictionary<byte[], int>(new MD5Comparer());

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
  }


}
