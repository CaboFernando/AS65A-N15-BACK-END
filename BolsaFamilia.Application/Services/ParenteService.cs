using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
using BolsaFamilia.Domain.Entities;
using BolsaFamilia.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BolsaFamilia.Application.Services
{
    public class ParenteService : IParenteService
    {
        private readonly IParenteRepository _parenteRepository;
        private readonly ILogger<ParenteService> _logger;

        public ParenteService(IParenteRepository parenteRepository, ILogger<ParenteService> logger)
        {
            _parenteRepository = parenteRepository;
            _logger = logger;
        }

        public async Task<bool> AdicionarAsync(ParenteDto dto)
        {
            try
            {
                if (await _parenteRepository.BuscarByCpf(dto.Cpf) != null)
                    return false;

                var parent = MapToEntity(dto);
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
                var parent = await _parenteRepository.BuscarByCpf(dto.Cpf);
                if (parent == null) return false;

                parent.Nome = dto.Nome;
                parent.GrauParentesco = dto.GrauParentesco;
                parent.Sexo = dto.Sexo;
                parent.EstadoCivil = dto.EstadoCivil;
                parent.Cpf = dto.Cpf;
                parent.Ocupacao = dto.Ocupacao;
                parent.Telefone = dto.Telefone;
                parent.Renda = dto.Renda;
                await _parenteRepository.AtualizarAsync(parent);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar o membro familiar cpf: {dto.Cpf}");
                return false;
            }
        }

        public async Task<ParenteDto> BuscarByCpf(string cpf)
        {
            try
            {
                var parent = await _parenteRepository.BuscarByCpf(cpf);
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
                var parent = await _parenteRepository.ListarTodos();
                return parent.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao listar os membros familiares");
                return Enumerable.Empty<ParenteDto>();
            }
        }

        public async Task<bool> RemoverAsync(string cpf)
        {
            try
            {
                var parent = await _parenteRepository.BuscarByCpf(cpf);
                if (parent == null) return false;

                await _parenteRepository.RemoverAsync(parent);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao remover o membro familiar cpf: {cpf}");
                return false;
            }
        }

        private Parente MapToEntity(ParenteDto dto) => new Parente
        {
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
