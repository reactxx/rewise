using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace wordNet {

  public class Context {
    public bool firstPhase;
    public string language;
    public Dictionary<string, string> ids = new Dictionary<string, string>();
    public int getId(string key) {
      if (!ids.TryGetValue(key, out string val)) throw new Exception();
      return int.Parse(val.Split('=')[2]);
    }
  }

  public class Node {
    public Node() { }
    public static Node create(string className) {
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
    [JsonIgnore]
    public string className { get { var n = GetType().Name; return n == "Node" ? propName : n; } }
    [JsonIgnore]
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
      else if (node is Lexicon) lexicon = node as Lexicon;
      else if (node is SenseAxes) senseAxes = node as SenseAxes;
      else throw new Exception();
      return null;
    }
    public GlobalInformation globalInformation;
    public Lexicon lexicon;
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
      else if (node.propName == "language") ctx.language = language = propValue;
      else if (node.propName == "version") {
      } else if (node.propName == "languageCoding") {
      } else if (node is LexicalEntry) return node;
      else if (node is Synset) return node;
      else throw new Exception();
      return null;
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
        if (ctx.firstPhase) {
          if (ctx.ids.ContainsKey(propValue)) throw new Exception();
          ctx.ids[propValue] = ctx.language + "=entry=" + ctx.ids.Count;
        }
        id = propValue;
      } else if (node is Lemma) {
        if (lemma != null) throw new Exception();
        lemma = node as Lemma;
      } else if (node is Sense) senses.Add(node as Sense);
      else throw new Exception();
      return null;
    }
    public Lemma lemma;
    public List<Sense> senses = new List<Sense>();
    public string id;
    public override IEnumerable<object> createDB(Context ctx) {
      var lid = ctx.getId(id);
      yield return new wordNetDB.LexicalEntry { Id = lid, PartOfSpeech = lemma.partOfSpeech, Lemma = lemma.writtenForm };
      foreach (var s in senses.Select(s => new wordNetDB.Sense { LexicalEntryId = lid, SynsetId = ctx.getId(s.synset) }))
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
          if (!ctx.ids.TryGetValue(propValue, out string id) || !id.StartsWith(ctx.language + "=synset="))
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
        if (ctx.firstPhase) {
          if (ctx.ids.ContainsKey(propValue)) throw new Exception();
          ctx.ids[propValue] = ctx.language + "=synset=" + ctx.ids.Count;
        }
        id = propValue;
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
    public override IEnumerable<object> createDB(Context ctx) {
      var sid = ctx.getId(id);
      if (definition != null) {
        yield return new wordNetDB.Synset { Id = sid, Gloss = definition.gloss };
        foreach (var s in definition.statements.Select(s => new wordNetDB.Statement { Example = s.example, SynsetId = sid }))
          yield return s;
      }
      if (synsetRelations != null)
        foreach (var s in synsetRelations.synsetRelations.Select(r =>
          new wordNetDB.SynsetRelation { SynsetFromId = sid, SynsetToId = ctx.getId(r.targets), RelType = r.relType }))
          yield return s;
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
          if (!ctx.ids.TryGetValue(propValue, out string id) || !id.StartsWith(ctx.language + "=synset="))
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
    //public List<SenseAxis> senseAxes = new List<SenseAxis>();
  }

  public class SenseAxis : Node {
    public SenseAxis() : base() { }
    public override Node addNode(Context ctx, Node node, string propValue = null) {
      if (node is Target) targets.Add(node as Target);
      else if (node.propName == "id") id = propValue;
      else if (node.propName == "relType") {
      } else throw new Exception();
      return null;
    }
    public List<Target> targets = new List<Target>();
    public string id; // ?? not used
    //public string relType; // const "eq_synonym"
    public override IEnumerable<object> createDB(Context ctx) {
      if (targets.Count != 2) throw new Exception();
      var t0 = targets[0].testInnerId(ctx); var t1 = targets[1].testInnerId(ctx);
      if ((t0 > 0) == (t1 > 0)) throw new Exception();
      yield return new wordNetDB.Translation {
        SynsetFromId = t0 > 0 ? t0 : t1,
        SynsetToId = t0 > 0 ? -t1 : -t0,
        Language = ctx.language,
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
    public int testInnerId(Context ctx) {
      var innerId = ID.StartsWith(ctx.language + "-10-");
      var outerId = ID.StartsWith("eng-30-");
      if (!innerId && !outerId) throw new Exception();
      var testValue = outerId ? ID.Replace("eng-30-", "eng-10-") : ID;
      if (!ctx.ids.TryGetValue(testValue, out string id)) throw new Exception();
      return innerId ? ctx.getId(testValue) : -ctx.getId(testValue);
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
