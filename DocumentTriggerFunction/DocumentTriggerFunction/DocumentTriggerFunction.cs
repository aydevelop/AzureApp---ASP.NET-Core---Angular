using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace DocumentTriggerFunction
{
	public class DocumentTriggerFunction
	{
		[FunctionName("DocumentTriggerFunction")]
		public void Run(
			[BlobTrigger("files/{name}", Connection = "BlobConnectionString")] Stream blobSource,
			string name,
			ILogger log)
		{
			log.LogInformation($"Blob trigger is working with file: {name}");

			try
			{
				var (email, fileName) = GetDataFromFileName(name);
				if (!string.IsNullOrEmpty(email) && !string.IsNullOrWhiteSpace(fileName))
				{
					var bodyEmail = $"File with name {fileName} uploaded successfully";
					SendEmail(email, bodyEmail);
					log.LogInformation("Blob trigger worked successfully");
				}
			}
			catch (Exception ex)
			{
				log.LogError("Error of blob trigger: " + ex.Message);
			}
		}

		public (string fileName, string email) GetDataFromFileName(string fileName)
		{
			var result = new string[2];
			if (fileName.Contains(@"/"))
			{
				int lastIndex = fileName.IndexOf(@"/");

				result[0] = fileName.Substring(0, lastIndex);
				result[1] = fileName.Substring(lastIndex + 1);
			}

			return (result[0], result[1]);
		}

		public void SendEmail(string toEmail, string bodyEmail)
		{
			var subject = Environment.GetEnvironmentVariable("Subject");
			var fromEmailAddress = Environment.GetEnvironmentVariable("FromEmailAddress");
			var userSMTP = Environment.GetEnvironmentVariable("UserSMTP");
			var passwordSMTP = Environment.GetEnvironmentVariable("PasswordSMTP");
			var serverSMTP = Environment.GetEnvironmentVariable("ServerSMTP");
			int.TryParse(Environment.GetEnvironmentVariable("PortSMTP"), out int portSMTP);

			var message = new MailMessage();
			message.From = new MailAddress(fromEmailAddress);
			message.To.Add(new MailAddress(toEmail));
			message.Subject = subject;
			message.Body = bodyEmail;

			var client = new SmtpClient(serverSMTP, portSMTP)
			{
				Credentials = new NetworkCredential(userSMTP, passwordSMTP),
				EnableSsl = true
			};

			client.Send(message);
		}
	}
}
