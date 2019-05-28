//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using static WiktSchema;
//using static WiktTtlParser;

//// design time extension
//namespace WiktObjs {

//  // Page
//  public class PageD : Page {
//    public override void acceptProp(ParsedTriple t, WiktCtx ctx) { }
//  }

//  // Entry
//  public class EntryD : Entry {
//    public override void acceptProp(ParsedTriple t, WiktCtx ctx) { }
//  }

//  // Statement
//  public class StatementD : Statement {
//    public override void acceptProp(ParsedTriple t, WiktCtx ctx) { }
//  }


//  // Sense
//  public partial class SenseD : Sense {
//    public override void acceptProp(ParsedTriple t, WiktCtx ctx) { }
//  }

//  // Translation
//  public partial class TranslationD : Translation {
//    public override void acceptProp(ParsedTriple t, WiktCtx ctx) { }
//  }

//  public class GlossD : Gloss {
//    public string Value; // rdf_value,
//    public int Rank; // dbnary_rank - xsd:int
//    public string SenseNumber; //dbnary:senseNumber - xsd:string

//    public override void acceptProp(ParsedTriple t, WiktCtx ctx) {
//      t.setValue(ctx, "Gloss", WiktConsts.predicates.rdf_value, ref Value);
//      t.setValue(ctx, "Gloss", WiktConsts.predicates.dbnary_senseNumber, ref SenseNumber);
//      if (t.predicate == WiktConsts.predicates.dbnary_rank) {
//        var v = Rank.ToString();
//        t.setValue(ctx, "Gloss", WiktConsts.predicates.dbnary_rank, ref v);
//        if (!int.TryParse(v, out Rank)) Rank = -1;
//      }
//    }
//  }
//}

//public static class WiktToSQL {

//  public class HelperForm : WiktModel.Helper {
//    public override void acceptProp(ParsedTriple t, WiktCtx ctx) { }

//    public string Pronunciation; // lexinfo:pronunciation - rdf:langString
//    public string PhoneticRep; // ontolex:phoneticRep - rdf:langString
//    public string WrittenRep; // ontolex:writtenRep - rdf:langString,
//    public string Note; // skos:note - rdf:langString, skos:note - xsd:string

//    // Uri values
//    public byte Animacy; // lexinfo:animacy - @olia,
//    public byte Gender; // lexinfo:gender - @lexinfo,
//    public byte Number; // lexinfo:number - @lexinfo,
//    public byte Person; // lexinfo:person - @lexinfo,
//    public byte Tense; // lexinfo:tense - @lexinfo,
//    public byte VerbFormMood; // lexinfo:verbFormMood - @lexinfo,
//    public byte HasCase; // olia:hasCase - @olia,
//    public byte HasDefiniteness; // olia:hasDefiniteness - @lexinfo,
//    public byte HasDegree; // olia:hasDegree - @olia,
//    public byte HasGender; // olia:hasGender - @olia,
//    public byte HasInflectionType; // olia:hasInflectionType - @olia,
//    public byte HasMood; // olia:hasMood - @olia,
//    public byte HasNumber; // olia:hasNumber - @olia,
//    public byte HasPerson; // olia:hasPerson - @olia,
//    public byte HasTense; // olia:hasTense - @olia,
//    public byte HasValency; // olia:hasValency - @olia,
//    public byte HasVoice; // olia:hasVoice - @olia,

//  }

//  public class HelperGloss : Helper {
//    public string Value; // rdf_value,
//    public int Rank; // dbnary_rank - xsd:int
//    public string SenseNumber; //dbnary:senseNumber - xsd:string

//    public override void acceptProp(ParsedTriple t, WiktCtx ctx) {
//      t.setValue(ctx, "Gloss", WiktConsts.predicates.rdf_value, ref Value);
//      t.setValue(ctx, "Gloss", WiktConsts.predicates.dbnary_senseNumber, ref SenseNumber);
//      if (t.predicate == WiktConsts.predicates.dbnary_rank) {
//        var v = Rank.ToString();
//        t.setValue(ctx, "Gloss", WiktConsts.predicates.dbnary_rank, ref v);
//        if (!int.TryParse(v, out Rank)) Rank = -1;
//      }
//    }
//  }

//  static void consumeTriple(WiktCtx ctx, VDS.RDF.Triple tr) {

//  }


//}
