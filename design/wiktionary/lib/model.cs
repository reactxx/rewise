// inheritance: https://weblogs.asp.net/manavi/inheritance-mapping-strategies-with-entity-framework-code-first-ctp5-part-1-table-per-hierarchy-tph

//************* TODO
// GLOSS: t.setIntValue(ctx, this, predicates.ref rank) || // 14384x DUPL !!!! 
// Statement: what are type of subject and object
// for pl: TranslationD: t.setRefValue(ctx, this, predicates.dbnary_isTranslationOf, ref isTranslationOf) : 479919x DUPL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static WiktConsts;

namespace WiktModel {

  public class Page : Helper {
    internal string title1;
    internal List<NymRel> nyms1;
    internal Entry[] entries1;
    internal List<TranslationData> translations1;

    // ? ontolex_canonicalForm, ontolex_otherForm, ontolex_sense, lexinfo_partOfSpeech, olia_hasCountability, lime_language
    public string title { get => title1; set => title1 = value; }
    public List<NymRel> nyms { get => nyms1; set => nyms1 = value; }
    //public List<NymRel> nymsOf;
    public Entry[] entries { get => entries1; set => entries1 = value; }
    public List<TranslationData> translations { get => translations1; set => translations1 = value; }

    public override IEnumerable<TranslationData> getTrans() => translations1 ?? base.getTrans();
  }

  // Entry
  public class Entry : Helper { //, ITranslation {
    public Page page;
    internal FormData canonicalForm1 = new FormData();
    internal FormData[] otherForm1;
    internal lexinfo_partOfSpeech partOfSpeech1;
    internal List<lexinfo_partOfSpeechEx> partOfSpeechEx1;
    internal string writtenRep1;
    internal List<NymRel> nyms1;
    internal FormInfos infos1 = new FormInfos();
    internal List<TranslationData> translations1;
    internal Sense[] senses1;

    public void getPartOfSpeech(StringBuilder sb) {
      sb.Append('[');
      if (partOfSpeech1 != lexinfo_partOfSpeech.no) sb.Append(partOfSpeech1.ToString());
      if (partOfSpeechEx1 != null && partOfSpeechEx1.Count > 0) {
        var res = partOfSpeechEx1.Where(p => p != lexinfo_partOfSpeechEx.no).DefaultIfEmpty().Select(p => p.ToString()).Aggregate((r, i) => r + "," + i);
        if (res != "") sb.Append($" ({res})");
      }
      sb.Append("] ");
    }
    public string toString() {
      var sb = new StringBuilder();
      sb.AppendLine("==========");
      if (!string.IsNullOrEmpty(writtenRep1)) {
        sb.Append(writtenRep1); sb.Append(' ');
        getPartOfSpeech(sb);
        if (infos1 != null) infos1.toString(sb);
        //if (canonicalForm1 != null) sb.Append($"  ##-2 ");
      }
      if (canonicalForm1 != null) {
        canonicalForm1.toString(sb); sb.Append(' ');
        getPartOfSpeech(sb);
        if (otherForm1 != null) foreach (var f in otherForm1) {
            sb.AppendLine(); sb.Append("  ");
            f.toString(sb);
          }
      }
      sb.AppendLine();
      return sb.Replace("lexinfo_", "").Replace("olia_", "").Replace("dbnary_", "").ToString();
    }

    public override IEnumerable<TranslationData> getTrans() => translations1 ?? base.getTrans();
    public IEnumerable<FormData> getFormData() {
      if (canonicalForm != null) yield return canonicalForm;
      if (otherForm != null) foreach (var f in otherForm) yield return f;
    }
    // ? vartrans_lexicalRel, dbnary_describes, lime_language
    public FormData canonicalForm { get => canonicalForm1; set => canonicalForm1 = value; }
    public FormData[] otherForm { get => otherForm1; set => otherForm1 = value; }
    public lexinfo_partOfSpeech partOfSpeech { get => partOfSpeech1; set => partOfSpeech1 = value; }
    public List<lexinfo_partOfSpeechEx> partOfSpeechEx { get => partOfSpeechEx1; set => partOfSpeechEx1 = value; }
    public string writtenRep { get => writtenRep1; set => writtenRep1 = value; }
    public List<NymRel> nyms { get => nyms1; set => nyms1 = value; }
    public FormInfos infos { get => infos1; set => infos1 = value; }
    public List<TranslationData> translations { get => translations1; set => translations1 = value; }
    public Sense[] senses { get => senses1; set => senses1 = value; }
  }

  public class Gloss : Helper {
    internal GlossData gloss1;

    public GlossData gloss { get => gloss1; set => gloss1 = value; }
  }

  public class Form : Helper {
    internal FormData form1 = new FormData();

    public FormData form { get => form1; set => form1 = value; }
  }

  // Translation
  public class Translation : Helper {
    internal TranslationData trans1 = new TranslationData();

    public TranslationData trans { get => trans1; set => trans1 = value; }
  }

  // Sense
  public class Sense : Helper {
    public Entry entry;
    internal List<NymRel> nyms1;
    internal string senseNumber1;
    internal string definition1;
    internal string example1;
    internal List<TranslationData> translations1;

    public List<NymRel> nyms { get => nyms1; set => nyms1 = value; }
    public string senseNumber { get => senseNumber1; set => senseNumber1 = value; }
    public string definition { get => definition1; set => definition1 = value; }
    public string example { get => example1; set => example1 = value; }
    public List<TranslationData> translations { get => translations1; set => translations1 = value; }

    public override IEnumerable<TranslationData> getTrans() => translations1 ?? base.getTrans();
  }

  public class Statement : Helper {
    internal GlossData gloss1;
    internal int? subjectId1;
    internal int? objectId1;
    internal rdf_predicate predicate1;

    // ? dbnary_usage, 
    public GlossData gloss { get => gloss1; set => gloss1 = value; }
    public int? subjectId { get => subjectId1; set => subjectId1 = value; }
    public int? objectId { get => objectId1; set => objectId1 = value; }
    public rdf_predicate predicate { get => predicate1; set => predicate1 = value; }
  }

  //***************** HELPERs

  public struct GlossData {
    internal string value1;
    internal int? rank1;
    internal string senseNumber1;

    public string value { get => value1; set => value1 = value; } // rdf_value,
    public int? rank { get => rank1; set => rank1 = value; } // dbnary_rank - xsd:int
    public string senseNumber { get => senseNumber1; set => senseNumber1 = value; } //dbnary:senseNumber - xsd:string
  }

  public class FormData {
    internal string note1;
    internal string writtenRep1;
    internal FormInfos infos1 = new FormInfos();
    public bool isOther = true;
    public Entry entry;
    public string lang;
    public void toString(StringBuilder sb) {
      if (!string.IsNullOrEmpty(writtenRep1)) sb.Append(writtenRep1);
      sb.Append(' ');
      if (!string.IsNullOrEmpty(note1)) sb.Append($"({note1}) ");
      if (infos1 != null) infos1.toString(sb);
    }

    public string note { get => note1; set => note1 = value; }
    public string writtenRep { get => writtenRep1; set => writtenRep1 = value; }
    public FormInfos infos { get => infos1; set => infos1 = value; }
  }

  //public struct SenseData {//: ITranslation {
  //}

  public class TranslationData {
    internal string writtenForm1;
    internal string usage1;
    internal string targetLanguage1;
    public Helper owner;
    public string lang;

    public string writtenForm { get => writtenForm1; set => writtenForm1 = value; }
    public string usage { get => usage1; set => usage1 = value; }
    public string targetLanguage { get => targetLanguage1; set => targetLanguage1 = value; }
  }

  public struct NymRel {
    internal rdf_predicate type1;
    internal int relId1;

    public rdf_predicate type { get => type1; set => type1 = value; }
    public int relId { get => relId1; set => relId1 = value; }
  }

  public class FormInfos {
    public olia_hasCase hasCase { get => hasCase1; set => hasCase1 = value; }
    public olia_hasDegree hasDegree { get => hasDegree1; set => hasDegree1 = value; }
    public olia_hasInflectionType hasInflectionType { get => hasInflectionType1; set => hasInflectionType1 = value; }
    public olia_hasCountability hasCountability { get => hasCountability1; set => hasCountability1 = value; }
    public olia_hasMood hasMood { get => hasMood1; set => hasMood1 = value; }
    public olia_hasVoice hasVoice { get => hasVoice1; set => hasVoice1 = value; }
    public lexinfo_animacy animacy { get => animacy1; set => animacy1 = value; }
    public lexinfo_verbFormMood verbFormMood { get => verbFormMood1; set => verbFormMood1 = value; }
    public person person { get => person1; set => person1 = value; }
    public gender gender { get => gender1; set => gender1 = value; }
    public tense tense { get => tense1; set => tense1 = value; }
    public number number { get => number1; set => number1 = value; }
    internal olia_hasDegree hasDegree1;
    internal olia_hasInflectionType hasInflectionType1;
    internal olia_hasCountability hasCountability1;
    internal olia_hasMood hasMood1;
    internal olia_hasVoice hasVoice1;
    internal olia_hasCase hasCase1;
    internal lexinfo_animacy animacy1;
    internal lexinfo_verbFormMood verbFormMood1;
    internal number number1;
    internal person person1;
    internal gender gender1;
    internal tense tense1;
    public void toString(StringBuilder sb) {
      var all = new List<Tuple<string, string>> {
        new Tuple<string, string>("number", number1.ToString()),
        new Tuple<string, string>("case", hasCase1.ToString()),
        new Tuple<string, string>("degree", hasDegree1.ToString()),
        new Tuple<string, string>("inflectionType", hasInflectionType1.ToString()),
        new Tuple<string, string>("countability", hasCountability1.ToString()),
        new Tuple<string, string>("mood", hasMood1.ToString()),
        new Tuple<string, string>("voice", hasVoice1.ToString()),
        new Tuple<string, string>("animacy", animacy1.ToString()),
        new Tuple<string, string>("verbFormMood", verbFormMood1.ToString()),
        new Tuple<string, string>("person", person1.ToString()),
        new Tuple<string, string>("gender", gender1.ToString()),
        new Tuple<string, string>("tense", tense1.ToString()),
      };
      var res = all.Where(a => a.Item2 != "no").Select(t => $"{t.Item1}={t.Item2}").DefaultIfEmpty().Aggregate((r, i) => r + "," + i);
      if (res != "") sb.Append($"{{{res}}}");
    }
  }
  //public interface ITranslation {
  //  List<TranslationData> translations { get; set; }
  //}

  public partial class Helper {
    internal int id1;
    public string lang;
    public virtual IEnumerable<TranslationData> getTrans() => Enumerable.Empty<TranslationData>();
    public int id { get => id1; set => id1 = value; }
  }

}
