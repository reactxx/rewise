using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace wordNet {

  public static class Import {

    public static string driver = AppDomain.CurrentDomain.BaseDirectory[0].ToString();
    static string root = driver + @":\rewise\data\wordnet\";

    public static wordNetDB.Context getContext(bool recreate = false) {
      var ctx = new wordNetDB.Context();
      if (!ctx.Database.Exists())
        ctx.Database.Create();
      else if (recreate) {
        ctx.Database.Delete();
        ctx.Database.Create();
      }
      return ctx;
    }

  }
}
