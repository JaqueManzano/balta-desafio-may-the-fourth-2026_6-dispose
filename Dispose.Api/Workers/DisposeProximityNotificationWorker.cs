using Dispose.Core.Services.Abstractions;

namespace Dispose.Api.Workers;

public class DisposeProximityNotificationWorker(
    ILogger<DisposeProximityNotificationWorker> logger,
    IServiceScopeFactory scopeFactory)
    : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromHours(1);

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "→ Dispose Proximity Notification Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation(
                    "→ Simulando mudança de localização do usuário...");

                await DoWorkAsync(stoppingToken);

                logger.LogInformation(
                    "→ Próxima verificação em 10 minutos...");
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "→ Erro ao executar worker");
            }

            await Task.Delay(
                Interval,
                stoppingToken);
        }
    }

    private async Task DoWorkAsync(
        CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();

        var disposeProximityNotificationService =
            scope.ServiceProvider
                .GetRequiredService<IDisposeProximityNotificationService>();

        await disposeProximityNotificationService.SendAsync(
            cancellationToken);
    }
}