using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VDS.RDF;

public static class WiktToDB {

  public static void replaceUrlByIds() {
    foreach (var lang in WiktQueries.allLangs) replaceUrlById(lang);
  }

  static Dictionary<string, Dictionary<string, int>> idsToMemory(string dir) {
    var res = new Dictionary<string, Dictionary<string, int>>();
    var idFiles = Directory.GetFiles(dir, "*.ttl").Where(fn => Path.GetFileNameWithoutExtension(fn).ToLower().StartsWith("ids_")).ToArray();
    Parallel.ForEach(idFiles, new ParallelOptions { MaxDegreeOfParallelism = 4 }, fn => {
      var clsName = Path.GetFileNameWithoutExtension(fn).ToLower().Split('_')[1];
      var destFn = Path.ChangeExtension(fn, ".txt");
      Dictionary<string, int> ids;
      //if (File.Exists(destFn)) {
      //  var count = 0;
      //  ids = File.ReadAllLines(destFn).ToDictionary(id => id, id => count++);
      //} else
      using (var wr = new StreamWriter(destFn)) {
        ids = new Dictionary<string, int>();
        VDS.LM.Parser.parse(fn, (trip, count) => {
          var txt = System.Web.HttpUtility.UrlDecode((trip.Object as LiteralNode).Value);
          wr.WriteLine(txt);
          ids[txt] = ids.Count + 1;
        });
      }
      lock (res) res[clsName] = ids;
    });
    return res;
  }

  public static void replaceUrlById(string lang) {
    var dir = Corpus.Dirs.wikiesDbnary + @"graphDBExport\" + lang;
    if (!Directory.Exists(dir)) return;
    var files = Directory.GetFiles(dir).Select(fn => {
      var parts = Path.GetFileNameWithoutExtension(fn).ToLower().Split('_');
      var isProp = parts[0] == "prop" ? true : (parts[0] == "rel" ? false : (bool?)null);
      if (isProp == null) return null;
      return new ttlFile { fn = fn, name = Path.GetFileNameWithoutExtension(fn), isProp = (bool)isProp, fromCls = parts[1], predicate = parts[2], toCls = isProp == true ? null : parts[3] };
    }).
    Where(t => t != null).
    ToArray();

    var ids = idsToMemory(dir);
    var errors = new HashSet<string>();

    using (var wrProp = new StreamWriter(dir + @"\props.txt"))
    using (var wrRel = new StreamWriter(dir + @"\rels.txt"))
      foreach (var file in files) {
        var wr = file.isProp ? wrProp : wrRel;
        VDS.LM.Parser.parse(file.fn, (trip, count) => {
          wr.Write(WiktQueries.nameToId[file.fromCls]); wr.Write('|');
          var subjUri = trip.Subject as UriNode;
          try {
            wr.Write(ids[file.fromCls][subjUri.Uri.LocalPath]); wr.Write('|');
          } catch {
            errors.Add(subjUri.Uri.AbsolutePath);
            wr.Write("???" + subjUri.Uri.AbsolutePath);
          }
          wr.Write(propToId[file.name]); wr.Write('|');
          if (file.isProp) {
            var obj = trip.Object;
            if (obj is LiteralNode) wr.Write((obj as LiteralNode).Value);
            else if (obj is UriNode) wr.Write((obj as UriNode).Uri.LocalPath);
            else throw new Exception();
          } else {
            wr.Write(WiktQueries.nameToId[file.toCls]); wr.Write('|');
            var objUri = trip.Object as UriNode;
            try {
              wr.Write(ids[file.toCls][objUri.Uri.LocalPath]);
            } catch {
              errors.Add(objUri.Uri.AbsolutePath);
              wr.Write("???" + objUri.Uri.AbsolutePath);
            }
          }
          wr.WriteLine();
        });
      }
    if (errors.Count > 0)
      File.WriteAllLines(dir + @"\errors.txt", errors);
  }

  static Dictionary<string, string> propToId = new Dictionary<string, string> {
    {"prop_entry_language","el"},
    {"prop_entry_partofspeech","ep"},
    {"prop_form_note","fn"},
    {"prop_form_phoneticrep","fp"},
    {"prop_form_writtenrep","fw"},
    {"prop_gloss_rank","gr"},
    {"prop_gloss_sensenumber","gs"},
    {"prop_gloss_value","gv"},
    {"prop_sense_number","sn"},
    {"prop_trans_targetlanguagecode","tt"},
    {"prop_trans_usage","tu"},
    {"prop_trans_writtenform","tw"},
    {"rel_entry_canform_form","ecf"},
    {"rel_entry_otherform_form","eof"},
    {"rel_entry_sense_sense","ess"},
    {"rel_entry_synonyms_page","esp"},
    {"rel_page_descr_entry","pde"},
    {"rel_page_descr_multi","pdm"},
    {"rel_page_synonyms_page","psp"},
    {"rel_sense_synonyms_page","ssp"},
    {"rel_trans_gloss_gloss","tgg"},
    {"rel_trans_trans_entry","tte"},
    {"rel_trans_trans_sense","tts"},
  };


  class ttlFile {
    public string fn;
    public string name;
    public string fromCls;
    public bool isProp;
    public string predicate;
    public string toCls; // for isProp=false
  }
}
