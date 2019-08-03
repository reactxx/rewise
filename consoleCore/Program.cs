using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;

class Program {

  static void Main(string[] args) {

    //var invoker = GoogleCloudHelper.create("texttospeech.googleapis.com");
    //var cl = new Google.Cloud.Texttospeech.V1Beta1.TextToSpeech.TextToSpeechClient(invoker);
    //var resp = cl.ListVoices(new Google.Cloud.Texttospeech.V1Beta1.ListVoicesRequest { LanguageCode = "" });
    //var json = resp.ToString();
    //json = null;


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
  static IEnumerable<int> ints() {
    yield return 1;
    yield return 2;
  }
}

