using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BolsaFamilia.Application.DTOs
{
    public class RendaDto
    {
        public decimal RendaTotal { get; set; }
        public decimal RendaPerCapita { get; set; }
        public bool TemDireito { get; set; }
    }
}