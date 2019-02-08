using Newtonsoft.Json;
using Sepia.Globalization;
using Sepia.Globalization.Numbers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.XPath;

public static class CldrTextInfoLib {

  public static CldrTextInfo[] getNetCultureInfos(LocaleIdentifier[] cldrSpecifics) {

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
    Json.Serialize(LangsLib.Root.root + @"netSuportedCultures.json", lcids);

    // get NON cldr culture data
    var cldrs = new HashSet<string>(cldrSpecifics.Select(c => c.Language));
    var texts = CultureInfo.GetCultures(CultureTypes.AllCultures).
      Select(cu => {
        LocaleIdentifier lid = null;
        { try { lid = LocaleIdentifier.Parse(cu.Name); } catch { } }
        if (lid == null || string.IsNullOrEmpty(lid.Region) || char.IsDigit(lid.Region[0])) return null;
        if (cldrs.Contains(lid.Language)) return null;
        var res = new CldrTextInfo(cu);
        var scr = LangsLib.UnicodeBlockNames.getBlockNames(res.texts, true).Select(kv => kv.Key).Single();
        res.id = LocaleIdentifier.Parse(string.Format("{0}-{1}-{2}", lid.Language, scr, lid.Region));
        return res;
      }).
      Where(lt => lt != null).
      ToArray();
    return texts;
  }

  public static CldrTextInfo[] load() {
    return Json.Deserialize<CldrTextInfo[]>(Root.unicode + "cldrInfos.json");
  }
  public static void save(CldrTextInfo[] infos) {
    Json.Serialize(Root.unicode + "cldrInfos.json", infos);
  }

  public static Dictionary<LocaleIdentifier, string> toDictionary(CldrTextInfo[] infos) {
    return infos.
      SelectMany(inf => inf.ids.Select(id => new { inf.texts, id })).
      ToDictionary(inf => inf.id, inf => inf.texts, CldrLib.LocaleIdentifierEqualityComparer.Instance);
  }

  public static Dictionary<string, Dictionary<string, string>> checkTexts(CldrTextInfo[] infos) {
    var res = new Dictionary<string, Dictionary<string, string>>();
    foreach (var info in infos) {
      var wrongs = LangsLib.UnicodeBlockNames.checkBlockNames(info.texts, info.id.Script, true);
      if (wrongs != null) {
        res[info.id.ToString()] = wrongs;
      }
    }
    return res;
  }

}

public class CldrTextInfos : LangMatrix {

  public static CldrTextInfos fromLocales(LocaleIdentifier[] locs) {
    return new CldrTextInfos(count, locs.Select(l => l.ToString()));
  }
  CldrTextInfos(int valuesCount, IEnumerable<string> _langs = null) : base(valuesCount, _langs) { }

  //***** INDEXES
  public static int[] numsSource = Enumerable.Range(0, 21).ToArray();
  //=============
  public static int monthsIdx = 0;
  public static int smonthsIdx = monthsIdx + 12;
  public static int daysIdx = smonthsIdx + 12;
  public static int sdaysIdx = daysIdx + 7;
  public static int numsIdx = sdaysIdx + 7;
  public static int snumsIdx = numsIdx + numsSource.Length;
  public static int alphaIdx = snumsIdx + numsSource.Length;
  public static int extraIdx = alphaIdx + 1;
  //=============
  public static int compareCount = extraIdx + 1;
  public static int alphaAuxilityIdx = extraIdx + 1;
  public static int alphaIndexIdx = alphaAuxilityIdx + 1;
  public static int alphaNumsIdx = alphaIndexIdx + 1;
  //=============
  public static int count = alphaNumsIdx + 1;
}

public class CldrTextInfo {

  public string[] idsStr { get { return ids == null ? null : ids.Select(i => i.ToString()).ToArray(); } set { ids = value.Select(v => LocaleIdentifier.Parse(v)).ToArray(); } }
  public string[] months;
  public string[] months2;
  public string[] days;
  public string[] days2;
  public string[] nums;
  public string[] nums2;
  public string alpha;
  public string alphaAuxility;
  public string alphaIndex;
  public string alphaNums;
  public string extra;

  [JsonIgnore]
  public LocaleIdentifier id;
  [JsonIgnore]
  public LocaleIdentifier[] ids;
  [JsonIgnore]
  public string texts {
    get { return _texts == null ? _texts = getTexts().JoinStrings(";").ToLower() + ";" + extra : _texts; }
  }

  public CldrTextInfo() { }

  public CldrTextInfo(CultureInfo lc) {
    var fmt = lc.DateTimeFormat;
    months = fmt.MonthNames.Take(12).ToArray();
    months2 = fmt.MonthGenitiveNames.Take(12).ToArray();
    if (months == months2) months2 = null;
    days = fmt.DayNames.Take(7).ToArray();
  }

  public CldrTextInfo(LocaleIdentifier locId) {

    id = locId;
    var locStr = locId.ToString();
    var loc = new Locale(locId);

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
      months = gregorian.Select("./*/text()").Cast<object>().Select(o => normalize(o.ToString())).ToArray();
    } catch { }
    try {
      gregorian = loc.Find("//calendar[@type=\"gregorian\"]//monthContext[@type=\"format\"]/monthWidth[@type=\"wide\"]");
      months2 = gregorian.Select("./*/text()").Cast<object>().Select(o => normalize(o.ToString())).ToArray();
      if (months == months2) months = null;
    } catch { }

    try {
      gregorian = loc.Find("//calendar[@type=\"gregorian\"]//dayContext[@type=\"stand-alone\"]/dayWidth[@type=\"wide\"]");
      if (loc.ToString() != "lrc-Arab-IQ" && loc.ToString() != "lrc-Arab-IR" && loc.ToString() != "mzn-Arab-IR")
        days = gregorian.Select("./*/text()").Cast<object>().Select(o => normalize(o.ToString())).ToArray();
    } catch { }
    try {
      gregorian = loc.Find("//calendar[@type=\"gregorian\"]//dayContext[@type=\"format\"]/dayWidth[@type=\"wide\"]");
      if (loc.ToString() != "lrc-Arab-IQ" && loc.ToString() != "lrc-Arab-IR" && loc.ToString() != "mzn-Arab-IR")
        days2 = gregorian.Select("./*/text()").Cast<object>().Select(o => normalize(o.ToString())).ToArray();
      if (days == days2) days2 = null;
    } catch { }


    // **** SPELL NUMBERS
    try {
      var spell = SpellingFormatter.Create(loc, new SpellingOptions { Style = SpellingStyle.Cardinal });
      nums = numsSource.Select(n => normalize(spell.Format(n))).ToArray();
    } catch { }
    try {
      var spell = SpellingFormatter.Create(loc, new SpellingOptions { Style = SpellingStyle.Ordinal });
      nums2 = numsSource.Select(n => normalize(spell.Format(n))).ToArray();
      if (nums == nums2) nums2 = null;
    } catch { }

    // ALPHABETS
    try {
      alpha = sortedCharsOnly(normalize(loc.Find("//characters/exemplarCharacters[not(@type)]/text()").ToString()));
    } catch { }
    try {
      alphaAuxility = sortedCharsOnly(normalize(loc.Find("//characters/exemplarCharacters[@type=\"auxiliary\"]/text()").ToString()));
    } catch { }
    try {
      if (Array.IndexOf(new string[] { "zh-Hans-CN", "zh-Hans-HK", "zh-Hans-MO", "zh-Hans-SG", "yue-Hans-CN" }, loc.ToString()) < 0)
        alphaIndex = normalize(loc.Find("//characters/exemplarCharacters[@type=\"index\"]/text()").ToString());
    } catch { }
    try {
      alphaNums = normalize(loc.Find("//characters/exemplarCharacters[@type=\"numbers\"]/text()").ToString());
    } catch { }

    // force lang distinction
    switch (loc.ToString()) {
      case "en-Latn-GB":
        extra = "1"; break;
      case "zh-Hant-HK":
        extra = "2"; break;
      case "zh-Hans-SG":
        extra = "3"; break;
      case "zh-Hant-MO":
        extra = "4"; break;
    }
  }

  static int[] numsSource = Enumerable.Range(0, 21).ToArray();

  public IEnumerable<string> getTexts() {
    return Linq.Items(months.NullsWhenEmpty(12), months2.NullsWhenEmpty(12), days.NullsWhenEmpty(7), days2.NullsWhenEmpty(7), nums.NullsWhenEmpty(numsSource.Length), nums2.NullsWhenEmpty(numsSource.Length)).
      SelectMany(s => s.Select(ss => ss == null ? null : LangsLib.UnicodeBlockNames.filterChars(ss).ToLower()));
    //Concat(Linq.Items(alpha/*, alphaAuxility*/).Select(ss => ss == null ? null : LangsLib.UnicodeBlockNames.filterChars(ss)));
  }

  public IEnumerable<string> diff(CldrTextInfo _second) {
    var first = getTexts().ToArray();
    var second = _second.getTexts().ToArray();
    for (var i = 0; i < first.Length; i++) {
      if (first[i] == second[i]) continue;
      yield return string.Format("[{0}]{1}!={2}", i, first[i], second[i]);
    }
  }

  string _texts;
  string doCatch(Func<string> fnc) {
    try { return fnc(); } catch { return null; }
  }
  string sortedCharsOnly(string str) {
    return new String(str.ToCharArray().Where(ch => LangsLib.UnicodeBlockNames.isLetter(ch)).Distinct().OrderBy(ch => ch).ToArray());
  }

}

