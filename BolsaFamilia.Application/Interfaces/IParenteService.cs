using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BolsaFamilia.Application.DTOs;

namespace BolsaFamilia.Application.Interfaces
{
    public interface IParenteService
    {
        Task<IEnumerable<ParenteDto>> ListarTodos();
        Task<ParenteDto> BuscarByCpf(string cpf);
        Task<bool> AdicionarAsync(ParenteDto dto);
        Task<bool> AtualizarAsync(ParenteDto dto);
        Task<bool> RemoverAsync(string cpf);
    }
}
