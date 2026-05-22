using System;

namespace IntranetHub.Models
{
    public class CincoSImage
    {
        public int Id { get; set; }
        public int CincoSAuditId { get; set; }
        public CincoSAudit? CincoSAudit { get; set; }
        public required string ImagePath { get; set; }
        public string? Description { get; set; }
    }
}