using Sepia.Globalization;
using Sepia.Globalization.Numbers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.XPath;

public class CldrLangMatrix : LangMatrixLow<CldrWrapper> {
  public CldrLangMatrix() : base() { }
  public CldrLangMatrix(IEnumerable<CldrLangMatrixValue> values) : base(values) { }
  public CldrLangMatrix(string path) : base(path) { }
  public CldrLangMatrix(StreamReader rdr) : base(rdr) { }
  public CldrLangMatrix(LocaleIdentifier[] allSpecifics, CldrLangMatrixValue[] netSpecifics) : this(getAllMatrixValues(allSpecifics, netSpecifics)) {
    // CHECK
    var wrongTexts = checkTexts();
    if (wrongTexts.Count > 1)
      throw new Exception();
  }

  public static string[] loadLangs() {
    using (var rdr = new StreamReader(LangsDesignDirs.cldr + "cldrInfos.csv"))
      return new CldrLangMatrix().readLangs(rdr);
  }

  public static CldrLangMatrix load() {
    return new CldrLangMatrix(LangsDesignDirs.cldr + "cldrInfos.csv");
  }
  public void save() {
    save(LangsDesignDirs.cldr + "cldrInfos.csv", true);
  }
  
  // ******* checkTexts
  Dictionary<string, Dictionary<string, string>> checkTexts() {
    var res = new Dictionary<string, Dictionary<string, string>>();
    data.ForEach(wrp => {
      var wrongs = UnicodeBlocks.checkBlockNames(wrp.values.Skip(CldrWrapper.skip), wrp.script);
      if (wrongs == null) return;
      res[wrp.getLocId.ToString()] = wrongs;
    });
    return res;
  }

  static CldrLangMatrixValue[] getAllMatrixValues(LocaleIdentifier[] allSpecifics, CldrLangMatrixValue[] netSpecifics) {
    var isNet = new HashSet<string>(netSpecifics.Select(n => n.lang));
    var cldrInfos = CldrTextLib.fromCldrLocaleIdentifiers(allSpecifics.Where(c => !isNet.Contains(c.ToString()))).ToArray();
    var all = cldrInfos.Concat(netSpecifics).ToArray();

    //set moreScripts flag
    var moreScripts = all.
      Select(l => LocaleIdentifier.Parse(l.wrapper.lang + "-" + l.wrapper.script)).
      Distinct(LangMatrixComparer.Comparer).
      GroupBy(l => l.Language).
      Where(g => g.Count() > 1).
      Select(g => g.Key).
      ToHashSet();

    all.Where(lv => moreScripts.Contains(lv.wrapper.lang)).ForEach(lv => lv.wrapper.moreScripts = true);

    return all;
  }

  override protected CldrWrapper wrapp(string[] vals) { return new CldrWrapper { values = vals == null ? new string[CldrWrapper.count] : vals }; }
}

public class CldrLangMatrixValue : LangMatrixValues<CldrWrapper> { }

public class CldrWrapper : LangMatrixWrapper {

  public CldrWrapper() { }

  public CldrWrapper(CultureInfo lc, string Language, string Region, out LocaleIdentifier locId) {
    values = new string[count];
    var fmt = lc.DateTimeFormat;
    fmt.MonthNames.Take(12).ToArray(values, monthsIdx);
    fmt.MonthGenitiveNames.Take(12).ToArray(values, smonthsIdx);
    fmt.DayNames.Take(7).ToArray(values, daysIdx);

    var script = UnicodeBlocks.getBlockNames(values).Select(kv => kv.Key).Single();
    locId = LocaleIdentifier.Parse(string.Format("{0}-{1}-{2}", Language, script, Region));
    fillFirstColumns(locId);
  }

  public CldrWrapper(LocaleIdentifier locId) {

    var locStr = locId.ToString();
    var loc = new Locale(locId);
    values = new string[count];

    fillFirstColumns(locId);

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

  }

  public LocaleIdentifier getLocId { get { return LocaleIdentifier.Parse(string.Format("{0}-{1}-{2}", lang, script, defaultRegion)); } }

  public string lang { get { return values[langIdx]; } set { values[langIdx] = value; } }
  public string script { get { return values[scriptIdx]; } set { values[scriptIdx] = value; } }
  public bool moreScripts { get { return !string.IsNullOrEmpty(values[moreScriptsIdx]); } set { values[moreScriptsIdx] = value ? "1" : null; } }
  public string defaultRegion { get { return values[defaultRegionIdx]; } set { values[defaultRegionIdx] = value; } }
  public string extra { get { return values[extraIdx]; } set { values[extraIdx] = value; } }

  void fillFirstColumns(LocaleIdentifier locId) {
    lang = locId.Language;
    script = locId.Script;
    defaultRegion = LocaleIdentifier.Parse(string.Format("{0}-{1}", lang, script)).MostLikelySubtags().Region;
    switch (locId.ToString()) {
      case "en-Latn-GB": extra = "1"; break;
      case "zh-Hant-HK": extra = "2"; break;
      case "zh-Hans-SG": extra = "3"; break;
      case "zh-Hant-MO": extra = "4"; break;
    }
  }

  static string sortedCharsOnly(string str) {
    return new String(str.ToCharArray().Where(ch => UnicodeBlocks.isLetter(ch)).Distinct().OrderBy(ch => ch).ToArray());
  }

  //***** INDEXES
  public static int[] numsSource = Enumerable.Range(0, 21).ToArray();
  //=============
  public static int langIdx = 0;
  public static int scriptIdx = langIdx + 1;
  public static int defaultRegionIdx = scriptIdx + 1;
  public static int moreScriptsIdx = defaultRegionIdx + 1;
  public static int extraIdx = moreScriptsIdx + 1;
  public static int monthsIdx = extraIdx + 1;
  public static int smonthsIdx = monthsIdx + 12;
  public static int daysIdx = smonthsIdx + 12;
  public static int sdaysIdx = daysIdx + 7;
  public static int numsIdx = sdaysIdx + 7;
  public static int snumsIdx = numsIdx + numsSource.Length;
  //=============
  public static int skip = 5;
  public static int count = snumsIdx + numsSource.Length + 1;

}

public static class CldrTextLib {

  public static IEnumerable<CldrLangMatrixValue> fromCldrLocaleIdentifiers(IEnumerable<LocaleIdentifier> locs) {
    return locs.Select(loc => new CldrLangMatrixValue { lang = loc.ToString(), wrapper = new CldrWrapper(loc) });
  }

  public static IEnumerable<CldrLangMatrixValue> fromNetCultureInfos(LocaleIdentifier[] cldrSpecifics) {
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

        var res = new CldrWrapper(cu, lid.Language, lid.Region, out LocaleIdentifier locId);
        return new CldrLangMatrixValue {
          lang = locId.ToString(),
          wrapper = res,
        };
      }).
      Where(lt => lt != null);
  }

}

