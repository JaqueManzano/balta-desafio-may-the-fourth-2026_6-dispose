using Dispose.Core.Models;

namespace Dispose.Core.Repositories.Abstractions;

public interface IDisposalPointRepository
{
    Task<IEnumerable<DisposalPoint>> GetNearbyAsync(
        double latitude,
        double longitude,
        double radiusInMeters,
        CancellationToken cancellationToken);
}