using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Scheduler
{
    public class Email
    {
        public void Send()
        {

            string subject = "Test Email from Reminder Service";

            string body = "Test message body";

            //using (MailMessage mm = new MailMessage(ConfigurationManager.AppSettings["FromEmail"], to))
            //{
                //mm.Subject = subject;
                //mm.Body = body;
                //mm.IsBodyHtml = false;
                //SmtpClient smtp = new SmtpClient();
                //smtp.Host = ConfigurationManager.AppSettings["Host"];
                //smtp.EnableSsl = true;
                //NetworkCredential NetworkCred = new NetworkCredential(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);

                string to = "mpurohit88@gmail.com";

                //It seems, your mail server demands to use the same email-id in SENDER as with which you're authenticating. 
                //string from = "sender@domain.com";
                string from = "reminder@somiconveyor.com";

                MailMessage message = new MailMessage(from, to, subject, body);
                SmtpClient client = new SmtpClient("mail.somiconveyor.com");
                SmtpClient smtpClient = new SmtpClient("host", 587);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
                client.Send(message);
            //}
        }
    }
}
