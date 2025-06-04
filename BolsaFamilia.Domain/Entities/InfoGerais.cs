using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BolsaFamilia.Domain.Entities
{
    public class InfoGerais
    {
        public int Id { get; set; }
        public decimal ValorBaseRendaPerCapita { get; set; }
        public string TiposParentescoPermitidos { get; set; }
    }
}
