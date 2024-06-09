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
                }
                if (errors.Any()) {
                    errorsAccumulator.Add(new ErrorRowResult(i, errors.Select(e => e.ErrorMessage).Where(msg => msg != null).ToList()));
                }
            }
        }
    }
}
