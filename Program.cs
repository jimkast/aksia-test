using Microsoft.EntityFrameworkCore;
using TransactionApi;
using TransactionApi.Services;
using TransactionApi.Repo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<TransactionImport, TransactionImport>();
builder.Services.AddScoped<TransactionRepo, TransactionRepo>();
builder.Services.AddSingleton<DbContext, AppDbContext>();
builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("TransactionsSqlConnString")));

builder.Services.AddSingleton<CurrencyDbRepo, CurrencyDbRepo>();
builder.Services.AddSingleton<ICurrencyRepo, CurrencyCacheRepo>(s => {
    return new CurrencyCacheRepo(s.GetService<CurrencyDbRepo>().All().Result);
    });
// builder.Services.AddSingleton<ICurrencyRepo, CurrencyCacheRepo>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Init and fetch currencies from db
app.Services.GetService<ICurrencyRepo>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

Console.WriteLine("aaaaa");