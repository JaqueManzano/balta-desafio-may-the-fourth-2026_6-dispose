namespace Dispose.Core.Services.Abstractions
{
    public interface INotificationService
    {
        Task SendAsync(
            string message,
            CancellationToken cancellationToken);
    }
}
