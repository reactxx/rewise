using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace WikimediaProcessing {
  public class Wikimedia: IDisposable {
    private Stream input;
    private XmlReader xmlReader;
    private readonly XmlReaderSettings settings = new XmlReaderSettings() {
      ValidationType = ValidationType.None,
      ConformanceLevel = ConformanceLevel.Fragment
    };

    public Wikimedia(string filename) {
      input =  new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 100000000);
      xmlReader = XmlReader.Create(input, settings);
    }

    /// <summary>
    /// Reads articles one by one from an opened stream. The XML must be directly accessible from this stream
    /// </summary>
    public IEnumerable<WikimediaPage> Articles {
      get {
        if (xmlReader != null) {
          while (xmlReader.ReadToFollowing("page")) {
            if (xmlReader.NodeType == XmlNodeType.Element) {
              yield return ReadArticle(xmlReader);
            }
          }
        } else {
          using (var bh = new BinaryReader(input, Encoding.UTF8)) {
            while (input.Position != input.Length) {
              var json = bh.ReadString();

              var article = Json.Deserialize<WikimediaPage>(json);

              yield return article;
            }
          }
        }
      }
    }

    /// <summary>
    /// Read a single article from the XML dump
    /// </summary>
    /// <param name="x">Previously opened Xmlreader</param>
    /// <returns>One article containing the title and wiki-markdown + HTML content.</returns>
    private static WikimediaPage ReadArticle(XmlReader x) {
      x.ReadToFollowing("title");
      var title = x.ReadString();

      x.ReadToFollowing("text");
      var text = x.ReadElementContentAsString();

      return new WikimediaPage {
        Title = title,
        Text = text
      };
    }


    /// <summary>
    /// Writes a set of <see cref="WikimediaPage"/>s to disk in a simple binary format consisting of the article title and the plaintext contents.
    /// </summary>
    /// <param name="articles">A set of articles, probably from <see cref="ReadArticlesFromXmlDump"/></param>
    /// <param name="outputFilename">The filename into which articles should be saved</param>
    /// <returns>The number of articles written</returns>
    public static int WriteToDisk(IEnumerable<WikimediaPage> articles, string outputFilename) {
      var numberOfArticles = 0;
      JsonNew.SerializeEnum<WikimediaPage>(outputFilename, articles.identityEnum(a => ++numberOfArticles));
      //using (var bh = new JsonStreamWriter(outputFilename)) {
      //  foreach (var article in articles) {
      //    //var json = JsonConvert.SerializeObject(article);

      //    bh.Serialize(article);
      //    ++numberOfArticles;
      //  }
      //}
      return numberOfArticles;
    }

    public void Dispose() {
      input.Close();
      xmlReader.Close();
    }
  }
}
