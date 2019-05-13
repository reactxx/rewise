using Sepia.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class MSTrans {

  public static void CldrPatch() {
    //Json.Serialize(LangsDesignDirs.root + @"patches\msTrans.json", res);
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
