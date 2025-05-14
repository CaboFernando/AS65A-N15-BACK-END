using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BolsaFamilia.Application.DTOs;

namespace BolsaFamilia.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDto>> ListarTodos();
        Task<int?> BuscarUsuarioLogadoIdAsync();
        Task<UsuarioDto> BuscarById(int id);
        Task<UsuarioDto> BuscarByCpf(string cpf);
        Task<bool> AdicionarAsync(UsuarioDto dto);
        Task<bool> AtualizarAsync(UsuarioDto dto);
        Task<bool> RemoverAsync(string cpf);
    }
}
