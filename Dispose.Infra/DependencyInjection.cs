using CleaningSchedule.Infra.Repositories;
using Dispose.Core.Repositories.Abstractions;
using Dispose.Core.Services.Abstractions;
using Dispose.Infra.Repositories;
using Dispose.Infra.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Dispose.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IDailyCollectionNotificationService, DailyCollectionNotificationService>();
        services.AddScoped<IDisposalItemService, DisposalItemService>();
        services.AddScoped<IDisposalPointService, DisposalPointService>();
        services.AddScoped<IDisposeProximityNotificationService, DisposeProximityNotificationService>();


        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICollectionScheduleRepository, CollectionScheduleRepository>();
        services.AddScoped<IDisposalPointRepository, DisposalPointRepository>();
        services.AddScoped<IDisposalItemRepository, DisposalItemRepository>();

        return services;
    }
}