using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Responses;

namespace BolsaFamilia.Application.Interfaces
{
    public interface IInfoGeraisService
    {
        Task<Response<InfoGeraisDto>> BuscaInfoGerais();
        Task<Response<bool>> AtualizarAsync(InfoGeraisDto dto);

    }
}
