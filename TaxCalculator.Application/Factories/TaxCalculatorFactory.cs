namespace TaxCalculator.Application.Factories;

using TaxCalculator.Application.Abstractions;
using TaxCalculator.Domain;
using TaxCalculator.Domain.Abstractions;
using TaxCalculator.Domain.Entities;

public class TaxCalculatorFactory : ITaxCalculatorFactory
{
    private readonly Dictionary<string, Func<ITaxCalculatorStrategy>> _calculatorCreators;

    public TaxCalculatorFactory(
        ITaxBracketRepository taxBracketRepository,
        IPostalCodeTaxCalculatorRepository postalCodeTaxCalculatorRepository)
    {
        var taxBrackets = taxBracketRepository.GetTaxBrackets();
        var postalCodeTaxCalculators = postalCodeTaxCalculatorRepository.GetPostalCodeTaxCalculators();

        _calculatorCreators = postalCodeTaxCalculators
            .ToDictionary(
                kvp => kvp.Key,
                kvp => GetCalculatorCreator(kvp.Value, taxBrackets)
            );
    }

    private Func<ITaxCalculatorStrategy> GetCalculatorCreator(string calculatorType, List<TaxBracket> taxBrackets)
    {
        return calculatorType switch
        {
            "Progressive" => () => new TaxBracketDecorator(new ProgressiveTaxStrategy(), taxBrackets),
            "FlatValue" => () => new FlatValueTaxStrategy(),
            "FlatRate" => () => new FlatRateTaxStrategy(),
            _ => throw new ArgumentException($"Unknown calculator type: {calculatorType}")
        };
    }

    public ITaxCalculatorStrategy Create(string postalCode)
    {
        if (_calculatorCreators.TryGetValue(postalCode, out var creator))
        {
            return creator();
        }

        throw new ArgumentException($"No tax calculator found for postal code: {postalCode}");
    }
}



