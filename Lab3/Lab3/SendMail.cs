using System;
using System.Net;
using System.Net.Mail;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void SendMail (string data)
    {
        const string smtp_account = "vladislav.grishkevich@outlook.com";
        const string smtp_password = "secret_word_228";
        const string smtp_reciever = "vlad.grishkevich0503@gmail.com";

        var from_email = new MailAddress(smtp_account);
        var to_emal = new MailAddress(smtp_reciever);
        var message = new MailMessage(from_email, to_emal);
        message.Subject = "Changes in object";
        message.Body = data;

        var smtp = new SmtpClient
        {
            Host = "smtp.office365.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(from_email.Address, smtp_password),
        };

        try
        {
            smtp.Send(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return;
        }
    }
}
