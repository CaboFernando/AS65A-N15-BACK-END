using BolsaFamilia.Application.DTOs;

namespace BolsaFamilia.Application.Interfaces
{
    public interface IInfoGeraisService
    {
        Task<InfoGeraisDto> BuscaInfoGerais();
        Task<bool> AtualizarAsync(InfoGeraisDto dto);

    }
}
