using Dispose.Core.Enums;

namespace Dispose.Core.Models
{
    public class DisposalItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public WasteType Type { get; set; }
    }
}
