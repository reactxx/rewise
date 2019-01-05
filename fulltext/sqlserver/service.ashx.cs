using System;
using System.Configuration;
using System.Net.Mail;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace fulltext
{
  public class IGetStemGroupsItem
  {
    public int occurrence;
    public string display_term;
    public int expansion_type;
  }
  //public class IGetStemGroupsOUT : services.serviceOUT {
  //  public IGetStemGroupsItem[] items;
  //}
  //public class IGetStemGroupsIN {
  //  public int lcid;
  //  public string[] texts;
  //}

  public class Stemmer : IHttpHandler {

    static Stemmer() {

    }

    public void ProcessRequest(HttpContext context) {
      //processRequest.run<IGetStemGroupsIN, IGetStemGroupsOUT>(context, inPar => {
        try {
          DataSet ds = new DataSet();
          using (var imp = new Impersonator.Impersonator("pavel", "LANGMaster", "zvahov88_"))
          using (SqlConnection subconn = new SqlConnection(""))
          using (SqlDataAdapter adapter = new SqlDataAdapter { SelectCommand = new SqlCommand("", subconn) })
            adapter.Fill(ds);
          var items = ds.Tables[0].Rows.Cast<DataRow>().Select(r => new IGetStemGroupsItem {
            display_term = (string)r["display_term"],
            expansion_type = (int)r["expansion_type"],
            occurrence = (int)r["occurrence"]
          });
          //return new IGetStemGroupsOUT { items = items.ToArray() };
        } catch (Exception ex) {
          //return new IGetStemGroupsOUT { error = "Exception: " + ex.Message };
        }
      //});
    }

    public bool IsReusable { get { return true; } }
  }
}


