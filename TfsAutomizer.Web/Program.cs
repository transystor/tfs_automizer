var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
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
        "Добавить конфиг подключения к TFS / Azure DevOps Server",
        "Добавить клиент для стандартного TFS API",
        "Добавить клиент для tsapi WorkItemFormTab",
        "Реализовать read-only PoC для истории списаний"
    },
    endpoints = new[]
    {
        "GET /health",
        "GET /poc/notes"
    }
}));

app.Run();
