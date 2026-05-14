using CleaningSchedule.Core.Enums;
using Dispose.Ai.Providers.Abstractions;
using Dispose.Core.Agents.Abstractions;
using Dispose.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OllamaSharp;
using OllamaSharp.Models;
using System.Text.Json;

namespace Dispose.Ai.Agents;

public class WasteClassificationAgent
    : IAgent<string, IEnumerable<DisposalItem>>
{
    private const string AgentName = "WasteClassificationAgent";
    private const float Temperature = 0.1f;

    private readonly ILogger<WasteClassificationAgent> _logger;
    private readonly IPromptProvider _promptProvider;
    private readonly OllamaApiClient _client;

    public WasteClassificationAgent(
        ILogger<WasteClassificationAgent> logger,

        [FromKeyedServices(PromptProvider.File)]
        IPromptProvider promptProvider)
    {
        _logger = logger;
        _promptProvider = promptProvider;

        _client = OllamaClientFactory.Create();
    }

    public async Task<IEnumerable<DisposalItem>> RunAsync(
        string data,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("• Classificando itens para descarte...");

        var instructions = await _promptProvider.GetPromptAsync(
            AgentName,
            cancellationToken);

        var prompt = $"""
                      {instructions}

                      Entrada do usuário:
                      {data}
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

        finalResponse = finalResponse
        .Replace("```json", string.Empty)
        .Replace("```", string.Empty)
        .Trim();

        _logger.LogInformation("• Resposta gerada pela IA...");
        _logger.LogInformation("---");
        _logger.LogInformation(finalResponse);
        _logger.LogInformation("---");

        var items = JsonSerializer.Deserialize<IEnumerable<DisposalItem>>(
            finalResponse,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return items ?? [];
    }
}