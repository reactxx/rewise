using System.Reflection;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Threading.Tasks;
using System.IO;

public class HackJsonService : Rw.HackJson.CSharpService.CSharpServiceBase {

  public override Task<Rw.HackJson.HackJsonPar> HackJson(Rw.HackJson.HackJsonPar req, ServerCallContext context) {
    var resp = new Rw.HackJson.HackJsonPar { QualifiedMessageName = req.QualifiedMessageName };
    if (req.IsToJson)
      resp.S = bytesToString(req.QualifiedMessageName, req.B.ToByteArray());
    else {
      var bytes = stringToBytes(req.QualifiedMessageName, req.S);
      resp.B = ByteString.CopyFrom(bytes, 0, bytes.Length);
    }
    return Task.FromResult(resp);
  }

  public override Task<Empty> HackJsonFile(Rw.HackJson.HackJsonFilePar req, ServerCallContext context) {
    if (req.IsToJson)
      File.WriteAllText(req.Files.Dest, bytesToString(req.QualifiedMessageName, File.ReadAllBytes(req.Files.Src)));
    else
      File.WriteAllBytes(req.Files.Dest, stringToBytes(req.QualifiedMessageName, File.ReadAllText(req.Files.Src)));
    return Task.FromResult(new Empty());
  }

  static IMessage objectFromString(string str) {
    str = str.Replace("rewiseDom.", "RewiseDom.");
    Assembly asm = typeof(Rw.HackJson.HackJsonPar).Assembly;
    System.Type type = asm.GetType(str);
    return System.Activator.CreateInstance(type) as IMessage;
  }

  byte[] stringToBytes(string QualifiedMessageName, string data) {
    var msg = Protobuf.FromJson(data, () => objectFromString(QualifiedMessageName));
    return msg.ToByteArray();
  }

  string bytesToString(string QualifiedMessageName, byte[] data) {
    var msg = objectFromString(QualifiedMessageName);
    Protobuf.FromBytes(data, msg);
    return Protobuf.ToJson(msg);
  }

}
