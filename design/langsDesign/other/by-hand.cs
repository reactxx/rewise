using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

public static class ByHand {

  public static void Parse(Dictionary<int, Meta> res, string byHandFile) {
    var ser = new XmlSerializer(typeof(Meta[]));
    Meta[] items;
    using (var fs = File.OpenRead(byHandFile))
      items = ser.Deserialize(fs) as Meta[];
    foreach (var item in items) {
      var lcid = CultureInfo.GetCultureInfo(item.id).LCID;
      Meta meta;
      if (!res.TryGetValue(lcid, out meta)) res.Add(lcid, meta = new Meta());
      meta.isEuroTalk = item.isEuroTalk;
      meta.isGoethe = item.isGoethe;
      meta.isLingea = item.isLingea;
    }
  }

  public static void Parse(Dictionary<string, LangMatrixRow> res, string byHandFile) {
    var ser = new XmlSerializer(typeof(Meta[]));
    Meta[] items;
    using (var fs = File.OpenRead(byHandFile))
      items = ser.Deserialize(fs) as Meta[];
    foreach (var item in items) {
      var row = LangsDesignLib.adjustNewfulltextDataRow(res, Langs.oldToNew(CultureInfo.GetCultureInfo(item.id).Name));
      if (item.isEuroTalk) row.row[2] = "1";
      if (item.isLingea) row.row[3] = "1";
      if (item.isGoethe) row.row[4] = "1";
    }
  }

  //  public static void firstCreateByHand()
  //  {
  //    var metas = Metas.fromFile(@"d:\rw\libs\DesignConsole\ImportAllLangs\RewiseJazyky.xml");
  //    List<Meta> arr = new List<Meta>();
  //    foreach (var m in metas.Items)
  //      arr.Add(new Meta()
  //      {
  //        Id = m.Id,
  //        IsEuroTalk = m.IsEuroTalk,
  //        IsGoethe = m.IsGoethe,
  //        IsLingea = m.IsLingea,
  //      });
  //    var ser = new XmlSerializer(typeof(Meta[]));
  //    using (var fs = File.OpenWrite(@"D:\rewise\fulltext\sqlserver\langs\by-hand.xml"))
  //      ser.Serialize(fs, arr.ToArray());
  //  }
}
