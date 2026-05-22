using IntranetHub.Data;
using IntranetHub.Models;
using IntranetHub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntranetHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly BcbApiService _bcbApiService;
        private readonly WeatherService _weatherService;
        private readonly ApplicationDbContext _context;

        public HomeController(BcbApiService bcbApiService, WeatherService weatherService, ApplicationDbContext context)
        {
            _bcbApiService = bcbApiService;
            _weatherService = weatherService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rates = await _bcbApiService.GetLatestRatesAsync();
            var weather = await _weatherService.GetWeatherAsync("São Paulo");

            var activeBranchesCount = await _context.Branches.CountAsync();
            var ongoingAuditsCount = await _context.CincoSAudits.CountAsync();

            var viewModel = new DashboardViewModel
            {
                UsdRate = rates.usd,
                EurRate = rates.eur,
                CurrentTemp = weather.temp,
                WeatherDescription = weather.description,
                TotalOperationsToday = 142,
                ActiveBranches = activeBranchesCount,
                OngoingAudits = ongoingAuditsCount
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
