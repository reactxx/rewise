using System;

public static class LowUtilsDirs {
  public static char drive = AppDomain.CurrentDomain.BaseDirectory[0];
  public static string root = drive + @":\rewise\clibs\utils\";
  public static string res = "LowUtils.";
  public static string logs = drive + @":\rewise\data\logs\";
}

