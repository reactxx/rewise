using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using static WiktSchema;

// design time extension
namespace WiktModel {

  // Page
  public partial class Page : WiktToSQL.Helper, WiktToSQL.IHelper {
    [JsonIgnore]
    public List<Page_Nym> Page_Nyms;

    public override IEnumerable<object> getChilds() => Page_Nyms != null ? Page_Nyms : Enumerable.Empty<object>();
    public override void addProp(byte type, dynamic value) { }
  }

  // Entry
  public partial class Entry : WiktToSQL.Helper, WiktToSQL.IHelper {
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
    public override void addProp(byte type, dynamic value) { }
  }

  // Sense
  public partial class Sense : WiktToSQL.Helper, WiktToSQL.IHelper {
    [JsonIgnore]
    public List<Sense_Nym> Sense_Nyms;
    [JsonIgnore]
    public string blankDefinition;
    [JsonIgnore]
    public string blankExample;

    public override IEnumerable<object> getChilds() => Sense_Nyms != null ? Sense_Nyms : Enumerable.Empty<object>();
    public override void addProp(byte type, dynamic value) { }
  }

  // Translation
  public partial class Translation : WiktToSQL.Helper, WiktToSQL.IHelper {
    [JsonIgnore]
    public WiktToSQL.HelperGloss Gloss;

    public override void addProp(byte type, dynamic value) { }
  }

}

public static class WiktToSQL {

  public interface IHelper {
    int Id { get; set; }
  }

  public class HelperForm : Helper, IHelper {
    public int Id { get; set; }

    public string Pronunciation;
    public string PhoneticRep;
    public string WrittenRep;
    public string Note;

    public override void addProp(byte type, dynamic value) { }
  }

  public class HelperGloss : Helper, IHelper {
    public int Id { get; set; }

    public string Rank;
    public int SenseNumber;

    public override void addProp(byte type, dynamic value) { }
  }

  // ********** Main tables

  public abstract class Helper {
    public virtual IEnumerable<object> getChilds() { yield break; }
    public virtual void addProp(byte type, dynamic value) { }
  }


  public class Context {
    public int[] counters = Enumerable.Range(0, NodeTypesLen).ToArray();
    public Dictionary<string, IHelper>[] dirs = Enumerable.Range(0, NodeTypesLen).Select(i => new Dictionary<string, IHelper>()).ToArray();
    // "_.???" blank nodes
    public Dictionary<string, string> blanks= new Dictionary<string, string>();
    // not yet type known
    public Dictionary<string, Dictionary<string, string>> unknownType;
  }

  static void consumeTriple(Context ctx, VDS.RDF.Triple tr) {
  }

  static IHelper createNode(string url, Context ctx) {

    int getNodeType(string u) => NotNymClasses.TryGetValue(u, out byte res) ? res : (Classes.TryGetValue(u, out byte res2) ? -res2 : int.MaxValue);

    IHelper createLow(int tp) {
      switch (tp) {
        case NodeTypes.Gloss: return new HelperGloss();
        case NodeTypes.Form: return new HelperForm();
        case NodeTypes.LexicalSense: return new WiktModel.Sense();
        case NodeTypes.Page: return new WiktModel.Page();
        case NodeTypes.Translation: return new WiktModel.Translation();
        default: return new WiktModel.Entry { NymType = (byte)-tp };
      }
    }

    var type = getNodeType(url);
    if (type > byte.MaxValue) return null;
    var idx = type - 100;
    return ctx.dirs[idx].AddEx(url, v => v, k => {
      var res = createLow(type);
      res.Id = ++ctx.counters[idx];
      return res;
    });
  }


}
