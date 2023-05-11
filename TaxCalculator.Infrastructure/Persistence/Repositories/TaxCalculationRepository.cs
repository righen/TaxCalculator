namespace TaxCalculator.Infrastructure.Persistence.Repositories;

using TaxCalculator.Application.Abstractions;
using TaxCalculator.Domain.Entities;

public class TaxCalculationRepository : ITaxCalculationRepository
{
    private readonly TaxCalculatorDbContext _context;

    public TaxCalculationRepository(TaxCalculatorDbContext context)
    {
        _context = context;
    }

    public async Task SaveTaxCalculationAsync(TaxCalculation taxCalculationEntity)
    {
        _context.TaxCalculations.Add(taxCalculationEntity);
        await _context.SaveChangesAsync();
    }
}