using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
//https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-apis/
public static class Json {

  public static class JsonStreamWriter {
    public static void write<T>(string fn, IEnumerable<T> objs) {
      if (File.Exists(fn)) File.Delete(fn);
      using (var str = File.OpenWrite(fn))
        JsonSerializer.WriteAsync(objs, typeof(IEnumerable<T>), str, new JsonSerializerOptions { WriteIndented = true }).Wait();
    }
    //public JsonStreamWriter(string fn) {
    //  if (File.Exists(fn)) File.Delete(fn);
    //  str = File.OpenWrite(fn);
    //  wr = new Utf8JsonWriter(str, new JsonWriterOptions { Indented = true });
    //  wr.WriteStartArray();
    //  wr.Flush();
    //}
    //public async void Serialize(Object obj) => 
    //  await JsonSerializer.WriteAsync(obj, obj.GetType(), str);
    //Utf8JsonWriter wr;
    //Stream str;
    //public void Dispose() {
    //  //str.Flush();
    //  wr.WriteEndArray();
    //  wr.Dispose();
    //}
  }


}
