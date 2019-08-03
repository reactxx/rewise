//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading;
//using json = System.Text.Json;

//public static class JsonNew {

//  public static void write<T>(string fn, IEnumerable<T> objs) {
//    if (File.Exists(fn)) File.Delete(fn);
//    using (var str = File.OpenWrite(fn))
//      json.JsonSerializer.SerializeAsync(str, objs, new json.JsonSerializerOptions { WriteIndented = true }).Wait();
//    //json.JsonDocument.ParseValue
//  }

//  public static void read<T>(string fn, Action<T> addData) where T : class {
//    using (var fs = File.OpenRead(fn)) using (var str = new Str<T>(fs, addData)) {
//      var task = json.JsonSerializer.DeserializeAsync<JsonList<T>>(str).AsTask();
//      try {
//        task.Wait();
//      } finally {
//        Str<T>.threadAddData = null;
//        //JsonList<T>.addData = null;
//      }
//    }
//  }


//  class Str<T> : Stream where T : class {
//    Stream s;
//    [ThreadStatic]
//    public static Action<T> threadAddData;
//    Action<T> addData;
//    public Str(Stream s, Action<T> addData) : base() { this.s = s; this.addData = addData; }
//    public override int Read(byte[] buffer, int offset, int count) {
//      threadAddData = addData;
//      //JsonList<T>.addData2.Value = addData;
//      return s.Read(buffer, offset, count);
//    }
//    public override long Position { get => s.Position; set => s.Position = value; }
//    public override long Length { get => s.Length; }
//    public override bool CanWrite { get => s.CanWrite; }
//    public override bool CanSeek { get => s.CanSeek; }
//    public override bool CanRead { get => s.CanRead; }
//    public override void Flush() => s.Flush();
//    public override long Seek(long offset, SeekOrigin origin) => s.Seek(offset, origin);
//    public override void SetLength(long value) => s.SetLength(value);
//    public override void Write(byte[] buffer, int offset, int count) => s.Write(buffer, offset, count);

//  }
//  public class JsonList<T> : IList<T>, IList where T : class {

//    public JsonList() {
//      addData = Str<T>.threadAddData;
//    }
//    Action<T> addData;

//    public int Add(object value) {
//      if (addData == null)
//        return 0;
//      addData(value as T);
//      return 0;
//      //if (addData2.Value == null)
//      //  return 0;
//      //addData2.Value(value as T);
//      //return 0;
//    }

//    public void Add(T item) {
//      if (addData == null)
//        return;
//      addData(item as T);
//    }

//    List<T> l = new List<T>();

//    public T this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//    public int Count => throw new NotImplementedException();
//    public bool IsReadOnly => throw new NotImplementedException();
//    public bool IsFixedSize => throw new NotImplementedException();
//    public bool IsSynchronized => throw new NotImplementedException();
//    public object SyncRoot => throw new NotImplementedException();
//    object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
//    public void Clear() => throw new NotImplementedException();
//    public bool Contains(T item) => throw new NotImplementedException();
//    public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();
//    public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();
//    public int IndexOf(T item) => throw new NotImplementedException();
//    public void Insert(int index, T item) => throw new NotImplementedException();
//    public bool Remove(T item) => throw new NotImplementedException();
//    public void RemoveAt(int index) => throw new NotImplementedException();
//    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
//    public bool Contains(object value) => throw new NotImplementedException();
//    public int IndexOf(object value) => throw new NotImplementedException();
//    public void Insert(int index, object value) => throw new NotImplementedException();
//    public void Remove(object value) => throw new NotImplementedException();
//    public void CopyTo(Array array, int index) => throw new NotImplementedException();
//  }

//  //class Converter<T> : json.Serialization.JsonConverter<T> where T : class {
//  //  public override T Read(ref json.Utf8JsonReader reader, Type typeToConvert, json.JsonSerializerOptions options) {
//  //    using (json.JsonDocument document = json.JsonDocument.ParseValue(ref reader)) {
//  //      var obj = document.RootElement;
//  //      var obj2 = obj.Clone();
//  //      //var isEq = obj == obj2;
//  //      return null; // document.RootElement.Clone();
//  //    }
//  //    //reader.
//  //    //throw new NotImplementedException();
//  //    //return null;
//  //  }

//  //  public override void Write(json.Utf8JsonWriter writer, T value, json.JsonSerializerOptions options) {
//  //    throw new NotImplementedException();
//  //  }
//  //}

//  //class Conv<T> : json.Serialization.JsonConverter<T> where T : class {
//  //  public Action<T> addData;
//  //  public override bool CanConvert(Type typeToConvert) {
//  //    JsonList<T>.addData = addData;
//  //    return false;
//  //  }
//  //  public override T Read(ref json.Utf8JsonReader reader, Type typeToConvert, json.JsonSerializerOptions options) {
//  //    JsonList<T>.addData = addData;
//  //    using (json.JsonDocument document = json.JsonDocument.ParseValue(ref reader)) {
//  //      var res = document.RootElement.Clone();
//  //      return null;
//  //    }
//  //    //using (json.JsonDocument document = json.JsonDocument.ParseValue(ref reader)) {
//  //    //  JsonList<T>.addData = addData;
//  //    //  var obj = document.RootElement;
//  //    //  var obj2 = obj.Clone();
//  //    //  //var isEq = obj == obj2;
//  //    //  return null; // document.RootElement.Clone();
//  //    //}
//  //    //reader.
//  //    //throw new NotImplementedException();
//  //    //return null;
//  //  }

//  //  public override void Write(json.Utf8JsonWriter writer, T value, json.JsonSerializerOptions options) {
//  //    throw new NotImplementedException();
//  //  }
//  //}

//}

//    JsonNew.write(@"c:\temp\pom.json", objs());
//    for (var i = 0; i< 10; i++) {
//      var count = 0;
//JsonNew.read<X>(@"c:\temp\pom.json", x => {
//        count++;
//      });
//      Debug.Assert(count == 200000);
//      count = 0;
//    }