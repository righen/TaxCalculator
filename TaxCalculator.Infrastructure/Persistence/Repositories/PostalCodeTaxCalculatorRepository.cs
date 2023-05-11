namespace TaxCalculator.Infrastructure.Persistence.Repositories;

using TaxCalculator.Application.Abstractions;

public class PostalCodeTaxCalculatorRepository : IPostalCodeTaxCalculatorRepository
{
    private readonly TaxCalculatorDbContext _context;

    public PostalCodeTaxCalculatorRepository(TaxCalculatorDbContext context)
    {
        _context = context;
    }

    public Dictionary<string, string> GetPostalCodeTaxCalculators()
    {
        return _context.PostalCodeTaxCalculators
                       .ToDictionary(p => p.PostalCode, p => p.CalculatorType);
    }
}