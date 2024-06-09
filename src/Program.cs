using Microsoft.EntityFrameworkCore;
using TransactionApi;
using TransactionApi.Services;
using TransactionApi.Repo;
using TransactionApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<TransactionImport, TransactionImport>();
builder.Services.AddScoped<TransactionRepo, TransactionRepo>();
builder.Services.AddScoped<DbContext, AppDbContext>();
builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("TransactionsSqlConnString")));

builder.Services.AddSingleton<CurrencyDbRepo, CurrencyDbRepo>();
builder.Services.AddSingleton<ICurrencyRepo, CurrencyCacheRepo>(serviceLocator => 
    new CurrencyCacheRepo(serviceLocator.GetService<CurrencyDbRepo>().All().Result)
);
builder.Services.AddExceptionHandler<GlobalUnexpectedErrorHandler>();
builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseExceptionHandler();

// Init and fetch currencies from db
app.Services.GetService<ICurrencyRepo>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
