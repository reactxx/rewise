using Newtonsoft.Json.Bson;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using NewJson = System.Text.Json.Serialization;

public static class JsonNew {

  public static void write<T>(string fn, IEnumerable<T> objs) {
    if (File.Exists(fn)) File.Delete(fn);
    using (var str = File.OpenWrite(fn))
      NewJson.JsonSerializer.WriteAsync(objs, typeof(IEnumerable<T>), str, new NewJson.JsonSerializerOptions { WriteIndented = true }).Wait();
  }

  //public static IEnumerable<T> read<T>(string fn) {
  //  using (var str = File.OpenRead(fn)) {
  //    NewJson.JsonSerializer. .Deserialize<IEnumerable<T>>()
  //    var reader = new Js.Utf8JsonReader()
  //    JS.Utf8JsonReader.
  //  }
  //}

}
