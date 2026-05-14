using Dispose.Core.Models;
using Dispose.Core.Repositories.Abstractions;
using Dispose.Core.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace Dispose.Infra.Services;

public class DisposalPointService(
    ILogger<DisposalPointService> logger,
    IDisposalPointRepository disposalPointRepository)
    : IDisposalPointService
{
    private const double RadiusInMeters = 500;

    public async Task<IEnumerable<DisposalPoint>> GetNearbyAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "• Buscando pontos de coleta próximos...");

        var points = await disposalPointRepository.GetNearbyAsync(
            latitude,
            longitude,
            RadiusInMeters,
            cancellationToken);

        logger.LogInformation(
            "• {Count} pontos encontrados",
            points.Count());

        return points;
    }
}