using BolsaFamilia.Application.DTOs;

namespace BolsaFamilia.Application.Interfaces
{
    public interface ICalculaRendaService
    {
        Task<bool> VerificarElegibilidadeBolsaFamiliaAsync(int usuarioId);

    }
}
