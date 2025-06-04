using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
using BolsaFamilia.Application.Utils;
using BolsaFamilia.Domain.Entities;
using BolsaFamilia.Domain.Enums;
using BolsaFamilia.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BolsaFamilia.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;        
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IParenteRepository _parenteRepository;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(IUsuarioRepository usuarioRepository, IHttpContextAccessor httpContextAccessor, IParenteRepository parenteRepository, ILogger<UsuarioService> logger)
        {
            _usuarioRepository = usuarioRepository;            
            _httpContextAccessor = httpContextAccessor;
            _parenteRepository = parenteRepository;
            _logger = logger;
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

                if (await _usuarioRepository.BuscarByCpf(dto.Cpf) != null)
                {
                    _logger.LogWarning($"Usuário com CPF já cadastrado: {dto.Cpf}");
                    return false;
                }
                if (await _usuarioRepository.BuscarByEmail(dto.Email) != null)
                {
                    _logger.LogWarning($"Usuário com Email já cadastrado: {dto.Email}");
                    return false;
                }

                var user = MapToEntity(dto);
                await _usuarioRepository.AdicionarAsync(user);

                var UserByCpf = await _usuarioRepository.BuscarByCpf(dto.Cpf);

                var parenteTitular = new Parente
                {
                    Nome = dto.Nome,
                    GrauParentesco = "Responsável",
                    Sexo = Sexo.NaoInformado,
                    EstadoCivil = EstadoCivil.NaoInformado,
                    Cpf = dto.Cpf, 
                    Ocupacao = "",
                    Telefone = "00000000000",
                    Renda = 0, 
                    
                    UsuarioId = UserByCpf.Id
                };
                await _parenteRepository.AdicionarAsync(parenteTitular);

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
                var loggedUserId = await BuscarUsuarioLogadoIdAsync();
                if (loggedUserId != dto.Id)
                {
                    _logger.LogWarning($"O usuário logado e o usuário a ser alterado são diferentes. Só é possível alterar o próprio cadastro");
                    return false;
                }
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
                
                var user = await _usuarioRepository.BuscarById(dto.Id);
                if (user == null) return false;

                user.Nome = dto.Nome;
                user.Cpf = dto.Cpf;
                user.Email = dto.Email;
                user.SenhaHash = HashPassword(dto.Senha);
                await _usuarioRepository.AtualizarAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar o usuário id: {dto.Id}");
                return false;
            }
        }

        public async Task<bool> AtualizarSenhaAsync(PasswordInputDto dto)
        {
            try
            {
                var user = await _usuarioRepository.BuscarByCpf(dto.Cpf);
                if (user == null)
                {
                    _logger.LogWarning($"O CPF informado não foi encontrado na base de dados: {dto.Cpf}");
                    return false;
                }
                if (!user.Email.Equals(dto.Email))
                {
                    _logger.LogWarning($"O CPF informado não foi confere com o CPF do email informado: CPF {dto.Cpf}, Email {dto.Email}");
                    return false;
                }

                user.SenhaHash = HashPassword(dto.NovaSenha);
                await _usuarioRepository.AtualizarAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao alterar a senha do usuário CPF: {dto.Cpf}");
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

        public async Task<bool> RemoverAsync(int id)
        {
            try
            {                
                var user = await _usuarioRepository.BuscarById(id);
                if (user == null) return false;

                await _usuarioRepository.RemoverAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao remover o usuário id: {id}");
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
            Id = usuario.Id,
            Nome = usuario.Nome,
            Cpf = usuario.Cpf,
            Email = usuario.Email,
            Senha = usuario.SenhaHash,
            Parentes = usuario.Parentes.Select(p => new ParenteDto
            {
                Id = p.Id,
                Nome = p.Nome,
                GrauParentesco = p.GrauParentesco,
                Sexo = p.Sexo,
                EstadoCivil = p.EstadoCivil,
                Cpf = p.Cpf,
                Ocupacao = p.Ocupacao,
                Telefone = p.Telefone,
                Renda = p.Renda
            }).ToList()
        };

        private string HashPassword(string senha) =>
            BCrypt.Net.BCrypt.HashPassword(senha);
    }
}