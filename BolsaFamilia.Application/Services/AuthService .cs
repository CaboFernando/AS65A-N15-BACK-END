using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
using BolsaFamilia.Domain.Entities;
using BolsaFamilia.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;



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

        public async Task<string> AutenticarAsync(string email, string senha)
        {
            var user = await _usuarioRepository.BuscarByEmail(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(senha, user.SenhaHash))
            {
                return null;
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Nome),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
