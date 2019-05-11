using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Corpus {

  public static class DownloadWikies {

    public class Header {
      public bool valid;
      public string url;
      public string name;
      public long size;
    }

    public class MyWebClient : WebClient {
      public MyWebClient(long size) {
        this.size = size;
      }
      long size;
      protected override WebRequest GetWebRequest(Uri uri) {
        WebRequest w = base.GetWebRequest(uri);
        w.Timeout = (int)((size / 1000) * 6); // (16 gbyte za 4 hodiny) znasobene 6x  
        return w;
      }
    }

    public static Header[] getUrls() => Json.Deserialize<Header[]>(Directory.GetCurrentDirectory() + @"\wiki\validUrls.json");
    
      public static void download() {

      void down(Header url) {
        //if (getContentLen(url) == 0) {
        //  Console.WriteLine("ERROR: ", url.name);
        //  return;
        //}
        Console.WriteLine(string.Format("{0} - {1}Mb", url.name, Math.Round((double)url.size / 1000000)));
        var srcFn = @"c:\temp\" + url.name + ".bz2";
        try {
          new MyWebClient(url.size).DownloadFile(url.url, srcFn);
        } catch {
          Console.WriteLine("ERROR: " + url.name);
          return;
        }
        var destFn = @"c:\temp\" + url.name + ".bz2";
        var zip = @"c:\Program Files\7-Zip\7zG.exe";
        var dest = Dirs.wikies;
        //https://sevenzip.osdn.jp/chm/cmdline/syntax.htm
        var cmd = string.Format("call \"{0}\" e \"{1}\" -o\"{2}\"", zip, srcFn, dest);
        //var process = System.Diagnostics.Process.Start("CMD.exe", "/C " + cmd);
        //process.WaitForExit();
        //File.Delete(srcFn);
      }

      var todo = getUrls().Where(u => u.valid && !File.Exists(Dirs.wikies + u.name)).OrderBy(u => u.size);

      //foreach (var url in todo) down(url);

      Parallel.ForEach(todo, new ParallelOptions { MaxDegreeOfParallelism = 2 }, url => down(url));
    }

    public static void parseHome() {
      XElement root = XElement.Load(Directory.GetCurrentDirectory() + @"\wiki\downloadHome.xml");
      var wikies = root.Descendants("a").Select(e => e.Attribute("href").Value.TrimEnd('/')).Distinct().OrderBy(w => w).ToArray();
      var types = wikies.Select(w => "wi" + w.Split(new string[] { "wi" }, StringSplitOptions.RemoveEmptyEntries)[1]).Distinct().OrderBy(w => w).ToArray();

      var urls = wikies.Select(w => new Header {
        url = string.Format("https://ftp.acc.umu.se/mirror/wikimedia.org/dumps/{0}/20190420/{0}-20190420-pages-articles.xml.bz2", w),
        name = w,
      }).ToArray();

      void processUrl(Header url) {
        url.size = getContentLen(url);
        url.valid = url.size > 0;
      }

      Parallel.ForEach(urls, url => {
        lock (urls) processUrl(url);
      });

      Json.Serialize(@"C:\rewise\design\corpus\wiki\validUrls.json", urls);
    }

    static int getContentLen(Header url) {
      var request = (HttpWebRequest)WebRequest.Create(url.url);
      request.Timeout = 20000;
      request.Method = "HEAD";
      try {
        using (var response = (HttpWebResponse)request.GetResponse()) {
          if (response.StatusCode != HttpStatusCode.OK) return 0;
          return (int)response.ContentLength;
        }
      } catch { }
      return 0;
    }



  }


}
