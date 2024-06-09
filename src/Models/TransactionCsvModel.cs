using System.ComponentModel.DataAnnotations;

namespace TransactionApi.Models;

public class TransactionCsvModel
{
    public string? Id { get; set; }
    public string? ApplicationName { get; set; }
    public string? Email { get; set; }
    public string? Filename { get; set; }
    public string? Url { get; set; }
    public string? Inception { get; set; }
    public string? Amount { get; set; }
    public string? Allocation { get; set; }


    public (IEnumerable<ValidationResult>, TransactionModel?) ToModel(Func<string, string?> curreniesMap)
    {
        Guid? guid = null;
        if (!string.IsNullOrEmpty(Id)) {
            if (Guid.TryParse(Id, out var guidParsed))
            {
                guid = guidParsed;
            }
            else
            {
                return ([new ValidationResult($"Invalid guid format for Id '{Id}'.", [nameof(Id)])], null);
            }
        }
        if (string.IsNullOrEmpty(ApplicationName))
        {
            return ([new ValidationResult($"Missing mandatory ApplicationName field.", [nameof(ApplicationName)])], null);
        }
        if (string.IsNullOrEmpty(Email))
        {
            return ([new ValidationResult($"Missing mandatory Email field.", [nameof(Email)])], null);
        }
        if (string.IsNullOrEmpty(Amount))
        {
            return ([new ValidationResult($"Missing mandatory Amount field.", [nameof(Amount)])], null);
        }
        var (successParse, currencyString, amountParsed) = TryParseAmountWithCurrencySymbol(Amount);
        if (!successParse)
        {
            return ([new ValidationResult($"Invalid amount or currency '{Amount}'.", [nameof(Amount)])], null);
        }
        var currencyCode = curreniesMap(currencyString);
        if (string.IsNullOrEmpty(currencyCode))
        {
            return ([new ValidationResult($"Unknown currency symbol '{currencyString}'.", [nameof(Amount)])], null);
        }
        if (string.IsNullOrEmpty(Inception))
        {
            return ([new ValidationResult($"Missing mandatory Inception field.", [nameof(Inception)])], null);
        }
        if (string.IsNullOrEmpty(Allocation))
        {
            return ([new ValidationResult($"Missing mandatory Allocation field.", [nameof(Allocation)])], null);
        }
        if (!decimal.TryParse(Allocation, out var allocationParsed)) {
            return ([new ValidationResult($"Invalid decimal Allocation '{Allocation}'.", [nameof(Allocation)])], null);
        }
        return ([], new TransactionModel
        {
            Id = guid,
            ApplicationName = ApplicationName,
            Email = Email,
            Filename = Filename,
            Amount = amountParsed,
            Currency = currencyCode,
            Inception = DateOnly.ParseExact(Inception, "M/d/yyyy"),
            Allocation = allocationParsed,
            Url = Url,
        });
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