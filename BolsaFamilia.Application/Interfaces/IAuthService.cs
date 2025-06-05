using BolsaFamilia.Application.Responses;

namespace BolsaFamilia.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Response<string>> AutenticarAsync(string email, string senha);
    }
}