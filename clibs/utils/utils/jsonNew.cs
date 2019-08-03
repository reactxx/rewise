using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using json = System.Text.Json;

public static class JsonNew {

  public static void write<T>(string fn, IEnumerable<T> objs, bool WriteIndented = false) {
    using (var str = File.Open(fn, FileMode.Create, FileAccess.Write))
      write<T>(str, objs, WriteIndented);
  }

  public static void write<T>(Stream str, IEnumerable<T> objs, bool WriteIndented = false) =>
      json.JsonSerializer.SerializeAsync(str, objs, new json.JsonSerializerOptions { WriteIndented = WriteIndented }).Wait();

  public static void read<T>(Stream s, Action<T> onAdd) where T : class {
    Debug.Assert(JsonList<T>.threadAddData == null);
    Debug.Assert(onAdd != null);
    JsonList<T>.threadAddData = onAdd;
    try {
      using (var str = new Str(s))
        json.JsonSerializer.DeserializeAsync<JsonList<T>>(str).AsTask().Wait();
    } finally { JsonList<T>.threadAddData = null; }
  }
  public static void read(Type type, Stream s, Action<object> onAdd) {
    Debug.Assert(threadAddData == null);
    Debug.Assert(onAdd != null);
    threadAddData = onAdd;
    try {
      using (var str = new Str(s))
        json.JsonSerializer.DeserializeAsync(str, type).AsTask().Wait();
    } finally { threadAddData = null; }
  }

  public static void read<T>(string fn, Action<T> onAdd) where T : class {
    using (var fs = File.OpenRead(fn)) read(fs, onAdd);
  }

  [ThreadStatic]
  static Action<object> threadAddData;
  class Str : Stream {
    Stream s;
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
  public class JsonList<T> : IList<T>, IList where T : class {

    [ThreadStatic]
    public static Action<T> threadAddData;

    public int Add(object value) {
      Debug.Assert(threadAddData != null);
      threadAddData(value as T);
      return 0;
    }

    public void Add(T item) => throw new NotImplementedException();
    public T this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int Count => throw new NotImplementedException();
    public bool IsReadOnly => throw new NotImplementedException();
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

  public static void Test() {
    write(@"c:\temp\pom.json", objs());
    Parallel.ForEach(Enumerable.Range(0, 100), idx => {
      var count = 0;
      read<X>(@"c:\temp\pom.json", x => {
        count++;
      });
      Debug.Assert(count == 200000);
      count = 0;
    });
  }

  class X {
    public int a { get; set; }
    public string b { get; set; }
  }

  static IEnumerable<X> objs() {
    for (var i = 0; i < 100000; i++) {
      yield return new X { a = 5, b = "xxx" };
      yield return new X { a = 6, b = "yyy" };
    }
  }

}
