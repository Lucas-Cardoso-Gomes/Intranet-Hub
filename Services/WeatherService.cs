using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using IntranetHub.Models;

namespace IntranetHub.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BranchWeather>> GetWeatherForBranchesAsync(List<Branch> branches)
        {
            var branchWeathers = new List<BranchWeather>();
            
            var branchCoordinates = new Dictionary<string, (double lat, double lon)>
            {
                { "Uruguaiana", (-29.75472, -57.08833) },
                { "São Borja", (-28.66056, -56.00444) },
                { "Itajaí", (-26.90778, -48.66194) },
                { "Foz do Iguaçu", (-25.54778, -54.58806) },
                { "São Paulo", (-23.5505, -46.6333) }
            };

            foreach (var branch in branches)
            {
                decimal? temp = null;
                
                if (branchCoordinates.TryGetValue(branch.Name, out var coords))
                {
                    try
                    {
                        var url = $"https://api.open-meteo.com/v1/forecast?latitude={coords.lat}&longitude={coords.lon}&current_weather=true";
                        var response = await _httpClient.GetStringAsync(url);
                        using var doc = JsonDocument.Parse(response);
                        
                        if (doc.RootElement.TryGetProperty("current_weather", out var current))
                        {
                            temp = current.GetProperty("temperature").GetDecimal();
                        }
                    }
                    catch { /* swallow error to continue with other branches */ }
                }

                branchWeathers.Add(new BranchWeather 
                { 
                    BranchName = branch.Name, 
                    Temp = temp 
                });
            }

            return branchWeathers;
        }
    }
}
