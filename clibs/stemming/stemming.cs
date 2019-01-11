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

    public static List<WordStemm> getStemms(string[] words, LangsLib.langs lang, int batchSize = int.MaxValue) {
      var res = new List<WordStemm>();
      getStemms(words, lang, stemms => {
        res.AddRange(stemms);
      }, batchSize);
      return res;
    }

    public static void getStemms(string[] words, LangsLib.langs lang, OnStemmed onStemmed, int batchSize = int.MaxValue) {
      var inters = Intervals.intervals(words.Length, batchSize).ToArray();
      var lc = LangsLib.Metas.Items[lang].lc;
      Parallel.ForEach(inters, inter => {
        StringBuilder sb = new StringBuilder();
        for (var i = inter.start; i < inter.end; i++) {
          if (i != inter.start) sb.Append(',');
          sb.Append(words[i]);
        }
        sb.Replace("'", "''").Replace("\\", "\\\\").Replace("\"", "");
        var wordList = sb.ToString(); // words.Skip(inter.skip).Take(inter.take).Aggregate((r, i) => r + "," + i).Replace("'", "''");
        //var wordList = words.Skip(inter.skip).Take(inter.take).Aggregate((r, i) => r + "," + i).Replace("'", "''");
        DataSet ds = new DataSet();
        //var query = string.Format("SELECT display_term FROM sys.dm_fts_parser (N'FORMSOF( FREETEXT, \"{0}\")', {1}, 0, 1) ", word, (int)lang);
        var query = string.Format("SELECT * FROM dbo.wordsStemms(N'{0}', {1}) ", wordList, (int)lang);
        using (var imp = new Impersonator.Impersonator("pavel", "LANGMaster", "zvahov88_"))
        using (SqlConnection subconn = new SqlConnection("data source=localhost\\SQLEXPRESS01;initial catalog=FulltextDesign;integrated security=True"))
        using (SqlDataAdapter adapter = new SqlDataAdapter { SelectCommand = new SqlCommand(query, subconn) })
          adapter.Fill(ds);
        lock (words)
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
      });
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
