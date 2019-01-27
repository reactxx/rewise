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
  public class DartDriver : Driver {

    protected override bool onNodes(SyntaxNode node, SyntaxKind kind) {
      switch (kind) {
        case SyntaxKind.Parameter:
          var type = getType(node.ChildNodes().First());
          var name = node.ChildTokens().First().ValueText;
          sb.AppendFormat("{0} {1}", type, name);
          return true;
        default: return false;
      }
      
    }

    protected override string replaceType(string type, bool isArray) {
      if (isArray && type == "byte")
        return "Uint8List";
      switch (type) {
        case "string":
          return "String";
        case "byte":
          return "int";
        default: return type;
      }
    }

  }
}