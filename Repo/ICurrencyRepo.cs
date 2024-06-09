using TransactionApi.Models;

namespace TransactionApi.Repo;

public interface ICurrencyRepo
{
    Task<IEnumerable<Currency>> All();
    Task<Currency?> SingleByCode(string code);
    Task<Currency?> SingleBySymbol(string symbol);

    Func<string, string?> AsSymbolIndexFunc() => x => SingleBySymbol(x).Result?.Code;
}
