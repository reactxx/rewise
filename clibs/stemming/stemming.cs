using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
SELECT  *  FROM sys.dm_fts_parser (N'FORMSOF( FREETEXT, "koněm")', 1029, 0, 1)  
SELECT * FROM sys.dm_fts_parser (N'FORMSOF ( FREETEXT, "берлинский")', 1049, 0, 1)

use FulltextDesign
SELECT * FROM [dbo].[getStemms](N'KoněM',1029)


*/
namespace fulltext {
  public static class Stemming {

    //public static List<WordStemm> getStemms(string[] words, LangsLib.langs lang, int batchSize = int.MaxValue) {
    //  var res = new List<WordStemm>();
    //  getStemms(words, lang, batchSize, stemms => {
    //    res.AddRange(stemms);
    //  });
    //  return res;
    //}

    //public static void getStemmsWithBreaking(string[] words, LangsLib.langs lang, int batchSize, OnStemmed onStemmed) {
    //  var inters = Intervals.intervals(words.Length, batchSize).ToArray();
    //  //var comparer = StringComparer.Create(LangsLib.Metas.Items[lang].lc, true);
    //  var lc = LangsLib.Metas.Items[lang].lc;
    //  Parallel.ForEach(inters, inter => {
    //    StringBuilder sb = new StringBuilder();
    //    for (var i = inter.start; i < inter.end; i++) {
    //      if (i != inter.start) sb.Append(' ');
    //      sb.Append(words[i]);
    //    }
    //    var source = sb.ToString();
    //    sb.Clear();
    //    StemmerBreaker.Services.getService(lang).wordBreak(source, (type, pos, len) => {
    //      if (type != StemmerBreaker.PutTypes.put) return;
    //      if (sb.Length > 0) sb.Append(',');
    //      var word = source.Substring(pos, len).ToLower(lc);
    //      sb.Append(word);
    //    });
    //    getStemmsLow(inters, sb, lang, onStemmed);
    //  });
    //}

    public static void getStemms(List<string> words, LangsLib.langs lang, int batchSize, OnStemmed onStemmed) {
      //var inters = .ToArray();
      Parallel.ForEach(Intervals.intervals(words.Count, batchSize), inter => {
        StringBuilder sb = new StringBuilder();
        for (var i = inter.start; i < inter.end; i++) {
          if (i != inter.start) sb.Append(',');
          var word = words[i];
          sb.Append(word);
        }
        getStemmsLow(sb, lang, onStemmed);
      });
    }

    static void getStemmsLow(StringBuilder sb, LangsLib.langs lang, OnStemmed onStemmed) {
      sb.Replace("'", "''").Replace("\\", "\\\\").Replace("\"", "");
      var words = sb.ToString();
      DataSet ds = new DataSet();
      //var query = string.Format("SELECT display_term FROM sys.dm_fts_parser (N'FORMSOF( FREETEXT, \"{0}\")', {1}, 0, 1) ", word, (int)lang);
      var query = string.Format("SELECT * FROM dbo.wordsStemms(N'{0}', {1}) ", words, (int)lang);
      using (var imp = new Impersonator.Impersonator("pavel", "LANGMaster", "zvahov88_"))
      using (SqlConnection subconn = new SqlConnection("data source=localhost\\SQLEXPRESS01;initial catalog=FulltextDesign;integrated security=True"))
      using (SqlDataAdapter adapter = new SqlDataAdapter { SelectCommand = new SqlCommand(query, subconn) })
        adapter.Fill(ds);
      lock (onStemmed)
        //foreach (var r in ds.Tables[0].Rows.Cast<DataRow>())
        onStemmed(ds.Tables[0].Rows.Cast<DataRow>().Select(r => {
          try {
            return new WordStemm {
              word = (string)r["value"],
              stemms = r["stemms"] as string,
            };
          } catch {
            return null;
          }
        }));
    }

  }

  public delegate void OnStemmed(IEnumerable<WordStemm> x);

  public class WordStemm {
    public string word;
    public string stemms;
  }
}


/*
ALTER FUNCTION [dbo].[getStemms]
(  
   @word NVARCHAR(50),
   @lcid int
)  
RETURNS TABLE AS
RETURN  
   SELECT 
	    STRING_AGG (CAST(display_term AS NVARCHAR(MAX)) + IIF (expansion_type=0, CHAR (9), '' ), ',') WITHIN GROUP (ORDER BY display_term ASC) as stemms
	 FROM sys.dm_fts_parser (N'FORMSOF ( FREETEXT, "' + @word + '")', @lcid, 0, 1)

USE [FulltextDesign]
GO

ALTER FUNCTION[dbo].[splitWords]
(
  @words NVARCHAR(max)  
)  
RETURNS TABLE AS
RETURN

  SELECT value

  FROM STRING_SPLIT(@words, ',');

ALTER FUNCTION [dbo].[wordsStemms]
(  
   @words NVARCHAR(max),
   @lcid int
)  
RETURNS TABLE AS
RETURN  
	SELECT *
	FROM dbo.splitWords(@words) as W
    CROSS APPLY 
    dbo.getStemms(W.value, @lcid)

*/
