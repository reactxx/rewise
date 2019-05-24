using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using static WiktSchema;
using static WiktTtlParser;

// design time extension
namespace WiktModel {

  public abstract partial class Helper {
    public virtual IEnumerable<object> getChilds() { yield break; }
    public virtual void acceptProp(ParsedTriple t, Context ctx) { }
  }

  // Page
  public partial class Page {
    [JsonIgnore]
    public List<Page_Nym> Page_Nyms;

    public override IEnumerable<object> getChilds() => Page_Nyms != null ? Page_Nyms : Enumerable.Empty<object>();
    public override void acceptProp(ParsedTriple t, Context ctx) { }
  }

  // Entry
  public partial class Entry {
    [JsonIgnore]
    public WiktToSQL.HelperForm CanonicalForm;
    [JsonIgnore]
    public List<WiktToSQL.HelperForm> OtherForm;
    [JsonIgnore]
    public List<Entry_Sense> Entry_Senses;
    [JsonIgnore]
    public List<Entry_Nym> Entry_Nyms;

    public override IEnumerable<object> getChilds() {
      var res = Enumerable.Empty<object>();
      if (Entry_Senses != null) res = res.Concat(Entry_Senses);
      if (Entry_Nyms != null) res = res.Concat(Entry_Nyms);
      return res;
    }
    public override void acceptProp(ParsedTriple t, Context ctx) { }
  }

  // Statement
  public partial class Statement {
    [JsonIgnore]
    public WiktToSQL.HelperGloss Gloss;
  }


  // Sense
  public partial class Sense {
    [JsonIgnore]
    public List<Sense_Nym> Sense_Nyms;
    [JsonIgnore]
    public string blankDefinition;
    [JsonIgnore]
    public string blankExample;

    public override IEnumerable<object> getChilds() => Sense_Nyms != null ? Sense_Nyms : Enumerable.Empty<object>();
    public override void acceptProp(ParsedTriple t, Context ctx) { }
  }

  // Translation
  public partial class Translation {
    [JsonIgnore]
    public WiktToSQL.HelperGloss Gloss;

    public override void acceptProp(ParsedTriple t, Context ctx) { }
  }

}

public static class WiktToSQL {

  public class HelperForm : WiktModel.Helper {
    public override void acceptProp(ParsedTriple t, Context ctx) { }

    public string Pronunciation; // lexinfo:pronunciation - rdf:langString
    public string PhoneticRep; // ontolex:phoneticRep - rdf:langString
    public string WrittenRep; // ontolex:writtenRep - rdf:langString,
    public string Note; // skos:note - rdf:langString, skos:note - xsd:string

    // Uri values
    public byte Animacy { get; set; } // lexinfo:animacy - @olia,
    public byte Gender { get; set; } // lexinfo:gender - @lexinfo,
    public byte Number { get; set; } // lexinfo:number - @lexinfo,
    public byte Person { get; set; } // lexinfo:person - @lexinfo,
    public byte Tense { get; set; } // lexinfo:tense - @lexinfo,
    public byte VerbFormMood { get; set; } // lexinfo:verbFormMood - @lexinfo,
    public byte HasCase { get; set; } // olia:hasCase - @olia,
    public byte HasDefiniteness { get; set; } // olia:hasDefiniteness - @lexinfo,
    public byte HasDegree { get; set; } // olia:hasDegree - @olia,
    public byte HasGender { get; set; } // olia:hasGender - @olia,
    public byte HasInflectionType { get; set; } // olia:hasInflectionType - @olia,
    public byte HasMood { get; set; } // olia:hasMood - @olia,
    public byte HasNumber { get; set; } // olia:hasNumber - @olia,
    public byte HasPerson { get; set; } // olia:hasPerson - @olia,
    public byte HasTense { get; set; } // olia:hasTense - @olia,
    public byte HasValency { get; set; } // olia:hasValency - @olia,
    public byte HasVoice { get; set; } // olia:hasVoice - @olia,

  }

  public class HelperGloss : WiktModel.Helper {
    public byte partOfSpeech; //dbnary:partOfSpeech - @lexinfo,
    public string Rank; //dbnary:rank - xsd:int
    public int SenseNumber; //dbnary:senseNumber - xsd:string

    public override void acceptProp(ParsedTriple t, Context ctx) { }
  }

  static void consumeTriple(WiktTtlParser.Context ctx, VDS.RDF.Triple tr) {

  }

  public static WiktModel.Helper adjustNode(int? classType, string id, WiktTtlParser.Context ctx) {

    WiktModel.Helper createLow(int tp) {
      switch (tp) {
        case NodeTypes.Gloss: return new HelperGloss();
        case NodeTypes.Form: return new HelperForm();
        case NodeTypes.LexicalSense: return new WiktModel.Sense();
        case NodeTypes.Page: return new WiktModel.Page();
        case NodeTypes.Translation: return new WiktModel.Translation();
        case NodeTypes.Statement: return new WiktModel.Statement();
        default: return new WiktModel.Entry { NymType = (byte)-tp };
      }
    }

    ctx.idToObj.TryGetValue(id, out WiktModel.Helper res);

    if (classType == null && res == null) {
      ctx.addError("adjustNode", id);
      return null;
    }
    //Debug.Assert(classType != null || res != null);

    if (res != null) return res;

    res = ctx.idToObj[id] = createLow((int)classType);
    res.Id = ctx.idToObj.Count() + 1;
    return res;

  }


}
