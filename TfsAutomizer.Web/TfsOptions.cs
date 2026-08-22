namespace TfsAutomizer.Web;

/// <summary>
/// Настройки подключения к TFS / Azure DevOps Server и внутреннему tsapi.
/// </summary>
public sealed class TfsOptions
{
    public const string SectionName = "Tfs";

    public string BaseUrl { get; set; } = string.Empty;

    public string TsApiBaseUrl { get; set; } = string.Empty;

    public string CollectionString { get; set; } = string.Empty;

    public string ProjectUri { get; set; } = string.Empty;

    public string DefaultAdUserId { get; set; } = string.Empty;

    public string DefaultCreatedById { get; set; } = string.Empty;
}
