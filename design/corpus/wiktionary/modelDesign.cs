using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using static WiktConsts;
using static WiktTriple;

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
    public virtual void finish(WiktCtx ctx) { }
  }

  public class PageD : Page {
    [JsonIgnore]
    public List<int> describes;

    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) =>
      t.setRefValues<Entry>(ctx, this, predicates.dbnary_describes, ref describes) ||
      t.setNymsValue(ctx, this, ref nyms) ||
      base.acceptProp(t, ctx);

    public override void finish(WiktCtx ctx) {
      if (describes != null) entries = describes.Select(id => ctx.designGetObj<Entry>(id)).ToArray();
    }
  }

  public class EntryD : Entry {
    [JsonIgnore]
    public int? canonicalFormId;
    [JsonIgnore]
    public List<int> otherFormIdx;
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) =>
      t.setValue(ctx, this, predicates.ontolex_writtenRep, ref writtenRep) ||
      t.setNymsValue(ctx, this, ref nyms) ||
      t.setFormInfosValue(ctx, this, ref infos) ||
      t.setRefValue<Form>(ctx, this, predicates.ontolex_canonicalForm, ref canonicalFormId) ||
      t.setRefValues<Form>(ctx, this, predicates.ontolex_otherForm, ref otherFormIdx) ||
      t.setRefValues<Sense>(ctx, this, predicates.ontolex_sense, ref senseIds) ||
      t.setUriValue(ctx, this, predicates.lexinfo_partOfSpeech, ref partOfSpeech) ||
      t.setUriValues(ctx, this, predicates.lexinfo_partOfSpeechEx, ref partOfSpeechEx) ||
      base.acceptProp(t, ctx);

    public override void finish(WiktCtx ctx) {
      if (canonicalFormId != null) canonicalForm = ctx.designGetObj<FormD>(canonicalFormId).form;
      otherForm = otherFormIdx == null ? null : otherFormIdx.Select(id => ctx.designGetObj<FormD>(id).form).ToArray();
      if (senseIds != null) foreach (var sid in senseIds) {
          var sense = ctx.designGetObj<Sense>(sid);
          if (sense.senseOf == null) sense.senseOf = new List<int>();
          sense.senseOf.Add(id);
        }
    }
  }

  public class StatementD : Statement {
    public int? glossId;
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) =>
      t.setRefValue<Helper>(ctx, this, predicates.dbnary_gloss, ref glossId) ||
      t.setRefValue<Entry>(ctx, this, predicates.rdf_subject, ref subjectId) ||
      t.setRefValue<Page>(ctx, this, predicates.rdf_object, ref objectId) ||
      t.setUriValue(ctx, this, predicates.rdf_predicate, ref predicate) ||
      base.acceptProp(t, ctx);

    public override void finish(WiktCtx ctx) {
      if (glossId == null) return;
      var gl = ctx.designGetObj<Gloss>(glossId);
      gl.id = -1;
      gloss = gl.gloss;
    }
  }

  public class SenseD : Sense {
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) =>
      t.setNymsValue(ctx, this, ref nyms) ||
      t.setValue(ctx, this, predicates.dbnary_senseNumber, ref senseNumber) ||
      t.setValue(ctx, this, predicates.skos_definition, ref definition) ||
      t.setValue(ctx, this, predicates.skos_example, ref example) ||
      base.acceptProp(t, ctx);
  }

  public class TranslationD : Translation {
    [JsonIgnore]
    public int? translationOfId;
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) =>
      t.setRefValue<Gloss>(ctx, this, predicates.dbnary_gloss, ref trans.glossId) ||
      t.setRefValue<Helper>(ctx, this, predicates.dbnary_isTranslationOf, ref translationOfId) ||
      t.setValue(ctx, this, predicates.dbnary_targetLanguage, ref trans.targetLanguage) ||
      t.setValue(ctx, this, predicates.dbnary_targetLanguageCode, ref trans.targetLanguage) ||
      t.setValue(ctx, this, predicates.dbnary_usage, ref trans.usage) ||
      t.setValueWithLang(ctx, this, predicates.dbnary_writtenForm, ref trans.writtenForm, ref trans.targetLanguage) ||
      base.acceptProp(t, ctx);

    public override void finish(WiktCtx ctx) {
      //if (glossId!=null) trans.gloss = ctx.designGetObj<Gloss>(glossId).gloss;
      var trAble = ctx.designGetObj(translationOfId) as ITranslation;
      if (trAble.translations == null) trAble.translations = new List<TranslationData>();
      trAble.translations.Add(trans);
    }
  }

  public class FormD : Form {
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) =>
      t.setValue(ctx, this, predicates.ontolex_writtenRep, ref form.writtenRep) ||
      t.setFormInfosValue(ctx, this, ref form.infos) ||
      t.setValue(ctx, this, predicates.skos_note, ref form.note) ||
      base.acceptProp(t, ctx);
  }

  public class GlossD : Gloss {
    public bool inTranslation;
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) =>
      t.setValue(ctx, this, predicates.rdf_value, ref gloss.value) ||
      t.setValue(ctx, this, predicates.dbnary_senseNumber, ref gloss.senseNumber) ||
      t.setIntValue(ctx, this, predicates.dbnary_rank, ref gloss.rank) || // 14384x DUPL !!!! 
      base.acceptProp(t, ctx);
  }


}
