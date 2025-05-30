using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
using BolsaFamilia.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BolsaFamilia.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ParentesController : ControllerBase
    {
        private readonly IParenteService _parentesService;

        public ParentesController(IParenteService parentesService)
        {
            _parentesService = parentesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _parentesService.ListarTodos();
            return Ok(result);
        }

        [HttpGet("cpf/{cpf}")]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            var result = await _parentesService.BuscarByCpf(cpf);
            return result is null ? NotFound($"Parente cpf: {cpf} não encontrado em nossa base de dados.") : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ParenteDto dto)
        {
            var success = await _parentesService.AdicionarAsync(dto);
            return success ? Ok("Parente cadastrado com sucesso!") : BadRequest("Erro ao criar parente.");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ParenteDto dto)
        {
            var success = await _parentesService.AtualizarAsync(dto);
            return success ? Ok("Parente atualizado com sucesso!") : BadRequest("Erro ao atualizar parente.");
        }

        [HttpDelete("{cpf}")]
        public async Task<IActionResult> Remove(string cpf)
        {
            var success = await _parentesService.RemoverAsync(cpf);
            return success ? Ok("Parente removido com sucesso!") : BadRequest("Erro ao remover parente.");
        }

        [HttpGet("renda")]
        public async Task<IActionResult> CalcularRenda()
        {
            var rendaDto = await _parentesService.CalcularRendaFamiliarAsync();
            return Ok(rendaDto);
        }
    }

}
