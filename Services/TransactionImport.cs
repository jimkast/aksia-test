using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TransactionApi.Models;

namespace TransactionApi.Services;

public class TransactionImport
{
    public async IAsyncEnumerable<TransactionModel> StreamRows(TextReader input, Func<string, string?> curreniesMap, List<ErrorRowResult> errorsAccumulator)
    {
        using (input)
        {
            using var csv = new CsvReader(input, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            });
            int i = 0;
            csv.Context.RegisterClassMap<TransactionCsvMap>();
            var records = csv.GetRecordsAsync<TransactionCsvModel>();
            await foreach (var record in records)
            {
                i++;
                var (errors, model) = record.ToModel(curreniesMap);
                if (!errors.Any() && model != null)
                {
                    errors = model.Validate();
                    if (!errors.Any())
                    {
                        yield return model;
                    }
                    else
                    {
                        Console.WriteLine($"Error validating model {i}.");
                        Console.WriteLine(errors.First().ErrorMessage);
                    }
                }
                else
                {
                    
                    errorsAccumulator.Add(new ErrorRowResult(i, errors.Select(e => e.ErrorMessage)));
                    Console.WriteLine($"Error validating row {i} " + record);
                }
                if (errors.Any()) {
                    foreach (var err in errors) {
                        errorsAccumulator.Add(new ErrorRowResult(i, errors.Select(e => e.ErrorMessage).Where(msg => msg != null).AsEnumerable<string>()));
                    }
                }
            }
        }
    }
}
