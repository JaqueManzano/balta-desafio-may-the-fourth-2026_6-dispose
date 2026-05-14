using Dispose.Core.Models;

namespace Dispose.Core.Repositories.Abstractions
{
    public interface IDisposalItemRepository
    {
        Task<IEnumerable<DisposalItem>> GetAllAsync(CancellationToken cancellationToken);
        Task CreateAsync(IEnumerable<DisposalItem> items, CancellationToken cancellationToken);
    }
}
