using System.Reflection;
using Google.Protobuf;
using Grpc.Core;
using System.Threading.Tasks;

public class HackJsonService: Rw.HackJson.CSharpService.CSharpServiceBase {

  public override Task<Rw.HackJson.HackJsonBytes> HackFromJson(Rw.HackJson.HackJsonString req, ServerCallContext context) {
    var msg = Protobuf.FromJson(req.Value, () => objectFromString(req.QualifiedMessageName));
    var bytes = msg.ToByteArray();
    var resp = new Rw.HackJson.HackJsonBytes { QualifiedMessageName = req.QualifiedMessageName, Value = Google.Protobuf.ByteString.CopyFrom(bytes, 0, bytes.Length) };
    return Task.FromResult(resp);
  }

  public override Task<Rw.HackJson.HackJsonString> HackToJson(Rw.HackJson.HackJsonBytes req, ServerCallContext context) {
    var msg = objectFromString(req.QualifiedMessageName);
    Protobuf.FromBytes(req.Value.ToByteArray(), msg);
    var resp = new Rw.HackJson.HackJsonString { QualifiedMessageName = req.QualifiedMessageName, Value = Protobuf.ToJson(msg) };
    return Task.FromResult(resp);
  }


  static IMessage objectFromString(string str) {
    str = str.Replace("rewiseDom.", "RewiseDom.");
    Assembly asm = typeof(Rw.HackJson.HackJsonString).Assembly;
    System.Type type = asm.GetType(str);
    return System.Activator.CreateInstance(type) as IMessage;
  }

}
