using Sepia.Globalization;
using Sepia.Globalization.Numbers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

public static class CldrTextInfoLib {

  public static CldrTextInfo[] getNetCultureInfos(LocaleIdentifier[] cldrSpecifics) {
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
    var serInfo = new XmlSerializer(typeof(CldrTextInfo[]));
    using (var fs = File.OpenRead(Root.unicode + "cldrInfos.xml"))
      return serInfo.Deserialize(fs) as CldrTextInfo[];
  }
  public static void save(CldrTextInfo[] infos) {
    var serInfo = new XmlSerializer(typeof(CldrTextInfo[]));
    var fn = Root.unicode + "cldrInfos.xml";
    if (File.Exists(fn)) File.Delete(fn);
    using (var fs = File.OpenWrite(Root.unicode + "cldrInfos.xml"))
      serInfo.Serialize(fs, infos);
  }

  public static Dictionary<LocaleIdentifier, string> toDictionary(CldrTextInfo[] infos) {
    var serInfo = new XmlSerializer(typeof(CldrTextInfo[]));
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

public class CldrTextInfo {

  public string[] idsStr { get { return ids.Select(i => i.ToString()).ToArray(); } set { ids = value.Select(v => LocaleIdentifier.Parse(v)).ToArray(); } }
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

  [XmlIgnore]
  public LocaleIdentifier id;
  [XmlIgnore]
  public LocaleIdentifier[] ids;
  [XmlIgnore]
  public string texts {
    get {
      if (_texts == null) {
        _texts = new string[][] { months, months2, days, days2, nums, nums2 }.
          Select(arr => arr == null || arr.Length == 0 ? "" : arr.Aggregate((rr, i) => rr + ";" + LangsLib.UnicodeBlockNames.filterChars(i))).
          Concat(new string[] { alpha, alphaAuxility, alphaIndex }.Select(s => LangsLib.UnicodeBlockNames.filterChars(s)).OrderBy(ch => ch)).
          Aggregate((r, i) => r + "\n" + i) + "\n" + extra;
      }
      return _texts;
    }
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
    var numsSource = Enumerable.Range(0, 21); //.Concat(new int[] { 100, 1000 });
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
      alpha = normalize(loc.Find("//characters/exemplarCharacters[not(@type)]/text()").ToString());
    } catch { }
    try {
      alphaAuxility = normalize(loc.Find("//characters/exemplarCharacters[@type=\"auxiliary\"]/text()").ToString());
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
        extra = "en-Latn-GB"; break;
    }
  }

  string _texts;
  string doCatch(Func<string> fnc) {
    try { return fnc(); } catch { return null; }
  }

}
