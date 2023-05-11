namespace TaxCalculator.Application.Abstractions;

using TaxCalculator.Domain.Entities;

public interface ITaxCalculationRepository
{
    Task SaveTaxCalculationAsync(TaxCalculation taxCalculationEntity);
}