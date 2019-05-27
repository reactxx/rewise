using System;
using System.Collections.Generic;
using System.Text;
using VDS.RDF;
using static WiktTtlParser;

//http://kaiko.getalp.org/static/lemon/dbnary-doc/
public static class WiktSchema {

  public class ParsedTriple {

    public ParsedTriple(Context ctx, Triple t) {
      var items = new[] { TripleItem.Create(t.Subject, ctx, 0), TripleItem.Create(t.Predicate, ctx, 1), TripleItem.Create(t.Object, ctx, 2) };
      foreach (var it in items) if (predType != WiktConsts.PredicateType.Ignore) parsedItem(ctx, it);
    }

    void parsedItem(Context ctx, TripleItem item) {
      var sb = new StringBuilder();
      switch (item.type) {
        case 0: //Uri
          var isData = item.Scheme == ctx.lang;
          var url = item.Scheme + ":" + item.Path;
          switch (item.triplePart) {
            case 0: // subject
              if (!isData) ctx.addError("!isData", url); else subjDataId = item.Path;
              return;
            case 1: // predicate
              predicateUri = url;
              if (WiktConsts.IgnoredProps.Contains(url)) { predType = WiktConsts.PredicateType.Ignore; return; }
              if (WiktConsts.parsePredicate(url, out predicate, out predType)) return;
              ctx.addError("wrong prop", url);
              return;
            case 2: // object
              if (isData) { objDataId = item.Path; return; }
              if (predType == WiktConsts.PredicateType.a) {
                if (WiktConsts.IgnoredClasses.Contains(url)) { predType = WiktConsts.PredicateType.Ignore; return; }
                objDataType = WiktConsts.NodeTypes.Contains(url) ? url : null;
                if (objDataType == null)
                  ctx.addError("classType != null", $"{subjDataId} {predicateUri} {url}");
                return;
              }
              if (predType == WiktConsts.PredicateType.UriValuesProps) {
                objUri = url;
                if (predicateUri == "lexinfo:partOfSpeech" && !WiktConsts.partOfSpeechDir.Contains(objUri)) predicateUri = "lexinfo:partOfSpeechEx";
                try { WiktConsts.ConstMan.enumValue(predicateUri, objUri); } catch {
                  ctx.addError("wrong uri value", $"{predicateUri}:{objUri}");
                  return;
                }
                return;
                //WiktConsts.ConstMan
              }
              if (item.Scheme == "lexvo") {
                objLang = item.Path; return;
              }
              ctx.addError("wrong value", $"{subjDataId} {predicateUri} {url}");
              return;
          }
          break;
        case 1: //blank
          switch (item.triplePart) {
            case 0: subjBlankId = item.InternalID; return;
            case 2: objBlankId = item.InternalID; return;
            case 1: ctx.addError("blank in prop", item.InternalID); return;
          }
          break;
        case 2: //literal
          switch (item.triplePart) {
            case 2: objLang = item.Language; objValue = item.Value; return;
            default: ctx.addError("literal not in object", item.Value); return;
          }
      }
    }

    public void dumpForAcceptProp2(string className, string lang, Dictionary<string, dynamic[]> res) {
      void add(string key, string v) {
        var k = key + v;
        if (res.TryGetValue(k, out dynamic[] t)) t[1]++; else res[k] = new dynamic[] { key, 1 };
      }

      var val = $"";
      add("", val);
      add(lang, val);
      val += $"={className}";
      add("", val);
      add(lang, val);
      val += $"={predType}={predicate}";
      add("", val);
      add(lang, val);
      if (predType != WiktConsts.PredicateType.UriValuesProps) return;
      val += "=" + objUri;
      add("", val);
      add(lang, val);
    }

    public void dumpForAcceptProp(string className, string lang, Dictionary<string, int[]> res) {
      //if (predSchemeInfo == null) return;
      var sb = new StringBuilder();
      var langIdx = WiktQueries.allLangsIdx[lang];
      var allIdx = WiktQueries.allLangsIdx.Count;

      void fmt(string l, string r, bool cond = true) { if (!cond) return; sb.Append(r); sb.Append('('); sb.Append(l); sb.Append(')'); }


      sb.Append(className); sb.Append(": ");

      fmt(predType.ToString(), predicateUri); sb.Append('=');

      fmt("dataId", "", objDataId != null);
      fmt("value", ""/*objLang*/, objValue != null);
      fmt("lang", "" /*objLang*/, objValue == null && objLang != null);
      fmt("uriValue", objUri != null ? objUri.ToString() : "", objUri != null);

      var key = sb.ToString();
      res[key] = res.AddEx(key, arr => { arr[langIdx]++; arr[allIdx]++; return arr; }, () => new int[allIdx + 1]);
    }

    // **** Processed in ttlParser.parseTtls:
    public string subjDataId;  // e.g. eng:<subjDataId>
    public string subjBlankId; // e.g. .:<blankId>

    //WiktConsts.PredicateType = a

    public string objDataType; // objDataType contains className, "ontolex:Form"
    public string objBlankId; // evaluated to objValue. e.g. .:<blankId>. 

    // **** Processed in node.acceptProp:
    //public PredicateInfo predInfo;
    public string predicateUri;
    public WiktConsts.PredicateType predType;
    public WiktConsts.predicates predicate;

    public string objDataId;  // Data id for relation target. e.g. eng:<objDataId>
    public string objValue; // string value or objBlankId's value
    public string objLang; // iso-3 lang code from lexvo:<objLang>
    public string objUri; // e.g. "olia:hasGender"
  }

  public class TripleItem {
    public static TripleItem Create(INode node, Context ctx, int triplePart) {
      var s = node as UriNode;
      if (s != null) {
        var res = ctx.decodePath(s.Uri);
        res.triplePart = triplePart; res.type = 0;
        return res;
      }
      var b = node as BlankNode;
      if (b != null) return new TripleItem { InternalID = b.InternalID, triplePart = triplePart, type = 1 };
      var l = node as LiteralNode;
      if (l != null) return new TripleItem { Language = l.Language, Value = l.Value, triplePart = triplePart, type = 2 };
      throw new Exception();
    }

    public int type;
    public int triplePart;
    // url - 0
    public string Scheme;
    public string Path;
    // blank - 1
    public string InternalID;
    // literal - 2
    public string Language;
    public string Value;
  }

}
