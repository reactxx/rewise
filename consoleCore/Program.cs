using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using static Json;

class Program {

  static void Main(string[] args) {

    var invoker = GoogleCloudHelper.create("texttospeech.googleapis.com");
    var cl = new Google.Cloud.Texttospeech.V1Beta1.TextToSpeech.TextToSpeechClient(invoker);
    var resp = cl.ListVoices(new Google.Cloud.Texttospeech.V1Beta1.ListVoicesRequest { LanguageCode = "" });
    var json = resp.ToString();
    json = null;

    //proc();

    //var x = new X { a = 5, b = "xxx" };
    //using (var wr = new Json.JsonStreamWriter(@"c:\temp\pom.json")) {
    //  wr.Serialize(ints());
    //  //wr.Serialize(x);
    //  //wr.Serialize(x);
    //  //wr.Serialize(x);
    //}
  }

  class X {
    public int a { get; set; }
    public string b { get; set; }
  }

  static IEnumerable<X> objs() {
    yield return new X { a = 5, b = "xxx" };
    yield return new X { a = 5, b = "xxx" };
  }
  static IEnumerable<int> ints() {
    yield return 1;
    yield return 2;
  }
}

