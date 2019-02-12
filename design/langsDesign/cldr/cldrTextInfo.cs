using Sepia.Globalization;
using Sepia.Globalization.Numbers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.XPath;

public static class CldrUtils {

  public static IEnumerable<LangMatrixRow> fromCldrLocaleIdentifiers(IEnumerable<LocaleIdentifier> locs) {
    return locs.Select(loc => new LangMatrixRow { lang = loc.ToString(), wrapper = CldrUtils.getRowData(loc) });
  }
  public static IEnumerable<LangMatrixRow> fromNetCultureInfos(LocaleIdentifier[] cldrSpecifics) {
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

        var res = CldrUtils.getNetRowData(cu, lid.Language, lid.Region, out LocaleIdentifier locId);
        return new LangMatrixRow {
          lang = locId.ToString(),
          wrapper = res,
        };
      }).
      Where(lt => lt != null);
  }


  static string[] getNetRowData(CultureInfo lc, string Language, string Region, out LocaleIdentifier locId) {
    var values = new string[count];
    var fmt = lc.DateTimeFormat;
    fmt.MonthNames.Take(12).ToArray(values, monthsIdx);
    fmt.MonthGenitiveNames.Take(12).ToArray(values, smonthsIdx);
    fmt.DayNames.Take(7).ToArray(values, daysIdx);

    var script = UnicodeBlocks.getBlockNames(values).Select(kv => kv.Key).Single();
    locId = LocaleIdentifier.Parse(string.Format("{0}-{1}-{2}", Language, script, Region));
    return values;
  }

  static string[] getRowData(LocaleIdentifier locId) {

    var locStr = locId.ToString();
    var loc = new Locale(locId);
    var values = new string[count];

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

    gregorian = loc.FindOrDefault("//calendar[@type=\"gregorian\"]//monthContext[@type=\"stand-alone\"]/monthWidth[@type=\"wide\"]");
    if (gregorian != null) gregorian.Select("./*/text()").Cast<object>().Select(o => normalize(o.ToString())).ToArray(values, monthsIdx);

    gregorian = loc.FindOrDefault("//calendar[@type=\"gregorian\"]//monthContext[@type=\"format\"]/monthWidth[@type=\"wide\"]");
    if (gregorian != null) gregorian.Select("./*/text()").Cast<object>().Select(o => normalize(o.ToString())).ToArray(values, smonthsIdx);

    gregorian = loc.FindOrDefault("//calendar[@type=\"gregorian\"]//dayContext[@type=\"stand-alone\"]/dayWidth[@type=\"wide\"]");
    if (gregorian != null && loc.ToString() != "lrc-Arab-IQ" && loc.ToString() != "lrc-Arab-IR" && loc.ToString() != "mzn-Arab-IR")
      gregorian.Select("./*/text()").Cast<object>().Select(o => normalize(o.ToString())).ToArray(values, daysIdx);

    gregorian = loc.FindOrDefault("//calendar[@type=\"gregorian\"]//dayContext[@type=\"format\"]/dayWidth[@type=\"wide\"]");
    if (gregorian != null && loc.ToString() != "lrc-Arab-IQ" && loc.ToString() != "lrc-Arab-IR" && loc.ToString() != "mzn-Arab-IR")
      gregorian.Select("./*/text()").Cast<object>().Select(o => normalize(o.ToString())).ToArray(values, sdaysIdx);


    // **** SPELL NUMBERS
    try {
      var spell = SpellingFormatter.Create(loc, new SpellingOptions { Style = SpellingStyle.Cardinal });
      numsSource.Select(n => normalize(spell.Format(n))).ToArray(values, numsIdx);
    } catch { }
    try {
      var spell = SpellingFormatter.Create(loc, new SpellingOptions { Style = SpellingStyle.Ordinal });
      numsSource.Select(n => normalize(spell.Format(n))).ToArray(values, snumsIdx);
    } catch { }

    return values;

  }

  //***** INDEXES
  static int[] numsSource = Enumerable.Range(0, 21).ToArray();
  //=============
  static int monthsIdx = 0;
  static int smonthsIdx = monthsIdx + 12;
  static int daysIdx = smonthsIdx + 12;
  static int sdaysIdx = daysIdx + 7;
  static int numsIdx = sdaysIdx + 7;
  static int snumsIdx = numsIdx + numsSource.Length;
  //=============
  static int count = snumsIdx + numsSource.Length + 1;

}
