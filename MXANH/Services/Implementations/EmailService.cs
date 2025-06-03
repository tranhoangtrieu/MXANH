using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using MXANH.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using MXANH.Services.Implementations;
using System;

namespace MXANH.Services.Implementations
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
            {
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }

        public async Task SendEmailConfirmationAsync(string email, string callbackUrl)
        {
            var subject = "Xác nhận tài khoản";
            var htmlMessage = $@"
                <h2>Xác nhận tài khoản của bạn</h2>
                <p>Vui lòng xác nhận tài khoản của bạn bằng cách nhấp vào liên kết: <a href='{callbackUrl}'>Xác nhận</a></p>
                <p>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này.</p>
                <p>Trân trọng,</p>
                <p>Đội ngũ Hệ thống bán phế liệu</p>";

            await SendEmailAsync(email, subject, htmlMessage);
        }

        public async Task SendPasswordResetAsync(string email, string callbackUrl)
        {
            var subject = "Đặt lại mật khẩu";
            var htmlMessage = $@"
                <h2>Đặt lại mật khẩu của bạn</h2>
                <p>Vui lòng đặt lại mật khẩu của bạn bằng cách nhấp vào liên kết: <a href='{callbackUrl}'>Đặt lại mật khẩu</a></p>
                <p>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này.</p>
                <p>Trân trọng,</p>
                <p>Đội ngũ Hệ thống bán phế liệu</p>";

            await SendEmailAsync(email, subject, htmlMessage);
        }
    }
}

// Tạo token xác nhận email
//var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//var callbackUrl = Url.Action(
//    "ConfirmEmail",
//    "Account",
//    values: new { userId = user.Id, code },
//    protocol: Request.Scheme);
//await _emailService.SendEmailConfirmationAsync(model.Email, callbackUrl);