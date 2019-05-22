using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace VDS.LM {
  public static class Parser {
    public static void parse(string fn, Action<Triple, int> onTriple, IEnumerable<string> namespaces = null) {
      var ttlparser = new TurtleParser();
      Options.InternUris = false;
      using (var graph = new MyGraph(onTriple))
      using (var rdr = new MyStreamReader(fn, namespaces))
        ttlparser.Load(new MyHadler(graph, null), rdr);
    }
  }

  class MyStreamReader : StreamReader {
    bool nsReaded = true;
    char[] ns = null;
    public MyStreamReader(string path, IEnumerable<string> namespaces) : base(path) {
      if (namespaces != null) {
        nsReaded = false;
        var s = string.Join("", namespaces.Select(n => $"@prefix {n}: <{n}:> ."));
        ns = s.ToCharArray();
        while (Peek() == '@') {
          while (Read() != '\n') { }
        }
      }
    }
    public override int Read(char[] buffer, int index, int count) {
      if (!nsReaded) {
        nsReaded = true;
        Debug.Assert(count>= ns.Length);
        Array.Copy(ns, buffer, ns.Length);
        return ns.Length;
      }
      return base.Read(buffer, index, count);
    }
  }

  class MyHadler : RDF.Parsing.Handlers.GraphHandler {
    public MyHadler(IGraph g, Dictionary<string, string> namespaces) : base(g) {
      if (namespaces != null) {
        g.NamespaceMap.Clear();
        foreach (var key in g.NamespaceMap.Prefixes.ToArray())
          g.NamespaceMap.RemoveNamespace(key);
        foreach (var kv in namespaces)
          g.NamespaceMap.AddNamespace(kv.Key, new Uri(kv.Value));
      }
    }
    protected override bool HandleNamespaceInternal(string prefix, Uri namespaceUri) {
      return true;
    }
    public override IUriNode CreateUriNode(Uri uri) {
      return base.CreateUriNode(uri);
    }
  }

  class MyGraph : NonIndexedGraph {
    public MyGraph(Action<Triple, int> onTriple) : base() {
      this.onTriple = onTriple;
    }
    Action<Triple, int> onTriple;
    int count = 0;
    public override bool Assert(Triple t) {
      onTriple(t, count++);
      return true;
    }

  }

}
