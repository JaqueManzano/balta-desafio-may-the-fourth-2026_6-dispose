using CleaningSchedule.Core.Enums;
using Dispose.Core.Agents.Abstractions;
using Dispose.Core.Models;
using Dispose.Core.Repositories.Abstractions;
using Dispose.Core.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Dispose.Infra.Services;

public class DisposeProximityNotificationService(
    ILogger<DisposeProximityNotificationService> logger,
    INotificationService notificationService,

    IDisposalItemRepository disposalItemRepository,
    IDisposalPointService disposalPointService,

    [FromKeyedServices(AgentType.DisposeProximityNotificationAgent)]
    IAgent<DisposeNotificationInput, string> disposeProximityNotificationAgent)

    : IDisposeProximityNotificationService
{
    private readonly Random _random = new();

    public async Task SendAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "• Iniciando verificação de proximidade...");

        // Simula mudança de localização do usuário
        var latitude = -22.3156 + (_random.NextDouble() - 0.5) / 100;
        var longitude = -49.0601 + (_random.NextDouble() - 0.5) / 100;

        logger.LogInformation(
            "• Buscando itens cadastrados para descarte...");

        var disposalItems = await disposalItemRepository.GetAllAsync(
            cancellationToken);

        if (!disposalItems.Any())
        {
            logger.LogInformation(
                "• Nenhum item cadastrado");

            return;
        }

        logger.LogInformation(
            "• Buscando pontos de coleta próximos...");

        var nearbyPoints = await disposalPointService.GetNearbyAsync(
            latitude,
            longitude,
            cancellationToken);

        if (!nearbyPoints.Any())
        {
            logger.LogInformation(
                "• Nenhum ponto encontrado próximo");

            return;
        }

        // Filtra apenas pontos compatíveis com os itens
        var compatiblePoints = nearbyPoints
            .Where(point =>
                disposalItems.Any(item =>
                    item.Type == point.WasteType))
            .ToList();

        if (!compatiblePoints.Any())
        {
            logger.LogInformation(
                "• Nenhum ponto compatível encontrado");

            return;
        }

        var input = new DisposeNotificationInput
        {
            Latitude = latitude,
            Longitude = longitude,
            DisposalItems = disposalItems,
            DisposalPoints = compatiblePoints
        };

        logger.LogInformation(
            "• Gerando notificação...");

        var notification = await disposeProximityNotificationAgent.RunAsync(
            input,
            cancellationToken);

        await notificationService.SendAsync(
        notification,
        cancellationToken);

        logger.LogInformation("---");
        logger.LogInformation(notification);
        logger.LogInformation("---");

        logger.LogInformation(
            "• Processo finalizado");
    }
}