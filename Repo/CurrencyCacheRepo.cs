using TransactionApi.Models;

namespace TransactionApi.Repo;

public class CurrencyCacheRepo : ICurrencyRepo
{
    private readonly Dictionary<string, Currency> codeIndex = [];
    private readonly Dictionary<string, Currency> symbolIndex = [];


    public CurrencyCacheRepo(IEnumerable<Currency> currencies)
    {
        foreach (var cur in currencies)
        {
            codeIndex.Add(cur.Code, cur);
            symbolIndex.Add(cur.Symbol, cur);
        }
    }

    public Task<IEnumerable<Currency>> All()
    {
        return Task.FromResult(codeIndex.Values.AsEnumerable());
    }

    public Task<Currency?> SingleByCode(string code)
    {
        return Task.FromResult(codeIndex.GetValueOrDefault(code));
    }

    public Task<Currency?> SingleBySymbol(string symbol)
    {
        return Task.FromResult(symbolIndex.GetValueOrDefault(symbol));
    }
}
