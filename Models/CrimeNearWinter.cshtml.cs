using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  
namespace crimes.Pages
{  
    public class CrimeNearWinterModel : PageModel  
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
						string sql = string.Format(@"SELECT table1.Year,October,November,December 
FROM (SELECT COUNT(Crimes.IUCR) as October,  Year  FROM Crimes LEFT JOIN Codes ON (Codes.IUCR = Crimes.IUCR)  WHERE CrimeDate LIKE '%-10-%' and PrimaryDesc LIKE 'ASSAULT' GROUP BY Crimes.Year)table1 JOIN
     (SELECT COUNT(Crimes.IUCR) as November, Year  FROM Crimes LEFT JOIN Codes ON (Codes.IUCR = Crimes.IUCR)  WHERE CrimeDate LIKE '%-11-%' and PrimaryDesc LIKE 'ASSAULT' GROUP BY Crimes.Year)table2 ON (table1.Year = table2.Year) JOIN
     (SELECT COUNT(Crimes.IUCR) as December, Year  FROM Crimes LEFT JOIN Codes ON (Codes.IUCR = Crimes.IUCR)  WHERE CrimeDate LIKE '%-12-%' and PrimaryDesc LIKE 'ASSAULT' GROUP BY Crimes.Year)table3 ON (table1.Year = table3.Year)
     ORDER BY table1.Year ASC");

						DataSet ds = DataAccessTier.DB.ExecuteNonScalarQuery(sql);

						foreach (DataRow row in ds.Tables["TABLE"].Rows)
						{
							Models.Crime m = new Models.Crime();

							m.Year = Convert.ToString(row["Year"]);
							m.October = Convert.ToString(row["October"]);
							m.November = Convert.ToString(row["November"]);
							m.December = Convert.ToString(row["December"]);
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