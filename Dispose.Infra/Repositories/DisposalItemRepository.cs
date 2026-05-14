using Dispose.Core.Models;
using Dispose.Core.Repositories.Abstractions;
using System.Collections.Concurrent;

namespace Dispose.Infra.Repositories;

public class DisposalItemRepository : IDisposalItemRepository
{
    private static readonly ConcurrentDictionary<Guid, DisposalItem> DisposalItems = new();

    public Task<IEnumerable<DisposalItem>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        var items = DisposalItems.Values
            .OrderBy(item => item.Name)
            .ToArray();

        return Task.FromResult<IEnumerable<DisposalItem>>(items);
    }

    public Task CreateAsync(
        IEnumerable<DisposalItem> items,
        CancellationToken cancellationToken)
    {
        foreach (var item in items)
        {
            var itemToSave = new DisposalItem
            {
                Id = item.Id == Guid.Empty
                    ? Guid.NewGuid()
                    : item.Id,

                Name = item.Name,
                Type = item.Type
            };

            DisposalItems[itemToSave.Id] = itemToSave;
        }

        return Task.CompletedTask;
    }
}