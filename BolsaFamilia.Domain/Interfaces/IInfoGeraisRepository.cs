using BolsaFamilia.Domain.Entities;

namespace BolsaFamilia.Domain.Interfaces
{
    public interface IInfoGeraisRepository
    {
        Task<InfoGerais> BuscaInfoGerais();
        Task AtualizarAsync(InfoGerais parent);        
    }
}
