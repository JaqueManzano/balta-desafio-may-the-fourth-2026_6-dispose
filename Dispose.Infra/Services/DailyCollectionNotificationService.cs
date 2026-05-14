using CleaningSchedule.Core.Enums;
using Dispose.Core.Agents.Abstractions;
using Dispose.Core.Models;
using Dispose.Core.Repositories.Abstractions;
using Dispose.Core.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Dispose.Infra.Services;

public class DailyCollectionNotificationService(
    ICollectionScheduleRepository collectionScheduleRepository,

     [FromKeyedServices(AgentType.DailyCollectionNotificationAgent)]
    IAgent<CollectionNotificationInput, string>
        dailyCollectionNotificationAgent,
    INotificationService notificationService)
    : IDailyCollectionNotificationService
{
    public async Task SendTodayNotificationsAsync(
        CancellationToken cancellationToken)
    {
        var today = DateTime.Now.DayOfWeek;

        // Simulação do bairro do usuário
        var neighborhood = "Centro";

        var schedules =
            await collectionScheduleRepository
                .GetByDayOfWeekAsync(
                    neighborhood,
                    today,
                    cancellationToken);

        if (!schedules.Any())
            return;

        var wasteTypes = schedules
            .Select(x => x.WasteType)
            .Distinct()
            .ToList();

        var input =
            new CollectionNotificationInput
            {
                Neighborhood = neighborhood,
                DayOfWeek = today.ToString(),
                WasteTypes = wasteTypes
                    .Select(x => x.ToString())
                    .ToList()
            };

        var message =
            await dailyCollectionNotificationAgent
                .RunAsync(
                    input,
                    cancellationToken);

        await notificationService.SendAsync(
            message,
            cancellationToken);
    }
}