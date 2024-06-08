using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using CsvHelper.Configuration.Attributes;

namespace TransactionApi.Models;

public class TransactionModel // : IValidatableObject
{
    private static readonly HashSet<string> ALLOWED_EXTENSIONS = new HashSet<string> { ".png", ".mp3", ".tiff", ".xls", ".pdf" };
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string ApplicationName { get; set; }
    public required string Email { get; set; }
    public string? Filename { get; set; }
    public string? Url { get; set; }
    public required DateOnly Inception { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public decimal? Allocation { get; set; }

    public IEnumerable<ValidationResult> Validate() // (ValidationContext validationContext)
    {
        if (ApplicationName.Length > 200)
        {
            yield return new ValidationResult(
                $"Application name {ApplicationName} should not exceed 200 characters.",
                [nameof(ApplicationName)]);
        }

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

        if (!Uri.TryCreate(Url, UriKind.Absolute, out _))
        {
            yield return new ValidationResult(
                $"Invalid (absolute) url '{Url}'.",
                [nameof(Url)]);
        }

        if (Inception.CompareTo(DateOnly.FromDateTime(DateTime.UtcNow)) >= 0)
        {
            yield return new ValidationResult(
                $"Transaction date '{Inception}' should be in the past.",
                [nameof(Inception)]);
        }

        if (Allocation < 0 || Allocation > 100)
        {
            yield return new ValidationResult(
                $"Allocation amount '{Allocation}' should be between 0 and 100'.",
                [nameof(Allocation)]);
        }
    }

    // public IEnumerable<ValidationResult> ValidateFilename(ValidationContext validationContext) {
    //     if (Filename != null && !ALLOWED_EXTENSIONS.Contains(Path.GetExtension(Filename).ToLower()))
    //     {
    //         yield return new ValidationResult(
    //             $"Invalid filename extension for filename: '{Filename}'.",
    //             [nameof(Filename)]);
    //     }
    // }

    // public IEnumerable<ValidationResult> ValidateUrl(ValidationContext validationContext) {
    //     if (Uri.TryCreate(Url, UriKind.Absolute, out _))
    //     {
    //         yield return new ValidationResult(
    //             $"Invalid (absolute) url '{Url}'.",
    //             [nameof(Url)]);
    //     }
    // }

    // public IEnumerable<ValidationResult> ValidateDate(ValidationContext validationContext) {
    //     if (Inception.CompareTo(DateOnly.FromDateTime(DateTime.UtcNow)) >= 0)
    //     {
    //         yield return new ValidationResult(
    //             $"Transaction date '{Inception}' should be in the past'.",
    //             [nameof(Inception)]);
    //     }
    // }
}