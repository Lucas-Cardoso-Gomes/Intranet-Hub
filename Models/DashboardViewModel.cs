using System;
using System.Collections.Generic;

namespace IntranetHub.Models
{
    public class DashboardViewModel
    {
        public decimal? UsdCompra { get; set; }
        public decimal? UsdVenda { get; set; }
        public List<BranchWeather> BranchWeathers { get; set; } = new();
        public int TotalOperationsToday { get; set; }
        public int ActiveBranches { get; set; }
        public int OngoingAudits { get; set; }
        public DateTime? DolarUpdatedAt { get; set; }
    }
}