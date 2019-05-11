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

    public static void download() {
      var urls = Json.Deserialize<Header[]>(Directory.GetCurrentDirectory() + @"\wiki\validUrls.json");

      void down(Header url) {
        var srcFn = @"c:\temp\" + url.name + ".bz2";
        new WebClient().DownloadFile(url.url, srcFn);
        var destFn = @"c:\temp\" + url.name + ".bz2";
        var zip = @"c:\Program Files\7-Zip\7zG.exe";
        var dest = Dirs.wikies;
        var cmd = string.Format("call \"{0}\" e \"{1}\" -o\"{2}\"", zip, srcFn, dest);
        var process = System.Diagnostics.Process.Start("CMD.exe", "/C " + cmd);
        process.WaitForExit();
        File.Delete(srcFn);
      }

      //down(urls.Where(u => u.valid).OrderBy(u => u.size).First());

      Parallel.ForEach(urls.Where(u => u.valid).OrderBy(u => u.size), new ParallelOptions { MaxDegreeOfParallelism = 4 }, url => down(url));
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
            //validUrls[url] = request.ContentLength;
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
      //File.WriteAllLines(@"c:\temp\allUrls.txt", Linq.Items<string>("SUM=" + validUrls.Sum(h => h.size).ToString()).Concat(urls));
    }

  }


}



/*
 * https://ftp.acc.umu.se/mirror/wikimedia.org/dumps/enwiki/20190420/enwiki-20190420-pages-articles-multistream.xml.bz2
 * https://ftp.acc.umu.se/mirror/wikimedia.org/dumps/enwikibooks/20190420/enwikibooks-20190420-pages-articles.xml.bz2
 * https://ftp.acc.umu.se/mirror/wikimedia.org/dumps/enwikinews/20190420/enwikinews-20190420-pages-articles.xml.bz2
 * https://ftp.acc.umu.se/mirror/wikimedia.org/dumps/enwikiquote/20190420/enwikiquote-20190420-pages-articles.xml.bz2
 * https://ftp.acc.umu.se/mirror/wikimedia.org/dumps/enwikisource/20190420/enwikisource-20190420-pages-articles.xml.bz2
 * https://ftp.acc.umu.se/mirror/wikimedia.org/dumps/enwikiversity/20190420/enwikiversity-20190420-pages-articles.xml.bz2
 * https://ftp.acc.umu.se/mirror/wikimedia.org/dumps/enwikivoyage/20190420/enwikivoyage-20190420-pages-articles.xml.bz2
 * https://ftp.acc.umu.se/mirror/wikimedia.org/dumps/enwiktionary/20190420/enwiktionary-20190420-pages-articles.xml.bz2
 */
