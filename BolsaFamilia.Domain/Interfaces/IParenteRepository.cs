using BolsaFamilia.Domain.Entities;

namespace BolsaFamilia.Domain.Interfaces
{
    public interface IParenteRepository
    {
        Task<IEnumerable<Parente>> ListarTodos();
        Task<Parente> BuscarById(int id);
        Task<Parente> BuscarByCpf(string cpf);
        Task AdicionarAsync(Parente parent);
        Task AtualizarAsync(Parente parent);
        Task RemoverAsync(Parente parent);
    }
}
