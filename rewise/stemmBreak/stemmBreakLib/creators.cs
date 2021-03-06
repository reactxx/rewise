//Data from registry: HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\ContentIndex\Language
using System;
using System.Collections.Generic;

namespace StemmerBreakerNew {

  public static class Creators {

    public static bool hasStemmer(string lang) {
      return langToStemmerGuid.ContainsKey(lang);
    }

    static public IStemmer createStemmer(string lang) {
      if (!langToStemmerGuid.TryGetValue(lang, out string guid)) return null;
      var res = ComHelper.CreateInstance<IStemmer>(guidToStemmerFactory[guid], typeof(IStemmer));
      bool pfLicense = false;
      res.Init(1000, out pfLicense);
      return res;
    }

    static public IWordBreaker createBreaker(string lang) {
      var factory = langToBreakerGuid.TryGetValue(lang, out string guid) ? guidToBreakerFactory[guid] : invariantBreakerFactory;
      var res = ComHelper.CreateInstance<IWordBreaker>(factory, typeof(IWordBreaker));
      bool pfLicense = false;
      res.Init(true, 1000, out pfLicense);
      return res;
    }

    static Creators() {
      var modules = new Dictionary<string, LibraryModule>();
      foreach (var m in Langs.meta) {
        if (m.StemmerClass != null) {
          if (!langToStemmerGuid.ContainsKey(m.StemmerClass))
            langToStemmerGuid[m.Id] = m.StemmerClass;
          if (!guidToStemmerFactory.ContainsKey(m.StemmerClass))
            guidToStemmerFactory[m.StemmerClass] = getClassFactory(m.StemmerClass, modules);
        }
        if (m.BreakerClass != null) {
          if (!langToBreakerGuid.ContainsKey(m.BreakerClass))
            langToBreakerGuid[m.Id] = m.BreakerClass;
          if (!guidToBreakerFactory.ContainsKey(m.BreakerClass))
            guidToBreakerFactory[m.BreakerClass] = getClassFactory(m.BreakerClass, modules);
        }
      }
      invariantBreakerFactory = guidToBreakerFactory[langToBreakerGuid[Langs.invariantId]];
    }
    static Dictionary<string, IClassFactory> guidToStemmerFactory = new Dictionary<string, IClassFactory>();
    static Dictionary<string, IClassFactory> guidToBreakerFactory = new Dictionary<string, IClassFactory>();
    static Dictionary<string, string> langToStemmerGuid = new Dictionary<string, string>();
    static Dictionary<string, string> langToBreakerGuid = new Dictionary<string, string>();
    static IClassFactory invariantBreakerFactory;

    static IClassFactory getClassFactory(string classInfo, Dictionary<string, LibraryModule> modules) {
      var parts = classInfo.Split('/');
      var fn = parts[1];
      lock (modules) {
        if (!modules.TryGetValue(fn, out LibraryModule module))
          modules.Add(fn, module = LibraryModule.LoadModule(Root.root + fn));
        var guid = new Guid(parts[0]);
        return ComHelper.GetClassFactory(modules[fn], guid);
      }
    }
  }
}
//SQL query: EXEC sp_help_fulltext_system_components 'wordbreaker'
//SELECT * FROM sys.dm_fts_parser (' "einem Pferd die Sporen geben" ', 1031, 0, 0)
//SELECT * FROM sys.dm_fts_parser('FormsOf(INFLECTIONAL, "Sporen")', 1031, 0, 0)
/*
EXEC sys.sp_fulltext_load_thesaurus_file 1031
SELECT* FROM sys.dm_fts_parser('FormsOf(FREETEXT, "writer")', 1033, 0, 0)
SELECT* FROM sys.dm_fts_parser('FormsOf(THESAURUS, "author")', 1033, 0, 0)
SELECT* FROM sys.dm_fts_parser('FormsOf(INFLECTIONAL, "Bücherregal")', 1031, 0, 0)
SELECT* FROM sys.dm_fts_parser('FormsOf(THESAURUS, "Bücherregal")', 1031, 0, 0)

SELECT* FROM sys.dm_fts_parser('FormsOf(INFLECTIONAL, "Nahrungsmittelunverträglichkeit")', 1031, 0, 0)
SELECT* FROM sys.dm_fts_parser('FormsOf(THESAURUS, "Nahrungsmittelunverträglichkeit")', 1031, 0, 0)
SELECT* FROM sys.dm_fts_parser('FormsOf(FREETEXT, "Nahrungsmittelunverträglichkeit")', 1031, 0, 0)
*/
