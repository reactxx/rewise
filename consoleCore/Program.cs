using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using grpc = global::Grpc.Core;
using pb = global::Google.Protobuf;

class Program {

  static void Main(string[] args) {

    var invoker = GoogleCloudHelper.create("texttospeech.googleapis.com");
    var cl = new Google.Cloud.Texttospeech.V1Beta1.TextToSpeech.TextToSpeechClient(invoker);
    var request = new Google.Cloud.Texttospeech.V1Beta1.ListVoicesRequest { LanguageCode = "en-US" };
    var resp = cl.ListVoices(request);
    var json = resp.ToString();
    json = null;

    //Json.Test();

  }

}

