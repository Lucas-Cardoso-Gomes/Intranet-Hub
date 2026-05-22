using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntranetHub.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(decimal? temp, string? description)> GetWeatherAsync(string city)
        {
            try
            {
                double lat = -23.5505, lon = -46.6333; // SP

                var url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current_weather=true";
                var response = await _httpClient.GetStringAsync(url);
                using var doc = JsonDocument.Parse(response);
                
                if (doc.RootElement.TryGetProperty("current_weather", out var current))
                {
                    var temp = current.GetProperty("temperature").GetDecimal();
                    return (temp, "Clima Atualizado");
                }
                return (null, null);
            }
            catch { return (null, null); }
        }
    }
}
