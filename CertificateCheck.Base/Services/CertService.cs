using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

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
				var response = client.GetAsync(domain).Result;
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


		//private Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> ServerCertificateValidation()
		//{
		//	throw new NotImplementedException();
		//}

		private bool ServerCertificateValidation(HttpRequestMessage? requestMessage, X509Certificate2? x509Certificate2, X509Chain? x509Chain, SslPolicyErrors sslPolicyErrors)
		{

			x509Certificate2.GetEffectiveDateString();
			x509Certificate2.GetExpirationDateString();
			_ = x509Certificate2.Issuer;
			_ = x509Certificate2.Subject;
			_ = sslPolicyErrors;
			return sslPolicyErrors == SslPolicyErrors.None;
		}
	}
}

