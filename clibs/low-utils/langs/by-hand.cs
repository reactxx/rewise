using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace LangsLib
{
  public static class ByHand
  {

    public static void Parse(Dictionary<int, Meta> res, string byHandFile)
    {
      var ser = new XmlSerializer(typeof(Meta[]));
      Meta[] items;
      using (var fs = File.OpenRead(byHandFile))
        items = ser.Deserialize(fs) as Meta[];
      foreach (var item in items)
      {
        var lcid = CultureInfo.GetCultureInfo(item.Id).LCID;
        Meta meta;
        if (!res.TryGetValue(lcid, out meta)) res.Add(lcid, meta = new Meta());
        meta.IsEuroTalk = item.IsEuroTalk;
        meta.IsGoethe = item.IsGoethe;
        meta.IsLingea = item.IsLingea;
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

}