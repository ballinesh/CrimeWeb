//
// One movie
//

namespace crimes.Models
{

  public class Crime
	{
	
		// data members with auto-generated getters and setters:
		public string IUCR { get; set; }
		public string PrimaryDesc { get; set; }
		public string SecondaryDesc {get; set;}
		public string AreaName{get; set;}
		public string Area{get;set;}
		public int NumCrimes { get; set; }
		public double ArrestedPercent {get;set;}
		public double CrimePercent {get;set;}
		public string CrimeDate{get;set;}
		public string Year{get;set;}
		public string October{get;set;}
		public string November{get;set;}
		public string December{get; set;}
	 
		
	
		// default constructor:
		public Crime()
		{ }
		
		// constructor:
		public Crime(string id, string primary, string secondary, int numCrimes, double crimePercent, double arrestedPercent)
		{
			 
            IUCR = id;
            PrimaryDesc = primary;
            SecondaryDesc = secondary;
            NumCrimes   = numCrimes;
            CrimePercent = crimePercent;
			ArrestedPercent = arrestedPercent;
				
		}
		
	}//class

}//namespace