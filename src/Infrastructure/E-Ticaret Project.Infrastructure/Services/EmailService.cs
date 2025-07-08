﻿using System.Net.Mail;
using System.Net;
using E_Ticaret_Project.Application.Shared.Settings;
using Microsoft.Extensions.Options;
using E_Ticaret_Project.Application.Abstracts.Services;

namespace E_Ticaret_Project.Infrastructure.Services;

public class EmailService:IEmailService
{
    private EmailSettings _emailSettings { get; }

    public EmailService(IOptions<EmailSettings> options)
    {
        _emailSettings = options.Value;
    }

    public async Task SendEmailAsync(IEnumerable<string> toEmail, string subject, string body)
    {
        using var smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
        {
            Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password),
            EnableSsl = true
        };

        var message = new MailMessage
        {
            From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        foreach (var email in toEmail.Distinct())
            message.To.Add(email);


        await smtp.SendMailAsync(message);
    }
}
