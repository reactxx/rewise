using System.Diagnostics;
using static WiktConsts;
using static WiktSchema;

// design time extension
namespace WiktModel {

  public partial class Helper {
    public virtual bool acceptProp(ParsedTriple t, WiktCtx ctx) {
      if (t.predType == PredicateType.a) {
        if (this is Page) {
          (this as Page).title = t.subjDataId.Split(':')[1];
        }
      } else {
        ctx.log(this, t.predicate, $"PROP not accepted {t.objUri}");
      }
      return true;
    }
  }

  public class PageD : Page {
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) {
      return t.setRefValues(ctx, this, predicates.dbnary_describes,
        target => {
          if (target is Entry) (target as Entry).pageId = id;
          else ctx.log(this, t.predicate, $"wrong Page.describes target type {target.GetType().Name}");
        }
      ) ||
      t.setNymsValue(ctx, this, ref nyms) ||
        base.acceptProp(t, ctx);
    }
  } 

  public class EntryD : Entry {
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) {
      return t.setValue(ctx, this, predicates.ontolex_writtenRep, ref writtenRep) ||
      t.setNymsValue(ctx, this, ref nyms) ||
      t.setFormInfosValue(ctx, this, ref infos) ||
      t.setRefValue(ctx, this, predicates.ontolex_canonicalForm, ref canonicalFormId, form => (form as Form).canonicalOf = id) ||
      t.setRefValues(ctx, this, predicates.ontolex_otherForm, ref otherFormIds, form => (form as Form).otherOf = id) ||
      t.setRefValues(ctx, this, predicates.ontolex_sense, ref senseIds) ||
      t.setUriValue(ctx, this, predicates.lexinfo_partOfSpeech, ref partOfSpeech) ||
      t.setUriValues(ctx, this, predicates.lexinfo_partOfSpeechEx, ref partOfSpeechEx) ||
      base.acceptProp(t, ctx);
    }
  }

  public class StatementD : Statement {
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) {
      return t.setRefValue(ctx, this, predicates.dbnary_gloss, ref gloss, gloss => (gloss as Gloss).statementOf = id) ||
      t.setRefValue(ctx, this, predicates.rdf_subject, ref subjectId) ||
      t.setRefValue(ctx, this, predicates.rdf_object, ref objectId) ||
      t.setUriValue(ctx, this, predicates.rdf_predicate, ref predicate) ||
      base.acceptProp(t, ctx);
    }
  }


  public class SenseD : Sense {
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) {
      return t.setNymsValue(ctx, this, ref nyms) ||
      t.setValue(ctx, this, predicates.dbnary_senseNumber, ref senseNumber) ||
      t.setValue(ctx, this, predicates.skos_definition, ref definition) ||
      t.setValue(ctx, this, predicates.skos_example, ref example) ||
      base.acceptProp(t, ctx);
    }
  }

  public class TranslationD : Translation {
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) {
      return t.setRefValue(ctx, this, predicates.dbnary_gloss, ref gloss, gloss => (gloss as Gloss).translationOf = id) ||
      t.setRefValue(ctx, this, predicates.dbnary_isTranslationOf, ref isTranslationOf) ||
      t.setValue(ctx, this, predicates.dbnary_targetLanguage, ref targetLanguage) ||
      t.setValue(ctx, this, predicates.dbnary_targetLanguageCode, ref targetLanguage) ||
      t.setValue(ctx, this, predicates.dbnary_usage, ref usage) ||
      t.setValueWithLang(ctx, this, predicates.dbnary_writtenForm, ref writtenForm, ref targetLanguage) ||
      base.acceptProp(t, ctx);
    }
  }

  public class FormD : Form {
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) {
      if (t.predicate == predicates.lexinfo_gender) {
        Debug.Assert(true);
        t.predicate = predicates.lexinfo_gender;
      }
      return t.setValue(ctx, this, predicates.ontolex_writtenRep, ref writtenRep) ||
      t.setFormInfosValue(ctx, this, ref infos) ||
      t.setValue(ctx, this, predicates.skos_note, ref note) ||
      base.acceptProp(t, ctx);
    }
  }

  public class GlossD : Gloss {
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) {
      return t.setValue(ctx, this, predicates.rdf_value, ref value) ||
      t.setValue(ctx, this, predicates.dbnary_senseNumber, ref senseNumber) ||
      t.setIntValue(ctx, this, predicates.dbnary_rank, ref rank) || // 14384x DUPL !!!! 
      base.acceptProp(t, ctx);
    }
  }

}
