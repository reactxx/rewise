using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Linq;

namespace wordNet {
  public static class LmfStats {
    public static string driver = AppDomain.CurrentDomain.BaseDirectory[0].ToString();
    static string root = driver + @":\rewise\data\wordnet\";
    public static void run() {
      var stat = new Dictionary<string, int>();
      void add(string p) {
        if (stat.TryGetValue(p, out int c)) stat[p] = c + 1;
        else stat[p] = 1;
        count++;
        if ((count & 0xffff) == 0) Console.Write(count + " ");
      }
      var sb = new StringBuilder();
      foreach (var fn in Directory.EnumerateFiles(root, "*.xml")) {
        var doc = XElement.Load(fn);
        Console.WriteLine(fn);
        foreach (var el in doc.Descendants()) {
          add(path(null, el, sb));
          foreach (var at in el.Attributes())
            add(path(at, null, sb));
        }
        break;
      }
      File.WriteAllLines(root + "stat.txt", stat.OrderByDescending(kv => kv.Value).Select(kv => string.Format("{0}: {1}", kv.Key, kv.Value)));
    }
    static int count = 0;

    static string path(XAttribute attr, XElement el, StringBuilder sb, bool isFirst = true) {
      if (isFirst) sb.Clear();
      if (attr != null) {
        sb.Insert(0, ":" + attr.Name.LocalName);
        return path(null, attr.Parent, sb, false);
      } else {
        sb.Insert(0, "." + el.Name.LocalName);
        return el.Parent==null ? sb.ToString().Substring(1) : path(null, el.Parent, sb, false);
      }
    }
  }
}
