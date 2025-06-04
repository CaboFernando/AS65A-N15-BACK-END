using BolsaFamilia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BolsaFamilia.Application.DTOs
{
    public class PasswordInputDto
    {
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string NovaSenha { get; set; }
    }
}
