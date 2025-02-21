using System.Net.Mail;
using System.Net;

namespace PRN222_Assignment_01.Service
{
    public interface IEmailService
    {
        bool SendEmail(string toEmail, string articleTitle, string author, string articleLink);
    }
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool SendEmail(string toEmail, string articleTitle, string author, string articleLink)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SMTPServer"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SMTPPort"]);
                var smtpUsername = _configuration["EmailSettings:SMTPUsername"];
                var smtpPassword = _configuration["EmailSettings:SMTPPassword"];
                var enableSSL = bool.Parse(_configuration["EmailSettings:EnableSSL"]);

                string emailBody = File.ReadAllText("wwwroot/NewArticleEmail.html")
                    .Replace("[ARTICLE_LINK]", articleLink)
                    .Replace("[ARTICLE_TITLE]", articleTitle)
                    .Replace("[AUTHOR]", author);

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    client.EnableSsl = enableSSL;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(smtpUsername),
                        Subject = "New Article Published: " + articleTitle,
                        Body = emailBody,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);
                    client.Send(mailMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
        }

}
