using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LinkShorter.Models
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger _logger;

        public EmailSender(IConfiguration configuration,
                           ILogger<EmailSender> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {

            Execute(email, subject, message).Wait();
            return Task.FromResult(0);
        }

        public static async Task<string> generateMailBody(string emailTemplatePath, ListDictionary replacements)
        {
            //get email template content
            //_logger.LogDebug("Starting reading email template from path: {0}", emailTemplatePath);
            string templateContent = await File.ReadAllTextAsync(emailTemplatePath);

            //replace template content by given values
            foreach(DictionaryEntry replacement in replacements)
            {
                templateContent.Replace(replacement.Key.ToString(), replacement.Value.ToString());
            }

            //return email template with replaced values
            return templateContent;
        }

        public async Task Execute(string email, string subject, string message)
        {
            try
            {
                //get email configuration parameters
                var smtpHost = Configuration.GetSection("Smtp").GetValue<string>("Server");
                int smtpPort = Configuration.GetSection("Smtp").GetValue<int>("Port");
                var smtpUserName = Configuration.GetSection("Smtp").GetValue<string>("UserName");
                var smtpPassword = Configuration.GetSection("Smtp").GetValue<string>("Password");
                var fromAddress = Configuration.GetSection("Smtp").GetValue<string>("FromAddress");
                var fromDisplayName = Configuration.GetSection("Smtp").GetValue<string>("FromDisplayName");

                //build email message
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromAddress, fromDisplayName);
                mailMessage.To.Add(new MailAddress(email));
                mailMessage.Subject = subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = message;



                //build smtp client
                var client = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUserName, smtpPassword),
                    EnableSsl = true
                };

                //send email
                await client.SendMailAsync(mailMessage);



            }
            catch (Exception ex)
            {
                _logger.LogError("There was an error on sending mail to: {0} . Exception message: {1}", email, ex.Message);
                throw new Exception("There was an error with mail server. Please try again later");
            }
        }
    }
}
