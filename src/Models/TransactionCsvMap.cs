using TransactionApi.Models;

public class TransactionCsvMap : CsvHelper.Configuration.ClassMap<TransactionCsvModel>
{
    public TransactionCsvMap()
    {
        Map(m => m.Id).Name("Id");
        Map(m => m.ApplicationName).Name("ApplicationName");
        Map(m => m.Email).Name("Email");
        Map(m => m.Filename).Name("Filename");
        Map(m => m.Url).Name("Url");
        Map(m => m.Inception).Name("Inception");
        Map(m => m.Amount).Name("Amount");
        Map(m => m.Allocation).Name("Allocation");
    }
}