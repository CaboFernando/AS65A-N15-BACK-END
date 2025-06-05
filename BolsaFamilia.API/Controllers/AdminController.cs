using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BolsaFamilia.API.Controllers
{   
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {        
        private readonly IInfoGeraisService _infoGeraisService;

        public AdminController(IInfoGeraisService infoGeraisService)
        {
            _infoGeraisService = infoGeraisService;
        }

        [HttpGet]
        [EndpointDescription("[SOMENTE PARA USUÁRIO ADM] Lista todas as informações gerais.")]
        public async Task<IActionResult> Get()
        {
            var result = await _infoGeraisService.BuscaInfoGerais();

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [EndpointDescription("[SOMENTE PARA USUÁRIO ADM] Altera as informações gerais.")]
        public async Task<IActionResult> Update(int id, [FromBody] InfoGeraisInputDto input)
        {
            var dto = new InfoGeraisDto
            {
                Id = id,
                ValorBaseRendaPerCapita = input.ValorBaseRendaPerCapita,
                TiposParentescoPermitidos = input.TiposParentescoPermitidos
            };

            var result = await _infoGeraisService.AtualizarAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}