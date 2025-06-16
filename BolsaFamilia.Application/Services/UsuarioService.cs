using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
using BolsaFamilia.Application.Responses;
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

        public async Task<Response<bool>> AdicionarAsync(UsuarioDto dto)
        {
            try
            {
                if (!ValidadorUtils.CpfValido(dto.Cpf))
                    return Response<bool>.FailureResult("CPF informado é inválido.");

                if (!ValidadorUtils.EmailValido(dto.Email))
                    return Response<bool>.FailureResult("Email informado é inválido.");

                if (await _usuarioRepository.BuscarByCpf(dto.Cpf) != null)
                    return Response<bool>.FailureResult("Já existe um usuário cadastrado com este CPF.");

                if (await _usuarioRepository.BuscarByEmail(dto.Email) != null)
                    return Response<bool>.FailureResult("Já existe um usuário cadastrado com este e-mail.");

                if (await _parenteRepository.BuscarByCpf(dto.Cpf) != null)
                    return Response<bool>.FailureResult("Já existe um usuário cadastrado com este CPF como membro familiar de outro usuário.");

                var user = MapToEntity(dto);
                await _usuarioRepository.AdicionarAsync(user);

                var userByCpf = await _usuarioRepository.BuscarByCpf(dto.Cpf);

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
                    UsuarioId = userByCpf.Id
                };

                await _parenteRepository.AdicionarAsync(parenteTitular);

                return Response<bool>.SuccessResult(true, "Usuário cadastrado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao adicionar o usuário cpf: {dto.Cpf}");
                return Response<bool>.FailureResult("Erro ao cadastrar o usuário.");
            }
        }

        public async Task<Response<bool>> AtualizarAsync(UsuarioDto dto)
        {
            try
            {
                var loggedUserId = await BuscarUsuarioLogadoIdAsync();
                if (loggedUserId != dto.Id)
                    return Response<bool>.FailureResult("Só é possível alterar o próprio cadastro.");

                if (!ValidadorUtils.CpfValido(dto.Cpf))
                    return Response<bool>.FailureResult("CPF informado é inválido.");

                if (!ValidadorUtils.EmailValido(dto.Email))
                    return Response<bool>.FailureResult("E-mail informado é inválido.");

                if (await _parenteRepository.BuscarByCpf(dto.Cpf) is Parente existingParente && existingParente.UsuarioId != loggedUserId)
                    return Response<bool>.FailureResult("Já existe um usuário cadastrado com este CPF como membro familiar de outro usuário.");

                var user = await _usuarioRepository.BuscarById(dto.Id);
                if (user == null)
                    return Response<bool>.FailureResult("Usuário não encontrado.");

                user.Nome = dto.Nome;
                user.Cpf = dto.Cpf;
                user.Email = dto.Email;
                user.SenhaHash = HashPassword(dto.Senha);

                await _usuarioRepository.AtualizarAsync(user);

                return Response<bool>.SuccessResult(true, "Usuário atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar o usuário id: {dto.Id}");
                return Response<bool>.FailureResult("Erro interno ao tentar atualizar o usuário.");
            }
        }

        public async Task<Response<bool>> AtualizarSenhaAsync(PasswordInputDto dto)
        {
            try
            {
                var user = await _usuarioRepository.BuscarByCpf(dto.Cpf);
                if (user == null)
                    return Response<bool>.FailureResult("CPF informado não foi encontrado.");

                if (!user.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase))
                    return Response<bool>.FailureResult("CPF e e-mail não conferem.");

                user.SenhaHash = HashPassword(dto.NovaSenha);
                await _usuarioRepository.AtualizarAsync(user);

                return Response<bool>.SuccessResult(true, "Senha atualizada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao alterar a senha do usuário CPF: {dto.Cpf}");
                return Response<bool>.FailureResult("Erro ao tentar atualizar a senha.");
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

        public async Task<Response<UsuarioDto>> BuscarByCpf(string cpf)
        {
            try
            {
                if (!ValidadorUtils.CpfValido(cpf))
                    return Response<UsuarioDto>.FailureResult("CPF informado é inválido.");

                var user = await _usuarioRepository.BuscarByCpf(cpf);

                if (user == null)
                    return Response<UsuarioDto>.FailureResult($"Usuário com CPF {cpf} não encontrado.");

                return Response<UsuarioDto>.SuccessResult(MapToDto(user), "Usuário encontrado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar o usuário com CPF: {cpf}");
                return Response<UsuarioDto>.FailureResult("Erro interno ao buscar o usuário.");
            }
        }

        public async Task<Response<UsuarioDto>> BuscarById(int id)
        {
            try
            {
                var loggedUserId = await BuscarUsuarioLogadoIdAsync();
                if (loggedUserId is null)
                    return Response<UsuarioDto>.FailureResult("É necessário estar logado para buscar as informações do usuário por id.");

                if (loggedUserId != id)
                    return Response<UsuarioDto>.FailureResult("Só é possível buscar informações do próprio cadastro.");

                var user = await _usuarioRepository.BuscarById(id);
                if (user == null)
                    return Response<UsuarioDto>.FailureResult($"Usuário id: {id} não encontrado.");

                return Response<UsuarioDto>.SuccessResult(MapToDto(user), "Usuário encontrado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar o usuário id: {id}");
                return Response<UsuarioDto>.FailureResult("Erro interno ao buscar o usuário.");
            }
        }

        public async Task<Response<UsuarioDto>> BuscarByEmail(string email)
        {
            try
            {
                var user = await _usuarioRepository.BuscarByEmail(email);
                if (user == null)
                    return Response<UsuarioDto>.FailureResult($"Usuário email: {email} não encontrado.");

                return Response<UsuarioDto>.SuccessResult(MapToDto(user), "Usuário encontrado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar o usuário email: {email}");
                return Response<UsuarioDto>.FailureResult("Erro interno ao buscar o usuário.");
            }
        }

        public async Task<Response<IEnumerable<UsuarioDto>>> ListarTodos()
        {
            try
            {
                var users = await _usuarioRepository.ListarTodos();
                var usuariosDto = users.Select(MapToDto);
                return Response<IEnumerable<UsuarioDto>>.SuccessResult(usuariosDto, "Usuários listados com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar os usuários");
                return Response<IEnumerable<UsuarioDto>>.FailureResult("Erro ao listar os usuários.");
            }
        }

        public async Task<Response<bool>> RemoverAsync(int id)
        {
            try
            {
                var user = await _usuarioRepository.BuscarById(id);
                if (user == null)
                    return Response<bool>.FailureResult("Usuário não encontrado.");

                await _usuarioRepository.RemoverAsync(user);
                return Response<bool>.SuccessResult(true, "Usuário removido com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao remover o usuário id: {id}");
                return Response<bool>.FailureResult("Erro ao tentar remover o usuário.");
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
            IsAdmin = usuario.IsAdmin,
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