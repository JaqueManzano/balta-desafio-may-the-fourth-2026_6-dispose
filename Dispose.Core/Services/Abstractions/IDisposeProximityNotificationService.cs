namespace Dispose.Core.Services.Abstractions
{
    public interface IDisposeProximityNotificationService
    {
        Task SendAsync(CancellationToken cancellationToken);
    }
}
