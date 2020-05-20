using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.AspNetCore.Mvc.RazorPages;  
using System.Data;
  

namespace crimes.Pages
{  
    public class AreaTop10Model : PageModel  
    {  
        public List<Models.Crime> CrimeList { get; set; }
		public Exception EX { get; set; }
		public String Input{get;set;}
		public string AreaNumber{get;set;}
		public string AreaNameString{get;set;}
		
        public void OnGet(String input)  
        {  
				  List<Models.Crime> crimes = new List<Models.Crime>();
					string sql = "";
					int id = 0;
					// clear exception:
					EX = null;
					Input = input;
					
					try
					{
						
						//
						// Do we have an input argument?  If so, we do a lookup:
						//
						if (input == null)
						{
							//
							// there's no page argument, perhaps user surfed to the page directly?  
							// In this case, nothing to do.
							//
						}
						else  
						{
							// 
							// Lookup movie(s) based on input, which could be id or a partial name:
							// 
							

							if (System.Int32.TryParse(input, out id))
							{
								// lookup movie by movie id:
								sql = string.Format(@"SELECT top 10 Crimes.IUCR,PrimaryDesc,SecondaryDesc, COUNT(Crimes.IUCR) as numCrimes,
													ROUND((( CAST(COUNT(Crimes.IUCR) as float ) / (SELECT COUNT(CID)  FROM Crimes WHERE Area = {0}))*100 ) , 2) as CrimePercent,
													((SUM(CAST(Arrested AS INT))) / (CAST(COUNT(Arrested) AS FLOAT) )) * 100 as ArrestedPercent,AreaName,Areas.Area
													FROM Crimes RIGHT JOIN Codes ON (Codes.IUCR = Crimes.IUCR) RIGHT JOIN Areas ON (Areas.Area = Crimes.Area)
													WHERE AREA = {0}
													GROUP BY Crimes.IUCR,PrimaryDesc,SecondaryDesc,Areas.Area
													ORDER BY COUNT(Crimes.IUCR) DESC", id);
													
							}
							else
							{
								// lookup movie(s) by partial name match:
								input = input.Replace("'", "''");

								sql = string.Format(@"SELECT top 10 Crimes.IUCR,PrimaryDesc,SecondaryDesc, COUNT(Crimes.IUCR) as numCrimes,
													ROUND((( CAST(COUNT(Crimes.IUCR) as float ) / (SELECT COUNT(CID)  FROM Crimes WHERE AreaName LIKE '{0}' ))*100 ) , 2) as CrimePercent,
													((SUM(CAST(Arrested AS INT))) / (CAST(COUNT(Arrested) AS FLOAT) )) * 100 as ArrestedPercent,AreaName,Areas.Area
													FROM Crimes RIGHT JOIN Codes ON (Codes.IUCR = Crimes.IUCR) RIGHT JOIN Areas ON (Areas.Area = Crimes.Area)
                                                    WHERE AreaName LIKE '{0}'
													GROUP BY Crimes.IUCR,PrimaryDesc,SecondaryDesc,AreaName,Areas.Area
													ORDER BY COUNT(Crimes.IUCR) DESC", input);
							}
						}

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
							AreaNameString = Convert.ToString(row["AreaName"]);
							AreaNumber = Convert.ToString(row["Area"]);
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