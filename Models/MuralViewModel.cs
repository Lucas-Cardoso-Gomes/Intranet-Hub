using System;
using System.Collections.Generic;

namespace IntranetHub.Models
{
    public class MuralViewModel
    {
        public List<Aviso> Avisos { get; set; } = new();
        public List<Aniversario> Aniversarios { get; set; } = new();
    }
}