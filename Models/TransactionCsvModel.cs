using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace TransactionApi.Models;

public class TransactionCsvModel
{
    public Guid? Id { get; set; }
    public required string ApplicationName { get; set; }
    public required string Email { get; set; }
    public string? Filename { get; set; }
    public string? Url { get; set; }
    public required string Inception { get; set; }
    public required string Amount { get; set; }
    public decimal? Allocation { get; set; }

    public (IEnumerable<ValidationResult>, TransactionModel?) ToModel(Func<string, string?> curreniesMap)
    {
        var (successParse, currencyString, amountParsed) = TryParseAmountWithCurrencySymbol(Amount);
        if (!successParse) {
            return ([new ValidationResult(
                $"Invalid amount or currency '{Amount}'.",
                [nameof(Amount)])], null);
        }
        var currencyCode = curreniesMap(currencyString);
        if (currencyCode == null || currencyCode == "") {
            return ([new ValidationResult(
                $"Unknown currency symbol '{currencyString}'.",
                [nameof(Amount)])], null);
        }

        var result = new TransactionModel
        {
            Id = Id,
            ApplicationName = ApplicationName,
            Email = Email,
            Filename = Filename,
            Amount = amountParsed,
            Currency = currencyCode,
            Inception = DateOnly.ParseExact(Inception, "M/d/yyyy"),
            Allocation = Allocation,
            Url = Url,
        };
        return ([], result);
    }

    private static (bool, string, decimal) TryParseAmountWithCurrencySymbol(string amount)
    {
        var amountString = new string(amount.SkipWhile(c => !char.IsDigit(c)).ToArray());
        var currencyString = amount[..^amountString.Length].Trim();
        if (amountString.Length == 0 || currencyString.Length == 0 || !decimal.TryParse(amountString, out decimal amountParsed))
        {
            return (false, "", 0);
        }
        return (true, currencyString, amountParsed);
    }
}