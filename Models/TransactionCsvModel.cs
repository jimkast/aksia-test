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

    public IEnumerable<ValidationResult> Validate()
    {
        if (Amount.Length < 1 || !decimal.TryParse(Amount.AsSpan(1).Trim(), out _))
        {
            yield return new ValidationResult(
                $"Invalid amount field '{Amount}'.",
                [nameof(Amount)]);
        }
    }

    public TransactionModel ToModel()
    {
        var currencySymbol = Amount[..1];
        decimal amountParsed = decimal.Parse(Amount.Substring(1));
        var result = new TransactionModel
        {
            // Id = Id,
            ApplicationName = ApplicationName,
            Email = Email,
            Filename = Filename,
            Amount = amountParsed,
            Currency = currencySymbol,
            Inception = DateOnly.ParseExact(Inception, "M/d/yyyy"),
            Allocation = Allocation,
            Url = Url,
        };
        if (Id != null && Id.HasValue) {
            result.Id = Id.Value;
        }
        return result;
    }

    private (bool, string, decimal) TryParseAmountWithCurrencySymbol(string amount)
    {
        if (amount.Length < 1 || !decimal.TryParse(Amount.AsSpan(1).Trim(), out decimal amountParsed))
        {
            return (false, "", 0);
        }
        return (true, amount[..1], amountParsed);
    }
}