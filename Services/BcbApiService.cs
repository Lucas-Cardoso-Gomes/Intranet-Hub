using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IntranetHub.Data;
using IntranetHub.Models;
using Microsoft.EntityFrameworkCore;

namespace IntranetHub.Services
{
    public class BcbApiService
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _httpClient;

        public BcbApiService(ApplicationDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<(decimal? usdCompra, decimal? usdVenda, DateTime? updatedAt)> GetLatestRatesAsync()
        {
            try
            {
                var latestRate = await _context.DolarRates
                    .OrderByDescending(r => r.Date)
                    .FirstOrDefaultAsync();

                if (latestRate != null)
                {
                    return (latestRate.ValorCompra, latestRate.ValorVenda, latestRate.UpdatedAt);
                }

                return (null, null, null);
            }
            catch { return (null, null, null); }
        }

        public async Task SyncRatesAsync(CancellationToken stoppingToken = default)
        {
            try
            {
                var today = DateTime.Now.ToString("dd/MM/yyyy");
                var start = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy");
                var url = $"https://ptax.bcb.gov.br/ptax_internet/consultaBoletim.do?method=gerarCSVFechamentoMoedaNoPeriodo&ChkMoeda=61&DATAINI={start}&DATAFIM={today}";

                var response = await _httpClient.GetByteArrayAsync(url, stoppingToken);
                var content = System.Text.Encoding.GetEncoding("ISO-8859-1").GetString(response);
                
                using var reader = new StringReader(content);
                string? line;
                while ((line = await reader.ReadLineAsync(stoppingToken)) != null)
                {
                    var parts = line.Split(';');
                    if (parts.Length >= 6 && parts[3] == "USD")
                    {
                        if (decimal.TryParse(parts[4], NumberStyles.Any, CultureInfo.GetCultureInfo("pt-BR"), out var valorCompra) &&
                            decimal.TryParse(parts[5], NumberStyles.Any, CultureInfo.GetCultureInfo("pt-BR"), out var valorVenda))
                        {
                            var datePart = parts[0]; // e.g. 23042024
                            var rateDate = DateTime.ParseExact(datePart, "ddMMyyyy", CultureInfo.InvariantCulture);
                            
                            var existingRate = await _context.DolarRates.FirstOrDefaultAsync(r => r.Date == rateDate, stoppingToken);
                            if (existingRate == null)
                            {
                                _context.DolarRates.Add(new DolarRate
                                {
                                    Date = rateDate,
                                    ValorCompra = valorCompra,
                                    ValorVenda = valorVenda,
                                    UpdatedAt = DateTime.Now
                                });
                            }
                            else
                            {
                                existingRate.ValorCompra = valorCompra;
                                existingRate.ValorVenda = valorVenda;
                                existingRate.UpdatedAt = DateTime.Now;
                            }
                            await _context.SaveChangesAsync(stoppingToken);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error syncing rates: {ex.Message}");
            }
        }
    }
}
