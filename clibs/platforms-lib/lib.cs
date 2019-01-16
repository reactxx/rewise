using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
//using System.Runtime;
using System.IO;
using System.Reflection.Metadata;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;
using System;

namespace platformsLib {
  public static class lib {
    public static string Test() { return "123"; }
    //  https://stackoverflow.com/questions/36789316/creating-a-syntax-tree-from-a-type-string-itypesymbol-in-roslyn
    public static Tuple<string, string> CreateTypeSyntax() {
      var options = new CSharpParseOptions(kind: SourceCodeKind.Regular);
      var root = CSharpSyntaxTree.ParseText(File.ReadAllText(@"C:\rewise\clibs\low-utils\trie\reader.cs"), options).GetRoot();
      var sb = new StringBuilder();
      var ts = new TSDriver();
      ts.onNodesAndTokens(root);
      var dart = new DartDriver();
      dart.onNodesAndTokens(root);
      return Tuple.Create(dart.sb.ToString(), ts.sb.ToString());
    }
  }

  public abstract class Driver {
    public StringBuilder sb = new StringBuilder();
    public virtual void onNodesAndTokens(SyntaxNode node) {
      foreach (var nt in node.ChildNodesAndTokens()) {
        if (nt.IsToken)
          sb.Append(nt.AsToken().ToFullString());
        else {
          var n = nt.AsNode(); var k = n.Kind();
          try {
            if (!onNodes(n, k)) onNodesAndTokens(n);
          } catch {
            var nodeText = n.ToFullString();
            throw;
          }
        }
      }
    }
    protected abstract bool onNodes(SyntaxNode node, SyntaxKind kind);
    protected abstract string replaceType(string type, bool isArray);

    protected string getType(SyntaxNode node) {
      var isArray = false;
      if (node.Kind() == SyntaxKind.ArrayType) {
        isArray = true;
        node = node.ChildNodes().First();
      }
      var name = node.ChildTokens().First().ValueText;
      return replaceType(name, isArray);
    }
  }

  public struct TypeInfo {
    public bool isArray;
    public string name;
  }
}