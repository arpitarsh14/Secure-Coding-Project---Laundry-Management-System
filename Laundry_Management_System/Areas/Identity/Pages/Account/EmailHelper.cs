using System.Net.Mail;

namespace Identity.Models
{
    public class EmailHelper
    {
        public async Task SendEmail(string userEmail,string subject, string confirmationLink)
        {
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("arpitarsh@outlook.com", "Pa04word@");
            client.Host = "smtp-mail.outlook.com";
            client.Port = 587;
            client.EnableSsl = true;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("arpitarsh@outlook.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Confirm your email";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = confirmationLink;


            try
            {
                client.Send(mailMessage);
                
            }
            catch (System.Exception ex)
            {
                // log exception
            }
           
        }
    }
}
