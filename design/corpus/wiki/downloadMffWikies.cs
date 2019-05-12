using System;
using System.IO;
using System.IO.Compression;
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
        w.Timeout = 1000 * 3600; // (int)((size / 1000) * 6); // (16 gbyte za 4 hodiny) znasobene 6x  
        return w;
      }
    }

    public static Header[] getUrls() => Json.Deserialize<Header[]>(Directory.GetCurrentDirectory() + @"\wiki\validUrls.json");
    const string workDir = @"c:\temp\wikies\";
    public static void download() {

      void decompress(string src, string dest) {
        using (var strIn = File.OpenRead(src))
        using (var strOut = File.OpenWrite(dest)) // $"{Dirs.wikies}{url.name}"))
        using (GZipStream decompressionStream = new GZipStream(strIn, CompressionMode.Decompress)) {
          decompressionStream.CopyTo(strOut);
        }
      }

      void down(Header url) {
        //if (getContentLen(url) == 0) {
        //  Console.WriteLine("ERROR: ", url.name);
        //  return;
        //}
        Console.WriteLine($"{url.name} - {Math.Round((double)url.size / 1000000)}Mb");
        var srcFn = workDir + url.name + ".gz_";
        var destFn = workDir + url.name + ".gz";
        if (!File.Exists(workDir + url.name + ".gz")) {
          try {

            new MyWebClient(url.size).DownloadFile(url.url, srcFn);
          } catch {
            Console.WriteLine("ERROR: " + url.name);
            return;
          }
          File.Move(srcFn, destFn);
        }

        decompress(destFn, $"{Dirs.wikiesMff}{url.name}");

        //var zip = @"c:\Program Files\7-Zip\7zG.exe";
        //var dest = Dirs.wikies;
        //https://sevenzip.osdn.jp/chm/cmdline/syntax.htm
        //var cmd = $"call \"{zip}\" e \"{destFn}\" -o\"{dest}\"";
        //var process = System.Diagnostics.Process.Start("CMD.exe", "/C " + cmd);
        //process.WaitForExit();
        //File.Delete(srcFn);
      }

      var todo = getUrls().
        Where(u => u.valid && !File.Exists(Dirs.wikiesMff + u.name)). //!File.Exists(Dirs.wikies + u.name)). // && !File.Exists(workDir + u.name + ".gz")).
        OrderBy(u => u.size);

      //down(todo.First());
      //foreach (var url in todo) down(url);

      Parallel.ForEach(todo, new ParallelOptions { MaxDegreeOfParallelism = 2 }, url => down(url));
    }

    //https://lindat.mff.cuni.cz/repository/xmlui/handle/11234/1-2735
    public static void parseHome() {
      XElement root = XElement.Load(Directory.GetCurrentDirectory() + @"\wiki\downloadHome.xml");
      var urls = root.
        Descendants("a").
        Select(e => e.Attribute("href").Value).
        Distinct().
        Select(u => u.Split(new string[] { ".txt.gz" }, StringSplitOptions.None)).
        Where(p => p.Length == 2).
        Select(p => new Header {
          url = $"https://lindat.mff.cuni.cz{p[0]}.txt.gz{p[1]}",
          name = p[0].Split('/')[7]
        }).
        ToArray();

      void processUrl(Header url) {
        url.size = getContentLen(url);
        url.valid = url.size > 0;
      }

      Parallel.ForEach(urls, url => {
        lock (urls) processUrl(url);
      });

      //Json.Serialize(@"C:\rewise\design\corpus\wiki\validUrls.json", urls.Where(u => u.size>1000).ToArray());
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
