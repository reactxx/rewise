using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace wordNet {
  public static class LmfStats {
    public static string driver = AppDomain.CurrentDomain.BaseDirectory[0].ToString();
    static string root = driver + @":\rewise\data\wordnet\";
    public static void run() {
      var stat = new Dictionary<string, int>();
      var names = new string[10];
      void add(int deep) {
        var path = string.Join(".", names.Take(deep + 1));
        stat[path] = stat.TryGetValue(path, out int cnt) ? cnt + 1 : 1;
        count++;
        if ((count & 0xffff) == 0) Console.WriteLine(count);
      }
      XmlReaderSettings settings = new XmlReaderSettings();
      settings.DtdProcessing = DtdProcessing.Parse;
      foreach (var fn in Directory.EnumerateFiles(root, "*.xml")) {
        Console.WriteLine(fn);
        using (var tr = new StreamReader(fn))
        using (var rdr = XmlReader.Create(tr, settings)) {
          while (rdr.MoveToNextAttribute() || rdr.Read()) {
            if (rdr.NodeType == XmlNodeType.Attribute) {
              names[rdr.Depth] = rdr.LocalName;
              add(rdr.Depth);
            } else if (rdr.IsStartElement() || rdr.IsEmptyElement) {
              names[rdr.Depth] = rdr.LocalName;
              add(rdr.Depth);
            }
          }
        }
      }
      File.WriteAllLines(root + "stat.txt", stat.OrderByDescending(kv => kv.Value).Select(kv => string.Format("{0}: {1}", kv.Key, kv.Value)));
    }
    static int count = 0;

    static void readSubtree(XmlReader rdr, Dictionary<string, int> res, string path) {

    }

    static string path(XAttribute attr, XElement el, StringBuilder sb, bool isFirst = true) {
      if (isFirst) sb.Clear();
      if (attr != null) {
        sb.Insert(0, ":" + attr.Name.LocalName);
        return path(null, attr.Parent, sb, false);
      } else {
        sb.Insert(0, "." + el.Name.LocalName);
        return el.Parent == null ? sb.ToString().Substring(1) : path(null, el.Parent, sb, false);
      }
    }
  }
}
