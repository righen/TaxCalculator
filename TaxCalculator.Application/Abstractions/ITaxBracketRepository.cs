namespace TaxCalculator.Application.Abstractions;

using TaxCalculator.Domain.Entities;

public interface ITaxBracketRepository
{
    List<TaxBracket> GetTaxBrackets();
}