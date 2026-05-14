namespace Dispose.Core.Services.Abstractions
{
    public interface IDailyCollectionNotificationService
    {
        Task SendTodayNotificationsAsync(
            CancellationToken cancellationToken);
    }
}
