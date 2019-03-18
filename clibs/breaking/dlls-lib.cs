//Data from registry: HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\ContentIndex\Language
//using LangsLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace StemmerBreaker {

  public class COM {

    public static COM getCOM(Langs.CldrLang meta) {
      return new COM(Root.root, meta.BreakerClass, meta.StemmerClass, modules);
    }
    static Dictionary<string, LibraryModule> modules = new Dictionary<string, LibraryModule>();

    public COM(string dllDir, string WBreakerClass, string StemmerClass, Dictionary<string, LibraryModule> modules) {
      stemmer = getClassFactory(dllDir, StemmerClass, modules);
      breaker = getClassFactory(dllDir, WBreakerClass, modules);
    }
    public IClassFactory stemmer;
    public IClassFactory breaker;

    static IClassFactory getClassFactory(string dllDir, string classInfo, Dictionary<string, LibraryModule> modules) {
      if (classInfo == null) return null;
      var parts = classInfo.Split('/');
      var fn = parts[1];
      lock (modules) {
        if (!modules.TryGetValue(fn, out LibraryModule module))
          modules.Add(fn, module = LibraryModule.LoadModule(dllDir + fn));
        var guid = new Guid(parts[0]);
        return ComHelper.GetClassFactory(modules[fn], guid);
      }
    }

    public IStemmer getStemmer() {
      if (stemmer == null) return null;
      var res = ComHelper.CreateInstance<IStemmer>(stemmer, typeof(IStemmer));
      bool pfLicense = false;
      res.Init(1000, out pfLicense);
      return res;
    }

    internal IWordBreaker getWordBreaker() {
      if (breaker == null) return null;
      var res = ComHelper.CreateInstance<IWordBreaker>(breaker, typeof(IWordBreaker));
      bool pfLicense = false;
      res.Init(true, 1000, out pfLicense);
      return res;
    }

  }

  //public static class LibManager {
  //  public static COM getCOM(Langs.CldrLang meta) {
  //    return new COM(Root.root + @"appdata\", meta.wBreakerClass, meta.stemmerClass, modules);
  //  }
  //  static Dictionary<string, LibraryModule> modules = new Dictionary<string, LibraryModule>();
  //  //public Dictionary<string, COM> COMs;
  //}

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
