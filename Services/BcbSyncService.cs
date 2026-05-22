using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IntranetHub.Data;
using IntranetHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IntranetHub.Services
{
    public class BcbSyncService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly HttpClient _httpClient;

        public BcbSyncService(IServiceProvider serviceProvider, HttpClient httpClient)
        {
            _serviceProvider = serviceProvider;
            _httpClient = httpClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Do an initial sync on startup immediately
            await SyncRatesAsync(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRunTime = GetNextRunTime(now);
                var delay = nextRunTime - now;

                await Task.Delay(delay, stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                {
                    await SyncRatesAsync(stoppingToken);
                }
            }
        }

        private DateTime GetNextRunTime(DateTime now)
        {
            var times = new[] {
                new TimeSpan(8, 0, 0),
                new TimeSpan(10, 0, 0),
                new TimeSpan(12, 0, 0),
                new TimeSpan(14, 0, 0),
                new TimeSpan(16, 0, 0),
                new TimeSpan(17, 0, 0)
            };

            foreach (var time in times)
            {
                var nextRun = now.Date.Add(time);
                if (now < nextRun)
                {
                    return nextRun;
                }
            }

            // Next run is 08:00 tomorrow
            return now.Date.AddDays(1).Add(times[0]);
        }

        public async Task SyncRatesAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var bcbApiService = scope.ServiceProvider.GetRequiredService<BcbApiService>();
            await bcbApiService.SyncRatesAsync(stoppingToken);
        }
    }
}
