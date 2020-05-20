using System;  
using System.Collections.Generic;  
using System.Linq;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace crimes.Pages
{  
    public class HolidayInfoModel : PageModel  
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
						string sql = string.Format(@" SELECT CrimeDate, COUNT(CrimeDate) AS NumCrimes, ROUND ((CAST(COUNT(CrimeDate) AS float) /  ((SELECT AVG(CrimeAvg) FROM (SELECT CrimeDate, COUNT(CID) as CrimeAvg FROM Crimes WHERE Year = 2015 GROUP BY CrimeDate) MyTable)) * 100) - 100 , 2) AS AVGDifference
														FROM Crimes
														WHERE CrimeDate IN ('01/01/2015','01/19/2015','05/25/15','07/04/2015','09/07/2015','11/26/2015','12/25/2015')
														GROUP BY CrimeDate");

						DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);

						foreach (DataRow row in ds.Tables["TABLE"].Rows)
						{
							Models.Crime m = new Models.Crime();

							m.CrimeDate = Convert.ToString(row["CrimeDate"]);
							m.NumCrimes = Convert.ToInt32(row["NumCrimes"]);
							m.CrimePercent = Convert.ToDouble(row["AVGDifference"]);
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