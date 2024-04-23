using System.Net;
using System.Net.Mail;
namespace Blog.Services;
using OneSignalApi.Api;
using OneSignalApi.Client;
using OneSignalApi.Model;
public class EmailService
{
    public bool Send(
        string toName,
        string toEmail,
        string subject,
        string body,
        string fromName ="Equipe No",
        string fromEmail = "email@no.com"
    )
    {
        var smtpClient = 
            new SmtpClient(Blog.Configuration.Smtp.Host, Blog.Configuration.Smtp.Port);
        smtpClient.Credentials =
            new NetworkCredential(Blog.Configuration.Smtp.UserName, Blog.Configuration.Smtp.Password);
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpClient.EnableSsl = true;


        var mail = new MailMessage();
        mail.From = new MailAddress(fromEmail, fromName);
        mail.To.Add(new MailAddress(fromEmail, fromName)); // WE CAN ADD NEW PEOPLES TO SEND THIS EMAIL
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;
        try
        {
            smtpClient.Send(mail);
            return true;
        }
        catch
        {
            return false;
        }
    }
}