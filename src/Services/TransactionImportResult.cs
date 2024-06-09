namespace TransactionApi.Services;

public class TransactionImportResult
{
    public DateTime StartedAt { get; set; }
    public int DurationMillis { get; set; }
    public int SuccessTotalRows { get; set; }
    public int ErrorTotalRows { get; set; }
    public IEnumerable<ErrorRowResult> Details { get; set; } = [];
}

public class ErrorRowResult(int ItemIndex, IEnumerable<string> ErrorMessages)
{
    private readonly int itemIndex = ItemIndex;

    private readonly IEnumerable<string> errorMessages = ErrorMessages;

    public int ItemIndex => itemIndex;

    public IEnumerable<string> ErrorMessages => errorMessages;
}
