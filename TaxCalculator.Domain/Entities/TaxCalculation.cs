namespace TaxCalculator.Domain.Entities;

public class TaxCalculation
{
    public int Id { get; set; }
    public string PostalCode { get; set; }
    public decimal AnnualIncome { get; set; }
    public decimal TaxAmount { get; set; }
    public DateTime CalculationDateTime { get; set; }
}