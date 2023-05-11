namespace TaxCalculator.Domain;

using TaxCalculator.Domain.Abstractions;

public class FlatRateTaxStrategy: ITaxCalculatorStrategy
{
    private const decimal FlatRate = 0.175m;

    public decimal CalculateTax(decimal annualIncome)
    {
        return annualIncome * FlatRate;
    }
}