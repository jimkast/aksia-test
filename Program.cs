using Microsoft.EntityFrameworkCore;
using TransactionApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<TransactionImport, TransactionImport>();
builder.Services.AddScoped<TransactionRepo, TransactionRepo>();
builder.Services.AddScoped<DbContext, AppDbContext>();
builder.Services.AddDbContext<AppDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("TransactionsSqlConnString")));
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
