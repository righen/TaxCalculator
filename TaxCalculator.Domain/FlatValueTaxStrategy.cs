namespace TaxCalculator.Domain;

using TaxCalculator.Domain.Abstractions;

public class FlatValueTaxStrategy : ITaxCalculatorStrategy
{
    private const decimal FlatValue = 10000m;
    private const decimal LowerIncomeRate = 0.05m;
    private const decimal LowerIncomeThreshold = 200000m;

    public decimal CalculateTax(decimal annualIncome)
    {
        return annualIncome < LowerIncomeThreshold ? annualIncome * LowerIncomeRate : FlatValue;
    }
}