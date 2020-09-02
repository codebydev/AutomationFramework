using AutomationFramework.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace AutomationFramework.Utilities
{
    static class Mailer
    {
        const string smtpServer = ApplicationConstants.smtpServer;
        const int smtpPort = ApplicationConstants.smtpPort;
        const string fromAddress = ApplicationConstants.fromAddress;
        const string password = ApplicationConstants.password;

        public static void SendMail(string toAddress, string subject, string body, string reportFileName)
        {
            SmtpClient client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(fromAddress, password),
                EnableSsl = true
            };

            MailMessage mail = new MailMessage();
            Attachment attachment = new Attachment(reportFileName, MediaTypeNames.Application.Octet);
            mail.Attachments.Add(attachment);

            string[] tos = toAddress.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string emailId in tos)
            {
                mail.To.Add(emailId);
            }
            mail.From = new MailAddress(fromAddress);
            mail.Body = body;
            mail.Subject = subject;
            mail.IsBodyHtml = true;

            try
            {
                client.Send(mail);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
