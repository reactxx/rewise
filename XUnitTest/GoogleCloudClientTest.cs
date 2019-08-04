using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GoogleCloudClientTest {
  public class Test {
    [Fact]
    public void Test1() {
      var invoker = GoogleCloudHelper.create("texttospeech.googleapis.com");
      var cl = new Google.Cloud.Texttospeech.V1Beta1.TextToSpeech.TextToSpeechClient(invoker);
      var request = new Google.Cloud.Texttospeech.V1Beta1.ListVoicesRequest { LanguageCode = "en-US" };
      var resp = cl.ListVoices(request);
      var json = resp.ToString();
      Assert.NotNull(json);
    }
  }

}
