using TfsAutomizer.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TfsOptions>(builder.Configuration.GetSection(TfsOptions.SectionName));
builder.Services.AddHttpClient<TsApiClient>();
builder.Services.AddRouting();

var app = builder.Build();

app.MapGet("/", () => Results.Redirect("/health"));

app.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    service = "TfsAutomizer.Web",
    stage = "poc"
}));

app.MapGet("/poc/notes", () => Results.Ok(new
{
    message = "Базовый каркас TFS automizer поднят.",
    nextSteps = new[]
    {
        "Заполнить appsettings локальными значениями TFS",
        "Проверить auth для стандартного TFS API и tsapi",
        "Привязать чтение списка work items",
        "Подготовить безопасный write PoC для time tracking"
    },
    endpoints = new[]
    {
        "GET /health",
        "GET /poc/notes",
        "GET /poc/tsapi/entries/{workItemId}",
        "GET /poc/tsapi/operators/{workItemId}"
    }
}));

app.MapGet("/poc/tsapi/entries/{workItemId:int}", async (int workItemId, TsApiClient client, CancellationToken cancellationToken) =>
{
    var result = await client.GetEntriesAsync(workItemId, cancellationToken);
    return Results.Ok(result);
});

app.MapGet("/poc/tsapi/operators/{workItemId:int}", async (int workItemId, TsApiClient client, CancellationToken cancellationToken) =>
{
    var result = await client.GetOperatorSummaryAsync(workItemId, cancellationToken);
    return Results.Ok(result);
});

app.Run();
