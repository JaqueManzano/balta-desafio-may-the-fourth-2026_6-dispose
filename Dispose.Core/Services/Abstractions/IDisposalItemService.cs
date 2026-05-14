using Dispose.Core.Models;

namespace Dispose.Core.Services.Abstractions
{
    public interface IDisposalItemService
    {
        Task<IEnumerable<DisposalItem>> ClassifyAndCreateAsync(ClassifyDisposalItemRequest request,
        CancellationToken cancellationToken);
    }
}
