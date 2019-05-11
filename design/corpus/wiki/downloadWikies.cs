﻿using System;
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

    public static void download() {
      var urls = Json.Deserialize<Header[]>(Directory.GetCurrentDirectory() + @"\wiki\validUrls.json");

      void down(Header url) {
        var srcFn = @"c:\temp\" + url.name + ".bz2";
        new MyWebClient(url.size).DownloadFile(url.url, srcFn);
        var destFn = @"c:\temp\" + url.name + ".bz2";
        var zip = @"c:\Program Files\7-Zip\7zG.exe";
        var dest = Dirs.wikies;
        //https://sevenzip.osdn.jp/chm/cmdline/syntax.htm
        var cmd = string.Format("call \"{0}\" e \"{1}\" -o\"{2}\"", zip, srcFn, dest);
        var process = System.Diagnostics.Process.Start("CMD.exe", "/C " + cmd);
        process.WaitForExit();
        File.Delete(srcFn);
      }

      var todo = urls.Where(u => u.valid && !File.Exists(Dirs.wikies + u.name)).OrderBy(u => u.size);

      Parallel.ForEach(todo, new ParallelOptions { MaxDegreeOfParallelism = 4 }, url => down(url));
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
        var request = (HttpWebRequest)WebRequest.Create(url.url);
        request.Timeout = 2000;
        request.Method = "HEAD";
        try {
          using (var response = (HttpWebResponse)request.GetResponse()) {
            if (response.StatusCode != HttpStatusCode.OK) return;
            url.valid = true;
            url.size = response.ContentLength;
          }
        } catch { }
      }

      Parallel.ForEach(urls, url => {
        lock (urls) processUrl(url);
      });

      Json.Serialize(@"C:\rewise\design\corpus\wiki\validUrls.json", urls);
    }

  }


}
