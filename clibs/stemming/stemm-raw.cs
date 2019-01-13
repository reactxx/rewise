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
    const int deepMax = 2;

    public StemmingRaw(CultureInfo lc, bool fromScratch /*true => create, false => update*/, int batchSize = 5000) {
      this.lc = lc;
      this.batchSize = batchSize;
      breakerService = StemmerBreaker.Services.getService((LangsLib.langs)lc.LCID);
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
      foreach (var lc in LangsLib.Metas.Items.Values.Where(it => it.StemmerClass != null).Select(it => it.lc)) {
        var dumpFn = Root.dumpRootLogs + lc.Name + ".xml";
        if (checkDumpExist && File.Exists(dumpFn))
          continue;
        var isFirst = true;
        for (var i = 0; i < dictSources.Length; i++) {
          var srcFn = Root.root + dictSources[i] + lc.Name + ".txt";
          if (!File.Exists(srcFn)) continue;
          Console.WriteLine(string.Format("{0}, WORDLIST {1}", lc.Name, i));
          var raw = new StemmingRaw(lc, fromScratch && isFirst, batchSize);
          raw.processLang(srcFn);
          Console.WriteLine();
          isFirst = false;
        }
      }
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
    public void processLang(string srcFileList) {
      var dumpFn = Root.dumpRootLogs + lc.Name + ".xml";
      var wordsFn = Root.dumpRootWords + lc.Name + ".txt";
      var saveFn = Root.root + @"dict-bins\" + lc.Name + ".bin";
      if (File.Exists(dumpFn)) File.Delete(dumpFn);
      if (File.Exists(wordsFn)) File.Delete(wordsFn);
      if (File.Exists(saveFn)) File.Delete(saveFn);
      try {
        var content = new List<string>(File.ReadLines(srcFileList));
        getAllStemms(content);
        saveLangStemms(saveFn);
        dumpLangStemms(dumpFn);
      } catch (Exception e) {
        File.WriteAllText(dumpFn + ".log", e.Message + "\r\n" + e.StackTrace);
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
    StemmerBreaker.Service breakerService;

    Dictionary<string, Word> wordsIdx = new Dictionary<string, Word>();
    Dictionary<Guid, Group> groups = new Dictionary<Guid, Group>(new MD5Comparer2());
    BitArray done = new BitArray(50000000);
    HashSet<ToDo> todo = new HashSet<ToDo>(new WordComparer());

    //*****************************************************************
    //  LOAD X SAVE DATABASE

    class groupItem { public byte[] md5; public int[] wordIds; }
    struct wordItem { public string key; public List<int> groupIds; public ushort deep; public string deepStr; }

    void saveLangStemms(string fn) {

      int maxGroupWordId = 0;
      // prepare words
      var words = new wordItem[wordsIdx.Count];
      foreach (var kv in wordsIdx) {
        words[kv.Value.id] = new wordItem { key = kv.Key, groupIds = kv.Value.groupIds, deep = kv.Value.deep, deepStr = kv.Value.deepStr };
        if (kv.Value.groupIds.Count > maxGroupsInWordLimit) {
          maxGroupWordId = kv.Value.id;
          moreGroupIdsCount++;
        }
      }
      wordsIdx = null;

      // prepare groups and set groupId to words
      var grps = new groupItem[groups.Count];
      foreach (var kv in groups)
        grps[kv.Value.id] = new groupItem() { wordIds = kv.Value.wordIds, md5 = kv.Key.ToByteArray() };
      groups = null;

      //DEBUG dump moreGroupIdsCount
      //using (var wr = new StreamWriter(Root.root + @"fulltext\sqlserver\dumps\" + lc.Name + ".txt")) {
      //  foreach (var ww in words.Where(w => w.deep > deepMax).Select(w => w.key + ": " + w.deepStr).OrderBy(w => w))
      //    wr.WriteLine(ww);
      //}
      var comparer = StringComparer.Create(lc, true);
      File.WriteAllLines(
        Root.dumpRootWords + lc.Name + ".txt",
        words.Select(w => w.key).OrderBy(w => w, comparer),
        EncodingEx.UTF8
      );

      // serialize words
      using (var wordBin = new BinaryWriter(File.Create(fn + ".words.bin"))) {
        wordBin.Write(words.Length);
        foreach (var word in words) {
          maxGroupsInWord = Math.Max(maxGroupsInWord, word.groupIds.Count);
          groupsInWord += word.groupIds.Count;
          wordChars += word.key.Length;
          wordBin.Write(word.key);
          wordBin.Write(word.deep);
          //DEEPSTR
          //wordBin.Write(word.deepStr);
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
        wordAutoIncrement = wordCount;
        wordsIdx = new Dictionary<string, Word>();
        for (var i = 0; i < wordCount; i++) {
          done[i] = true;
          var key = wordBin.ReadString();
          var deep = wordBin.ReadUInt16();
          //DEEPSTR
          string deepStr = null;
          //var deepStr = wordBin.ReadString();
          var count = wordBin.ReadUInt16();
          var groupIds = new List<int>(count);
          for (var j = 0; j < count; j++) groupIds.Add(wordBin.ReadInt32());
          wordsIdx[key] = new Word { id = i, groupIds = groupIds, deep = deep, deepStr = deepStr };
        }
      }
      if (wordAutoIncrement != wordsIdx.Count) throw new Exception("wordAutoIncrement != wordsIdx.Count");

      // deserialize groups
      using (var groupBin = new BinaryReader(File.OpenRead(fn + ".groups.bin"))) {
        var groupCount = groupBin.ReadInt32();
        groupIdAutoIncrement = groupCount;
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

      stemmedAll += batchSize;

      Console.Write(string.Format("\r{3} attempt: {0}/{1}/{2}, nextTodo: {5}, wordCount: {4}, stemmed: {6}               ",
        attemptCount, attemptLen, ++stemmedChunks * batchSize, lc.Name, wordAutoIncrement, todo.Count, stemmedAll));

      foreach (var stem in dbStems) {
        if (stem == null || stem.stemms == null) continue;
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
        if (groups.ContainsKey(hash))
          continue;

        // stemmed word:
        var sourceTxt = stem.word.ToLower(lc);
        var sourcIdx = Array.IndexOf(arr, sourceTxt);

        // source deep limit exceeded => continue
        var isInIndex = wordsIdx.TryGetValue(sourceTxt, out Word sourceObj);
        if (isInIndex && sourceObj.deep > deepMax)
          continue;

        // create new group
        var groupId = groupIdAutoIncrement++;

        var wordIds = arr.Select((w, idx) => {
          if (!wordsIdx.TryGetValue(w, out Word wid)) {
            int deep;
            var s = stem;

            if (w == sourceTxt)
              deep = 0;
            else
              deep = isInIndex ? sourceObj.deep + 1 : 1;

            wordsIdx[w] = wid = new Word {
              id = wordAutoIncrement++,
              groupIds = new List<int>() { groupId },
              deep = (ushort)deep,
              //DEEPSTR
              //deepStr = w == sourceTxt ? "" : (hasSource ? sourceObj.deepStr + ',' + sourceTxt : sourceTxt)
            };
          } else
            wid.groupIds.Add(groupId);
          todo.Add(new ToDo() { id = wid.id, word = w });
          return wid.id;
        }).ToArray();

        groups[hash] = new Group() {
          id = groupId,
          wordIds = wordIds
        };

      }

    }

    // stemm all words from source word-list. Called once for language
    void getAllStemms(List<string> words) {

      //List<string> words = breakerService.wordBreakLargeWordList(fileContent, 5000);

      attemptLen = words.Count;
      stemmedChunks = 0;
      if (attemptLen == 0) // nothing todo => break
        return;

      var lastWordsCount = 0;
      while (true) {

        attemptCount++;
        //DEBUG
        //words = words.Take(100000).ToList();

        Stemming.getStemms(words, (LangsLib.langs)lc.LCID, batchSize, processStemms);
        words = getTodoWords();
        attemptLen = words.Count;
        stemmedChunks = 0;
        if (attemptLen == 0)
          break;
        //getAllStemmsLow(words.Take(50000).ToArray());

        // nothing added => break
        if (lastWordsCount == wordsIdx.Count)
          break;
        lastWordsCount = wordsIdx.Count;
      }
    }

    //*****************************************************************
    //  MISC

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
      var dump = new Dump() {
        groupIdAutoIncrement = groupIdAutoIncrement,
        wordAutoIncrement = wordAutoIncrement,
        attemptCount = attemptCount,
        wordChars = wordChars,
        maxWordsInGroup = maxWordsInGroup,
        maxGroupsInWord = maxGroupsInWord,
        groupsInWord = groupsInWord,
        wordsInGroup = wordsInGroup,
        moreGroupIdsCount = moreGroupIdsCount,
        flaggedWords = flaggedWords,
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

    class ToDo {
      public int id;
      public string word;
    }

    struct Word {
      public int id;
      public List<int> groupIds;
      public ushort deep;
      public string deepStr;
    }

  }

  public class Dump {
    public int groupIdAutoIncrement;
    public int wordAutoIncrement;
    public int attemptCount;
    public int wordChars;
    public int maxWordsInGroup;
    public int maxGroupsInWord;
    public int wordsInGroup;
    public int groupsInWord;
    public int moreGroupIdsCount;
    public int stemmedAll;
    public int flaggedWords;
  }

}
