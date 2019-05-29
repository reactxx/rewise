using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VDS.RDF;
using WiktModel;
using static WiktConsts;

public static class WiktSchema {

  public class ParsedTriple {

    public bool setValue(WiktCtx ctx, Helper owner, predicates pred, ref string fld) {
      if (predicate != pred) return false;
      setValueLow(ctx, owner, pred, ref fld);
      return true;
    }
    public bool setValueWithLang(WiktCtx ctx, Helper owner, predicates pred, ref string fld, ref string lang) {
      if (predicate != pred) return false;
      setValueLow(ctx, owner, pred, ref fld);
      if (objLang != null) {
        if (pred == predicates.dbnary_writtenForm)
          lang = lang == null ? objLang : $"{lang}|{objLang}";
        else {
          if (lang != null) ctx.log(owner, pred, $"DUPL LANG");
          if (objLang != null) lang = objLang;
        }
      }
      return true;
    }
    void setValueLow(WiktCtx ctx, Helper owner, predicates pred, ref string fld) {
      if (pred == predicates.ontolex_writtenRep || pred == predicates.skos_example) {
        fld = fld == null ? objValue : $"{fld}|{objValue}";
      } else {
        if (fld != null) ctx.log(owner, pred, $"DUPL");
        fld = objValue;
      }
    }
    public bool setIntValue(WiktCtx ctx, Helper owner, predicates pred, ref int? fld) {
      if (predicate != pred) return false;
      if (fld != null) ctx.log(owner, pred, "DUPL");
      if (int.TryParse(objValue, out int v)) fld = v;
      else ctx.log(owner, pred, $"INT wrong value");
      return true;
    }
    public bool setUriValue<T>(WiktCtx ctx, Helper owner, predicates pred, ref T fld) where T : Enum {
      if (predicate != pred) return false;
      //if ((byte)(object)fld != 0) ctx.log(owner, pred, "DUPL");
      fld = (T)(object)(byte)((byte)(object)fld | (byte)(object)ConstMan.enumValue<T>(objUri));
      return true;
    }
    public bool setUriValues<T>(WiktCtx ctx, Helper owner, predicates pred, ref List<T> flds) where T : Enum {
      if (predicate != pred) return false;
      if (flds == null) flds = new List<T>();
      T fld = (T)(object)(byte)0;
      setUriValue<T>(ctx, owner, pred, ref fld);
      if (flds.Contains(fld)) ctx.log(owner, pred, "DUPL");
      flds.Add(fld);
      return true;
    }
    public bool setFormInfosValue(WiktCtx ctx, Helper owner, ref FormInfos fld) {
      return setUriValue(ctx, owner, predicates.olia_hasDegree, ref fld.hasDegree) ||
      setUriValue(ctx, owner, predicates.olia_hasDegree, ref fld.hasDegree) ||
      setUriValue(ctx, owner, predicates.olia_hasInflectionType, ref fld.hasInflectionType) ||
      setUriValue(ctx, owner, predicates.olia_hasCountability, ref fld.hasCountability) ||
      setUriValue(ctx, owner, predicates.olia_hasMood, ref fld.hasMood) ||
      setUriValue(ctx, owner, predicates.olia_hasVoice, ref fld.hasVoice) ||
      setUriValue(ctx, owner, predicates.olia_hasCase, ref fld.hasVoice) ||
      setUriValue(ctx, owner, predicates.lexinfo_animacy, ref fld.animacy) ||
      setUriValue(ctx, owner, predicates.lexinfo_verbFormMood, ref fld.verbFormMood) ||
      setUriValue(ctx, owner, predicates.lexinfo_gender, ref fld.number) ||
      setUriValue(ctx, owner, predicates.lexinfo_number, ref fld.number) ||
      setUriValue(ctx, owner, predicates.lexinfo_tense, ref fld.number) ||
      setUriValue(ctx, owner, predicates.lexinfo_person, ref fld.number) ||
      setUriValue(ctx, owner, predicates.olia_hasNumber, ref fld.number) ||
      setUriValue(ctx, owner, predicates.olia_hasPerson, ref fld.number) ||
      setUriValue(ctx, owner, predicates.olia_hasTense, ref fld.number) ||
      setUriValue(ctx, owner, predicates.olia_hasGender, ref fld.number);
      //setUriValue(ctx, owner, predicates.number, ref fld.number) ||
      //setUriValue(ctx, owner, predicates.person, ref fld.person) ||
      //setUriValue(ctx, owner, predicates.gender, ref fld.gender) ||
      //setUriValue(ctx, owner, predicates.tense, ref fld.tense);
    }
    public bool setNymsValue(WiktCtx ctx, Helper owner, ref List<NymRel> fld) {
      if (!nymProp2value.TryGetValue(predicate, out rdf_predicate type)) return false;
      if (fld == null) fld = new List<NymRel>();
      var obj = ctx.designGetObj(objDataId);
      if (obj == null) ctx.log(owner, predicate, "NYM REL not found");
      else if (!(obj is Page)) ctx.log(owner, predicate, "NYM REL not Page");
      else {
        var nr = new NymRel { relId = obj.id, type = type };
        fld.Add(nr);
        // add back reference
        var p = obj as Page;
        if (p.nymsOf == null) p.nymsOf = new List<NymRel>();
        p.nymsOf.Add(new NymRel { relId = owner.id, type = type });
      }
      return true;
    }

    public bool setRefValue<T>(WiktCtx ctx, Helper owner, predicates pred, ref int? fld) where T : Helper {
      if (predicate != pred) return false;
      if (fld != null) ctx.log(owner, pred, "DUPL");
      var obj = ctx.designGetObj(objDataId);
      if (obj == null) ctx.log(owner, pred, "REL not found");
      else if (!(obj is T)) ctx.log(owner, pred, $"REL: expected {typeof(T).Name}, found {obj.GetType().Name}");
      else fld = obj.id;
      return true;
    }

    public bool setRefValues<T>(WiktCtx ctx, Helper owner, predicates pred, ref List<int> flds) where T : Helper {
      if (predicate != pred) return false;
      var obj = ctx.designGetObj(objDataId);
      if (flds == null) flds = new List<int>();
      if (obj == null) ctx.log(owner, pred, "REF not found");
      else if (!(obj is T)) ctx.log(owner, pred, $"REL: expected {typeof(T).Name}, found {obj.GetType().Name}");
      else {
        if (flds.Contains(obj.id)) ctx.log(owner, pred, "DUPL");
        flds.Add(obj.id);
      }
      return true;
    }

    public static firstRunResult firstRun(WiktCtx ctx, Triple t) {
      var items = new[] { TripleItem.Create(t.Subject, ctx, 0), TripleItem.Create(t.Predicate, ctx, 1), TripleItem.Create(t.Object, ctx, 2) };

      if (items.Any(it => it.type != 0 /*not URI type*/)) return null;

      // 1.
      var item = items[1];
      if (!parsePredicate(item.url, out predicates predicate, out PredicateType predType)) return null;
      if (predType != PredicateType.a) return null;

      // 0.
      item = items[0];
      if (item.Scheme != ctx.lang) return null;
      var res = new firstRunResult { subjDataId = item.url };

      // 2.
      item = items[2];
      if (!NodeTypes.Contains(item.url)) return null;
      res.objDataType = item.url;
      return res;
    }
    public class firstRunResult { public string subjDataId; public string objDataType; }

    public ParsedTriple(WiktCtx ctx, Triple t) {
      var items = new[] { TripleItem.Create(t.Subject, ctx, 0), TripleItem.Create(t.Predicate, ctx, 1), TripleItem.Create(t.Object, ctx, 2) };
      foreach (var it in items) if (predType != PredicateType.Ignore) parsedItem(ctx, it);
    }

    void parsedItem(WiktCtx ctx, TripleItem item) {
      var sb = new StringBuilder();
      switch (item.type) {
        case 0: //Uri
          var isData = item.Scheme == ctx.lang;
          var url = item.url;
          switch (item.triplePart) {
            case 0: // subject
              if (!isData) ctx.log(null, predicates.no, "!isData" + url); else subjDataId = url;
              return;
            case 1: // predicate
              predicateUri = url;
              if (IgnoredProps.Contains(url)) { predType = PredicateType.Ignore; return; }
              if (parsePredicate(url, out predicate, out predType)) return;
              ctx.log(null, predicates.no, "wrong prop" + url);
              return;
            case 2: // object
              if (isData) { objDataId = url; return; }
              if (predType == PredicateType.a) {
                if (IgnoredClasses.Contains(url)) { predType = PredicateType.Ignore; return; }
                objDataType = NodeTypes.Contains(url) ? url : null;
                if (objDataType == null)
                  ctx.log(null, predicates.no, $"classType != null {subjDataId} {predicateUri} {url}");
                return;
              }
              if (predType == PredicateType.UriValuesProps) {
                objUri = url;
                if (predicateUri == "lexinfo:partOfSpeech" && !partOfSpeechDir.Contains(objUri)) {
                  predicateUri = "lexinfo:partOfSpeechEx";
                  predicate = predicates.lexinfo_partOfSpeechEx;
                }
                //try { ConstMan.enumValue(predicateUri, objUri); } catch {
                //  ctx.addError("wrong uri value", $"{predicateUri}:{objUri}");
                //  return;
                //}
                return;
              }
              if (item.Scheme == "lexvo") {
                objLang = item.Path; return;
              }
              ctx.log(null, predicates.no, $"wrong value {subjDataId} {predicateUri} {url}");
              return;
          }
          break;
        case 1: //blank
          switch (item.triplePart) {
            case 0: subjBlankId = item.InternalID; return;
            case 2: objBlankId = item.InternalID; return;
            case 1: ctx.log(null, predicates.no, "blank in prop" + item.InternalID); return;
          }
          break;
        case 2: //literal
          switch (item.triplePart) {
            case 2: objLang = item.Language; objValue = item.Value; return;
            default: ctx.log(null, predicates.no, "literal not in object" + item.Value); return;
          }
      }
    }

    public void dumAllProps(string className, string lang, Dictionary<string, dynamic[]> res) {
      void add(string key, string v) {
        var k = key + v; if (res.TryGetValue(k, out dynamic[] t)) t[1]++; else res[k] = new dynamic[] { key, 1 };
      }
      var val = $""; add("", val); add(lang, val);
      val += $"={className}"; add("", val); add(lang, val);
      val += $"={predType}={predicate}"; add("", val); add(lang, val);
      if (predType != PredicateType.UriValuesProps) return;
      val += "=" + objUri; add("", val); add(lang, val);
    }

    // **** Processed in ttlParser.parseTtls:
    public string subjDataId;  // e.g. eng:<subjDataId>
    public string subjBlankId; // e.g. .:<blankId>

    //PredicateType = a

    public string objDataType; // objDataType contains className, "ontolex:Form"
    public string objBlankId; // evaluated to objValue. e.g. .:<blankId>. 

    // **** Processed in node.acceptProp:
    //public PredicateInfo predInfo;
    public string predicateUri;
    public PredicateType predType;
    public predicates predicate;

    public string objDataId;  // Data id for relation target. e.g. eng:<objDataId>
    public string objValue; // string value or objBlankId's value
    public string objLang; // iso-3 lang code from lexvo:<objLang>
    public string objUri; // e.g. "olia:hasGender"
  }

  public class TripleItem {
    public static TripleItem Create(INode node, WiktCtx ctx, int triplePart) {
      var s = node as UriNode;
      if (s != null) {
        var res = ctx.decodePath(s.Uri);
        res.url = res.Scheme + ':' + res.Path;
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
    public string url;
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
