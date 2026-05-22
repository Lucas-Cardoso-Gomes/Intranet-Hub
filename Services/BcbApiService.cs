using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace IntranetHub.Services
{
    public class BcbApiService
    {
        private readonly HttpClient _httpClient;

        public BcbApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(decimal? usd, decimal? eur)> GetLatestRatesAsync()
        {
            try
            {
                var today = DateTime.Now.ToString("MM-dd-yyyy");
                var sevenDaysAgo = DateTime.Now.AddDays(-7).ToString("MM-dd-yyyy");
                
                var usdUrl = $"https://olinda.bcb.gov.br/olinda/servico/PTAX/versao/v1/odata/CotacaoMoedaPeriodo(moeda=@moeda,dataInicial=@dataInicial,dataFinalCotacao=@dataFinalCotacao)?@moeda='USD'&@dataInicial='{sevenDaysAgo}'&@dataFinalCotacao='{today}'&$top=1&$orderby=dataHoraCotacao%20desc&$format=json&$select=cotacaoCompra";
                var eurUrl = $"https://olinda.bcb.gov.br/olinda/servico/PTAX/versao/v1/odata/CotacaoMoedaPeriodo(moeda=@moeda,dataInicial=@dataInicial,dataFinalCotacao=@dataFinalCotacao)?@moeda='EUR'&@dataInicial='{sevenDaysAgo}'&@dataFinalCotacao='{today}'&$top=1&$orderby=dataHoraCotacao%20desc&$format=json&$select=cotacaoCompra";

                decimal? usd = null, eur = null;

                var usdResponse = await _httpClient.GetStringAsync(usdUrl);
                using var usdDoc = JsonDocument.Parse(usdResponse);
                if (usdDoc.RootElement.TryGetProperty("value", out var usdValues) && usdValues.GetArrayLength() > 0)
                    usd = usdValues[0].GetProperty("cotacaoCompra").GetDecimal();

                var eurResponse = await _httpClient.GetStringAsync(eurUrl);
                using var eurDoc = JsonDocument.Parse(eurResponse);
                if (eurDoc.RootElement.TryGetProperty("value", out var eurValues) && eurValues.GetArrayLength() > 0)
                    eur = eurValues[0].GetProperty("cotacaoCompra").GetDecimal();

                return (usd, eur);
            }
            catch { return (null, null); }
        }
    }
}
