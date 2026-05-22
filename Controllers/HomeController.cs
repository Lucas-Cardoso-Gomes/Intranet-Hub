using IntranetHub.Models;
using IntranetHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntranetHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly BcbApiService _bcbApiService;
        private readonly WeatherService _weatherService;

        public HomeController(BcbApiService bcbApiService, WeatherService weatherService)
        {
            _bcbApiService = bcbApiService;
            _weatherService = weatherService;
        }

        public async Task<IActionResult> Index()
        {
            var rates = await _bcbApiService.GetLatestRatesAsync();
            var weather = await _weatherService.GetWeatherAsync("São Paulo");

            var viewModel = new DashboardViewModel
            {
                UsdRate = rates.usd,
                EurRate = rates.eur,
                CurrentTemp = weather.temp,
                WeatherDescription = weather.description,
                TotalOperationsToday = 142,
                ActiveBranches = 5,
                OngoingAudits = 2
            };
            return View(viewModel);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public IActionResult Error()
{
    return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
    }
}
