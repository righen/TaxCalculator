namespace TaxCalculator.Infrastructure.Persistence.Repositories;

using TaxCalculator.Application.Abstractions;
using TaxCalculator.Domain.Entities;

public class TaxBracketRepository : ITaxBracketRepository
{
    private readonly TaxCalculatorDbContext _context;

    public TaxBracketRepository(TaxCalculatorDbContext context)
    {
        _context = context;
    }

    public List<TaxBracket> GetTaxBrackets()
    {
        return _context.TaxBrackets.ToList();
    }
}