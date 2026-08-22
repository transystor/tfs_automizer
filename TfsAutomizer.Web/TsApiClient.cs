using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace TfsAutomizer.Web;

/// <summary>
/// Лёгкий read-only клиент для внутреннего tsapi time-tracking расширения.
/// Сейчас это PoC-слой: читаем историю списаний и агрегат по пользователям.
/// </summary>
public sealed class TsApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly TfsOptions _options;

    public TsApiClient(HttpClient httpClient, IOptions<TfsOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<IReadOnlyList<TimeSheetEntryDto>> GetEntriesAsync(int workItemId, CancellationToken cancellationToken = default)
    {
        var uri = BuildUri("GetListDeltaByWorkitemIDMod", new Dictionary<string, string?>
        {
            ["WI_ID"] = workItemId.ToString(),
            ["CollectionString"] = _options.CollectionString
        });

        using var response = await _httpClient.GetAsync(uri, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var data = await JsonSerializer.DeserializeAsync<List<TimeSheetEntryDto>>(stream, JsonOptions, cancellationToken);
        return data ?? [];
    }

    public async Task<IReadOnlyList<OperatorTimeSummaryDto>> GetOperatorSummaryAsync(int workItemId, CancellationToken cancellationToken = default)
    {
        var uri = BuildUri("GetOperatorsListDeltaByWorkitemID", new Dictionary<string, string?>
        {
            ["WI_ID"] = workItemId.ToString(),
            ["CollectionString"] = _options.CollectionString
        });

        using var response = await _httpClient.GetAsync(uri, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var data = await JsonSerializer.DeserializeAsync<List<OperatorTimeSummaryDto>>(stream, JsonOptions, cancellationToken);
        return data ?? [];
    }

    private Uri BuildUri(string action, IReadOnlyDictionary<string, string?> query)
    {
        var baseUri = _options.TsApiBaseUrl?.TrimEnd('/');
        if (string.IsNullOrWhiteSpace(baseUri))
        {
            throw new InvalidOperationException("Не задан Tfs:TsApiBaseUrl.");
        }

        var builder = new UriBuilder($"{baseUri}/{action}");
        var parts = query
            .Where(x => !string.IsNullOrWhiteSpace(x.Value))
            .Select(x => $"{WebUtility.UrlEncode(x.Key)}={WebUtility.UrlEncode(x.Value)}");

        builder.Query = string.Join("&", parts);
        return builder.Uri;
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new HttpRequestException($"tsapi request failed: {(int)response.StatusCode} {response.ReasonPhrase}. Body: {body}");
    }
}
