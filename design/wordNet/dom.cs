using System;
using System.Collections.Generic;
using System.Linq;

namespace wordNet {

  public class Context {
    public Context() { }
    public Context(IEnumerable<string> lines) {
      ids = lines.Select(l => l.Split(new char[] { '=' }, 2)).ToDictionary(parts => parts[0], parts => parts[1]);
    }
    public bool firstPhase;
    //public string language;
    public Dictionary<string, string> ids = new Dictionary<string, string>(); // e.g. {'qcn-10-10535604-n' : 'qcn-10-10535604-n=qcn=synset=3901296'}
    public Dictionary<string, Node> nodes = new Dictionary<string, Node>();
    public HashSet<string> emptySynset = new HashSet<string>();
    public int getId(string key) {
      if (!ids.TryGetValue(key, out string val)) throw new Exception();
      return int.Parse(val.Split('=')[2]);
    }
  }

  public class Node {
    public Node() { }
    public string lang;
    public static Node create(string className, string lang) {
      var res = createLow(className);
      res.lang = lang;
      return res;
    }
    static Node createLow(string className) {
      switch (className) {
        case "LexicalResource": return new LexicalResource();
        case "GlobalInformation": return new GlobalInformation();
        case "Lexicon": return new Lexicon();
        case "LexicalEntry": return new LexicalEntry();
        case "Lemma": return new Lemma();
        case "Sense": return new Sense();

        case "Synset": return new Synset();
        case "Definition": return new Definition();
        case "Statement": return new Statement();
        case "SynsetRelations": return new SynsetRelations();
        case "SynsetRelation": return new SynsetRelation();

        case "SenseAxes": return new SenseAxes();
        case "SenseAxis": return new SenseAxis();
        case "Target": return new Target();

        case "MonolingualExternalRefs": return new MonolingualExternalRefs();
        case "MonolingualExternalRef": return new MonolingualExternalRef();

        default: return new Node() { propName = className };
      }
    }
    public virtual IEnumerable<object> createDB(Context ctx) { yield break; }
    //[JsonIgnore]
    public string className { get { var n = GetType().Name; return n == "Node" ? propName : n; } }
    //[JsonIgnore]
    public string propName;
    public virtual Node addNode(Context ctx, Node node, string propValue = null) { return null; }
  }

  public class Root : Node {
    public Root() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node is LexicalResource) lexicalResources.Add(node as LexicalResource);
      else throw new Exception();
      return null;
    }
    public List<LexicalResource> lexicalResources = new List<LexicalResource>();
  }

  public class LexicalResource : Node {
    public LexicalResource() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node is GlobalInformation) globalInformation = node as GlobalInformation;
      else if (node is Lexicon) return node;
      else if (node is SenseAxes) senseAxes = node as SenseAxes;
      else throw new Exception();
      return null;
    }
    public GlobalInformation globalInformation;
    // public Lexicon lexicon;
    public SenseAxes senseAxes;
  }

  public class GlobalInformation : Node {
    public GlobalInformation() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node.propName == "label") label = propValue;
      else throw new Exception();
      return null;
    }
    public string label;
  }

  public class Lexicon : Node {
    public Lexicon() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node.propName == "owner") owner = propValue;
      else if (node.propName == "label") label = propValue;
      else if (node.propName == "language") {
        if (propValue != lang)
          throw new Exception();
        language = propValue;
      } else if (node.propName == "version") {
      } else if (node.propName == "languageCoding") {
      } else if (node is LexicalEntry) return node;
      else if (node is Synset) return node;
      else throw new Exception();
      return null;
    }

    public override IEnumerable<object> createDB(Context ctx) {
      yield return new wordNetDB.Lang { Id = language };
    }
    //public string version; // const "10"
    //public string languageCoding; //const "ISO 639-3"
    public string owner;
    public string label;
    public string language;
  }

  public class LexicalEntry : Node {
    public LexicalEntry() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node.propName == "id") {
        id = propValue;
        if (ctx.firstPhase) {
          if (ctx.ids.ContainsKey(id)) throw new Exception();
          ctx.ids[id] = lang + "=entry=" + ctx.ids.Count;
        } else {
          ctx.nodes[id] = this;
        }
      } else if (node is Lemma) {
        if (lemma != null) throw new Exception();
        lemma = node as Lemma;
      } else if (node is Sense) {
        senses.Add(node as Sense);
      } else throw new Exception();
      return null;
    }
    public Lemma lemma;
    public List<Sense> senses = new List<Sense>();
    public string id;
    public override IEnumerable<object> createDB(Context ctx) {
      var lid = ctx.getId(id);
      yield return new wordNetDB.Entry { Id = lid, PartOfSpeech = lemma.partOfSpeech, Lemma = lemma.writtenForm, LangId = lang };
      foreach (var s in senses.Select(s => new wordNetDB.Sense { EntryId = lid, SynsetId = ctx.getId(s.synset), LangId = lang }))
        yield return s;
    }
  }

  public class Lemma : Node {
    public Lemma() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node.propName == "writtenForm") writtenForm = propValue;
      else if (node.propName == "partOfSpeech") partOfSpeech = propValue;
      else throw new Exception();
      return null;
    }
    public string writtenForm;
    public string partOfSpeech; // v, n, a, r, s
  }

  public class Sense : Node {
    public Sense() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node.propName == "id") id = propValue;
      else if (node.propName == "synset") {
        if (!ctx.firstPhase) {
          if (!ctx.ids.TryGetValue(propValue, out string id) || !id.StartsWith(lang + "=synset="))
            throw new Exception();
        }
        synset = propValue;
      } else throw new Exception();
      return null;
    }
    public string id;
    public string synset;
  }

  public class Synset : Node {
    public Synset() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node.propName == "baseConcept") {
      } else if (node.propName == "id") {
        id = propValue;
        if (ctx.firstPhase) {
          if (ctx.ids.ContainsKey(id)) throw new Exception();
          ctx.ids[id] = lang + "=synset=" + ctx.ids.Count;
        } else {
          ctx.nodes[id] = this;
        }
      } else if (node is Definition) {
        if (definition != null) new Exception();
        definition = node as Definition;
      } else if (node is MonolingualExternalRefs) monolingualExternalRefs = node as MonolingualExternalRefs;
      else if (node is SynsetRelations) {
        if (synsetRelations != null) new Exception();
        synsetRelations = node as SynsetRelations;
      } else throw new Exception();
      return null;
    }
    public SynsetRelations synsetRelations;
    public Definition definition;
    public MonolingualExternalRefs monolingualExternalRefs;
    public string id;
    //public string baseConcept; // const "3"
    public int senseCount;
    public override IEnumerable<object> createDB(Context ctx) {
      if (ctx.emptySynset.Contains(id))
        yield break;
      var sid = ctx.getId(id);
      var synset = new wordNetDB.Synset { Id = sid, LangId = lang };
      if (definition != null) {
        synset.Meaning = definition.gloss;
        foreach (var s in definition.statements.Select(s => new wordNetDB.Example { Text = s.example, SynsetId = sid, LangId = lang }))
          yield return s;
      }
      yield return synset;
      if (synsetRelations != null) {
        var tids = new HashSet<int>();
        foreach (var s in synsetRelations.synsetRelations.Select(r => {
          if (ctx.emptySynset.Contains(r.targets))
            return null;
          var tid = ctx.getId(r.targets);
          if (tids.Contains(tid)) return null;
          tids.Add(tid);
          return new wordNetDB.Relation { FromId = sid, ToId = tid, Type = r.relType, LangId = lang };
        }))
          if (s != null) yield return s;
      }
    }
  }

  public class Definition : Node {
    public Definition() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node.propName == "gloss") gloss = propValue;
      else if (node is Statement) statements.Add(node as Statement);
      else throw new Exception();
      return null;
    }
    public string gloss;
    public List<Statement> statements = new List<Statement>();
  }
  public class Statement : Node {
    public Statement() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node.propName == "example") example = propValue;
      else throw new Exception();
      return null;
    }
    public string example;
  }
  public class SynsetRelations : Node {
    public SynsetRelations() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node is SynsetRelation) synsetRelations.Add(node as SynsetRelation);
      else throw new Exception();
      return null;
    }
    public List<SynsetRelation> synsetRelations = new List<SynsetRelation>();
  }

  public class SynsetRelation : Node {
    public SynsetRelation() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node.propName == "relType") relType = propValue;
      else if (node.propName == "targets") {
        if (!ctx.firstPhase) {
          if (!ctx.ids.TryGetValue(propValue, out string id) || !id.StartsWith(lang + "=synset="))
            throw new Exception();
        }
        targets = propValue;
      } else throw new Exception();
      return null;
    }
    public string relType; // enum, see C:\rewise\data\wordnet\enums.txt
    public string targets;
  }

  public class SenseAxes : Node {
    public SenseAxes() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node is SenseAxis) return node;
      else throw new Exception();
    }
  }

  public class SenseAxis : Node {
    public SenseAxis() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node is Target) targets.Add(node as Target);
      else if (node.propName == "id") {
      } else if (node.propName == "relType") {
      } else throw new Exception();
      return null;
    }
    public List<Target> targets = new List<Target>();
    public override IEnumerable<object> createDB(Context ctx) {
      int srcId = -1, transId = -1; string trans = null;
      if (targets.Count != 2) throw new Exception();
      foreach (var t in targets) {
        var isSrcId = t.ID.StartsWith("eng-30-");
        if (!isSrcId && !t.ID.StartsWith(lang + "-10-")) throw new Exception();
        var id = isSrcId ? t.ID.Replace("eng-30-", "eng-10-") : t.ID;
        var intId = ctx.getId(id);
        if (isSrcId) srcId = intId;
        else {
          transId = intId;
          trans = id;
        }
      }
      if (ctx.emptySynset.Contains(trans))
        yield break;
      yield return new wordNetDB.Translation {
        SrcId = srcId,
        TransId = transId,
        LangId = lang,
      };
    }
  }

  public class Target : Node {
    public Target() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node.propName == "ID") ID = propValue;
      else throw new Exception();
      return null;
    }
    public string ID;
  }

  public class MonolingualExternalRefs : Node {
    public MonolingualExternalRefs() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node is MonolingualExternalRef) monolingualExternalRefs.Add(node as MonolingualExternalRef);
      else throw new Exception();
      return null;
    }
    public List<MonolingualExternalRef> monolingualExternalRefs = new List<MonolingualExternalRef>();
  }

  public class MonolingualExternalRef : Node {
    public MonolingualExternalRef() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node.propName == "externalSystem") externalSystem = propValue;
      else if (node.propName == "relType") relType = propValue;
      else if (node.propName == "externalReference") externalReference = propValue;
      else throw new Exception();
      return null;
    }
    public string externalSystem;
    public string relType;
    public string externalReference;
  }
}
