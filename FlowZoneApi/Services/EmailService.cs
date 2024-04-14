using System.Net.Mail;
using System.Net;

namespace FlowZoneApi.Services
{
    public class EmailService : IDisposable
    {
        private readonly SmtpClient _smtpClient;

        public EmailService()
        {
            _smtpClient = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("test2408097@outlook.com", "test@1234@"),
                EnableSsl = true,
            };
        }

        public async Task SendPasswordResetEmailAsync(string recipientEmail, string otp)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("test2408097@outlook.com"),
                Subject = "Password Reset OTP",
                Body = $"Your OTP for password reset is: {otp}",
                IsBodyHtml = false
            };
            mailMessage.To.Add(recipientEmail);

            try
            {
                await _smtpClient.SendMailAsync(mailMessage);
            }
            catch (SmtpException ex)
            {
                // Log the exception instead of writing to console
                Console.WriteLine($"Failed to send email: {ex.StatusCode} - {ex.Message}");
                throw; // Rethrow the exception for the caller to handle
            }
        }

        public void Dispose()
        {
            _smtpClient.Dispose();
        }
    }
}
