using Microsoft.EntityFrameworkCore;
using TransactionApi.Models;

namespace TransactionApi;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<TransactionModel> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var trans = modelBuilder.Entity<TransactionModel>();
        trans.ToTable("transactions").HasKey(o => o.Id);
        trans.Property(o => o.Id).ValueGeneratedOnAdd().IsRequired().HasColumnName("Id");
        trans.Property(o => o.ApplicationName).IsRequired().HasColumnName("ApplicationName");
        trans.Property(o => o.Email).IsRequired().HasColumnName("Email");
        trans.Property(o => o.Filename).HasColumnName("Filename");
        trans.Property(o => o.Url).HasColumnName("Url");
        trans.Property(o => o.Inception).IsRequired().HasColumnName("Inception");
        trans.Property(o => o.Amount).IsRequired().HasColumnName("Amount").HasPrecision(14, 2);
        trans.Property(o => o.Currency).IsRequired().HasColumnName("Currency");
        trans.Property(o => o.Allocation).HasColumnName("Allocation").HasPrecision(7, 2);
    }
}