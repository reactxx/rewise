using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

public static class ImportFromRJ {

  public static void Import() {
    Directory.GetDirectories(ImportFromRJDirs.appDataSource).ForEach(dir => {
      var lang = dir.Substring(ImportFromRJDirs.appDataSource.Length);
    });
  }
}

