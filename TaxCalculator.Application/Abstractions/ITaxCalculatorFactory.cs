namespace TaxCalculator.Application.Abstractions;

using TaxCalculator.Domain.Abstractions;

public interface ITaxCalculatorFactory
{
    ITaxCalculatorStrategy Create(string postalCode);
}
