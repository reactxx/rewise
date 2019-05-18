using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Text;

public class JsonStreamWriter : IDisposable {
  public JsonStreamWriter(string fn) {
    if (File.Exists(fn)) File.Delete(fn);
    ser = Json.Serializer();
    wr = new JsonTextWriter(new StreamWriter(fn));
    wr.WriteStartArray();
  }
  public void Serialize(Object obj) => ser.Serialize(wr, obj);
  JsonTextWriter wr;
  JsonSerializer ser;
  public void Dispose() {
    wr.WriteEndArray();
    wr.Close();
  }
}


public static class Json {
  public static JsonSerializer Serializer(bool packed = false) {
    return JsonSerializer.Create(packed ? packedOptions : options);
  }
  public static void Serialize(string fn, Object obj) {
    if (File.Exists(fn)) File.Delete(fn);
    var ser = Serializer();
    using (var fss = new StreamWriter(fn))
    using (var fs = new JsonTextWriter(fss))
      ser.Serialize(fs, obj);
  }
  public static string SerializeStr(Object obj, bool packed = false) {
    var ser = Serializer(packed);
    var sb = new StringBuilder();
    using (var fss = new StringWriter(sb))
    using (var fs = new JsonTextWriter(fss) { })
      ser.Serialize(fs, obj);
    return sb.ToString();
  }
  public static T Deserialize<T>(string fn) {
    var ser = Serializer();
    using (var fss = new StreamReader(fn))
    using (var fs = new JsonTextReader(fss))
      return ser.Deserialize<T>(fs);
  }
  public static T DeserializeStr<T>(string str) {
    var ser = Serializer();
    using (var fss = new StringReader(str))
    using (var fs = new JsonTextReader(fss))
      return ser.Deserialize<T>(fs);
  }
  public static T Deserialize<T>(Stream stream) {
    try {
      var ser = Serializer();
      using (var fss = new StreamReader(stream))
      using (var fs = new JsonTextReader(fss))
        return ser.Deserialize<T>(fs);
    } finally { stream.Close(); }
  }
  public static T DeserializeAssembly<T>(string resourceName) {
    var assembly = Assembly.GetExecutingAssembly();
    var ser = Serializer();
    using (var stream = assembly.GetManifestResourceStream(resourceName))
    using (var fss = new StreamReader(stream))
    using (var fs = new JsonTextReader(fss))
      return ser.Deserialize<T>(fs);
  }
  public static T DeserializeAssembly<T>(string resourceName, Func<string, T> deserialize) {
    var assembly = Assembly.GetExecutingAssembly();
    using (var stream = assembly.GetManifestResourceStream(resourceName))
    using (var fss = new StreamReader(stream))
      return deserialize(fss.ReadToEnd());
  }
  public static JsonSerializerSettings options = new JsonSerializerSettings {
    Formatting = Formatting.Indented,
    DefaultValueHandling = DefaultValueHandling.Ignore,
  };
  public static JsonSerializerSettings packedOptions = new JsonSerializerSettings {
    DefaultValueHandling = DefaultValueHandling.Ignore,
  };

}