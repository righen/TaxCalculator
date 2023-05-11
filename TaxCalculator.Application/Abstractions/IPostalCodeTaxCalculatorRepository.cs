namespace TaxCalculator.Application.Abstractions;

public interface IPostalCodeTaxCalculatorRepository
{
    Dictionary<string, string> GetPostalCodeTaxCalculators();
}