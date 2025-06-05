using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
using BolsaFamilia.Application.Responses;
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

        public async Task<Response<bool>> AtualizarAsync(InfoGeraisDto dto)
        {
            try
            {
                var info = await _infoGeraisRepository.BuscaInfoGerais();
                if (info == null)
                {
                    _logger.LogWarning($"Informações gerais não encontradas para atualização.");
                    return Response<bool>.FailureResult("Configurações gerais não encontradas para atualização.");
                }

                info.ValorBaseRendaPerCapita = dto.ValorBaseRendaPerCapita;
                info.TiposParentescoPermitidos = dto.TiposParentescoPermitidos;

                await _infoGeraisRepository.AtualizarAsync(info);
                return Response<bool>.SuccessResult(true, "Informações gerais atualizadas com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar o informações gerais.");
                return Response<bool>.FailureResult("Erro ao atualizar informações gerais.");
            }
        }

        public async Task<Response<InfoGeraisDto>> BuscaInfoGerais()
        {
            try
            {
                var info = await _infoGeraisRepository.BuscaInfoGerais();
                if (info == null)
                {
                    _logger.LogWarning($"Informações gerais não encontradas.");
                    return Response<InfoGeraisDto>.FailureResult("Configurações gerais não encontradas. É necessário criar o registro inicial.");
                }
                return Response<InfoGeraisDto>.SuccessResult(MapToDto(info), "Informações gerais encontradas com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar o informações gerais");
                return Response<InfoGeraisDto>.FailureResult("Erro ao buscar informações gerais.");
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