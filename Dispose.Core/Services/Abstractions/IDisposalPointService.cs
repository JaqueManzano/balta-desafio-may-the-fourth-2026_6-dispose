using Dispose.Core.Models;

namespace Dispose.Core.Services.Abstractions
{
    public interface IDisposalPointService
    {
        Task<IEnumerable<DisposalPoint>> GetNearbyAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken);
    }
}
