using CleaningSchedule.Ai.Providers;
using CleaningSchedule.Core.Enums;
using Dispose.Ai.Agents;
using Dispose.Ai.Providers.Abstractions;
using Dispose.Core.Agents.Abstractions;
using Dispose.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dispose.Ai;

public static class DependencyInjection
{
    public static IServiceCollection AddAgents(this IServiceCollection services)
    {
        services.AddKeyedTransient<IAgent<DisposeNotificationInput, string>, DisposeProximityNotificationAgent>(AgentType.DisposeProximityNotificationAgent);
        services.AddKeyedTransient<IAgent<string, IEnumerable<DisposalItem>>, WasteClassificationAgent>(AgentType.WasteClassificationAgent);
        services.AddKeyedTransient<IAgent<CollectionNotificationInput, string>, DailyCollectionNotificationAgent>(AgentType.DailyCollectionNotificationAgent);

        services.AddKeyedTransient<IPromptProvider, FilePromptProvider>(PromptProvider.File);
        return services;
    }
}