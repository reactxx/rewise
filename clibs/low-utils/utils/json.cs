using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

public static class Json {
  public static JsonSerializer Serializer() {
    return JsonSerializer.Create(options);
  }
  public static void Serialize(string fn, Object obj) {
    if (File.Exists(fn)) File.Delete(fn);
    var ser = Json.Serializer();
    using (var fss = new StreamWriter(fn))
    using (var fs = new JsonTextWriter(fss) { })
      ser.Serialize(fs, obj);
  }
  public static T Deserialize<T>(string fn) {
    var ser = Json.Serializer();
    using (var fss = new StreamReader(fn))
    using (var fs = new JsonTextReader(fss))
      return ser.Deserialize<T>(fs);
  }
  public static T Deserialize<T>(Stream stream) {
    try {
      var ser = Json.Serializer();
      using (var fss = new StreamReader(stream))
      using (var fs = new JsonTextReader(fss))
        return ser.Deserialize<T>(fs);
    } finally { stream.Close(); }
  }
  public static T DeserializeAssembly<T>(string resourceName) {
    var assembly = Assembly.GetExecutingAssembly();
    var ser = Json.Serializer();
    using (var stream = assembly.GetManifestResourceStream(resourceName))
    using (var fss = new StreamReader(stream))
    using (var fs = new JsonTextReader(fss))
      return ser.Deserialize<T>(fs);
  }
  public static JsonSerializerSettings options = new JsonSerializerSettings {
    Formatting = Formatting.Indented,
    DefaultValueHandling = DefaultValueHandling.Ignore,
  };
}