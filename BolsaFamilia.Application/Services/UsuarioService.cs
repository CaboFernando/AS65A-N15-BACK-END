using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
using BolsaFamilia.Application.Utils;
using BolsaFamilia.Domain.Entities;
using BolsaFamilia.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace BolsaFamilia.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<UsuarioService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UsuarioService(IUsuarioRepository usuarioRepository, ILogger<UsuarioService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> AdicionarAsync(UsuarioDto dto)
        {
            try
            {
                if (!ValidadorUtils.CpfValido(dto.Cpf))
                {
                    _logger.LogWarning($"CPF inválido: {dto.Cpf}");
                    return false;
                }
                if (!ValidadorUtils.EmailValido(dto.Email))
                {
                    _logger.LogWarning($"Email inválido: {dto.Email}");
                    return false;
                }

                if (await _usuarioRepository.BuscarByCpf(dto.Cpf) != null){
                    _logger.LogWarning($"Usuário com CPF já cadastrado: {dto.Cpf}");
                    return false;
                }
                if (await _usuarioRepository.BuscarByEmail(dto.Email) != null){
                    _logger.LogWarning($"Usuário com Email já cadastrado: {dto.Email}");
                    return false;
                }

                var user = MapToEntity(dto);
                await _usuarioRepository.AdicionarAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao adicionar o usuário cpf: {dto.Cpf}");
                return false;
            }
        }

        public async Task<bool> AtualizarAsync(UsuarioDto dto)
        {
            try
            {
                if (!ValidadorUtils.EmailValido(dto.Email))
                {
                    _logger.LogWarning($"Email inválido: {dto.Email}");
                    return false;
                }
                var user = await _usuarioRepository.BuscarByCpf(dto.Cpf);
                if (user == null) return false;

                user.Nome = dto.Nome;
                user.Email = dto.Email;
                user.SenhaHash = HashPassword(dto.Senha);
                await _usuarioRepository.AtualizarAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar o usuário cpf: {dto.Cpf}");
                return false;
            }
        }

        public async Task<int?> BuscarUsuarioLogadoIdAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return null;

            return userId;
        }

        public async Task<UsuarioDto> BuscarByCpf(string cpf)
        {
            try
            {
                if (!ValidadorUtils.CpfValido(cpf))
                {
                    _logger.LogWarning($"CPF inválido: {cpf}");
                    return null;
                }
                var user = await _usuarioRepository.BuscarByCpf(cpf);
                return user == null ? null : MapToDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar o usuário cpf: {cpf}");
                return null;
            }
        }

        public async Task<UsuarioDto> BuscarById(int id)
        {
            try
            {
                var user = await _usuarioRepository.BuscarById(id);
                return user == null ? null : MapToDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar o usuário id: {id}");
                return null;
            }
        }

        public async Task<IEnumerable<UsuarioDto>> ListarTodos()
        {
            try
            {
                var users = await _usuarioRepository.ListarTodos();
                return users.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao listar os usuários");
                return Enumerable.Empty<UsuarioDto>();
            }
        }

        public async Task<bool> RemoverAsync(string cpf)
        {
            try
            {
                if (!ValidadorUtils.CpfValido(cpf))
                {
                    _logger.LogWarning($"CPF inválido: {cpf}");
                    return false;
                }
                var user = await _usuarioRepository.BuscarByCpf(cpf);
                if (user == null) return false;

                await _usuarioRepository.RemoverAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao remover o usuário cpf: {cpf}");
                return false;
            }
        }

        private Usuario MapToEntity(UsuarioDto dto) => new Usuario
        {
            Nome = dto.Nome,
            Cpf = dto.Cpf,
            Email = dto.Email,
            SenhaHash = HashPassword(dto.Senha)
        };

        private UsuarioDto MapToDto(Usuario usuario) => new UsuarioDto
        {
            Nome = usuario.Nome,
            Cpf = usuario.Cpf,
            Email = usuario.Email,
            Senha = usuario.SenhaHash
        };

        private string HashPassword(string senha) =>
            BCrypt.Net.BCrypt.HashPassword(senha);
    }

}
