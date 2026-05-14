namespace Dispose.Core.Models
{
    public class DisposeNotificationInput
    {
        public IEnumerable<DisposalItem> DisposalItems { get; set; } = [];

        public IEnumerable<DisposalPoint> DisposalPoints { get; set; } = [];

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
  
}
