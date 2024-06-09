namespace TransactionApi.Controllers;

public class PaginatedResponse<T> {

    private readonly int total;
    private readonly int offset;
    private readonly IEnumerable<T> items;

    public PaginatedResponse(int total, int offset, IEnumerable<T> items) {
        this.total = total;
        this.items = items;
        this.offset = offset;
    }

    public int Total => total;
    public int Offset => offset;
    public IEnumerable<T> Items => items;
}