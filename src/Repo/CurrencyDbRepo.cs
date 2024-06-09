using Microsoft.EntityFrameworkCore;
using TransactionApi.Models;

namespace TransactionApi.Repo;

public class CurrencyDbRepo(DbContext dbContext) : ICurrencyRepo
{
    private readonly DbContext dbContext = dbContext;

    public async Task<IEnumerable<Currency>> All()
    {
        return await dbContext.Set<Currency>().ToListAsync();
    }
    public async Task<Currency?> SingleByCode(string code)
    {
        return await dbContext.Set<Currency>().SingleOrDefaultAsync(x => x.Code == code);
    }

    public async Task<Currency?> SingleBySymbol(string symbol)
    {
        return await dbContext.Set<Currency>().SingleOrDefaultAsync(x => x.Symbol == symbol);
    }
}
