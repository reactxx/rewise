using System.Collections.Generic;
using static WiktConsts;
using static WiktSchema;

// design time extension
namespace WiktModel {

  public struct NymRels {
    public int? antonym;
    public int? approximateSynonym;
    public int? holonym;
    public int? hypernym;
    public int? hyponym;
    public int? meronym;
    public int? synonym;
  }

  public struct FormInfos {
    public olia_hasCase hasCase;
    public olia_hasDegree hasDegree;
    public olia_hasInflectionType hasInflectionType;
    public olia_hasCountability hasCountability;
    public olia_hasMood hasMood;
    public olia_hasVoice hasVoice;
    public lexinfo_animacy animacy;
    public lexinfo_verbFormMood verbFormMood;
    public number number;
    public person person;
    public gender gender;
    public tense tense;
  }


  public class PageD : Page {
    public override void acceptProp(ParsedTriple t, WiktCtx ctx) { }
  }

  public class FormLikeD : FormLike {
    public override void acceptProp(ParsedTriple t, WiktCtx ctx) {
      t.setValue(ctx, this, predicates.ontolex_writtenRep, ref writtenRep);
      t.setNymsValue(ctx, this, ref nyms);
      t.setFormInfosValue(ctx, this, ref infos);
    }
  }

  public class EntryD : FormLikeD {
    public int? canonicalFormId;
    public List<int> otherFormIds;
    public List<int> senseIds;
    public lexinfo_partOfSpeech partOfSpeech;
    public List<lexinfo_partOfSpeechEx> partOfSpeechEx;
    public override void acceptProp(ParsedTriple t, WiktCtx ctx) {
      base.acceptProp(t, ctx);
      t.setRefValue(ctx, this, predicates.ontolex_canonicalForm, ref canonicalFormId);
      t.setRefValues(ctx, this, predicates.ontolex_otherForm, ref otherFormIds);
      t.setRefValues(ctx, this, predicates.ontolex_sense, ref senseIds);
      t.setUriValue(ctx, this, predicates.lexinfo_partOfSpeech, ref partOfSpeech);
      t.setUriValues(ctx, this, predicates.lexinfo_partOfSpeechEx, ref partOfSpeechEx);
    }
  }

  public class StatementD : Statement {
    public override void acceptProp(ParsedTriple t, WiktCtx ctx) { }
  }

  public class SenseD : Sense {
    public override void acceptProp(ParsedTriple t, WiktCtx ctx) { }
  }

  public class TranslationD : Translation {
    public override void acceptProp(ParsedTriple t, WiktCtx ctx) {
      t.setRefValue(ctx, this, predicates.dbnary_gloss, ref gloss);
      t.setRefValue(ctx, this, predicates.dbnary_isTranslationOf, ref isTranslationOf);
      t.setValue(ctx, this, predicates.dbnary_targetLanguage, ref targetLanguage);
      t.setValue(ctx, this, predicates.dbnary_targetLanguageCode, ref targetLanguage);
      t.setValue(ctx, this, predicates.dbnary_usage, ref usage);
      t.setValueWithLang(ctx, this, predicates.dbnary_writtenForm, ref writtenForm, ref targetLanguage);
    }
  }

  public class FormD : FormLikeD {
    public override void acceptProp(ParsedTriple t, WiktCtx ctx) {
      base.acceptProp(t, ctx);
    }
  }

  public class GlossD : Gloss {
    public override void acceptProp(ParsedTriple t, WiktCtx ctx) {
      t.setValue(ctx, this, predicates.rdf_value, ref value);
      t.setValue(ctx, this, predicates.dbnary_senseNumber, ref senseNumber);
      t.setIntValue(ctx, this, predicates.dbnary_rank, ref rank);
    }
  }

}
