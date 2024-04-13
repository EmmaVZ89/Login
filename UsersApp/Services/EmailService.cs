namespace UsersApp.Services
{
    using UsersApp.Models;
    using MailKit.Security;
    using MimeKit.Text;
    using MimeKit;
    using MailKit.Net.Smtp;
    using UsersApp.Models.Email;

    public static class EmailService
    {
        private static string _Host = "smtp.gmail.com";
        private static int _Port = 587;

        private static string _FromName = "Emmanuel Zelarayán";
        private static string _Email = "emmanuelvictorzelarayan@gmail.com";
        private static string _Password = "oxjdmyloptxuzjdu";

        public static bool Send(EmailDTO emailDTO)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_FromName, _Email));
                email.To.Add(MailboxAddress.Parse(emailDTO.To));
                email.Subject = emailDTO.About;
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = emailDTO.Content
                };

                var smtp = new SmtpClient();
                smtp.Connect(_Host, _Port, SecureSocketOptions.StartTls);

                smtp.Authenticate(_Email, _Password);
                smtp.Send(email);
                smtp.Disconnect(true);

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
