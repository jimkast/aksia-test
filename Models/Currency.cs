namespace TransactionApi.Models;

public class Currency
{

    public Currency() {
    }

    public Currency(string code, string symbol, string name, int fractionDigits) {
        Code = code;
        Symbol = symbol;
        Name = name;
        FractionDigits = fractionDigits;
    }      
    public required string Code { get; set; }
    public required string Symbol { get; set; }
    public required string Name { get; set; }
    public required int FractionDigits { get; set; }
}