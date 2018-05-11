using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EventRegistration.Mail
{
    public static class MailSender
    {
        public static void SendEmail(string from, string to, string subject, string body)
        {
            try
            {
                MailMessage message = new MailMessage(from, to);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("smtp.gmail.com");
                client.Port = 465;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("jjp4674@gmail.com", "BxxSxCp7#2");

                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendEmailMultipleTo(string from, List<string> to, string subject, string body, bool toAsBcc)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(from);

                foreach (var email in to)
                {
                    if (toAsBcc)
                    {
                        message.Bcc.Add(new MailAddress(email));
                    }
                    else
                    {
                        message.To.Add(new MailAddress(email));
                    }
                }

                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("smtp.gmail.com");
                client.Port = 465;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("jjp4674@gmail.com", "BxxSxCp7#2");

                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
