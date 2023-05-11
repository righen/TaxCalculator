namespace TaxCalculator.Domain.Entities;

public class TaxBracket
{
    public int Id { get; set; }
    public decimal LowerBound { get; set; }
    public decimal UpperBound { get; set; }
    public decimal Rate { get; set; }
}