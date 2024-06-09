using TransactionApi.Models;

namespace TransactionApi.Services;

public class TransactionImportResult
{
    public DateTime StartedAt { get; set; }
    public int Millis { get; set; }
    public int SuccessTotalRows { get; set; }
    public int ErrorTotalRows { get; set; }
    public IEnumerable<ErrorRowResult> Details { get; set; } = [];
}

public class ErrorRowResult(int LineNumber, IEnumerable<string> ErrorMessages)
{
    private readonly int lineNumber = LineNumber;

    private readonly IEnumerable<string> errorMessages = ErrorMessages;

    public int LineNumber => lineNumber;

    public IEnumerable<string> ErrorMessages => errorMessages;
}
