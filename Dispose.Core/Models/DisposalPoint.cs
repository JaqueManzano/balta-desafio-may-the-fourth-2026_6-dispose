using Dispose.Core.Enums;

namespace Dispose.Core.Models
{
    public class DisposalPoint
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public WasteType WasteType { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Address { get; set; } = string.Empty;

    }
}
