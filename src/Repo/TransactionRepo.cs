using System.Linq.Expressions;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using TransactionApi.Models;

namespace TransactionApi.Repo;

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
        List<T> buffer = [];
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

    public async Task<int> BulkMerge(IAsyncEnumerable<TransactionModel> transactions, int batchSize = 50)
    {
        var count = 0;
        await foreach (var chunk in ChunkSplit(batchSize, transactions))
        {
            await BulkMerge(chunk, batchSize);
            count += chunk.Count();
        }
        return count;
    }
    public async Task BulkMerge(IEnumerable<TransactionModel> transactions, int batchSize = 50)
    {
        await dbContext.BulkInsertOrUpdateAsync(transactions.Select(t =>
        {
            if (t.Id == null) {
                t.GenerateId();
            }
            return t;
        }), new BulkConfig { BatchSize = batchSize });
    }
    public async Task<TransactionModel?> SingleById(Guid id)
    {
        return await dbContext.Set<TransactionModel>().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> DeleteById(Guid id)
    {
        int updated = await dbContext.Set<TransactionModel>().Where(x => x.Id == id).ExecuteDeleteAsync();
        return updated > 0;
    }

    public async Task<TransactionModel> Upsert(TransactionModel t)
    {
        if (t.Id == null)
        {
            dbContext.Add(t);
        }
        else
        {
            dbContext.Attach(t);
            dbContext.Update(t);
        }
        int updated = await dbContext.SaveChangesAsync();
        return t;
    }

    public async Task<(int, List<TransactionModel>)> FetchManyPaginated(int page, int count, Expression<Func<TransactionModel, object>>? orderBy = null)
    {

        return (
            await dbContext.Set<TransactionModel>().CountAsync(),
            page < 1 || count < 1 ?
                new List<TransactionModel>() :
                await dbContext.Set<TransactionModel>().AsNoTracking()
                    .OrderBy(orderBy ?? (x => x.Inception))
                    .Skip((page - 1) * count)
                    .Take(count)
                    .ToListAsync()
            );
    }
}