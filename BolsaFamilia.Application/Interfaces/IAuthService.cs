namespace BolsaFamilia.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> AutenticarAsync(string email, string senha);
    }
}
