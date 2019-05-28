using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static WiktSchema;
using static WiktTtlParser;

// design time extension
namespace WiktModel {

  public abstract partial class Helper {
    public virtual IEnumerable<object> getChilds() { yield break; }
    public virtual object acceptProp(ParsedTriple t, WiktCtx ctx) { return null; }
  }

  // Page
  public partial class Page {
    //[JsonIgnore]
    //public List<Page_Nym> Page_Nyms;

    //public override IEnumerable<object> getChilds() => Page_Nyms != null ? Page_Nyms : Enumerable.Empty<object>();
    public override object acceptProp(ParsedTriple t, WiktCtx ctx) { return null; }
  }

  // Entry
  public partial class Entry {
    [JsonIgnore]
    public WiktToSQL.HelperForm CanonicalForm;
    [JsonIgnore]
    public List<WiktToSQL.HelperForm> OtherForm;
    [JsonIgnore]
    public List<Entry_Sense> Entry_Senses;
    //[JsonIgnore]
    //public List<Entry_Nym> Entry_Nyms;

    //public override IEnumerable<object> getChilds() {
    //  var res = Enumerable.Empty<object>();
    //  if (Entry_Senses != null) res = res.Concat(Entry_Senses);
    //  if (Entry_Nyms != null) res = res.Concat(Entry_Nyms);
    //  return res;
    //}
    public override object acceptProp(ParsedTriple t, WiktCtx ctx) { return null; }
  }

  // Statement
  public partial class Statement {
    [JsonIgnore]
    public WiktToSQL.HelperGloss Gloss;
  }


  // Sense
  public partial class Sense {
    //[JsonIgnore]
    //public List<Sense_Nym> Sense_Nyms;
    [JsonIgnore]
    public string blankDefinition;
    [JsonIgnore]
    public string blankExample;

    //public override IEnumerable<object> getChilds() => Sense_Nyms != null ? Sense_Nyms : Enumerable.Empty<object>();
    public override object acceptProp(ParsedTriple t, WiktCtx ctx) { return null; }
  }

  // Translation
  public partial class Translation {
    [JsonIgnore]
    public WiktToSQL.HelperGloss Gloss;

    public override object acceptProp(ParsedTriple t, WiktCtx ctx) { return null; }
  }

}

public static class WiktToSQL {

  public class HelperForm : WiktModel.Helper {
    public override object acceptProp(ParsedTriple t, WiktCtx ctx) { return null; }

    public string Pronunciation; // lexinfo:pronunciation - rdf:langString
    public string PhoneticRep; // ontolex:phoneticRep - rdf:langString
    public string WrittenRep; // ontolex:writtenRep - rdf:langString,
    public string Note; // skos:note - rdf:langString, skos:note - xsd:string

    // Uri values
    public byte Animacy; // lexinfo:animacy - @olia,
    public byte Gender; // lexinfo:gender - @lexinfo,
    public byte Number; // lexinfo:number - @lexinfo,
    public byte Person; // lexinfo:person - @lexinfo,
    public byte Tense; // lexinfo:tense - @lexinfo,
    public byte VerbFormMood; // lexinfo:verbFormMood - @lexinfo,
    public byte HasCase; // olia:hasCase - @olia,
    public byte HasDefiniteness; // olia:hasDefiniteness - @lexinfo,
    public byte HasDegree; // olia:hasDegree - @olia,
    public byte HasGender; // olia:hasGender - @olia,
    public byte HasInflectionType; // olia:hasInflectionType - @olia,
    public byte HasMood; // olia:hasMood - @olia,
    public byte HasNumber; // olia:hasNumber - @olia,
    public byte HasPerson; // olia:hasPerson - @olia,
    public byte HasTense; // olia:hasTense - @olia,
    public byte HasValency; // olia:hasValency - @olia,
    public byte HasVoice; // olia:hasVoice - @olia,

  }

  public class HelperGloss : WiktModel.Helper {
    public string Value; // rdf_value,
    public int Rank; // dbnary_rank - xsd:int
    public string SenseNumber; //dbnary:senseNumber - xsd:string

    public override object acceptProp(ParsedTriple t, WiktCtx ctx) {
      switch (t.predicate) {
        case WiktConsts.predicates.rdf_value: return Value = t.objValue; 
        case WiktConsts.predicates.dbnary_senseNumber: return SenseNumber = t.objValue;
        case WiktConsts.predicates.dbnary_rank: if (!int.TryParse(t.objValue, out Rank)) Rank = -1; return Rank;
      }
      return null;
    }
  }

  static void consumeTriple(WiktCtx ctx, VDS.RDF.Triple tr) {

  }


}
