using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
using BolsaFamilia.Application.Responses;
using BolsaFamilia.Application.Utils;
using BolsaFamilia.Domain.Entities;
using BolsaFamilia.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BolsaFamilia.Application.Services
{
    public class ParenteService : IParenteService
    {
        private readonly IParenteRepository _parenteRepository;        
        private readonly IUsuarioService _usuarioService;
        private readonly ICalculaRendaService _calculaRendaService;
        private readonly ILogger<ParenteService> _logger;

        public ParenteService(IParenteRepository parenteRepository, IUsuarioService usuarioService, ICalculaRendaService calculaRendaService, ILogger<ParenteService> logger)
        {
            _parenteRepository = parenteRepository;            
            _usuarioService = usuarioService;
            _calculaRendaService = calculaRendaService;
            _logger = logger;
        }

        public async Task<Response<bool>> AdicionarAsync(ParenteDto dto)
        {
            try
            {
                if (!ValidadorUtils.CpfValido(dto.Cpf))
                {
                    _logger.LogWarning($"CPF inválido: {dto.Cpf}");
                    return Response<bool>.FailureResult("CPF informado é inválido.");
                }

                var loggedUserId = await _usuarioService.BuscarUsuarioLogadoIdAsync();
                if (loggedUserId == null)
                    return Response<bool>.FailureResult("Usuário não logado ou ID de usuário não encontrado.");

                if (await _parenteRepository.BuscarByCpf(dto.Cpf, (int)loggedUserId) != null)
                    return Response<bool>.FailureResult("Já existe um parente cadastrado com este CPF para o usuário logado.");

                if (await _parenteRepository.BuscarByCpf(dto.Cpf) != null)
                    return Response<bool>.FailureResult("Já existe um parente cadastrado com este CPF como membro familiar de outro usuário.");

                var parent = MapToEntity(dto);
                parent.UsuarioId = (int)loggedUserId;

                await _parenteRepository.AdicionarAsync(parent);
                return Response<bool>.SuccessResult(true, "Parente cadastrado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao adicionar o membro familiar cpf: {dto.Cpf}");
                return Response<bool>.FailureResult("Erro ao cadastrar parente.", ex);
            }
        }

        public async Task<Response<bool>> AtualizarAsync(ParenteDto dto)
        {
            try
            {
                if (!ValidadorUtils.CpfValido(dto.Cpf))
                {
                    _logger.LogWarning($"CPF inválido: {dto.Cpf}");
                    return Response<bool>.FailureResult("CPF informado é inválido.");
                }

                var loggedUserId = await _usuarioService.BuscarUsuarioLogadoIdAsync();
                if (loggedUserId == null)
                    return Response<bool>.FailureResult("Usuário não logado ou ID de usuário não encontrado.");

                var parent = await _parenteRepository.BuscarById(dto.Id, (int)loggedUserId);
                if (parent == null)
                    return Response<bool>.FailureResult("Parente não encontrado para o usuário logado.");

                if (await _parenteRepository.BuscarByCpf(dto.Cpf) is Parente existingParente && existingParente.UsuarioId != loggedUserId)
                    return Response<bool>.FailureResult("Já existe um parente cadastrado com este CPF como membro familiar de outro usuário.");

                parent.Nome = dto.Nome;
                parent.GrauParentesco = dto.GrauParentesco;
                parent.Sexo = dto.Sexo;
                parent.EstadoCivil = dto.EstadoCivil;
                parent.Cpf = dto.Cpf;
                parent.Ocupacao = dto.Ocupacao;
                parent.Telefone = dto.Telefone;
                parent.Renda = dto.Renda;
                parent.UsuarioId = (int)loggedUserId;

                await _parenteRepository.AtualizarAsync(parent);
                return Response<bool>.SuccessResult(true, "Parente atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar o membro familiar id: {dto.Id}");
                return Response<bool>.FailureResult("Erro ao atualizar parente.", ex);
            }
        }

        public async Task<Response<ParenteDto>> BuscarByCpf(string cpf)
        {
            try
            {
                if (!ValidadorUtils.CpfValido(cpf))
                {
                    _logger.LogWarning($"CPF inválido: {cpf}");
                    return Response<ParenteDto>.FailureResult("CPF informado é inválido.");
                }
                var loggedUserId = await _usuarioService.BuscarUsuarioLogadoIdAsync();
                if (loggedUserId == null)
                    return Response<ParenteDto>.FailureResult("Usuário não logado ou ID de usuário não encontrado.");

                var parent = await _parenteRepository.BuscarByCpf(cpf, (int)loggedUserId);
                if (parent == null)
                    return Response<ParenteDto>.FailureResult($"Parente com CPF {cpf} não encontrado para o usuário logado.");

                return Response<ParenteDto>.SuccessResult(MapToDto(parent), "Parente encontrado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar o membro familiar cpf: {cpf}");
                return Response<ParenteDto>.FailureResult("Erro ao buscar parente.", ex);
            }
        }

        public async Task<Response<IEnumerable<ParenteDto>>> ListarTodos()
        {
            try
            {
                var loggedUserId = await _usuarioService.BuscarUsuarioLogadoIdAsync();
                if (loggedUserId == null)
                    return Response<IEnumerable<ParenteDto>>.FailureResult("Usuário não logado ou ID de usuário não encontrado.");

                var parentes = await _parenteRepository.ListarTodos((int)loggedUserId);
                var parentesDto = parentes.Select(MapToDto);
                return Response<IEnumerable<ParenteDto>>.SuccessResult(parentesDto, "Parentes listados com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao listar os membros familiares");
                return Response<IEnumerable<ParenteDto>>.FailureResult("Erro ao listar parentes.", ex);
            }
        }

        public async Task<Response<bool>> RemoverAsync(int id)
        {
            try
            {
                var loggedUserId = await _usuarioService.BuscarUsuarioLogadoIdAsync();
                if (loggedUserId == null)
                    return Response<bool>.FailureResult("Usuário não logado ou ID de usuário não encontrado.");

                var parent = await _parenteRepository.BuscarById(id, (int)loggedUserId);
                if (parent == null)
                    return Response<bool>.FailureResult("Parente não encontrado para o usuário logado.");

                await _parenteRepository.RemoverAsync(parent);
                return Response<bool>.SuccessResult(true, "Parente removido com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao remover o membro familiar id: {id}");
                return Response<bool>.FailureResult("Erro ao remover parente.", ex);
            }
        }

        public async Task<Response<string>> CalcularRendaFamiliarAsync()
        {
            var usuarioId = await _usuarioService.BuscarUsuarioLogadoIdAsync();
            if (usuarioId == null)
            {
                return Response<string>.FailureResult("Usuário não encontrado ou não está logado.");
            }

            var result = await _calculaRendaService.VerificarElegibilidadeBolsaFamiliaAsync((int)usuarioId);

            return result ?
                Response<string>.SuccessResult(null, "De acordo com o cálculo dos parentes cadastrado para o usuário logado, o grupo familiar é SIM elegível para o programa Bolsa Família")
                : Response<string>.SuccessResult(null, "De acordo com o cálculo dos parentes cadastrado para o usuário logado, o grupo familiar é NÃO elegível para o programa Bolsa Família");
        }

        private Parente MapToEntity(ParenteDto dto) => new Parente
        {
            Id = dto.Id,
            Nome = dto.Nome,
            GrauParentesco = dto.GrauParentesco,
            Sexo = dto.Sexo,
            EstadoCivil = dto.EstadoCivil,
            Cpf = dto.Cpf,
            Ocupacao = dto.Ocupacao,
            Telefone = dto.Telefone,
            Renda = dto.Renda
        };

        private ParenteDto MapToDto(Parente parente) => new ParenteDto
        {
            Id = parente.Id,
            Nome = parente.Nome,
            GrauParentesco = parente.GrauParentesco,
            Sexo = parente.Sexo,
            EstadoCivil = parente.EstadoCivil,
            Cpf = parente.Cpf,
            Ocupacao = parente.Ocupacao,
            Telefone = parente.Telefone,
            Renda = parente.Renda
        };
    }
}