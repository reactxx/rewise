using EntityFramework.BulkInsert;
using EntityFramework.BulkInsert.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    static void dump(string lang) {
      using (var dbCtx = wordNetDB.Context.getContext(false)) {
        var synsets = dbCtx.Synsets.Where(t => t.LangId == lang);
        var data = synsets.Select(syn => new { 
          meaning = syn.Meaning,
          src = syn.Senses.Select(s => s.Entry.Lemma),
          //trans = syn.TransTrans.Select(s => s.),
        }).ToArray();

      }
    }

  }
}
