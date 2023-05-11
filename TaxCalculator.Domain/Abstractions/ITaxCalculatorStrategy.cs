namespace TaxCalculator.Domain.Abstractions;

public interface ITaxCalculatorStrategy
{
    decimal CalculateTax(decimal annualIncome);
}