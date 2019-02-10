using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using Sepia.Globalization;

public static class LangsDesignDirs {
  public static string driver = AppDomain.CurrentDomain.BaseDirectory[0].ToString();
  public static string root = driver + @":\rewise\design\langsDesign\";
  public static string rootData = root + @"appdata\";
  public static string unicode = rootData + @"unicode\";
  public static string cldr = rootData + @"cldr\";
  public static string other = root + @"other\";
  public static string cldrRepo = Cldr.Instance.Repositories[0] + "\\";
}
