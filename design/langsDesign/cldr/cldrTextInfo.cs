using Sepia.Globalization;
using Sepia.Globalization.Numbers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.XPath;

public static class CldrTextLib {

  public static IEnumerable<LangMatrix.Values>fromCldrLocaleIdentifiers(IEnumerable<LocaleIdentifier> locs) {
    return locs.Select(loc => new LangMatrix.Values { lang = loc.ToString(), values = fromCldrLocaleIdentifier(loc) });
  }

  public static LangMatrix load() {
    return LangMatrix.load(LangsDesignDirs.cldr + "cldrInfos.csv");
  }
  public static void save(LangMatrix matrix) {
    matrix.save(LangsDesignDirs.cldr + "cldrInfos.csv", true);
  }

  public static IEnumerable<LangMatrix.Values> fromNetCultureInfos(LocaleIdentifier[] cldrSpecifics) {

    // get .NETsupported cultures (where it has unique non 4096 LCID):
    var wrongLcids = CultureInfo.GetCultures(CultureTypes.AllCultures).
      Select(c => new { c.Name, c.LCID }).
      GroupBy(ni => ni.LCID).
      Where(g => g.Count() > 1).
      Select(g => new { g.Key, dupls = g.Select(gg => gg.Name).ToArray() }).
      ToArray();
    if (wrongLcids.Length > 3) // 4096 (hundreds of items), 4 (2), 31748 (2)
      throw new Exception();
    var lcids = CultureInfo.GetCultures(CultureTypes.AllCultures).
      Select(c => new { c.Name, c.LCID }).
      Where(c => wrongLcids.All(cc => cc.Key != c.LCID)).
      OrderBy(c => c.Name).
      ToArray();
    Json.Serialize(LangsDirs.root + @"netSuportedCultures.json", lcids);

    // get NON cldr culture data
    var cldrs = new HashSet<string>(cldrSpecifics.Select(c => c.Language));
    return CultureInfo.GetCultures(CultureTypes.AllCultures).
      Select(cu => {
        LocaleIdentifier lid = null;
        { try { lid = LocaleIdentifier.Parse(cu.Name); } catch { } }
        if (
          lid == null ||
          string.IsNullOrEmpty(lid.Region) ||
          char.IsDigit(lid.Region[0]) ||
          cldrs.Contains(lid.Language)
        ) return null;

        var res = fromNetCultureInfo(cu);
        var scr = UnicodeBlocks.getBlockNames(res).Select(kv => kv.Key).Single();
        return new LangMatrix.Values {
          lang = string.Format("{0}-{1}-{2}", lid.Language, scr, lid.Region),
          values = res,
        };
      }).
      Where(lt => lt != null);
  }

  static string[] fromCldrLocaleIdentifier(LocaleIdentifier locId) {

    var locStr = locId.ToString();
    var loc = new Locale(locId);
    var res = new string[count];

    Func<string, string> normalize = (string str) => {
      // wrong i
      if (locStr == "be-Cyrl-BY")
        str = str.Replace('i', 'і');
      // convert \u00FF part of CLDR string to char
      var parts = str.Split(new string[] { "\\u" }, StringSplitOptions.None);
      if (parts.Length == 1) return str;
      return parts.Aggregate((r, i) => {
        var hex = i.Substring(0, 4);
        return r + Convert.ToChar(int.Parse(hex, System.Globalization.NumberStyles.HexNumber)) + i.Substring(4);
      });
    };

    // **** CALENDAR
    XPathNavigator gregorian;

    try {
      gregorian = loc.Find("//calendar[@type=\"gregorian\"]//monthContext[@type=\"stand-alone\"]/monthWidth[@type=\"wide\"]");
      gregorian.Select("./*/text()").Cast<object>().Select(o => normalize(o.ToString())).ToArray(res, monthsIdx);
    } catch { }
    try {
      gregorian = loc.Find("//calendar[@type=\"gregorian\"]//monthContext[@type=\"format\"]/monthWidth[@type=\"wide\"]");
      gregorian.Select("./*/text()").Cast<object>().Select(o => normalize(o.ToString())).ToArray(res, smonthsIdx);
      //if (res)
    } catch { }

    try {
      gregorian = loc.Find("//calendar[@type=\"gregorian\"]//dayContext[@type=\"stand-alone\"]/dayWidth[@type=\"wide\"]");
      if (loc.ToString() != "lrc-Arab-IQ" && loc.ToString() != "lrc-Arab-IR" && loc.ToString() != "mzn-Arab-IR")
        gregorian.Select("./*/text()").Cast<object>().Select(o => normalize(o.ToString())).ToArray(res, daysIdx);
    } catch { }
    try {
      gregorian = loc.Find("//calendar[@type=\"gregorian\"]//dayContext[@type=\"format\"]/dayWidth[@type=\"wide\"]");
      if (loc.ToString() != "lrc-Arab-IQ" && loc.ToString() != "lrc-Arab-IR" && loc.ToString() != "mzn-Arab-IR")
        gregorian.Select("./*/text()").Cast<object>().Select(o => normalize(o.ToString())).ToArray(res, sdaysIdx);
    } catch { }


    // **** SPELL NUMBERS
    try {
      var spell = SpellingFormatter.Create(loc, new SpellingOptions { Style = SpellingStyle.Cardinal });
      numsSource.Select(n => normalize(spell.Format(n))).ToArray(res, numsIdx);
    } catch { }
    try {
      var spell = SpellingFormatter.Create(loc, new SpellingOptions { Style = SpellingStyle.Ordinal });
      numsSource.Select(n => normalize(spell.Format(n))).ToArray(res, snumsIdx);
    } catch { }

    // ALPHABETS
    try {
      res[alphaIdx] = sortedCharsOnly(normalize(loc.Find("//characters/exemplarCharacters[not(@type)]/text()").ToString()));
    } catch { }
    try {
      res[alphaAuxilityIdx] = sortedCharsOnly(normalize(loc.Find("//characters/exemplarCharacters[@type=\"auxiliary\"]/text()").ToString()));
    } catch { }
    try {
      if (Array.IndexOf(new string[] { "zh-Hans-CN", "zh-Hans-HK", "zh-Hans-MO", "zh-Hans-SG", "yue-Hans-CN" }, loc.ToString()) < 0)
        res[alphaIndexIdx] = normalize(loc.Find("//characters/exemplarCharacters[@type=\"index\"]/text()").ToString());
    } catch { }
    try {
      res[alphaNumsIdx] = normalize(loc.Find("//characters/exemplarCharacters[@type=\"numbers\"]/text()").ToString());
    } catch { }

    // force lang distinction
    switch (loc.ToString()) {
      case "en-Latn-GB":
        res[extraIdx] = "1"; break;
      case "zh-Hant-HK":
        res[extraIdx] = "2"; break;
      case "zh-Hans-SG":
        res[extraIdx] = "3"; break;
      case "zh-Hant-MO":
        res[extraIdx] = "4"; break;
    }

    return res;
  }
  static string sortedCharsOnly(string str) {
    return new String(str.ToCharArray().Where(ch => UnicodeBlocks.isLetter(ch)).Distinct().OrderBy(ch => ch).ToArray());
  }

  static string[] fromNetCultureInfo(CultureInfo lc) {
    var res = new string[count];
    var fmt = lc.DateTimeFormat;
    fmt.MonthNames.Take(12).ToArray(res, monthsIdx);
    fmt.MonthGenitiveNames.Take(12).ToArray(res, smonthsIdx);
    fmt.DayNames.Take(7).ToArray(res, daysIdx);
    return res;
  }

  //***** INDEXES
  public static int[] numsSource = Enumerable.Range(0, 21).ToArray();
  //=============
  public static int monthsIdx = 0;
  public static int smonthsIdx = monthsIdx + 12;
  public static int daysIdx = smonthsIdx + 12;
  public static int sdaysIdx = daysIdx + 7;
  public static int numsIdx = sdaysIdx + 7;
  public static int snumsIdx = numsIdx + numsSource.Length;
  public static int extraIdx = snumsIdx + numsSource.Length;
  public static int alphaIdx = extraIdx + 1;
  public static int alphaAuxilityIdx = alphaIdx + 1;
  public static int alphaIndexIdx = alphaAuxilityIdx + 1;
  public static int alphaNumsIdx = alphaIndexIdx + 1;
  //=============
  public static int compareCount = extraIdx + 1;
  public static int count = alphaNumsIdx + 1;
}

