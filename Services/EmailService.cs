using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IntranetHub.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpServer = _config["EmailSettings:SmtpServer"];
                var fromEmail = _config["EmailSettings:FromEmail"];
                var fromName = _config["EmailSettings:FromName"];
                var username = _config["EmailSettings:Username"];
                var password = _config["EmailSettings:Password"];

                if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(fromEmail)) return;

                using var client = new SmtpClient(smtpServer, 587)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
            }
            catch { }
        }
    }
}
