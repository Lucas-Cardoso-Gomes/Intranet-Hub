using System;
using System.Collections.Generic;

namespace IntranetHub.Models
{
    public class Aviso
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}