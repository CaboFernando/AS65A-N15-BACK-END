using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BolsaFamilia.Domain.Enums
{
    public enum EstadoCivil
    {
        [Description("Não Informado")]
        NaoInformado = 0,

        [Description("Solteiro")]
        Solteiro = 1,

        [Description("Casado")]
        Casado = 2,

        [Description("Divorciado")]
        Divorciado = 3,

        [Description("Viuvo")]
        Viuvo = 4,

        [Description("União Estavel")]
        UniaoEstavel = 5
    }
}
