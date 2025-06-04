using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
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

        public async Task<bool> AdicionarAsync(ParenteDto dto)
        {
            try
            {
                if (!ValidadorUtils.CpfValido(dto.Cpf))
                {
                    _logger.LogWarning($"CPF inválido: {dto.Cpf}");
                    return false;
                }

                var loggedUserId = await _usuarioService.BuscarUsuarioLogadoIdAsync();
                if (loggedUserId == null)
                    return false;

                if (await _parenteRepository.BuscarByCpf(dto.Cpf, (int)loggedUserId) != null)
                    return false;

                var parent = MapToEntity(dto);
                parent.UsuarioId = (int)loggedUserId;

                await _parenteRepository.AdicionarAsync(parent);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao adicionar o membro familiar cpf: {dto.Cpf}");
                return false;
            }
        }

        public async Task<bool> AtualizarAsync(ParenteDto dto)
        {
            try
            {
                if (!ValidadorUtils.CpfValido(dto.Cpf))
                {
                    _logger.LogWarning($"CPF inválido: {dto.Cpf}");
                    return false;
                }

                var loggedUserId = await _usuarioService.BuscarUsuarioLogadoIdAsync();
                if (loggedUserId == null)
                    return false;

                var parent = await _parenteRepository.BuscarById(dto.Id, (int)loggedUserId);
                if (parent == null) return false;

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
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar o membro familiar id: {dto.Id}");
                return false;
            }
        }

        public async Task<ParenteDto> BuscarByCpf(string cpf)
        {
            try
            {
                if (!ValidadorUtils.CpfValido(cpf))
                {
                    _logger.LogWarning($"CPF inválido: {cpf}");
                    return null;
                }
                var loggedUserId = await _usuarioService.BuscarUsuarioLogadoIdAsync();

                var parent = await _parenteRepository.BuscarByCpf(cpf, (int)loggedUserId);
                return parent == null ? null : MapToDto(parent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar o membro familiar cpf: {cpf}");
                return null;
            }
        }

        public async Task<IEnumerable<ParenteDto>> ListarTodos()
        {
            try
            {
                var loggedUserId = await _usuarioService.BuscarUsuarioLogadoIdAsync();

                var parent = await _parenteRepository.ListarTodos((int)loggedUserId);
                return parent.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao listar os membros familiares");
                return Enumerable.Empty<ParenteDto>();
            }
        }

        public async Task<bool> RemoverAsync(int id)
        {
            try
            {                
                var loggedUserId = await _usuarioService.BuscarUsuarioLogadoIdAsync();

                var parent = await _parenteRepository.BuscarById(id, (int)loggedUserId);
                if (parent == null) return false;

                await _parenteRepository.RemoverAsync(parent);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao remover o membro familiar id: {id}");
                return false;
            }
        }

        public async Task<string> CalcularRendaFamiliarAsync()
        {
            var usuarioId = await _usuarioService.BuscarUsuarioLogadoIdAsync();
            if (usuarioId == null)
            {
                return "Usuário não encontrado ou não está logado.";
            }

            var result = await _calculaRendaService.VerificarElegibilidadeBolsaFamiliaAsync((int)usuarioId);

            return result ? 
                "De acordo com o cálculo dos parentes cadastrado para o usuário logado, o grupo familiar é SIM elegível para o programa Bolsa Família" 
                : "De acordo com o cálculo dos parentes cadastrado para o usuário logado, o grupo familiar é NÃO elegível para o programa Bolsa Família";            
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