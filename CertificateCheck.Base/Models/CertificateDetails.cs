using System;
 namespace CertificateCheck.Base.Models;

	public struct CertificateDetails
	{
		public string Subject { get; set; }
		public string Issuer { get; set; }
		public string ExpirationDate { get; set; }
		public string EffectiveDate { get; set; }
		public string RequestUrl { get; set; }

		public int DaysToExpiration
		{
			get
			{

				DateTime.TryParse(ExpirationDate, out var ed);
				DateTime.TryParse(EffectiveDate, out var ef);
				//get days between two dates and return as int
				return (int)(ed.Date - ef.Date).TotalDays + 1;
			}
		}

		public string FaviconUri => @"http://www.google.com/s2/favicons?{RequestUrl}";
	}



