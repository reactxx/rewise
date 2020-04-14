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

    static void dump(wordNetDB.Context dbCtx, Context ctx, string lang) {
      dbCtx.Database.CommandTimeout = 60;
      var synsets = dbCtx.Synsets.Where(t => t.LangId == "eng");
      var data = synsets.Select(syn => new {
        meaning = syn.Meaning,
        src = syn.Senses.Select(s => s.Entry.Lemma),
        id = syn.Id,
        examples = syn.Examples.Where(e => !string.IsNullOrEmpty(e.Text)).Select(e => e.Text),
        partOfSpeach = syn.Senses.Select(s => s.Entry.PartOfSpeech).Distinct(),
        trans = syn.Trans.Where(t => t.LangId == lang).SelectMany(s => s.Senses.Select(ss => ss.Entry.Lemma)),
      }).ToArray();
      var lines = data.Where(d => lang == "eng" ? true : d.trans.Any(t => t.Count() > 0)).Select(d => new List<string> {
        d.src.OrderBy(s => s).Aggregate((r,i) => r + ", " + i) + "|" + d.partOfSpeach.Single() + "|#" + ctx.getOrigId(d.id),
        "    @ " + d.meaning,
        lang=="eng" ? null : "    = " + d.trans.Aggregate((r,i) => r + ", " + i),
        d.examples.Count()==0 ? null : "    & " + d.examples.Where(e => !string.IsNullOrEmpty(e)).DefaultIfEmpty().Aggregate((r,i) => r + "\n    & " + i),
      }).OrderBy(s => s.First());
      File.WriteAllLines(root + "\\dump\\" + lang + ".txt", lines.SelectMany(s => s).Where(l => l != null));
    }

    static void dumpLemmas(wordNetDB.Context dbCtx, Context ctx) {
      var lemas = dbCtx.Entries.Where(e => e.LangId == "eng").Select(e => new {
        lemma = e.Lemma,
        partOfSpeech = e.PartOfSpeech,
        synsets = e.Senses.Select(s => s.SynsetId)
      }).ToArray();
      File.WriteAllLines(root + "\\dump\\eng_lemmas.txt", lemas.Select(l => l.lemma + "|" + l.partOfSpeech + "|" + l.synsets.Select(id => ctx.getOrigId(id)).Aggregate((r,i) => r + "|" + i)));
    }

    public static void dumps() {
      var ctx = new Context(File.ReadAllLines(root + "ids.txt"));
      using (var dbCtx = wordNetDB.Context.getContext(false)) {
        foreach (var lang in dbCtx.Langs.Where(l => l.Id != "").Select(l => l.Id)) {
          // if (lang != "slk" && lang != "eng") continue;
          dump(dbCtx, ctx, lang);
        }
      }
    }

  }
}
