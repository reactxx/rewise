using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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

    //public static List<WordStemm> getStemms(string[] words, LangsLib.langs lang, int batchSize = 5000) {
    //  var res = new List<WordStemm>();
    //  getStemmsLow(words, lang, batchSize, stemms => {
    //    res.AddRange(stemms);
    //  });
    //  return res;
    //}

    public static bool isStemmedWord(string word, CultureInfo lc) {
      bool res = false;
      getStemms(word, (LangsLib.langs)lc.LCID, dbStems => {
        foreach (var stem in dbStems) {
          if (stem == null || stem.stemms == null) continue; ;
          var arr = stem.stemms.Split(',');
          if (arr.Length == 1)
            continue;
          var sourceTxt = stem.word.ToLower(lc);
          var sourcIdx = Array.IndexOf(arr, sourceTxt);
          if (sourcIdx < 0)
            continue;
          res = true;
        }
      });
      return res;
    }

    public static string getQuery(List<string> words, int? begIdx = null, int? len = null) {
      StringBuilder sb = new StringBuilder();
      for (var i = begIdx == null ? 0 : (int)begIdx; i < (len==null ? words.Count : (int) len); i++) {
        if (i != begIdx) sb.Append(',');
        var word = words[i];
        sb.Append(word);
      }
      sb.Replace("'", "''").Replace("\\", "\\\\").Replace("\"", "");
      return sb.ToString();
    }

    public static void getStemms(List<string> words, LangsLib.langs lang, int batchSize, OnStemmed onStemmed) {
      Parallel.ForEach(Intervals.intervals(words.Count, batchSize), inter => {
        getStemms(getQuery(words, inter.start, inter.end), lang, onStemmed);
      });
    }

    static void getStemms(string queryPar, LangsLib.langs lang, OnStemmed onStemmed) {
      DataSet ds = new DataSet();
      var query = string.Format("SELECT * FROM dbo.wordsStemms(N'{0}', {1}) ", queryPar, (int)lang);
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
