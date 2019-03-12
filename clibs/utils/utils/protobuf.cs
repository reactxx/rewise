﻿using Google.Protobuf;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Text;

public static class Protobuf {

  public static byte[] ToBytes(dynamic msg) {
    using (var str = new MemoryStream()) {
      using (var wr = new CodedOutputStream(str))
        msg.WriteTo(wr);
      var bytes = str.ToArray();
      return bytes;
    }
  }

  public static T FromBytes<T>(byte[] bytes, Func<T> creator) where T : IMessage<T> {
    return new MessageParser<T>(creator).ParseFrom(bytes);
  }

  public static string ToJson(dynamic msg) {
    return new JsonFormatter(JsonFormatter.Settings.Default).Format(msg);
  }

  public static T FromJson<T>(string json, Func<T> creator) where T : IMessage<T> {
    return new MessageParser<T>(creator).ParseJson(json);
  }

  public static string ToBase64(dynamic msg) {
    using (var str = new MemoryStream()) {
      using (var wr = new CodedOutputStream(str))
        msg.WriteTo(wr);
      var bytes = str.ToArray();
      return Convert.ToBase64String(bytes);
    }
  }

  public static T FromBase64<T>(string bytes, Func<T> creator) where T : IMessage<T> {
    return new MessageParser<T>(creator).ParseFrom(Convert.FromBase64String(bytes));
  }


  public static void Test() {
    var msg = new RewiseDom.HelloReply { CsharpId = 1234 };
    var bytes = Protobuf.ToBytes(msg);
    msg = Protobuf.FromBytes(bytes, () => new RewiseDom.HelloReply());
    var json = Protobuf.ToJson(msg);
    msg = Protobuf.FromJson(json, () => new RewiseDom.HelloReply());
  }

}