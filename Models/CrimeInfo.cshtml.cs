using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace crimes.Pages
{  
    public class CrimeInfoModel : PageModel  
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
						string sql = string.Format(@" SELECT Codes.IUCR, PrimaryDesc,SecondaryDesc,COUNT(Crimes.IUCR) as NumCrimes
														FROM Codes LEFT JOIN Crimes on (Codes.IUCR = Crimes.IUCR)
														GROUP BY Codes.IUCR, PrimaryDesc,SecondaryDesc
														ORDER BY PrimaryDesc, SecondaryDesc");

						DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);

						foreach (DataRow row in ds.Tables["TABLE"].Rows)
						{
							Models.Crime m = new Models.Crime();

							m.IUCR = Convert.ToString(row["IUCR"]);
							m.PrimaryDesc = Convert.ToString(row["PrimaryDesc"]);
							m.SecondaryDesc = Convert.ToString(row["SecondaryDesc"]);
							m.NumCrimes = Convert.ToInt32(row["NumCrimes"]);
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