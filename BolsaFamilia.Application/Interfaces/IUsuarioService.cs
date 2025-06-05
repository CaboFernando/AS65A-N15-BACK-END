using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Responses;

namespace BolsaFamilia.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<Response<IEnumerable<UsuarioDto>>> ListarTodos();
        Task<int?> BuscarUsuarioLogadoIdAsync();
        Task<Response<UsuarioDto>> BuscarById(int id);
        Task<Response<UsuarioDto>> BuscarByCpf(string cpf);
        Task<Response<bool>> AdicionarAsync(UsuarioDto dto);
        Task<Response<bool>> AtualizarAsync(UsuarioDto dto);
        Task<Response<bool>> AtualizarSenhaAsync(PasswordInputDto dto);
        Task<Response<bool>> RemoverAsync(int id);
    }
}
