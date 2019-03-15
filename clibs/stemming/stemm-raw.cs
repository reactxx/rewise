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

  public static class Root {
    public static string root = AppDomain.CurrentDomain.BaseDirectory[0] + @":\rewise\";
    public static string dumpRoot = Root.root + @"dumps\stemming\";
    public static string dumpRootWords = dumpRoot + @"words\";
    public static string dumpRootLogs = dumpRoot + @"logs\";
  }

  public class StemmingRaw : Dump {

    const int maxGroupsInWordLimit = 5;
    //const int deepMax = 2;
    const int deepMax = 1;

    public StemmingRaw(CultureInfo lc, bool fromScratch /*true => create, false => update*/, int batchSize = 5000) {
      this.lc = lc;
      this.batchSize = batchSize;
      if (!fromScratch) {
        var savedFn = Root.root + @"dict-bins\" + lc.Name + ".bin";
        loadLangStemms(savedFn);
      }
    }

    //*****************************************************************
    // MAIN DESIGN TIME PROC
    // call eg. StemmingRaw.processLangs(@"d:\rewise\");
    // process all langs for word-lists from <dictSources>.
    // checkDumpExist = false => run all, else run when dump file not exists
    public static void processLangs(string[] dictSources, bool fromScratch = true, bool checkDumpExist = true, int batchSize = 5000) {
      foreach (var lc in LangsLib.Metas.Items.Values.Where(it => it.stemmerClass != null).Select(it => it.lc))
        processLang(lc, dictSources, fromScratch, checkDumpExist, batchSize);
    }

    public static void processLang(CultureInfo lc, string[] dictSources, bool fromScratch = true, bool checkDumpExist = true, int batchSize = 5000) {
      var dumpFn = Root.dumpRootLogs + lc.Name + ".xml";
      if (checkDumpExist && File.Exists(dumpFn))
        return;
      List<string> words = null;
      for (var i = 0; i < dictSources.Length; i++) {
        var srcFn = Root.root + dictSources[i] + lc.Name + ".txt";
        if (!File.Exists(srcFn)) continue;
        if (words == null) words = File.ReadLines(srcFn).ToList();
        else words.AddRange(File.ReadAllLines(srcFn));
      }
      if (words == null) return;
      var raw = new StemmingRaw(lc, fromScratch, batchSize);
      raw.processLang(words);
    }

    //*****************************************************************
    // DESIGN TIME FOR SINGLE LANG
    /*
      var root = @"d:\rewise\";
      var raw = new StemmingRaw(LangsLib.Metas.Items[LangsLib.langs.de_de].lc, true);
      raw.processLang(root + @"dicts_source\de-de.txt");

      raw = new StemmingRaw(LangsLib.Metas.Items[LangsLib.langs.de_de].lc, false);
      raw.processLang(root + @"dicts_source2\de-de.txt");
     */
    // read saved stemming database 

    // process single word-list
    public void processLang(List<string> words) {
      var dumpFn = Root.dumpRootLogs + lc.Name + ".xml";
      var wordsFn = Root.dumpRootWords + lc.Name + ".txt";
      var saveFn = Root.root + @"dict-bins\" + lc.Name + ".bin";
      if (File.Exists(dumpFn)) File.Delete(dumpFn);
      if (File.Exists(wordsFn)) File.Delete(wordsFn);
      if (File.Exists(saveFn)) File.Delete(saveFn);
      try {
        getAllStemms(words);
        saveLangStemms(saveFn);
        dumpLangStemms(dumpFn);
      } catch (Exception e) {
        File.WriteAllText(dumpFn + ".log", e.Message + "\r\n" + e.StackTrace);
        //throw;
      }
    }

    //*****************************************************************
    //*****************************************************************
    //  PRIVATE
    //*****************************************************************

    int stemmedChunks;
    int attemptLen;
    CultureInfo lc;
    int batchSize;
    Dictionary<int, int> groupsInWordCount;
    HashSet<char> chars;
    int stemmedAllAttempts;

    Dictionary<string, Word> wordsIdx = new Dictionary<string, Word>();
    Dictionary<Guid, Group> groups = new Dictionary<Guid, Group>(new MD5Comparer2());
    BitArray done = new BitArray(50000000);
    HashSet<ToDo> todo = new HashSet<ToDo>(new WordComparer());

    //*****************************************************************
    //  LOAD X SAVE DATABASE

    struct groupItem { public byte[] md5; public int[] wordIds; }
    struct wordItem { public string key; public List<int> groupIds; public ushort deep; }

    void saveLangStemms(string fn) {

      int maxGroupWordId = 0;
      // prepare words
      var words = new wordItem[wordsIdx.Count];
      foreach (var kv in wordsIdx) {
        words[kv.Value.id] = new wordItem { key = kv.Key, groupIds = kv.Value.groupIds, deep = kv.Value.deep };
        if (kv.Value.groupIds.Count > maxGroupsInWordLimit) {
          maxGroupWordId = kv.Value.id;
        }
      }

      // null check
      if (words.Any(w => w.groupIds == null))
        throw new Exception("any words is empty");

      wordsCount = wordsIdx.Count;
      wordsIdx = null;

      // prepare groups and set groupId to words
      var grps = new groupItem[groups.Count];
      foreach (var kv in groups)
        grps[kv.Value.id] = new groupItem() { wordIds = kv.Value.wordIds, md5 = kv.Key.ToByteArray() };
      groups = null;

      // null check
      if (grps.Any(w => w.wordIds == null))
        throw new Exception("any grps is empty");

      var comparer = StringComparer.Create(lc, true);
      File.WriteAllLines(
        Root.dumpRootWords + lc.Name + ".txt",
        words.Select(w => w.key).OrderBy(w => w, comparer),
        EncodingEx.UTF8
      );

      // serialize words
      using (var wordBin = new BinaryWriter(File.Create(fn + ".words.bin"))) {
        wordBin.Write(words.Length);
        groupsInWordCount = new Dictionary<int, int>();
        chars = new HashSet<char>();
        foreach (var word in words) {
          foreach (var ch in word.key) chars.Add(ch);
          if (!groupsInWordCount.TryGetValue(word.groupIds.Count, out int count)) count = 0;
          groupsInWordCount[word.groupIds.Count] = count + 1;
          maxGroupsInWord = Math.Max(maxGroupsInWord, word.groupIds.Count);
          groupsInWord += word.groupIds.Count;
          wordChars += word.key.Length;
          wordBin.Write(word.key);
          wordBin.Write(word.deep);
          wordBin.Write((UInt16)word.groupIds.Count);
          word.groupIds.Sort();
          foreach (var id in word.groupIds) wordBin.Write(id);
        }
      }

      // serialize groups
      using (var groupBin = new BinaryWriter(File.Create(fn + ".groups.bin"))) {
        groupBin.Write(grps.Length);
        foreach (var grp in grps) {
          maxWordsInGroup = Math.Max(maxWordsInGroup, grp.wordIds.Length);
          wordsInGroup += grp.wordIds.Length;
          groupBin.Write(grp.md5);
          groupBin.Write((UInt16)grp.wordIds.Length);
          Array.Sort(grp.wordIds);
          foreach (var id in grp.wordIds) groupBin.Write(id);
        }
      }
    }

    void loadLangStemms(string fn) {

      // deserialize words
      using (var wordBin = new BinaryReader(File.OpenRead(fn + ".words.bin"))) {
        var wordCount = wordBin.ReadInt32();
        wordsIdx = new Dictionary<string, Word>();
        for (var i = 0; i < wordCount; i++) {
          done[i] = true;
          var key = wordBin.ReadString();
          var deep = wordBin.ReadUInt16();
          var count = wordBin.ReadUInt16();
          var groupIds = new List<int>(count);
          for (var j = 0; j < count; j++) groupIds.Add(wordBin.ReadInt32());
          wordsIdx[key] = new Word { id = i, groupIds = groupIds, deep = deep };
        }
      }

      // deserialize groups
      using (var groupBin = new BinaryReader(File.OpenRead(fn + ".groups.bin"))) {
        var groupCount = groupBin.ReadInt32();
        groups = new Dictionary<Guid, Group>();
        for (var i = 0; i < groupCount; i++) {
          var guid = groupBin.ReadBytes(16);
          var count = groupBin.ReadUInt16();
          var grp = groups[new Guid(guid)] = new Group() {
            id = i,
            wordIds = new int[count]
          };
          for (var j = 0; j < count; j++) grp.wordIds[j] = groupBin.ReadInt32();
        }
      }

    }

    //*****************************************************************
    //  GET STEMMS

    void processStemms(IEnumerable<WordStemm> dbStems) {

      MD5 md5 = MD5.Create();
      var comparer = StringComparer.Create(lc, true);

      stemmedAllAttempts += batchSize;

      Console.Write(string.Format("\r{3} attempt: {0}/{1}/{2}, todo: {5}, wordCount: {4}, stemmed: {6}               ",
        attemptCount, attemptLen, ++stemmedChunks * batchSize, lc.Name, wordsIdx.Count, todo.Count, stemmedAllAttempts));

      foreach (var stem in dbStems) {
        if (stem == null || stem.stemms == null)
          continue;
        var arr = stem.stemms.Split(',');

        // single stemm set => continue
        if (arr.Length == 1)
          continue;

        // get group hash
        byte[] bytes;
        using (var mem = new MemoryStream()) {
          using (var wr = new BinaryWriter(mem)) {
            Array.Sort(arr, comparer);
            foreach (var s in arr) {
              wr.Write(s);
              wr.Write((byte)0);
            }
          }
          bytes = md5.ComputeHash(mem.ToArray());
        }
        var hash = new Guid(bytes);

        // group already exists => continue
        var newGroupId = groups.TryGetValue(hash, out Group actGroup) ? actGroup.id : -1;
        var groupId = newGroupId < 0 ? groups.Count : newGroupId;

        // find stemming surce
        var sourceTxt = stem.word.ToLower(lc);
        var isInIndex = wordsIdx.TryGetValue(sourceTxt, out Word sourceObj);

        var wordIds = arr.Select(w => {

          if (!wordsIdx.TryGetValue(w, out Word wid)) {
            int deep;

            if (w == sourceTxt)
              deep = 0;
            else
              deep = isInIndex ? sourceObj.deep + 1 : 1;

            wordsIdx[w] = wid = new Word {
              id = wordsIdx.Count, //wordAutoIncrement++,
              groupIds = new List<int>() { groupId },
              deep = (ushort)deep,
            };
          } else {
            if (wid.groupIds.IndexOf(groupId) < 0)
              wid.groupIds.Add(groupId);
          }

          if (wid.deep <= deepMax && sourceObj.id!= wid.id)
            todo.Add(new ToDo() { id = wid.id, word = w });

          return wid.id;

        }).ToArray();

        if (newGroupId < 0)
          groups[hash] = new Group() {
            id = groups.Count,
            wordIds = wordIds
          };

      }

    }

    // stemm all words from source word-list. Called once for language
    void getAllStemms(List<string> words) {

      var lastWordsIdxCount = 0;

      while (true) {

        if (words.Count == 0) // nothing todo => break
          break;

        attemptCount++;
        stemmedChunks = 0;
        attemptLen = words.Count;

        // words = words.Take(100000).ToList();
        Stemming.getStemms(words, (LangsLib.langs)lc.LCID, batchSize, processStemms);
        Console.WriteLine();

        words = getTodoWords();

        if (lastWordsIdxCount == wordsIdx.Count) // nothing added => break
          break;
        lastWordsIdxCount = wordsIdx.Count;
      }
    }

    //*****************************************************************
    //  MISC

    void fillTodo(List<string> words) {
      foreach (var w in words) {
        var wid = new Word {
          id = wordsIdx.Count,
          groupIds = new List<int>(),
          deep = (ushort)0,
        };
        wordsIdx[w] = wid;
        todo.Add(new ToDo() { id = wid.id, word = w });
      }
    }

    List<string> getTodoWords() {
      var td = todo.Where(t => !done[t.id]).Select(t => {
        done[t.id] = true;
        return t.word;
      }).ToList(); ;
      todo.Clear();
      return td;
    }

    void dumpLangStemms(string fn) {
      if (File.Exists(fn)) File.Delete(fn);
      string charsStrLenS;
      var dump = new Dump() {
        attemptCount = attemptCount,
        wordChars = wordChars,
        maxWordsInGroup = maxWordsInGroup,
        maxGroupsInWord = maxGroupsInWord,
        groupsInWord = groupsInWord,
        wordsInGroup = wordsInGroup,
        wordsCount = wordsCount,
        groupsInWordCountStr = "\n" + groupsInWordCount.OrderBy(kv => kv.Key).Select(kv => string.Format("{0}: {1}", kv.Key, kv.Value)).DefaultIfEmpty().Aggregate((r, i) => r + '\n' + i) + "\n",
        charsStr = charsStrLenS = "\n" + chars.OrderBy(ch => ch).Select(ch => ch.ToString()).DefaultIfEmpty().Aggregate((r,i) => r + i) + "\n",
        charsStrLen = charsStrLenS.Length,
      };
      var ser = new XmlSerializer(typeof(Dump));
      using (var fs = File.OpenWrite(fn))
        ser.Serialize(fs, dump);
    }

    //*****************************************************************
    //  HELPER CLASSES

    class MD5Comparer2 : IEqualityComparer<Guid> {
      public bool Equals(Guid a, Guid b) {
        return a.Equals(b);
      }

      public int GetHashCode(Guid a) {
        return a.GetHashCode();
      }
    }

    class WordComparer : IEqualityComparer<ToDo>, IComparer<ToDo> {
      public bool Equals(ToDo a, ToDo b) {
        return a.id == b.id;
      }

      public int GetHashCode(ToDo a) {
        return a.id;
      }

      public int Compare(ToDo x, ToDo y) {
        return x.word.CompareTo(y.word);
      }
    }

    struct Group {
      public int id;
      public int[] wordIds;
    }

    struct ToDo {
      public int id;
      public string word;
    }

    struct Word {
      public int id;
      public List<int> groupIds;
      public ushort deep;
    }

  }

  public class Dump {
    public int attemptCount;
    public int wordChars;
    public int maxWordsInGroup;
    public int maxGroupsInWord;
    public int wordsInGroup;
    public int groupsInWord;
    public string groupsInWordCountStr;
    public int wordsCount;
    public string charsStr;
    public int charsStrLen;
  }

}
