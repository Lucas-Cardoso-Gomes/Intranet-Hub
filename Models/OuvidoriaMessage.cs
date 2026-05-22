using System;

namespace IntranetHub.Models
{
    public class OuvidoriaMessage
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public required string Subject { get; set; }
        public required string Message { get; set; }
        public bool IsAnonymous { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }
}