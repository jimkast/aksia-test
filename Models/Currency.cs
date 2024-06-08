using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using CsvHelper.Configuration.Attributes;

namespace TransactionApi.Models;

public class Currency
{
    public Currency(string code, string symbol, string name) {
        this.Code = code;
        this.Symbol = symbol;
        this.Name = name;
    }
    
    public required string Code { get; set; }
    public required string Symbol { get; set; }
    public required string Name { get; set; }
}