using CleaningSchedule.Core.Enums;
using Dispose.Ai.Providers.Abstractions;
using Dispose.Core.Agents.Abstractions;
using Dispose.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OllamaSharp;
using OllamaSharp.Models;

namespace Dispose.Ai.Agents;

public class DailyCollectionNotificationAgent
    : IAgent<CollectionNotificationInput, string>
{
    private const string AgentName =
        "DailyCollectionNotificationAgent";

    private const float Temperature = 0.3f;

    private readonly ILogger<DailyCollectionNotificationAgent>
        _logger;

    private readonly IPromptProvider
        _promptProvider;

    private readonly OllamaApiClient
        _client;

    public DailyCollectionNotificationAgent(
        ILogger<DailyCollectionNotificationAgent> logger,

        [FromKeyedServices(PromptProvider.File)]
        IPromptProvider promptProvider)
    {
        _logger = logger;
        _promptProvider = promptProvider;

        _client = OllamaClientFactory.Create();
    }

    public async Task<string> RunAsync(
        CollectionNotificationInput data,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "• Gerando notificação diária de coleta...");

        var instructions =
            await _promptProvider.GetPromptAsync(
                AgentName,
                cancellationToken);

        var wasteTypes = string.Join(
            Environment.NewLine,
            data.WasteTypes.Select(type =>
                $"- {type}"));

        var prompt =
            $"""
             {instructions}

             Bairro do usuário:
             {data.Neighborhood}

             Dia da semana:
             {data.DayOfWeek}

             Tipos de coleta disponíveis hoje:
             {wasteTypes}

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
            if (!string.IsNullOrWhiteSpace(
                    chunk?.Response))
            {
                finalResponse += chunk.Response;
            }
        }

        _logger.LogInformation(
            "• Notificação diária gerada com sucesso");

        _logger.LogInformation("---");

        _logger.LogInformation(
            finalResponse);

        _logger.LogInformation("---");

        return finalResponse;
    }
}