using Dispose.Core.Enums;

public class CollectionSchedule
{
    public Guid Id { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public WasteType WasteType { get; set; }
    public string Neighborhood { get; set; } = string.Empty;
}