using CertificateCheck.Base.Models;
using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace CertificateCheck.Base.Services
{
	public class CertService
	{
		public CertService()
		{
		}

		public async void Validate(string domain)
		{
			var clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback += ServerCertificateValidation;

			var client = new HttpClient(clientHandler);
			
			try
			{
				var response = await client.GetAsync(domain);
				response.EnsureSuccessStatusCode();
				var responseBody = await response.Content.ReadAsStringAsync();
				var a = clientHandler.ClientCertificates;
			}
			catch(HttpRequestException rex)
			{

			}
			clientHandler.Dispose();
			client.Dispose();

		}

		public X509Certificate GetCertificateFromDomain(string domain)
		{
			var clientHandler = new HttpClientHandler();
			clientHandler.ServerCertificateCustomValidationCallback += ServerCertificateValidation;

			var client = new HttpClient(clientHandler);
			
			try
			{
				var response = client.GetAsync(HttpUtility.UrlDecode(domain)).Result;
				response.EnsureSuccessStatusCode();
				string responseBody = response.Content.ReadAsStringAsync().Result;
				return clientHandler.ClientCertificates[0];
			}
			catch(HttpRequestException rex)
			{
				return null;
			}
			client.Dispose();
			clientHandler?.Dispose();
			
		}

		public CertificateDetails Validate2(string domain)
		{
			X509Certificate2? c = null;
			var handler = new HttpClientHandler
			{
				UseDefaultCredentials = true,

				ServerCertificateCustomValidationCallback = (sender, cert, chain, error) =>
				{

					/// Access cert object.
					c = new X509Certificate2(cert.GetRawCertData());
					return true;
					
				}
			};

			using (HttpClient client = new HttpClient(handler))
			{
				using (var response = client.GetAsync(HttpUtility.UrlDecode(domain)).Result)
				{
				}
			}

			var cd = new CertificateDetails()
			{
				ExpirationDate = c.GetExpirationDateString(),
				EffectiveDate = c.GetEffectiveDateString(),
				Subject = c.Subject,
				RequestUrl = HttpUtility.UrlDecode(domain)
				
			};
			return cd;
		}
			
		

		private bool ServerCertificateValidation(HttpRequestMessage? requestMessage, X509Certificate2? x509Certificate2, X509Chain? x509Chain, SslPolicyErrors sslPolicyErrors)
		{
			requestMessage.Properties.Add("Certificate", x509Certificate2);
			
			x509Certificate2.GetEffectiveDateString();
			x509Certificate2.GetExpirationDateString();
			_ = x509Certificate2.Issuer;
			_ = x509Certificate2.Subject;
			_ = sslPolicyErrors;
			return sslPolicyErrors == SslPolicyErrors.None;
		}
	}
}

