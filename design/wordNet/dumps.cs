using EntityFramework.BulkInsert;
using EntityFramework.BulkInsert.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

/*
results in 
- DB
- rewise\data\wordnet\ids.txt
- rewise\data\wordnet\root.json
*/

namespace wordNet {

  public static class Dumps {

    public static string driver = AppDomain.CurrentDomain.BaseDirectory[0].ToString();
    static string root = driver + @":\rewise\data\wordnet\";

    static void dump(wordNetDB.Context dbCtx, string lang) {
      var synsets = dbCtx.Synsets.Where(t => t.LangId == "eng");
      var data = synsets.Select(syn => new {
        meaning = syn.Meaning,
        src = syn.Senses.Select(s => s.Entry.Lemma),
        examples = syn.Examples.Where(e => !string.IsNullOrEmpty(e.Text)).Select(e => e.Text),
        partOfSpeach = syn.Senses.Select(s => s.Entry.PartOfSpeech).Distinct(),
        trans = syn.Trans.Where(t => t.LangId == lang).Select(s => s.Trans.Senses.Select(ss => ss.Entry.Lemma)),
      }).ToArray();
      //var wrongs = data.Where(d => d.partOfSpeach.Count() != 1).Count();
      //if (wrongs > 0) {
      //}
      var lines = data.Where(d => lang=="eng" ? true : d.trans.Any(t => t.Count() > 0)).Select(d => new List<string> {
        d.src.OrderBy(s => s).Aggregate((r,i) => r + ", " + i) + " (" + d.partOfSpeach.Single() + ")",
        "    @ " + d.meaning,
        lang=="eng" ? null : "    = " + d.trans.Single().Aggregate((r,i) => r + ", " + i),
        d.examples.Count()==0 ? null : "    # " + d.examples.Where(e => !string.IsNullOrEmpty(e)).DefaultIfEmpty().Aggregate((r,i) => r + "\n    # " + i),
      }).OrderBy(s => s.First());
      File.WriteAllLines(root + "\\dump\\" + lang + ".txt", lines.SelectMany(s => s).Where(l => l != null));
    }


    public static void dumps() {
      using (var dbCtx = wordNetDB.Context.getContext(false)) {
        foreach (var lang in dbCtx.Langs.Where(l => l.Id != "").Select(l => l.Id)) {
          // if (lang != "slk" && lang != "eng") continue;
          dump(dbCtx, lang);
        }
      }
    }

  }
}
