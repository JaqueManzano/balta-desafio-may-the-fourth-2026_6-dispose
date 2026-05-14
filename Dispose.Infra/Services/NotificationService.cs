using Dispose.Core.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace Dispose.Infra.Services
{
    public class NotificationService(
        ILogger<NotificationService> logger)
        : INotificationService
    {
        public Task SendAsync(
            string message,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "🔔 NOTIFICAÇÃO ENVIADA: {Message}",
                message);

            return Task.CompletedTask;
        }
    }
}
