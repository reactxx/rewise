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
    public List<int> senseIds;
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
      if (senseIds != null)
        senses = senseIds.Select(sid => ctx.designGetObj<Sense>(sid)).ToArray();
      //if (senseIds != null) foreach (var sid in senseIds) {
      //    var sense = ctx.designGetObj<Sense>(sid);
      //    if (sense.senseOf == null) sense.senseOf = new List<int>();
      //    sense.senseOf.Add(id);
      //  }
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
      //var gl = ctx.designGetObj<Gloss>(glossId);
      //gl.id = -1;
      //gloss = gl.gloss;
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
    public int? pageTransId;
    public int? senseTransId;
    public int? entryTransId;
    [JsonIgnore]
    public int? glossId;
    public override bool acceptProp(ParsedTriple t, WiktCtx ctx) {
      int? trId = null;
      if (t.setRefValue<Helper>(ctx, this, predicates.dbnary_isTranslationOf, ref trId)) {
        WiktIdManager.decodeLowByte((int)trId, out string langId, out string classUri);
        switch (classUri) {
          case WiktConsts.NodeTypeNames.Page: pageTransId = trId; break;
          case WiktConsts.NodeTypeNames.LexicalÈntry: entryTransId = trId; break;
          case WiktConsts.NodeTypeNames.LexicalSense: senseTransId = trId; break;
        }
        return true;
      } else
        return t.setRefValue<Gloss>(ctx, this, predicates.dbnary_gloss, ref glossId) ||
        t.setValue(ctx, this, predicates.dbnary_targetLanguage, ref trans.targetLanguage) ||
        t.setValue(ctx, this, predicates.dbnary_targetLanguageCode, ref trans.targetLanguage) ||
        t.setValue(ctx, this, predicates.dbnary_usage, ref trans.usage) ||
        t.setValueWithLang(ctx, this, predicates.dbnary_writtenForm, ref trans.writtenForm, ref trans.targetLanguage) ||
        base.acceptProp(t, ctx);
    }


    // http://kaiko.getalp.org/about-dbnary/disambiguated-translation-are-now-systematically-computed/
    // parse 
    public override void finish(WiktCtx ctx) {
      if (senseTransId != null) {
        var s = ctx.designGetObj<Sense>(senseTransId);
        (s.translations == null ? (s.translations = new List<TranslationData>()) : s.translations).Add(trans);
      } else if (entryTransId != null) {
        var en = ctx.designGetObj<Entry>(entryTransId);
        (en.translations == null ? (en.translations = new List<TranslationData>()) : en.translations).Add(trans);
      } else if (pageTransId != null) {
        var pg = ctx.designGetObj<Page>(pageTransId);
        (pg.translations == null ? (pg.translations = new List<TranslationData>()) : pg.translations).Add(trans);
      }
      //var pageOrEntry = ctx.designGetObj(translationOfId);
      //var page = pageOrEntry as Page; var entry = pageOrEntry as Entry;
      //if (page != null) {
      //  if (page.translations == null) page.translations = new List<TranslationData>();
      //  page.translations.Add(trans);
      //} else if (entry != null) {
      //  if (glossId == null) {
      //    if (entry.translations == null) entry.translations = new List<TranslationData>();
      //    entry.translations.Add(trans);
      //  } else {
      //    if (entry.translationGlosses == null) entry.translationGlosses = new List<Gloss>();
      //    var gl = entry.translationGlosses.FirstOrDefault(g => g.id == glossId);
      //    if (gl == null) {
      //      entry.translationGlosses.Add(gl = ctx.designGetObj<Gloss>(glossId));
      //      gl.gloss.translations = new List<TranslationData>();
      //    }
      //    gl.gloss.translations.Add(trans);
      //  }
      //} else {
      //  //144x PL Sense, see H:\rewise\data\logs\dump-trans-num.txt
      //}
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
