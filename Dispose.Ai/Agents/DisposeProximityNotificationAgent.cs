using CleaningSchedule.Core.Enums;
using Dispose.Ai.Providers.Abstractions;
using Dispose.Core.Agents.Abstractions;
using Dispose.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OllamaSharp;
using OllamaSharp.Models;

namespace Dispose.Ai.Agents;

public class DisposeProximityNotificationAgent
    : IAgent<DisposeNotificationInput, string>
{
    private const string AgentName = "DisposeProximityNotificationAgent";
    private const float Temperature = 0.1f;

    private readonly ILogger<DisposeProximityNotificationAgent> _logger;
    private readonly IPromptProvider _promptProvider;
    private readonly OllamaApiClient _client;

    public DisposeProximityNotificationAgent(
        ILogger<DisposeProximityNotificationAgent> logger,

        [FromKeyedServices(PromptProvider.File)]
        IPromptProvider promptProvider)
    {
        _logger = logger;
        _promptProvider = promptProvider;

        _client = OllamaClientFactory.Create();
    }

    public async Task<string> RunAsync(
        DisposeNotificationInput data,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "• Gerando notificação de proximidade...");

        var instructions = await _promptProvider.GetPromptAsync(
            AgentName,
            cancellationToken);

        var collectionPoints = string.Join(
            Environment.NewLine,
            data.DisposalPoints.Select(point =>
                $"""
                Nome: {point.Name}
                Tipo: {point.WasteType}
                Endereço: {point.Address}
                Latitude: {point.Latitude}
                Longitude: {point.Longitude}
                """));

        var disposalItems = string.Join(
            Environment.NewLine,
            data.DisposalItems.Select(item =>
                $"""
                Nome: {item.Name}
                Tipo: {item.Type}
                """));

        var prompt = $"""
                      {instructions}

                      Localização atual do usuário:
                      Latitude: {data.Latitude}
                      Longitude: {data.Longitude}

                      Itens cadastrados para descarte:
                      {disposalItems}

                      Pontos de coleta encontrados em um raio de 500 metros:
                      {collectionPoints}

                      Gere uma notificação amigável incentivando o descarte correto.

                      Informe:
                      - quais itens podem ser descartados
                      - quais pontos de coleta estão próximos
                      - quais tipos de descarte são aceitos
                      - uma mensagem curta e amigável
                      """;

        var finalResponse = string.Empty;

        await foreach (var chunk in _client.GenerateAsync(
                           new GenerateRequest
                           {
                               Prompt = prompt,
                               Options = new RequestOptions
                               {
                                   Temperature = Temperature
                               }
                           },
                           cancellationToken))
        {
            if (!string.IsNullOrWhiteSpace(chunk?.Response))
                finalResponse += chunk.Response;
        }

        _logger.LogInformation(
            "• Notificação gerada com sucesso");

        _logger.LogInformation("---");
        _logger.LogInformation(finalResponse);
        _logger.LogInformation("---");

        return finalResponse;
    }
}