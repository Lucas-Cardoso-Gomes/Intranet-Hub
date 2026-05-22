using System;

namespace IntranetHub.Models
{
    public class Pop
    {
        public int Id { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }
        public string? FileAttachmentPath { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
        public string? Client { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}