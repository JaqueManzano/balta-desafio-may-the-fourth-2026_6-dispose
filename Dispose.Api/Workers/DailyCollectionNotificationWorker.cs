using Dispose.Core.Services.Abstractions;

namespace Dispose.Api.Workers;

public class DailyCollectionNotificationWorker(
    ILogger<DailyCollectionNotificationWorker> logger,
    IServiceScopeFactory scopeFactory)
    : BackgroundService
{
    private static readonly TimeSpan Interval =
        TimeSpan.FromMinutes(1);

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "→ Daily Collection Notification Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation(
                    "→ Simulando verificação diária de coleta...");

                await DoWorkAsync(stoppingToken);

                logger.LogInformation(
                    $"→ Próxima simulação em {Interval} minuto...");
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "→ Erro ao executar worker diário");
            }

            await Task.Delay(
                Interval,
                stoppingToken);
        }
    }

    private async Task DoWorkAsync(
        CancellationToken cancellationToken)
    {
        using var scope =
            scopeFactory.CreateScope();

        var service =
            scope.ServiceProvider
                .GetRequiredService<IDailyCollectionNotificationService>();

        await service.SendTodayNotificationsAsync(
            cancellationToken);
    }
}