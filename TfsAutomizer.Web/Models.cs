namespace TfsAutomizer.Web;

/// <summary>
/// Упрощённая модель записи списанного времени по задаче.
/// </summary>
public sealed class TimeSheetEntryDto
{
    public int Id { get; set; }

    public string? AdUserId { get; set; }

    public DateTime? PeriodDate { get; set; }

    public DateTime? CreationDate { get; set; }

    public int Duration { get; set; }

    public string? TextComment { get; set; }

    public int? PeriodState { get; set; }
}

/// <summary>
/// Сводная информация по списаниям времени в рамках work item.
/// </summary>
public sealed class OperatorTimeSummaryDto
{
    public string? AdUserId { get; set; }

    public int Duration { get; set; }

    public int? PeriodState { get; set; }
}

/// <summary>
/// Запрос на чтение истории списаний по work item.
/// </summary>
public sealed class WorkItemTimeQuery
{
    public int WorkItemId { get; set; }

    public string? CollectionString { get; set; }
}
