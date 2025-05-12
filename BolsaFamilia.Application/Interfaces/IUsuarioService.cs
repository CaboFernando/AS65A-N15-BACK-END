using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BolsaFamilia.Domain.Entities;

namespace BolsaFamilia.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> ListarTodos();
        Task<Usuario> BuscarById(int id);
        Task<Usuario> BuscarByCpf(string cpf);
        Task AdicionarAsync(Usuario user);
        Task AtualizarAsync(Usuario user);
        Task RemoverAsync(string cpf);
    }
}
