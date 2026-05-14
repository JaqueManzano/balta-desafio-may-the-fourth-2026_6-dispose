using Dispose.Core.Enums;
using Dispose.Core.Models;
using Dispose.Core.Repositories.Abstractions;
using System.Collections.Concurrent;

namespace Dispose.Infra.Repositories;

public class DisposalPointRepository : IDisposalPointRepository
{
    private static readonly ConcurrentDictionary<Guid, DisposalPoint> DisposalPoints = new(
        new Dictionary<Guid, DisposalPoint>
        {
            [Guid.NewGuid()] = new()
            {
                Id = Guid.NewGuid(),
                Name = "Eco Ponto Centro",
                WasteType = WasteType.Electronic,
                Latitude = -22.3156,
                Longitude = -49.0601,
                Address = "Rua XV de Novembro"
            },

            [Guid.NewGuid()] = new()
            {
                Id = Guid.NewGuid(),
                Name = "Coleta de Vidro Norte",
                WasteType = WasteType.Glass,
                Latitude = -22.3189,
                Longitude = -49.0582,
                Address = "Av. Brasil"
            }
        });

    public Task<IEnumerable<DisposalPoint>> GetNearbyAsync(
        double latitude,
        double longitude,
        double radiusInMeters,
        CancellationToken cancellationToken)
    {
        var nearbyPoints = DisposalPoints.Values
            .Where(point =>
                CalculateDistanceInMeters(
                    latitude,
                    longitude,
                    point.Latitude,
                    point.Longitude) <= radiusInMeters)
            .ToArray();

        return Task.FromResult<IEnumerable<DisposalPoint>>(nearbyPoints);
    }

    private static double CalculateDistanceInMeters(
        double lat1,
        double lon1,
        double lat2,
        double lon2)
    {
        const double earthRadius = 6371000;

        var dLat = DegreesToRadians(lat2 - lat1);
        var dLon = DegreesToRadians(lon2 - lon1);

        var a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(DegreesToRadians(lat1)) *
            Math.Cos(DegreesToRadians(lat2)) *
            Math.Sin(dLon / 2) *
            Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return earthRadius * c;
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }
}