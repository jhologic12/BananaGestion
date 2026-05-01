namespace BananaGestion.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlContent);
    Task SendTaskReminderAsync(string toEmail, string workerName, string taskName, string loteName, DateTime fecha);
    Task SendLowStockAlertAsync(string toEmail, string productName, decimal currentStock, decimal minStock);
}
