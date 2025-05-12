using BolsaFamilia.Domain.Entities;

namespace BolsaFamilia.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario> ListarTodos();
        Task<Usuario> BuscarById(int id);
        Task<Usuario> BuscarByCpf(string cpf);
        Task AdicionarAsync(Usuario user);
        Task AtualizarAsync(Usuario user);
        Task RemoverAsync(string cpf);
    }
}
