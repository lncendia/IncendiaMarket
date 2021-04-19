using System.Net;
using System.Net.Mail;

namespace WebApplication.Data.Repository
{
    public class EmailService
    {
        public void SendMessage(string userName,string email, string message)
        {
            MailMessage mail = new MailMessage {From = new MailAddress("incendia.market@mail.ru")};
            // Адрес отправителя
            mail.To.Add(email); // Адрес получателя
            mail.Subject = userName;
            mail.Body = message;
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.mail.ru";
            client.Port = 587;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("incendia.market@mail.ru", "321_Lagor_321");
            client.Send(mail);
        }
    }
}