using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using CsvHelper.Configuration.Attributes;

namespace TransactionApi.Models;

public class TransactionModel
{
    private static readonly HashSet<string> ALLOWED_EXTENSIONS = [".png", ".mp3", ".tiff", ".xls", ".pdf"];
    public Guid? Id { get; set; }
    public required string ApplicationName { get; set; }
    public required string Email { get; set; }
    public string? Filename { get; set; }
    public string? Url { get; set; }
    public required DateOnly Inception { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public decimal? Allocation { get; set; }

    public Guid GenerateId() {
        var guid = Guid.NewGuid();
        Id = Guid.NewGuid();
        return guid;
    }

    public IEnumerable<ValidationResult> Validate()
    {
        return ValidateName()
            .Concat(ValidateEmail())
            .Concat(ValidateFilename())
            .Concat(ValidateUrl())
            .Concat(ValidateDate())
            .Concat(ValidateAllocation());
    }

    private IEnumerable<ValidationResult> ValidateName() {
        if (ApplicationName.Length > 200)
        {
            yield return new ValidationResult(
                $"Application name {ApplicationName} should not exceed 200 characters.",
                [nameof(ApplicationName)]);
        }
    }
    
    private IEnumerable<ValidationResult> ValidateEmail() {
        MailAddress? mailAddressParsed = null;
        try
        {
            mailAddressParsed = new MailAddress(Email);

        }
        catch
        {
        }
        if (mailAddressParsed == null)
        {
            yield return new ValidationResult(
            $"Invalid email '{Email}'.",
            [nameof(Email)]);
        }
        else if (Email.Length > 200)
        {
            yield return new ValidationResult(
                $"Email {Email} should not exceed 200 characters.",
                [nameof(Email)]);
        }
    }


    private IEnumerable<ValidationResult> ValidateFilename() {
        if (Filename != null && Filename.Length > 300)
        {
            yield return new ValidationResult(
                $"Filename {Filename} should not exceed 300 characters.",
                [nameof(Filename)]);
        }
        else if (Filename != null && !ALLOWED_EXTENSIONS.Contains(Path.GetExtension(Filename).ToLower()))
        {
            yield return new ValidationResult(
                $"Invalid filename extension for filename: '{Filename}'.",
                [nameof(Filename)]);
        }
    }

    private IEnumerable<ValidationResult> ValidateUrl() {
        if (!Uri.TryCreate(Url, UriKind.Absolute, out _))
        {
            yield return new ValidationResult(
                $"Invalid (absolute) url '{Url}'.",
                [nameof(Url)]);
        }
    }

    private IEnumerable<ValidationResult> ValidateDate() {
        if (Inception.CompareTo(DateOnly.FromDateTime(DateTime.UtcNow)) >= 0)
        {
            yield return new ValidationResult(
                $"Transaction date '{Inception}' should be in the past.",
                [nameof(Inception)]);
        }
    }

    private IEnumerable<ValidationResult> ValidateAllocation() {
        if (Allocation < 0 || Allocation > 100)
        {
            yield return new ValidationResult(
                $"Allocation amount '{Allocation}' should be between 0 and 100'.",
                [nameof(Allocation)]);
        }
    }
}