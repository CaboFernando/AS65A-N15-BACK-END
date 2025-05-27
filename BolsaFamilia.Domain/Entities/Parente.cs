using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BolsaFamilia.Domain.Enums;

namespace BolsaFamilia.Domain.Entities
{
    public class Parente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string GrauParentesco { get; set; }
        public Sexo Sexo { get; set; }
        public EstadoCivil EstadoCivil { get; set; }
        public string Cpf { get; set; }
        public string Ocupacao { get; set; }
        public string Telefone { get; set; }
        public decimal Renda { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public static decimal CalcularRendaTotal(List<Parente> parentes)
        {
            return parentes.Sum(p => p.Renda);
        }

        public static decimal CalcularRendaPerCapita(List<Parente> parentes)
        {
            if (parentes == null || !parentes.Any()) return 0;
            return CalcularRendaTotal(parentes) / parentes.Count;
        }
    }
}
