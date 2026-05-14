using CleaningSchedule.Core.Enums;
using Dispose.Core.Agents.Abstractions;
using Dispose.Core.Models;
using Dispose.Core.Repositories.Abstractions;
using Dispose.Core.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dispose.Infra.Services;

public class DisposalItemService(
    ILogger<DisposalItemService> logger,

    IDisposalItemRepository disposalItemRepository,

    [FromKeyedServices(AgentType.WasteClassificationAgent)]
    IAgent<string, IEnumerable<DisposalItem>> wasteClassificationAgent)

    : IDisposalItemService
{
    public async Task<IEnumerable<DisposalItem>> ClassifyAndCreateAsync(
        ClassifyDisposalItemRequest request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("• Classificando itens para descarte...");

        var items = await wasteClassificationAgent.RunAsync(
            request.Description,
            cancellationToken);

        if (!items.Any())
            return [];

        logger.LogInformation("• Persistindo itens classificados...");

        await disposalItemRepository.CreateAsync(
            items,
            cancellationToken);

        logger.LogInformation("• Itens persistidos com sucesso");

        return items;
    }
}