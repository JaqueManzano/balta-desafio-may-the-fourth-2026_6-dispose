using Dispose.Core.Enums;
using Dispose.Core.Repositories.Abstractions;
using System.Collections.Concurrent;

namespace CleaningSchedule.Infra.Repositories;

public class CollectionScheduleRepository : ICollectionScheduleRepository
{
    private static readonly ConcurrentDictionary<Guid, CollectionSchedule> CollectionSchedules = new(
        new Dictionary<Guid, CollectionSchedule>
        {
            [Guid.NewGuid()] = new()
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Monday,
                WasteType = WasteType.Organic,
                Neighborhood = "Centro"
            },

            [Guid.NewGuid()] = new()
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Tuesday,
                WasteType = WasteType.Recyclable,
                Neighborhood = "Centro"
            },

            [Guid.NewGuid()] = new()
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Wednesday,
                WasteType = WasteType.Plastic,
                Neighborhood = "Centro"
            },

            [Guid.NewGuid()] = new()
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Thursday,
                WasteType = WasteType.Glass,
                Neighborhood = "Centro"
            },

            [Guid.NewGuid()] = new()
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Friday,
                WasteType = WasteType.Metal,
                Neighborhood = "Centro"
            },

            [Guid.NewGuid()] = new()
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Saturday,
                WasteType = WasteType.Electronic,
                Neighborhood = "Centro"
            }
        });
    
    public Task<IEnumerable<CollectionSchedule>> GetByDayOfWeekAsync(
        string Neighborhood,
        DayOfWeek dayOfWeek,
        CancellationToken cancellationToken)
    {
        var schedules = CollectionSchedules.Values
            .Where(schedule => schedule.DayOfWeek == dayOfWeek 
            && schedule.Neighborhood == Neighborhood)
            .OrderBy(schedule => schedule.WasteType)
            .ToArray();

        return Task.FromResult<IEnumerable<CollectionSchedule>>(schedules);
    }
}