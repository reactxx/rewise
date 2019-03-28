using System;
using Sepia.Globalization;

public static class LangsDesignDirs {
  public static string driver = AppDomain.CurrentDomain.BaseDirectory[0].ToString();
  public static string root = driver + @":\rewise\design\langsDesign\";
  public static string rootData = root + @"appdata\";
  public static string unicode = rootData + @"unicode\";
  public static string cldr = rootData + @"cldr\";
  public static string other = root + @"other\";
  public static string otherappdata = rootData + @"other\";
  public static string cldrRepo = Cldr.Instance.Repositories[0] + "\\";
  public static string data = driver + @":\rewise\data\";
}
