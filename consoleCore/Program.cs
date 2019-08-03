using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

class Program {

  static void Main(string[] args) {

    //var invoker = GoogleCloudHelper.create("texttospeech.googleapis.com");
    //var cl = new Google.Cloud.Texttospeech.V1Beta1.TextToSpeech.TextToSpeechClient(invoker);
    //var resp = cl.ListVoices(new Google.Cloud.Texttospeech.V1Beta1.ListVoicesRequest { LanguageCode = "" });
    //var json = resp.ToString();
    //json = null;

    Json.Test();

  }

}

