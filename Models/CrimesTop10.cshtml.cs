using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace crimes.Pages
{  
    public class CrimesTop10Model : PageModel  
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
						string sql = string.Format(@"SELECT top 10 Crimes.IUCR,PrimaryDesc,SecondaryDesc, COUNT(Crimes.IUCR) as numCrimes,
													ROUND( (( CAST(COUNT(Crimes.IUCR) as float ) / (SELECT COUNT(CID)  FROM Crimes))*100 ), 2) as CrimePercent,
													ROUND((( SUM(CAST(Arrested AS INT))) / (CAST(COUNT(Arrested) AS FLOAT) )) * 100 , 2) as ArrestedPercent
													FROM Codes LEFT JOIN Crimes ON (Codes.IUCR = Crimes.IUCR)
													GROUP BY Crimes.IUCR,PrimaryDesc,SecondaryDesc
													ORDER BY COUNT(Crimes.IUCR) DESC");

						DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);

						foreach (DataRow row in ds.Tables["TABLE"].Rows)
						{
							Models.Crime m = new Models.Crime();

							m.IUCR = Convert.ToString(row["IUCR"]);
							m.PrimaryDesc = Convert.ToString(row["PrimaryDesc"]);
							m.SecondaryDesc = Convert.ToString(row["SecondaryDesc"]);
							m.NumCrimes = Convert.ToInt32(row["numCrimes"]);
							m.CrimePercent = Convert.ToDouble(row["CrimePercent"]);
							m.ArrestedPercent = Convert.ToDouble(row["ArrestedPercent"]);
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