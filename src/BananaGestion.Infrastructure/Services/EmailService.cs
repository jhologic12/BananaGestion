using BananaGestion.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BananaGestion.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlContent)
    {
        var apiKey = _configuration["EmailSettings:SendGridApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine($"[Email Mock] To: {to}, Subject: {subject}");
            return;
        }

        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(
            _configuration["EmailSettings:FromEmail"],
            _configuration["EmailSettings:FromName"]);
        var toEmail = new EmailAddress(to);
        var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, null, htmlContent);

        var response = await client.SendEmailAsync(msg);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Email failed: {response.StatusCode}");
        }
    }

    public async Task SendTaskReminderAsync(string toEmail, string workerName, string taskName, string loteName, DateTime fecha)
    {
        var subject = $"Recordatorio: {taskName} - {fecha:dd/MM/yyyy}";
        var html = $@"
            <h2>Recordatorio de Tarea</h2>
            <p>Hola {workerName},</p>
            <p>Tienes una tarea pendiente:</p>
            <ul>
                <li><strong>Tarea:</strong> {taskName}</li>
                <li><strong>Lote:</strong> {loteName}</li>
                <li><strong>Fecha:</strong> {fecha:dd/MM/yyyy}</li>
            </ul>
            <p>Por favor, ingresa a la aplicación para registrarla.</p>
            <p>Saludos,<br/>BananaGestion</p>
        ";
        await SendEmailAsync(toEmail, subject, html);
    }

    public async Task SendLowStockAlertAsync(string toEmail, string productName, decimal currentStock, decimal minStock)
    {
        var subject = $"Alerta: Stock bajo de {productName}";
        var html = $@"
            <h2>Alerta de Inventario</h2>
            <p>El producto <strong>{productName}</strong> tiene stock bajo:</p>
            <ul>
                <li><strong>Stock Actual:</strong> {currentStock}</li>
                <li><strong>Stock Mínimo:</strong> {minStock}</li>
            </ul>
            <p>Por favor, gestionar la reposición.</p>
        ";
        await SendEmailAsync(toEmail, subject, html);
    }
}
