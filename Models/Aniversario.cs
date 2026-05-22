using System;
using System.Collections.Generic;

namespace IntranetHub.Models
{
    public class Aniversario
    {
        public int Id { get; set; }
        public required string EmployeeName { get; set; }
        public required string Department { get; set; }
        public DateTime Date { get; set; }
    }

}