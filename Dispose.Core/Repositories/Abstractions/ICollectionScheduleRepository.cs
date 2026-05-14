namespace Dispose.Core.Repositories.Abstractions
{
    public interface ICollectionScheduleRepository
    {
        Task<IEnumerable<CollectionSchedule>> GetByDayOfWeekAsync(
        string Neighborhood,
        DayOfWeek dayOfWeek,
        CancellationToken cancellationToken);
    }
}
