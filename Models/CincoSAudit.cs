using System;

namespace IntranetHub.Models
{
    public class CincoSAudit
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public DateTime AuditDate { get; set; } = DateTime.Now;
        public int UtilizacaoScore { get; set; }
        public int OrganizacaoScore { get; set; }
        public int LimpezaScore { get; set; }
        public int PadronizacaoScore { get; set; }
        public int DisciplinaScore { get; set; }
        public string? Notes { get; set; }
        public int TotalScore => UtilizacaoScore + OrganizacaoScore + LimpezaScore + PadronizacaoScore + DisciplinaScore;
    }
}