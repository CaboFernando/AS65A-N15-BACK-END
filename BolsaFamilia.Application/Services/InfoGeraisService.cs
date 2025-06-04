using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
using BolsaFamilia.Application.Utils;
using BolsaFamilia.Domain.Entities;
using BolsaFamilia.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BolsaFamilia.Application.Services
{
    public class InfoGeraisService : IInfoGeraisService
    {
        private readonly IInfoGeraisRepository _infoGeraisRepository;
        private readonly ILogger<InfoGeraisService> _logger;

        public InfoGeraisService(IInfoGeraisRepository infoGeraisRepository, ILogger<InfoGeraisService> logger)
        {
            _infoGeraisRepository = infoGeraisRepository;
            _logger = logger;
        }
        
        public async Task<bool> AtualizarAsync(InfoGeraisDto dto)
        {
            try
            {
                var info = await _infoGeraisRepository.BuscaInfoGerais();
                if (info == null) return false;

                info.ValorBaseRendaPerCapita = dto.ValorBaseRendaPerCapita;
                info.TiposParentescoPermitidos = dto.TiposParentescoPermitidos;
                

                await _infoGeraisRepository.AtualizarAsync(info);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar o informações gerais.");
                return false;
            }
        }

        public async Task<InfoGeraisDto> BuscaInfoGerais()
        {
            try
            {
                var parent = await _infoGeraisRepository.BuscaInfoGerais();
                return parent == null ? null : MapToDto(parent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar o informações gerais");
                return null;
            }
        }

        private InfoGeraisDto MapToDto(InfoGerais parente) => new InfoGeraisDto
        {
            Id = parente.Id,
            ValorBaseRendaPerCapita = parente.ValorBaseRendaPerCapita,
            TiposParentescoPermitidos = parente.TiposParentescoPermitidos
        };
    }
}