using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
using BolsaFamilia.Application.Utils;
using BolsaFamilia.Domain.Entities;
using BolsaFamilia.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BolsaFamilia.Application.Services
{
    public class CalculaRendaService : ICalculaRendaService
    {
        private readonly IInfoGeraisRepository _infoGeraisRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<CalculaRendaService> _logger;

        public CalculaRendaService(IInfoGeraisRepository infoGeraisRepository, IUsuarioRepository usuarioRepository, ILogger<CalculaRendaService> logger)
        {
            _infoGeraisRepository = infoGeraisRepository;            
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        private async Task<decimal> CalcularRendaPerCapitaAsync(List<Parente> parentes)
        {
            if (parentes == null || !parentes.Any()) 
                return 0;

            decimal rendaTotal = parentes.Sum(p => p.Renda);

            return rendaTotal / parentes.Count;
        }

        public async Task<bool> VerificarElegibilidadeBolsaFamiliaAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.BuscarById(usuarioId);
            if (usuario == null || !usuario.Parentes.Any())
            {
                return false;
            }

            var infoGerais = await _infoGeraisRepository.BuscaInfoGerais();
            if (infoGerais == null)
            {
                throw new InvalidOperationException("Configurações gerais não encontradas.");
            }

            decimal rendaPerCapita = await CalcularRendaPerCapitaAsync(usuario.Parentes.ToList());

            return rendaPerCapita <= infoGerais.ValorBaseRendaPerCapita;
        }
    }
}