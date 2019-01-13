using System.IO;

namespace fulltext {

  public class LoadStemms {

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
  }

}
