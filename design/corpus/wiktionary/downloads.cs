using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

// from http://kaiko.getalp.org/about-dbnary/ontolex/latest/
public static class WiktDownlad {
  public static void download() {
    var urls = XElement.Load(Directory.GetCurrentDirectory() + @"\wiktionary\downloads.xml").
      Descendants("tr").
      Select(el => el.Elements().First().Element("a").Attribute("href").Value).
      ToArray();
    Parallel.ForEach(urls, new ParallelOptions { MaxDegreeOfParallelism = 4 }, url => {
      var destFn = Corpus.Dirs.wikiesDbnary + @"src\" + url;
      if (File.Exists(destFn)) return;
      new MyWebClient().DownloadFile($"http://kaiko.getalp.org/about-dbnary/ontolex/latest/{url}", destFn);
      Console.WriteLine(url);
    });
  }

  class MyWebClient : WebClient {
    protected override WebRequest GetWebRequest(Uri uri) {
      WebRequest w = base.GetWebRequest(uri);
      w.Timeout = 10 * 1000 * 3600; // 10 hours
      return w;
    }
  }

}
