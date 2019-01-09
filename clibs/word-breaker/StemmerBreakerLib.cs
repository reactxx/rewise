//Data from registry: HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\ContentIndex\Language
using LangsLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace StemmerBreaker {

  public class COM {
    public COM(string dllDir, string WBreakerClass, string StemmerClass, Dictionary<string, LibraryModule> modules) {
      stemmer = getClassFactory(dllDir, StemmerClass, modules);
      breaker = getClassFactory(dllDir, WBreakerClass, modules);
    }
    public IClassFactory stemmer;
    public IClassFactory breaker;

    static IClassFactory getClassFactory(string dllDir, string classInfo, Dictionary<string, LibraryModule> modules) {
      if (classInfo == null) return null;
      var parts = classInfo.Split('/');
      LibraryModule module;
      var fn = parts[1];
      if (!modules.TryGetValue(fn, out module))
        modules.Add(fn, module = LibraryModule.LoadModule(dllDir + fn));
      var guid = new Guid(parts[0]);
      var res = ComHelper.GetClassFactory(modules[fn], guid);
      if (res == null)
        Console.WriteLine(string.Format("*** Error: file={0}", fn));
      return res;
    }

    public IStemmer getStemmer() {
      var res = stemmer == null ? null : ComHelper.CreateInstance<IStemmer>(stemmer, typeof(IStemmer));
      if (res == null) return null;
      bool pfLicense = false;
      res.Init(1000, out pfLicense);
      return res;
    }

    internal IWordBreaker getWordBreaker() {
      var res = breaker == null ? null : ComHelper.CreateInstance<IWordBreaker>(breaker, typeof(IWordBreaker));
      if (res == null) return null;
      bool pfLicense = false;
      res.Init(true, 1000, out pfLicense);
      return res;
    }

  }

  public class LibManager {
    static LibManager instance;
    public static COM getCOM(langs lang, string root) {
      if (instance == null) instance = new LibManager(root);
      return instance.COMs[lang];
    }
    LibManager(string root) {
      var metas = new LangsLib.Metas();
      var modules = new Dictionary<string, LibraryModule>();
      COMs = metas.Items.ToDictionary(it => it.Key, it => new COM(root + @"clibs\FulltextDlls\", it.Value.WBreakerClass, it.Value.StemmerClass, modules));
    }
    public Dictionary<langs, COM> COMs;
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
