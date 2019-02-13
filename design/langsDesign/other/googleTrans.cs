using Sepia.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class GoogleTrans {

  public static void Parse(Dictionary<string, LangMatrixRow> res) {
    var googleLocsCodes = File.ReadAllLines(LangsDesignDirs.otherappdata + "googleTrans.txt").
      Select(l => l.Split('\t')).
      Select(p => p[1].Split(' ')[0].Replace("**", "")).
      ToArray();
    var googleLocs = googleLocsCodes.
      Select(w => LocaleIdentifier.Parse(w).MostLikelySubtags()).
      ToArray();
    var oks = googleLocs.
      Select(loc => Langs.fullNameToMeta.TryGetValue(loc.ToString(), out Langs.CldrLang cl) ? cl : null).
      NotNulls().
      ToArray();
    var wrongs = googleLocs.
      Select(loc => Langs.fullNameToMeta.TryGetValue(loc.ToString(), out Langs.CldrLang cl) ? null : loc.ToString()).
      NotNulls().
      ToArray();
    if (googleLocsCodes.Length != oks.Length)
      throw new Exception();
    oks.ForEach((item, idx) => {
      var row = LangsDesignLib.adjustNewfulltextDataRow(res, item.id.ToString());
      row.row[7] = googleLocsCodes[idx];
    });
  }

  public static Langs.CldrLang[] getMissingLangs() {
    var wrongs = new string[] { "ceb", "ht", "hmn", "la", "ny", "sm", "su", Langs.invariantId.Split('-')[0] };
    var wrongsEx = wrongs.Select(w => LocaleIdentifier.Parse(w).MostLikelySubtags()).ToArray();
    var newLangs = wrongsEx.
      Select(l => new Langs.CldrLang {
        id = string.Format("{0}-{1}", l.Language, l.Region),
        lang = l.Language,
        scriptId = l.Script,
        defaultRegion = l.Region,
        regions = new string[] { l.Region }
      }).
      ToArray();
    //var newLangsStr = Json.SerializeStr(newLangs);
    return newLangs;
  }
}
/*
*** REPLACE
zh-CN=zh-Hans-CN => zh-Hans
zh-TW=zh-Hant-TW => zh-Hant
jw=jv-Latn-ID => jv-ID
no=nb-Latn-NO => nb-NO
tl=fil-Latn-PH => fil-PH
*** NEW
ceb=ceb-Latn-PH
ht=ht-Latn-HT
hmn=hmn-Latn-US
la=la-Latn-VA
ny=ny-Latn-MW
sm=sm-Latn-WS
su=su-Latn-ID


ceb; Latin, https://en.wikipedia.org/wiki/Cebuano_language
zh-CN
zh-TW
ht: Latin, https://en.wikipedia.org/wiki/Haitian_Creole
hmn: Latin, https://en.wikipedia.org/wiki/Hmong_language
jw: ma byt jv???
la: latin language
no: nb | nn, 2 norstiny
ny: latin, https://en.wikipedia.org/wiki/Chewa_language
sm: latin, https://en.wikipedia.org/wiki/Samoan_language
su: latin, https://en.wikipedia.org/wiki/Sundanese_language
tl: latin, https://en.wikipedia.org/wiki/Tagalog_language
 
*/
