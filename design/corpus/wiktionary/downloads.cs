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
      new WebClient().DownloadFile($"http://kaiko.getalp.org/about-dbnary/ontolex/latest/{url}", Corpus.Dirs.wikiesDbnary + @"src\" + url);
      Console.WriteLine(url);
    });
  }
}
