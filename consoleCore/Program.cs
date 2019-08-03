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

    JsonNew.write(@"c:\temp\pom.json", objs());
    Parallel.ForEach(Enumerable.Range(0, 100), idx => {
      var count = 0;
      JsonNew.read<X>(@"c:\temp\pom.json", x => {
        count++;
      });
      Debug.Assert(count == 20000);
      count = 0;
    });
    //for (var i = 0; i < 10; i++) {
    //  var count = 0;
    //  JsonNew.read<X>(@"c:\temp\pom.json", x => {
    //    count++;
    //  });
    //  Debug.Assert(count == 200000);
    //  count = 0;
    //}

  }

  class X {
    public int a { get; set; }
    public string b { get; set; }
  }

  static IEnumerable<X> objs() {
    for (var i = 0; i < 10000; i++) {
      yield return new X { a = 5, b = "xxx" };
      yield return new X { a = 6, b = "yyy" };
    }
  }
  static IEnumerable<int> ints() {
    yield return 1;
    yield return 2;
  }
}

