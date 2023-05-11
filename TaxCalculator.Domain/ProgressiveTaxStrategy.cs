namespace TaxCalculator.Domain;

using TaxCalculator.Domain.Abstractions;
using TaxCalculator.Domain.Entities;

public class ProgressiveTaxStrategy: ITaxCalculatorStrategy
{
    public List<TaxBracket> TaxBrackets { private get; set; }
    
    public decimal CalculateTax(decimal annualIncome)
    {
        if (TaxBrackets == null)
        {
            throw new InvalidOperationException("Tax brackets are not set.");
        }

        decimal tax = 0m;

        foreach (var bracket in TaxBrackets)
        {
            if (annualIncome > bracket.LowerBound)
            {
                decimal taxableIncomeInBracket = Math.Min(annualIncome, bracket.UpperBound) - bracket.LowerBound + 1;
                tax += taxableIncomeInBracket * bracket.Rate;
            }
            else
            {
                break;
            }
        }

        return tax;
    }
}