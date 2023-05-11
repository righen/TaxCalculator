using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TaxCalculator.UI;

public class TaxCalculatorModel : PageModel
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;


    public TaxCalculatorModel(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
    {
        _httpClient = httpClientFactory.CreateClient("TaxCalculatorApi");
        _baseUrl = apiSettings.Value.BaseUrl;
    }

    [BindProperty]
    public string PostalCode { get; set; }

    [BindProperty]
    public decimal AnnualIncome { get; set; }

    public decimal? TaxAmount { get; set; }

    public bool? IsRequestSuccessful { get; set; }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var requestData = new
        {
            PostalCode = PostalCode,
            AnnualIncome = AnnualIncome
        };

        var jsonRequest = JsonConvert.SerializeObject(requestData);
        var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/TaxCalculator?postalCode={requestData.PostalCode}&annualIncome={requestData.AnnualIncome}", httpContent);
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            TaxAmount = JsonConvert.DeserializeObject<decimal>(jsonResponse);
            IsRequestSuccessful = true;
        }
        else
        {
            IsRequestSuccessful = false;
        }

        return Page();
    }

}