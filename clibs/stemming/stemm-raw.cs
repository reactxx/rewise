﻿using System;
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
    public static string dumpRoot = Root.root + @"fulltext\sqlserver\dumps-raw\";
  }


  public class StemmingRaw : Dump {

    const int maxGroupsInWordLimit = 10;

    //*****************************************************************
    // MAIN DESIGN TIME PROC
    // call eg. StemmingRaw.processLangs(@"d:\rewise\");
    // process all langs for word-lists from <dictSources>.
    // checkDumpExist = false => run all, else run when dump file not exists
    public static void processLangs(string[] dictSources, bool fromScratch = true, bool checkDumpExist = true, int batchSize = 5000) {
      foreach (var lc in LangsLib.Metas.Items.Values.Where(it => it.StemmerClass != null).Select(it => it.lc)) {
        var dumpFn = Root.dumpRoot + lc.Name + ".xml";
        if (checkDumpExist && File.Exists(dumpFn))
          continue;
        var isFirst = true;
        for (var i = 0; i < dictSources.Length; i++) {
          var srcFn = Root.root + dictSources[i] + lc.Name + ".txt";
          if (!File.Exists(srcFn)) continue;
          var raw = new StemmingRaw(lc, fromScratch && isFirst, batchSize);
          Console.WriteLine(string.Format("{0}, WORDLIST {1}", lc.Name, i));
          raw.processLang(srcFn, batchSize);
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

    public StemmingRaw(CultureInfo lc, bool fromScratch, int batchSize = 5000) {
      this.lc = lc;
      this.batchSize = batchSize;
      if (!fromScratch) {
        var savedFn = Root.root + @"dict-bins\" + lc.Name + ".bin";
        loadLangStemms(savedFn);
      }
    }

    // process single word-list
    public void processLang(string srcFileList, int batchSize = 5000) {
      var dumpFn = Root.dumpRoot + lc.Name;
      var saveFn = Root.root + @"dict-bins\" + lc.Name + ".bin";
      try {
        var words = File.ReadAllLines(srcFileList);
        getAllStemms(words.Select(w => w.ToLower(lc)).ToArray());
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

    int stemmedChunks;
    int attemptLen;
    CultureInfo lc;
    int batchSize;

    Dictionary<string, Word> wordsIdx = new Dictionary<string, Word>();
    Dictionary<Guid, Group> groups = new Dictionary<Guid, Group>(new MD5Comparer2());
    BitArray done = new BitArray(50000000);
    HashSet<ToDo> todo = new HashSet<ToDo>(new WordComparer());

    //*****************************************************************
    //  LOAD X SAVE DATABASE

    void saveLangStemms(string fn) {
      // prepare words
      var words = new wordItem[wordsIdx.Count];
      foreach (var kv in wordsIdx) {
        words[kv.Value.id] = new wordItem { key = kv.Key, groupIds = kv.Value.groupIds };
        if (kv.Value.groupIds.Count > maxGroupsInWordLimit) moreGroupIdsCount++;
      }
      wordsIdx = null;

      // prepare groups and set groupId to words
      var grps = new groupItem[groups.Count];
      foreach (var kv in groups) {
        grps[kv.Value.id] = new groupItem() { wordIds = kv.Value.wordIds, md5 = kv.Key.ToByteArray() };

        //foreach (var wordId in kv.Value.wordIds) {
        //  var hashSet = words[wordId].groups;
        //  if (hashSet.Count < maxGroupsInWordLimit)
        //    words[wordId].groups.Add(kv.Value.id);
        //}
      }
      groups = null;

      // serialize words
      using (var wordBin = new BinaryWriter(File.Create(fn + ".words.bin"))) {
        wordBin.Write(words.Length);
        foreach (var word in words) {
          maxGroupsInWord = Math.Max(maxGroupsInWord, word.groupIds.Count);
          groupsInWord += word.groupIds.Count;
          wordChars += word.key.Length;
          wordBin.Write(word.key);
          wordBin.Write((UInt16)word.groupIds.Count);
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
          foreach (var id in grp.wordIds) groupBin.Write(id);
        }
      }
    }
    class groupItem { public byte[] md5; public int[] wordIds; }
    struct wordItem { public string key; public List<int> groupIds; }

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
          var groupIds = new List<int>(count);
          for (var j = 0; j < count; j++) groupIds.Add(wordBin.ReadInt32());
          wordsIdx[key] = new Word { id = i, groupIds = groupIds };
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

        stemmedAll += batchSize;

        Console.Write(string.Format("\r{3} attempt: {0}/{1}/{2}, nextTodo: {5}, wordCount: {4}, stemmed: {6}               ",
          attemptCount, attemptLen, ++stemmedChunks * batchSize, lc.EnglishName, wordAutoIncrement, todo.Count, stemmedAll));

        var stems = new List<Tuple<Guid, string[]>>();
        foreach (var stem in dbStems) {
          if (stem == null || stem.stemms == null) continue;
          var arr = stem.stemms.Split(',');
          if (arr.Length == 1)
            continue;
          var hash = new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes(stem.stemms)));

          if (groups.ContainsKey(hash))
            continue;

          var groupId = groupIdAutoIncrement++;
          var wordIds = arr.Select(w => {
            if (!wordsIdx.TryGetValue(w, out Word wid))
              wordsIdx[w] = wid = new Word { id = wordAutoIncrement++, groupIds = new List<int>() { groupId } };
            else
              wid.groupIds.Add(groupId);
            todo.Add(new ToDo() { id = wid.id, word = w });
            return wid.id;
          }).ToArray();
          Array.Sort(wordIds);

          groups[hash] = new Group() {
            id = groupId,
            wordIds = wordIds
          };


          //stems.Add(Tuple.Create(hash, arr));

          //if (attemptCount == 1) {
          //  // assign ID to word
          //  if (Array.IndexOf(arr, stem.word) >= 0) { // source word is in stemms
          //    if (!wordsIdx.TryGetValue(stem.word, out Word wid))
          //      wordsIdx[stem.word] = wid = new Word { id = wordAutoIncrement++ };
          //    done[wid.id] = true;
          //  }
          //}

        }

        //foreach (var stemms in stems) {

        //  if (!groups.TryGetValue(stemms.Item1, out Group group))
        //    group = groups[stemms.Item1] = new Group() {
        //      id = groupIdAutoIncrement++,
        //    };

        //  // adjust stemms word IDs and add them to TODO
        //  var ids = stemms.Item2.Select(w => {
        //    if (!wordsIdx.TryGetValue(w, out Word wid))
        //      wordsIdx[w] = wid = new Word { id = wordAutoIncrement++, groupCount = 1 };
        //    else
        //      wid.groupCount++;
        //    todo.Add(new ToDo() { id = wid.id, word = w });
        //    return wid.id;
        //  }).ToArray();
        //  Array.Sort(ids);


        //  // create stemm group
        //  groups[stemms.Item1] = new Group() {
        //    id = groupIdAutoIncrement++,
        //    wordIds = ids
        //  };
        //}
      }, batchSize);
    }

    // stemm all words from source word-list. Called once for language
    void getAllStemms(string[] initialWords) {
      start = DateTime.Now;
      var lastWordsCount = 0;
      while (true) {
        string[] words;
        if (base.attemptCount > 0) // continue => process TODO words
          words = getTodoWords();
        else { // first attempt
          if (wordsIdx.Count == 0) // wordsIdx is empty => database created
            words = initialWords;
          else //update database
            words = initialWords.Where(w => !wordsIdx.ContainsKey(w)).ToArray();
        }

        attemptCount++;
        attemptLen = words.Length;
        stemmedChunks = 0;
        if (words.Length == 0) // nothing todo => break
          break;
        //first 30.000 words has two byte ID
        //if (attemptCount == 1 && words.Length > 30000) {
        //  getAllStemmsLow(words.Take(30000).ToArray());
        //  getAllStemmsLow(words.Skip(30000).ToArray());
        //} else
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
        attemptCount = attemptCount,
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
      public List<int> groupIds;
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
    public int stemmedAll;
  }

}
