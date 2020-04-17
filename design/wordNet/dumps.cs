using System.Collections.Generic;
using System.IO;
using System.Linq;

/*
results in 
- DB
- rewise\data\wordnet\ids.txt
- rewise\data\wordnet\root.json
*/

namespace wordNet {

  public static class Dumps {

    public static void dumps() {
      var ctx = new Context(false);
      using (var dbCtx = wordNetDB.Context.getContext()) {
        var data = dbCtx.Synsets.Where(t => t.LangId == "eng").Select(syn => new {
          meaning = syn.Meaning,
          src = syn.Senses.Select(s => s.Entry.Lemma),
          id = syn.Id,
          examples = syn.Examples.Where(e => !string.IsNullOrEmpty(e.Text)).Select(e => e.Text),
          partOfSpeach = syn.Senses.Select(s => s.Entry.PartOfSpeech).Distinct(),
          trans = syn.TransSrc.Select(s => new { s.TransEntry.Lemma, s.TransEntry.LangId }),
        }).ToArray();
        foreach (var lang in dbCtx.Langs.Select(l => l.Id)) {
          var lines = data.Where(d => lang == "eng" ? true : d.trans.Where(l => l.LangId == lang).Any()).Select(d => new List<string> {
            d.src.OrderBy(s => s).Aggregate((r,i) => r + ", " + i) + "|" + d.partOfSpeach.Single() + "|#" + ctx.getOrigId(d.id),
            "    @ " + d.meaning,
            lang=="eng" ? null : "    = " + d.trans.Where(l => l.LangId==lang).Select(l => l.Lemma).Aggregate((r,i) => r + ", " + i),
            d.examples.Count()==0 ? null : "    & " + d.examples.Where(e => !string.IsNullOrEmpty(e)).DefaultIfEmpty().Aggregate((r,i) => r + "\n    & " + i),
          }).OrderBy(s => s.First());
          File.WriteAllLines(Context.root + "\\dump\\" + lang + ".txt", lines.SelectMany(s => s).Where(l => l != null));
        }
      }
    }

    public static void langLemmas() {
      using (var dbCtx = wordNetDB.Context.getContext(false)) {
        var groups = dbCtx.Entries.GroupBy(e => e.LangId).ToArray();
        foreach (var grp in groups) {
          var lines = grp.Select(e => $"{e.Lemma} {(e.OriginNoWikt ? "w" : "")}[{e.PartOfSpeech}]").OrderBy(s => s);
          File.WriteAllLines(Context.root + $"\\dump-words\\{grp.Key}.txt", lines);
        }
      }
    }
    public static void lemmas() {
      var ctx = new Context(false);
      using (var dbCtx = wordNetDB.Context.getContext(false)) {
        var lemas = dbCtx.Entries.Where(e => e.LangId == "eng").Select(e => new {
          lemma = e.Lemma,
          partOfSpeech = e.PartOfSpeech,
          synsets = e.Senses.Select(s => s.Synset)
        }).ToArray();

        string dumpIds(IEnumerable<wordNetDB.Synset> synsets) =>
          synsets.OrderByDescending(s => s.Top5000).Select(s => (s.Top5000 ? "+" : "-") + ctx.getOrigId(s.Id)).DefaultIfEmpty().Aggregate((r, i) => r + "|" + i);

        File.WriteAllLines(Context.root + "\\dump\\eng_lemmas.txt",
          lemas.OrderBy(l => l.lemma).Select(l => l.lemma + "|" + l.partOfSpeech + "|"
          + dumpIds(l.synsets)));
        File.WriteAllLines(Context.root + "\\dump\\eng_lemmas_num.txt",
          lemas.OrderByDescending(l => l.synsets.Count()).Select(l
          => l.synsets.Count().ToString() + "/" + l.synsets.Where(s => s.Top5000).Count().ToString() + "|" + l.lemma + "|" + l.partOfSpeech + "|"
          + dumpIds(l.synsets)));
      }
    }

  }
}
