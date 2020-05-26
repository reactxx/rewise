using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using json = System.Text.Json;

public static class JsonNew {
  public static void Serialize(string fn, object obj, bool standard = false) {
    using (var str = File.Open(fn, FileMode.Create, FileAccess.Write)) {
      var task = json.JsonSerializer.SerializeAsync(str, obj, obj.GetType(), options(standard));
      task.Wait();
    }
  }
  public static string SerializeStr(object obj, bool standard = false) =>
    json.JsonSerializer.Serialize(obj, obj.GetType(), options(standard));

  public static T Deserialize<T>(string fn) {
    using (var fs = File.OpenRead(fn))
      return Deserialize<T>(fs);
  }
  public static T DeserializeStr<T>(string str) => json.JsonSerializer.Deserialize<T>(str);
  public static T Deserialize<T>(Stream stream) {
    var task = json.JsonSerializer.DeserializeAsync<T>(stream).AsTask();
    task.Wait();
    return task.Result;
  }
  public static T DeserializeAssembly<T>(string resourceName) {
    var assembly = Assembly.GetExecutingAssembly();
    using (var stream = assembly.GetManifestResourceStream(resourceName))
      return Deserialize<T>(stream);
  }
  //public static T DeserializeAssembly<T>(string resourceName, Func<string, T> deserialize) {
  //  var assembly = Assembly.GetExecutingAssembly();
  //  using (var stream = assembly.GetManifestResourceStream(resourceName))
  //  using (var fss = new StreamReader(stream))
  //    return deserialize(fss.ReadToEnd());
  //}

  static json.JsonSerializerOptions intendedOptions = new json.JsonSerializerOptions { WriteIndented = true };
  static json.JsonSerializerOptions standardOptions = new json.JsonSerializerOptions { WriteIndented = false };
  static json.JsonSerializerOptions options(bool standard) => standard ? standardOptions : intendedOptions;

  public static IEnumerable<T> identityEnum<T>(this IEnumerable<T> objs, Action<T> act) {
    foreach (var obj in objs) {
      act(obj);
      yield return obj;
    }
  }

  public static void SerializeEnum<T>(string fn, IEnumerable<T> objs, bool WriteIndented = false) {
    using (var str = File.Open(fn, FileMode.Create, FileAccess.Write))
      SerializeEnum(str, objs, WriteIndented);
  }

  public static void SerializeEnum(Type type, string fn, IEnumerable objs, bool WriteIndented = false) {
    using (var str = File.Open(fn, FileMode.Create, FileAccess.Write))
      SerializeEnum(type, str, objs, WriteIndented);
  }

  public static void SerializeEnum<T>(Stream str, IEnumerable<T> objs, bool WriteIndented = false) =>
      json.JsonSerializer.SerializeAsync(str, objs, new json.JsonSerializerOptions { WriteIndented = WriteIndented }).Wait();

  public static void SerializeEnum(Type type, Stream str, IEnumerable objs, bool WriteIndented = false) {
    Type listType = typeof(JsonList<>).MakeGenericType(new[] { type });
    json.JsonSerializer.SerializeAsync(str, objs, listType, new json.JsonSerializerOptions { WriteIndented = WriteIndented }).Wait();
  }

  public static void DeserializeEnum<T>(Stream s, Action<T> onAdd) where T : class {
    DeserializeEnum(typeof(T), s, obj => onAdd(obj as T));
  }
  public static void DeserializeEnum(Type type, Stream s, Action<object> onAdd) {
    Debug.Assert(threadAddData == null);
    Debug.Assert(onAdd != null);
    threadAddData = onAdd;
    Type listType = typeof(JsonList<>).MakeGenericType(new[] { type });
    try {
      using (var str = new Str(s))
        json.JsonSerializer.DeserializeAsync(str, listType).AsTask().Wait();
    } finally { threadAddData = null; }
  }

  public static void DeserializeEnum<T>(string fn, Action<T> onAdd) where T : class {
    using (var fs = File.OpenRead(fn)) DeserializeEnum(fs, onAdd);
  }

  public static void DeserializeEnum(Type type, string fn, Action<object> onAdd) {
    using (var fs = File.OpenRead(fn)) DeserializeEnum(type, fs, onAdd);
  }

  [ThreadStatic]
  static Action<object> threadAddData;
  class Str : Stream {
    readonly Stream s;
    public Str(Stream s) : base() {
      this.s = s;
    }

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {
      var res = s.Read(buffer, offset, count);
      return Task.FromResult(res);
    }

    public override int Read(byte[] buffer, int offset, int count) => s.Read(buffer, offset, count);
    public override long Position { get => s.Position; set => s.Position = value; }
    public override long Length { get => s.Length; }
    public override bool CanWrite { get => s.CanWrite; }
    public override bool CanSeek { get => s.CanSeek; }
    public override bool CanRead { get => s.CanRead; }
    public override void Flush() => s.Flush();
    public override long Seek(long offset, SeekOrigin origin) => s.Seek(offset, origin);
    public override void SetLength(long value) => s.SetLength(value);
    public override void Write(byte[] buffer, int offset, int count) => s.Write(buffer, offset, count);

  }
  public class JsonList<T> : IList where T : class {

    public int Add(object value) {
      Debug.Assert(threadAddData != null);
      threadAddData(value as T);
      return 0;
    }

    public void Add(T item) => throw new NotImplementedException();
    public T this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int Count => throw new NotImplementedException();
    public bool IsReadOnly => true;//throw new NotImplementedException();
    public bool IsFixedSize => throw new NotImplementedException();
    public bool IsSynchronized => throw new NotImplementedException();
    public object SyncRoot => throw new NotImplementedException();
    object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public void Clear() => throw new NotImplementedException();
    public bool Contains(T item) => throw new NotImplementedException();
    public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();
    public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();
    public int IndexOf(T item) => throw new NotImplementedException();
    public void Insert(int index, T item) => throw new NotImplementedException();
    public bool Remove(T item) => throw new NotImplementedException();
    public void RemoveAt(int index) => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    public bool Contains(object value) => throw new NotImplementedException();
    public int IndexOf(object value) => throw new NotImplementedException();
    public void Insert(int index, object value) => throw new NotImplementedException();
    public void Remove(object value) => throw new NotImplementedException();
    public void CopyTo(Array array, int index) => throw new NotImplementedException();
  }

}
