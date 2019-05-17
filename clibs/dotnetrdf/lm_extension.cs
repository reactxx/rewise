using System;
using System.IO;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace VDS.LM {
  public static class Parser {
    public static void parse(string fn, Action<Triple, int> onTriple) {
      var ttlparser = new TurtleParser();
      Options.InternUris = false;
      using (var graph = new MyGraph(onTriple))
      using (var rdr = new StreamReader(fn))
        ttlparser.Load(graph, rdr);
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
