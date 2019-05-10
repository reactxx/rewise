using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Corpus {

  public static class DownloadWikies {

    public class Header {
      public string url;
      public long size;
    }

    public static void parseHome() {
      XElement root = XElement.Load(Directory.GetCurrentDirectory() + @"\wiki\downloadHome.xml");
      var wikies = root.Descendants("a").Select(e => e.Attribute("href").Value.TrimEnd('/')).Distinct().OrderBy(w => w).ToArray();
      var types = wikies.Select(w => "wi" + w.Split(new string[] { "wi" }, StringSplitOptions.RemoveEmptyEntries)[1]).Distinct().OrderBy(w => w).ToArray();

      var urls = wikies.Select(w => string.Format("https://ftp.acc.umu.se/mirror/wikimedia.org/dumps/{0}/20190420/{0}-20190420-pages-articles.xml.bz2", w));
      var validUrls = new List<Header>(); 

      void processUrl(string url) {
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Timeout = 2000;
        request.Method = "HEAD";
        try {
          using (var response = (HttpWebResponse)request.GetResponse()) {
            //validUrls[url] = request.ContentLength;
            if (response.StatusCode != HttpStatusCode.OK) return;
            validUrls.Add(new Header { url = url, size = response.ContentLength }); // response.ContentLength;
          }
        } catch { }
      }

      Parallel.ForEach(urls, url => {
        lock (validUrls) processUrl(url);
      });

      Json.Serialize(@"c:\temp\validUrls.json", validUrls);
      File.WriteAllLines(@"c:\temp\allUrls.txt", Linq.Items<string>("SUM=" + validUrls.Sum(h => h.size).ToString()).Concat(urls));
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
