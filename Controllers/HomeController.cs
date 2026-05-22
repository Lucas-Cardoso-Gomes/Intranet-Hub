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
            
            var branches = await _context.Branches.ToListAsync();
            var branchWeathers = await _weatherService.GetWeatherForBranchesAsync(branches);

            var activeBranchesCount = branches.Count;
            var ongoingAuditsCount = await _context.CincoSAudits.CountAsync();

            var viewModel = new DashboardViewModel
            {
                UsdCompra = rates.usdCompra,
                UsdVenda = rates.usdVenda,
                DolarUpdatedAt = rates.updatedAt,
                BranchWeathers = branchWeathers,
                TotalOperationsToday = 142,
                ActiveBranches = activeBranchesCount,
                OngoingAudits = ongoingAuditsCount
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ForceSyncDolar()
        {
            await _bcbApiService.SyncRatesAsync();
            return RedirectToAction("Index");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public IActionResult Error()
{
    return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
    }
}
