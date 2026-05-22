using System;

namespace IntranetHub.Models
{
    public class DolarRate
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal ValorCompra { get; set; }
        public decimal ValorVenda { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
