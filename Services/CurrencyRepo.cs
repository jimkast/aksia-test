using TransactionApi.Models;

public class CurrencyRepo
{
    private readonly Dictionary<string, Currency> codeIndex = [];
    private readonly Dictionary<string, Currency> symbolIndex = [];


    public CurrencyRepo(IEnumerable<Currency> currencies) {
        foreach (var cur in currencies) {
            codeIndex.Add(cur.Code, cur);
            codeIndex.Add(cur.Symbol, cur);
        }
    }

    public IEnumerable<Currency> All()
    {
        return codeIndex.Values;
    }
    public Currency? SingleByCode(string code)
    {
        return codeIndex.GetValueOrDefault(code);
    }

    public Currency? SingleBySymbol(string symbol)
    {
        return symbolIndex.GetValueOrDefault(symbol);
    }
}