using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BolsaFamilia.Domain.Enums;

namespace BolsaFamilia.Application.DTOs
{
    public class ParenteInputDto
    {
        public string Nome { get; set; }
        public string GrauParentesco { get; set; }
        public Sexo Sexo { get; set; }
        public EstadoCivil EstadoCivil { get; set; }
        public string Cpf { get; set; }
        public string Ocupacao { get; set; }
        public string Telefone { get; set; }
        public decimal Renda { get; set; }
    }
}
