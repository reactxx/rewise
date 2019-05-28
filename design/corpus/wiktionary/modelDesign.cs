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
        ctx.addError("PROP not accepted", $"{GetType().Name}.{t.predicate}");
      }
      return true;
    }
  }

  public class PageD : Page {
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) {
      return t.setRefValues(ctx, this, predicates.dbnary_describes,
        target => {
          if (target is Entry) (target as Entry).pageId = id;
          else ctx.addError("wrong Page.describes target type", $"{target.GetType().Name}");
        }
      ) ||
        base.acceptProp(t, ctx);
    }
  }

  static class FormLikeD {
    public static bool acceptProp(FormLike form, ParsedTriple t, WiktCtx ctx) {
      return t.setValue(ctx, form, predicates.ontolex_writtenRep, ref form.writtenRep) ||
      t.setNymsValue(ctx, form, ref form.nyms) ||
      t.setFormInfosValue(ctx, form, ref form.infos);
    }
  }

  public class EntryD : Entry {
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) {
      return FormLikeD.acceptProp(this, t, ctx) ||
      t.setRefValue(ctx, this, predicates.ontolex_canonicalForm, ref canonicalFormId) ||
      t.setRefValues(ctx, this, predicates.ontolex_otherForm, ref otherFormIds) ||
      //t.setRefValue(ctx, this, predicates.ontolex_sense, ref sense) ||
      t.setRefValues(ctx, this, predicates.ontolex_sense, ref senseIds) ||
      t.setUriValue(ctx, this, predicates.lexinfo_partOfSpeech, ref partOfSpeech) ||
      t.setUriValues(ctx, this, predicates.lexinfo_partOfSpeechEx, ref partOfSpeechEx) ||
      base.acceptProp(t, ctx);
    }
  }

  public class StatementD : Statement {
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) {
      return t.setRefValue(ctx, this, predicates.dbnary_gloss, ref gloss) ||
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
      return t.setRefValue(ctx, this, predicates.dbnary_gloss, ref gloss) ||
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
      return FormLikeD.acceptProp(this, t, ctx) ||
      t.setValue(ctx, this, predicates.skos_note, ref note) ||
      base.acceptProp(t, ctx);
    }
  }

  public class GlossD : Gloss {
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) {
      return t.setValue(ctx, this, predicates.rdf_value, ref value) ||
      t.setValue(ctx, this, predicates.dbnary_senseNumber, ref senseNumber) ||
      t.setIntValue(ctx, this, predicates.dbnary_rank, ref rank) ||
      base.acceptProp(t, ctx);
    }
  }

}
