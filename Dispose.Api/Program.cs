using Dispose.Ai;
using Dispose.Api.Workers;
using Dispose.Core.Models;
using Dispose.Core.Services.Abstractions;
using Dispose.Infra;
using Microsoft.OpenApi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddControllers();
builder.Services.AddAgents();


builder.Services.AddHostedService<DailyCollectionNotificationWorker>();
builder.Services.AddHostedService<DisposeProximityNotificationWorker>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Collection Schedule API",
        Version = "v1",
        Description = "API notificação de pontos de coleta."
    });

    var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xml);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Collection Schedule API v1");
        options.RoutePrefix = "swagger";
    });
}

app.MapControllers();

// Permite que o usuário cadastre itens que ele possui para descarte
app.MapPost("/disposalItem/classify", async (
    ClassifyDisposalItemRequest request,
    IDisposalItemService service, 
    CancellationToken cancellationToken) =>
{
    var items = await service.ClassifyAndCreateAsync(request, cancellationToken);

    return Results.Ok(items);
});
app.Run();
