namespace TaxCalculator.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using TaxCalculator.Domain;
using TaxCalculator.Domain.Entities;

public class TaxCalculatorDbContext : DbContext
{
    public TaxCalculatorDbContext(DbContextOptions<TaxCalculatorDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaxCalculation> TaxCalculations { get; set; }
    public DbSet<TaxBracket> TaxBrackets { get; set; }
    public DbSet<PostalCodeTaxCalculator> PostalCodeTaxCalculators { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaxBracket>().ToTable("TaxBrackets");
        modelBuilder.Entity<PostalCodeTaxCalculator>().ToTable("PostalCodeTaxCalculators");
        
        modelBuilder.Entity<TaxBracket>().HasData(
            new TaxBracket { Id = 1, LowerBound = 0, UpperBound = 8350, Rate = 0.10m },
            new TaxBracket { Id = 2, LowerBound = 8351, UpperBound = 33950, Rate = 0.15m },
            new TaxBracket { Id = 3, LowerBound = 33951, UpperBound = 82250, Rate = 0.25m },
            new TaxBracket { Id = 4, LowerBound = 82251, UpperBound = 171550, Rate = 0.28m },
            new TaxBracket { Id = 5, LowerBound = 171551, UpperBound = 372950, Rate = 0.33m },
            new TaxBracket { Id = 6, LowerBound = 372951, UpperBound = decimal.MaxValue, Rate = 0.35m }
        );

        modelBuilder.Entity<PostalCodeTaxCalculator>().HasData(
            new PostalCodeTaxCalculator { Id = 1, PostalCode = "7441", CalculatorType = "Progressive" },
            new PostalCodeTaxCalculator { Id = 2, PostalCode = "A100", CalculatorType = "FlatValue" },
            new PostalCodeTaxCalculator { Id = 3, PostalCode = "7000", CalculatorType = "FlatRate" },
            new PostalCodeTaxCalculator { Id = 4, PostalCode = "1000", CalculatorType = "Progressive" }
        );
    }
}