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

  public class StemmingRaw : Dump {

    //*****************************************************************
    // MAIN DESIGN TIME PROC
    // call eg. StemmingRaw.processLangs(@"d:\rewise\");
    // process all langs for word-lists from <dictSource>. checkDumpExist = false => run all, else run when dump file not exists
    public static void processLangs(string root, string dictSource = @"dicts_source\", bool checkDumpExist = true, int batchSize = 5000) {
      var metas = new LangsLib.Metas();
      var srcDir = root + dictSource;
      var dumpDir = root + @"fulltext\sqlserver\dumps-raw\";
      foreach (var lc in metas.Items.Values.Select(it => it.lc)) {
        var srcFn = srcDir + lc.Name + ".txt";
        if (!File.Exists(srcFn)) continue;
        var dumpFn = dumpDir + lc.Name + ".xml";
        if (checkDumpExist && File.Exists(dumpFn)) continue;
        var srcFileList = root + @"dicts_source\" + lc.Name + ".txt";
        new StemmingRaw(root, lc, batchSize).processLang(srcFileList, batchSize);
      }
    }

    //*****************************************************************
    // DESIGN TIME FOR SINGLE LANG
    /*
      var root = @"d:\rewise\";
      var raw = StemmingRaw.createNew(root, LangsLib.langs.de_de);
      raw.processLang(root + @"dicts_source\de-de.txt");

      raw = StemmingRaw.createUpdate(root, LangsLib.langs.de_de);
      raw.processLang(root + @"dicts_source2\de-de.txt");
     */
    // read saved stemming database 
    public static StemmingRaw createUpdate(string root, LangsLib.langs lang, int batchSize = 5000) {
      StemmingRaw raw = new StemmingRaw(root, new LangsLib.Metas().Items[lang].lc, batchSize);
      var saveFn = root + @"dict-bins\" + raw.lc.Name + ".bin";
      raw.loadLangStemms(saveFn);
      return raw;
    }

    // start new stemming database
    public static StemmingRaw createNew(string root, LangsLib.langs lang, int batchSize = 5000) {
      StemmingRaw raw = new StemmingRaw(root, new LangsLib.Metas().Items[lang].lc, batchSize);
      return raw;
    }

    // process single word-list
    public void processLang(string srcFileList, int batchSize = 5000) {
      var dumpFn = root + @"fulltext\sqlserver\dumps-raw\" + lc.Name;
      var saveFn = root + @"dict-bins\" + lc.Name + ".bin";
      try {
        var words = File.ReadAllLines(root + @"dicts_source\" + lc.Name + ".txt");
        getAllStemms(words);
        saveLangStemms(saveFn);
        dumpLangStemms(dumpFn + ".xml");
      } catch (Exception e) {
        File.WriteAllText(dumpFn + ".log", e.Message + "\r\n" + e.StackTrace);
      }
    }

    //*****************************************************************
    //  USING DATABASE: LOAD WORDS x GROUPS FROM BINARIES

    public class wordsLoader {
      public bool loadGroupIds = true;
      public virtual void setAllWordsCount(int count) { }
      public virtual void loaded(int id, string key, int[] groupIds) { }
    }

    public static void loadWords(string fn, wordsLoader loader) {

      using (var wordBin = new BinaryReader(File.OpenRead(fn + ".words.bin"))) {
        var wordCount = wordBin.ReadInt32();
        loader.setAllWordsCount(wordCount);

        for (var i = 0; i < wordCount; i++) {
          var key = wordBin.ReadString();
          var count = wordBin.ReadUInt16();
          int[] groupIds = null;
          if (loader.loadGroupIds) {
            groupIds = new int[count];
            for (var j = 0; j < count; j++) groupIds[j] = wordBin.ReadInt32();
          } else
            // skip groupIds
            for (var j = 0; j < count; j++) wordBin.ReadInt32();
          loader.loaded(i, key, groupIds);
        }
      }
    }

    public class groupLoader {
      public bool loadWordIds = true;
      public virtual void setAllGroupsCount(int count) { }
      public virtual void loaded(int id, byte[] hash, int[] wordIds) { }
    }

    public static void loadGroups(string fn, groupLoader loader) {

      using (var groupBin = new BinaryReader(File.OpenRead(fn + ".groups.bin"))) {
        var groupCount = groupBin.ReadInt32();
        loader.setAllGroupsCount(groupCount);

        for (var i = 0; i < groupCount; i++) {
          var guid = groupBin.ReadBytes(16);
          var count = groupBin.ReadUInt16();
          int[] wordIds = null;
          if (loader.loadWordIds) {
            wordIds = new int[count];
            for (var j = 0; j < count; j++) wordIds[j] = groupBin.ReadInt32();
          } else
            // skip wordIds
            for (var j = 0; j < count; j++) groupBin.ReadInt32();
          loader.loaded(i, guid, wordIds);
        }
      }
    }

    //*****************************************************************
    //*****************************************************************
    //  PRIVATE
    //*****************************************************************

    int attemptCount;
    int attemptLen;
    CultureInfo lc;
    int batchSize;
    string root;
    Dictionary<string, Word> wordsIdx = new Dictionary<string, Word>();
    Dictionary<Guid, Group> groups = new Dictionary<Guid, Group>(new MD5Comparer2());
    BitArray done = new BitArray(50000000);
    HashSet<ToDo> todo = new HashSet<ToDo>(new WordComparer());
    int lastWordsCount;

    StemmingRaw(string root, CultureInfo lc, int batchSize = 5000) {
      this.lc = lc;
      this.batchSize = batchSize;
      this.root = root;
    }

    //*****************************************************************
    //  LOAD X SAVE DATABASE

    void saveLangStemms(string fn) {
      // prepare words
      var words = new wordItem[wordsIdx.Count];
      foreach (var kv in wordsIdx) {
        words[kv.Value.id] = new wordItem { key = kv.Key, groups = new HashSet<int>() };
        if (kv.Value.groupCount > 100) moreGroupIdsCount++;
      }
      wordsIdx = null;

      // prepare groups and set groupId to words
      var grps = new groupItem[groups.Count];
      foreach (var kv in groups) {
        grps[kv.Value.id] = new groupItem() { wordIds = kv.Value.wordIds, md5 = kv.Key.ToByteArray() };
        
        foreach (var wordId in kv.Value.wordIds) {
          var hashSet = words[wordId].groups;
          if (hashSet.Count<100)
            words[wordId].groups.Add(kv.Value.id);
        }
      }
      groups = null;

      // serialize words
      using (var wordBin = new BinaryWriter(File.Create(fn + ".words.bin"))) {
        wordBin.Write(words.Length);
        foreach (var word in words) {
          maxGroupsInWord = Math.Max(maxGroupsInWord, word.groups.Count);
          groupsInWord += word.groups.Count;
          wordChars += word.key.Length;
          wordBin.Write(word.key);
          wordBin.Write((UInt16)word.groups.Count);
          foreach (var id in word.groups) wordBin.Write(id);
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
          foreach (var id in grp.wordIds) groupBin.Write(id);
        }
      }
    }
    class groupItem { public byte[] md5; public int[] wordIds; }
    struct wordItem { public string key; public HashSet<int> groups; }

    void loadLangStemms(string fn) {

      // deserialize words
      using (var wordBin = new BinaryReader(File.OpenRead(fn + ".words.bin"))) {
        var wordCount = wordBin.ReadInt32();
        wordAutoIncrement = wordCount + 1;
        wordsIdx = new Dictionary<string, Word>();
        for (var i = 0; i < wordCount; i++) {
          done[i] = true;
          var key = wordBin.ReadString();
          var count = wordBin.ReadUInt16();
          wordsIdx[key] = new Word { id=i, groupCount = count };
          // skip groupIds
          for (var j = 0; j < count; j++) wordBin.ReadInt32();
        }
      }

      // deserialize groups
      using (var groupBin = new BinaryReader(File.OpenRead(fn + ".groups.bin"))) {
        var groupCount = groupBin.ReadInt32();
        groupIdAutoIncrement = groupCount + 1;
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

    // stemm all words from word-list. Could be called more times for language (in order to have two byte word ID for most frequent words)
    void getAllStemmsLow(string[] words) {

      MD5 md5 = MD5.Create();

      Stemming.getStemms(words, (LangsLib.langs)lc.LCID, dbStems => {

        Console.Write(string.Format("\r{3} attempt: {0}/{1}, batches: {2}      ", base.attemptCount, attemptLen, ++attemptCount * batchSize, lc.EnglishName));

        var stems = new List<Tuple<Guid, string[]>>();
        foreach (var stem in dbStems) {
          if (stem == null || stem.stemms == null) continue;
          var arr = stem.stemms.Split(',');
          if (arr.Length == 1)
            continue;
          var hash = new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes(stem.stemms)));
          stems.Add(Tuple.Create(hash, arr));

          if (base.attemptCount == 1) {
            // assign ID to word (first words => lower ID)
            string actWord = stem.word.ToLower();
            if (Array.IndexOf(arr, actWord) >= 0) { // source word is in stemms
              if (!wordsIdx.TryGetValue(actWord, out Word wid))
                wordsIdx[actWord] = wid = new Word { id = wordAutoIncrement++, groupCount = 1 };
              else
                wid.groupCount++;
              done[wid.id] = true;
            }
          }

        }

        foreach (var stemms in stems) {

          // adjust stemms word IDs and add them to TODO
          var ids = stemms.Item2.Select(w => {
            if (!wordsIdx.TryGetValue(w, out Word wid))
              wordsIdx[w] = wid = new Word { id = wordAutoIncrement++, groupCount = 0 };
            else
              wid.groupCount++;
            todo.Add(new ToDo() { id = wid.id, word = w });
            return wid.id;
          }).ToArray();
          Array.Sort(ids);

          // stemm group already exists => continue
          if (groups.ContainsKey(stemms.Item1)) continue;

          // create stemm group
          groups[stemms.Item1] = new Group() {
            id = groupIdAutoIncrement++,
            wordIds = ids
          };
        }
      }, batchSize);
    }

    // stemm all words from source word-list. Called once for language
    void getAllStemms(string[] initialWords) {
      start = DateTime.Now;
      while (true) {
        string[] words;
        if (base.attemptCount == 0) {
          if (wordsIdx.Count == 0) // wordsIdx is empty => database created
            words = initialWords;
          else { // update database
            foreach (var w in initialWords) {
              if (!wordsIdx.TryGetValue(w, out Word wid)) continue;
              todo.Add(new ToDo() { id = wid.id, word = w });
            }
            words = getTodoWords();
          }
        } else
          words = getTodoWords();

        base.attemptCount++;
        attemptLen = words.Length;
        attemptCount = 0;
        if (words.Length == 0) // nothing todo => break
          break;
        // first 30.000 words has two byte ID
        if (base.attemptCount == 1 && words.Length > 30000) {
          getAllStemmsLow(words.Take(30000).ToArray());
          getAllStemmsLow(words.Skip(30000).ToArray());
        } else
          getAllStemmsLow(words);

        // nothing added => break
        if (lastWordsCount == wordsIdx.Count)
          break;
        lastWordsCount = wordsIdx.Count;
      }
    }

    //*****************************************************************
    //  MISC

    string[] getTodoWords() {
      var td = todo.Where(t => !done[t.id]).Select(t => {
        done[t.id] = true;
        return t.word;
      }).ToArray(); ;
      todo.Clear();
      return td;
    }

    void dumpLangStemms(string fn) {
      duration = (int)Math.Round((DateTime.Now - start).TotalSeconds);
      if (File.Exists(fn)) File.Delete(fn);
      var dump = new Dump() {
        groupIdAutoIncrement = groupIdAutoIncrement,
        wordAutoIncrement = wordAutoIncrement,
        start = start,
        duration = duration,
        attemptCount = base.attemptCount,
        wordChars = wordChars,
        maxWordsInGroup = maxWordsInGroup,
        maxGroupsInWord = maxGroupsInWord,
        groupsInWord = groupsInWord,
        wordsInGroup = wordsInGroup,
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
      public ushort groupCount; 
    }

  }

  public class Dump {
    public int groupIdAutoIncrement;
    public int wordAutoIncrement;
    public DateTime start;
    public int duration;
    public int attemptCount;
    public int wordChars;
    public int maxWordsInGroup;
    public int maxGroupsInWord;
    public int wordsInGroup;
    public int groupsInWord;
    public int moreGroupIdsCount;
  }

}
