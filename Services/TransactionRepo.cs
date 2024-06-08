using System.Linq.Expressions;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using TransactionApi.Models;

public class TransactionRepo
{
    private readonly DbContext dbContext;

    public TransactionRepo(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    private async IAsyncEnumerable<IEnumerable<T>> ChunkSplit<T>(int chunkSize, IAsyncEnumerable<T> stream)
    {
        int i = 0;
        List<T> buffer = new List<T>();
        await foreach (var chunk in stream)
        {
            buffer.Add(chunk);
            if (i++ % chunkSize == 0)
            {
                yield return buffer;
                buffer.Clear();
            }

        }
        if (buffer.Any())
        {
            yield return buffer;
        }
    }

    public async Task BulkMerge(IAsyncEnumerable<TransactionModel> transactions, int batchSize = 50)
    {
        await foreach (var chunk in ChunkSplit (batchSize, transactions)) {
            await BulkMerge(chunk, batchSize);
        }
    }
    public async Task BulkMerge(IEnumerable<TransactionModel> transactions, int batchSize = 50)
    {
        await dbContext.BulkInsertOrUpdateAsync(transactions, new BulkConfig { BatchSize = batchSize });
    }
    public async Task<TransactionModel?> SingleById(Guid id)
    {
        return await dbContext.Set<TransactionModel>().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<TransactionModel>> FetchManyPaginated(int page, int count, Expression<Func<TransactionModel, object>>? orderBy)
    {
        return await dbContext.Set<TransactionModel>().AsNoTracking().OrderBy(x => x.Inception).Take(count).ToListAsync();
    }
}