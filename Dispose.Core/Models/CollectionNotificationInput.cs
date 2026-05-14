namespace Dispose.Core.Models;

public class CollectionNotificationInput
{
    public string Neighborhood { get; set; } = string.Empty;

    public string DayOfWeek { get; set; } = string.Empty;

    public IEnumerable<string> WasteTypes { get; set; } = [];
}