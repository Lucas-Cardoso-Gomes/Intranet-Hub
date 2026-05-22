using System;

namespace IntranetHub.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Department { get; set; }
        public string? Email { get; set; }
        public string? ExtensionLine { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }
    }
}