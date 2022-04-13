using System.Net;
using System.Net.Mail;

Console.WriteLine("Hello, World!");
SendEmail("<h1>MESSSAGE</h1>");
//WriteToFile("G:\\data.txt", "data");
static void SendEmail(string htmlString)
{
    const string smtp_account = "vlad-0503@tut.by";
    const string smtp_password = "iygmsliccdxlltde";
    const string smtp_reciever = "vlad-0503@tut.by";

    var from_email = new MailAddress(smtp_account);
    var to_emal = new MailAddress(smtp_reciever);
    var message = new MailMessage(from_email, to_emal);
    message.Subject = "Hello!";
    message.Body = "How are you?";

    var smtp = new SmtpClient
    {
        Host = "smtp.yandex.com",
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


static void WriteToFile(string filename, string data)
{
    if(!File.Exists(filename))
    {
        using (StreamWriter sw = File.CreateText(filename))
        {
            sw.WriteLine(data);
        }
        return;
    }
    using(StreamWriter sw = File.AppendText(filename))
    {
        sw.WriteLine(data);
    }
}