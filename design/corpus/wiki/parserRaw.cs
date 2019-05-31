using System;
using System.Collections.Generic;
using System.Linq;
using WikimediaProcessing;

public static class WikiRawParser {

  public static void run(string inputFile, string outputFile) {
    var startTime = DateTime.Now;

    var wm = new Wikimedia(inputFile);

    IEnumerable<WikimediaPage> articles = wm.Articles
            .Where(article => !article.IsDisambiguation && !article.IsRedirect && !article.IsSpecialPage);

    var numberOfArticles = Wikimedia.WriteToDisk(articles, outputFile);
    Console.WriteLine("Wrote {0} articles to disk.", numberOfArticles);

    var endTime = DateTime.Now;

    TimeSpan processTime = endTime - startTime;
    Console.WriteLine("Process took " + processTime);
  }
}
