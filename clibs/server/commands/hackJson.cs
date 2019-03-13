using Google.Protobuf.WellKnownTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using RewiseDom;
using StemmerBreaker;
using System.Reflection;
using Google.Protobuf;

public static class HackFromJsonTask {

  public static HackJsonBytes hackFromJson(HackJsonString req) {
    var msg = objectFromString(req.QualifiedMessageName);
    MessageParser parser = msg.GetType().GetProperty("Parser", BindingFlags.Public | BindingFlags.Static).GetValue(msg) as MessageParser;
    msg = parser.ParseJson(req.Value);
    //HackJsonString.Parser.ParseJson(req.Value);
    var bytes = msg.ToByteArray();
    return new HackJsonBytes { QualifiedMessageName = req.QualifiedMessageName, Value = Google.Protobuf.ByteString.CopyFrom(bytes, 0, bytes.Length) };
  }
  public static HackJsonString hackToJson(HackJsonBytes req) {
    var msg = objectFromString(req.QualifiedMessageName);
    Protobuf.FromBytes(req.Value.ToByteArray(), msg);
    return new HackJsonString { QualifiedMessageName = req.QualifiedMessageName, Value = Protobuf.ToJson(msg) };
  }

  static IMessage objectFromString(string str) {
    str = str.Replace("rewiseDom.", "RewiseDom.");
    Assembly asm = typeof(HackJsonString).Assembly;
    System.Type type = asm.GetType(str);
    return System.Activator.CreateInstance(type) as IMessage;
  }

  //static System.Type typeFromString(string str) {
  //  str = str.Replace("rewiseDom.", "RewiseDom.");
  //  Assembly asm = typeof(HackJsonString).Assembly;
  //  return asm.GetType(str);
  //}
}
