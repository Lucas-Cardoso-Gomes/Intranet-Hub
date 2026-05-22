namespace IntranetHub.Models
{
    public class DashboardViewModel
    {
        public decimal? UsdRate { get; set; }
        public decimal? EurRate { get; set; }
        public decimal? CurrentTemp { get; set; }
        public string? WeatherDescription { get; set; }
        public int TotalOperationsToday { get; set; }
        public int ActiveBranches { get; set; }
        public int OngoingAudits { get; set; }
    }
}