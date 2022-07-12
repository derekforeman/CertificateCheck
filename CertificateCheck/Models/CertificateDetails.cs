using System;
namespace CertificateCheck.Models
{
    public struct CertificateDetails
    {
        public string Subject { get; set; }
        public string Issuer { get; set; }
        public string ExpirationDate { get; set; }
        public string EffectiveDate { get; set; }
        public string RequestUrl { get; set; }
    }

}

