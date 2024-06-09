namespace TransactionApi.Controllers;

public class PaginatedResponse<T>(int total, int offset, IEnumerable<T> items)
{
    private readonly int total = total;
    private readonly int offset = offset;
    private readonly IEnumerable<T> items = items;

    public int Total => total;
    public int Offset => offset;
    public IEnumerable<T> Items => items;
}