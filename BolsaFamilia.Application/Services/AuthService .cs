using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BolsaFamilia.Application.Interfaces;
using BolsaFamilia.Application.Responses;
using BolsaFamilia.Domain.Entities;
using BolsaFamilia.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using BolsaFamilia.Application.Utils;


namespace BolsaFamilia.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task<Response<string>> AutenticarAsync(string email, string senha)
        {
            if (string.IsNullOrWhiteSpace(senha) || !ValidadorUtils.EmailValido(email))
            {
                return Response<string>.FailureResult("Dados de login inválidos. Verifique o email e a senha.");
            }

            var user = await _usuarioRepository.BuscarByEmail(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(senha, user.SenhaHash))
            {
                return Response<string>.FailureResult("Usuário ou senha inválidos.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.Email, user.Email)
            };
            if (user.IsAdmin)
            {
                claims = claims.Append(new Claim(ClaimTypes.Role, "Admin")).ToArray();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = creds
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var writtenToken = tokenHandler.WriteToken(token);

            return Response<string>.SuccessResult(writtenToken, "Autenticação realizada com sucesso.");
        }
    }
}