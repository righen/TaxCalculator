namespace TaxCalculator.Domain;

using TaxCalculator.Domain.Abstractions;
using TaxCalculator.Domain.Entities;

public class TaxBracketDecorator : ITaxCalculatorStrategy
{
    private readonly ITaxCalculatorStrategy _taxCalculator;

    public TaxBracketDecorator(ITaxCalculatorStrategy taxCalculator, List<TaxBracket> taxBrackets)
    {
        _taxCalculator = taxCalculator;

        if (_taxCalculator is ProgressiveTaxStrategy progressiveTaxStrategy)
        {
            progressiveTaxStrategy.TaxBrackets = taxBrackets;
        }
    }

    public decimal CalculateTax(decimal annualIncome)
    {
        return _taxCalculator.CalculateTax(annualIncome);
    }
}
