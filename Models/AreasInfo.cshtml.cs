using System;  
using System.Collections.Generic;  
using System.Linq;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace crimes.Pages
{  
    public class AreasInfoModel : PageModel  
    {  
        public List<Models.Crime> CrimeList { get; set; }
		public Exception EX { get; set; }
  
        public void OnGet()  
        {  
				  List<Models.Crime> crimes = new List<Models.Crime>();
					
					// clear exception:
					EX = null;
					
					try
					{
						string sql = string.Format(@" SELECT Areas.Area, AreaName,COUNT(CID) as NumCrimes, ROUND(COUNT(CID) / CAST((SELECT COUNT(CID) FROM Crimes) AS float) * 100, 2) as PercentTotal
														FROM Areas LEFT JOIN Crimes ON (Areas.Area = Crimes.Area)
														WHERE NOT Areas.Area LIKE '0'
														GROUP BY Areas.Area, AreaName
														Order By AreaName");

						DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);

						foreach (DataRow row in ds.Tables["TABLE"].Rows)
						{
							Models.Crime m = new Models.Crime();

							m.Area = Convert.ToString(row["Area"]);
							m.AreaName = Convert.ToString(row["AreaName"]);
							m.NumCrimes = Convert.ToInt32(row["NumCrimes"]);
							m.CrimePercent = Convert.ToDouble(row["PercentTotal"]);
							crimes.Add(m);
						}
					}
					catch(Exception ex)
					{
					  EX = ex;
					}
					finally
					{
					CrimeList = crimes;  
				  }
        }  
				
    }//class
}//namespace