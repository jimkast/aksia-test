using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TransactionApi.Models;

public class TransactionImport
{

    private async IAsyncEnumerable<IEnumerable<T>> Chunks<T>(int chunkSize, IAsyncEnumerable<T> stream)
    {
        int i = 0;
        List<T> buffer = new List<T>();
        await foreach (var chunk in stream) {
            if (i++ % chunkSize == 0) {
                buffer.Add(chunk);
            }
            yield return buffer;
        }
    }
    
    public async IAsyncEnumerable<TransactionModel> StreamRows(TextReader input)
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
            await foreach (TransactionCsvModel record in records)
            {
                i++;
                if (!record.Validate().Any())
                {
                    var model = record.ToModel();

                    if (!model.Validate().Any())
                    {
                        yield return model;
                    }
                    else
                    {
                        Console.WriteLine($"Error validating model {i} " + model);
                        Console.WriteLine(model.Validate().First().ErrorMessage);
                    }
                }
                else
                {
                    Console.WriteLine($"Error validating row {i} " + record);
                }
            }
        }
    }
}