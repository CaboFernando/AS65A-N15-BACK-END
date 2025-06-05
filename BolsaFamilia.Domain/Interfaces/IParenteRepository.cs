using BolsaFamilia.Domain.Entities;

namespace BolsaFamilia.Domain.Interfaces
{
    public interface IParenteRepository
    {
        Task<IEnumerable<Parente>> ListarTodos(int userId);
        Task<Parente> BuscarById(int id, int userId);
        Task<Parente> BuscarByCpf(string cpf, int userId);
        Task<Parente> BuscarByCpf(string cpf);
        Task AdicionarAsync(Parente parent);
        Task AtualizarAsync(Parente parent);
        Task RemoverAsync(Parente parent);
        Task<List<Parente>> ObterPorUsuarioIdAsync(int usuarioId);
    }
}
