using System;
using System.Globalization;
using System.Linq;

public static class MSCultures {

  public static void CldrPatch() {
    // get .NETsupported cultures (where it has unique non 4096 LCID):
    var wrongLcids = CultureInfo.GetCultures(CultureTypes.AllCultures).
      Select(c => new { c.Name, c.LCID }).
      GroupBy(ni => ni.LCID).
      Where(g => g.Count() > 1).
      Select(g => new { g.Key, dupls = g.Select(gg => gg.Name).ToArray() }).
      ToArray();
    if (wrongLcids.Length > 3) // 4096 (hundreds of items), 4 (zh-Hans, zh-CHS), 31748 (zh-CHT, zh-Hant)
      throw new Exception();

    var res = CultureInfo.GetCultures(CultureTypes.AllCultures).Select(ci => new Langs.CldrLang {
      Id = ci.Name,
      LCID = ci.LCID== 4096 ? 0 : ci.LCID,
      Name = ci.EnglishName,
    }).OrderBy(m => m.Id).ToArray();
    Json.Serialize(LangsDesignDirs.root + @"patches\msCultures.json", res);
  }

}
