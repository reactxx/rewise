using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using Sepia.Globalization;

public static class Root {
  public static string driver = AppDomain.CurrentDomain.BaseDirectory[0].ToString();
  public static string root = driver + @":\rewise\word-lists\lang_chars\appdata\";
  public static string mimerSite = root + @"mimer-site\";
  public static string unicode = root + @"unicode\";
  public static string cldr = root + @"cldr\";
  public static string cldrRepo = Cldr.Instance.Repositories[0] + "\\";
}
