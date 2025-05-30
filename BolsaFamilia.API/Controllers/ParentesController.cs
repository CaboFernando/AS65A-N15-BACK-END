using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
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
        public async Task<IActionResult> Create([FromBody] ParenteInputDto input)
        {
            var dto = new ParenteDto
            {
                Nome = input.Nome,
                Cpf = input.Cpf,
                GrauParentesco = input.GrauParentesco,
                Sexo = input.Sexo,
                EstadoCivil = input.EstadoCivil,
                Ocupacao = input.Ocupacao,
                Telefone = input.Telefone,
                Renda = input.Renda
            };

            var success = await _parentesService.AdicionarAsync(dto);
            return success ? Ok("Parente cadastrado com sucesso!") : BadRequest("Erro ao criar parente.");
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ParenteInputDto input)
        {
            var dto = new ParenteDto
            {
                Id = id,
                Nome = input.Nome,
                Cpf = input.Cpf,
                GrauParentesco = input.GrauParentesco,
                Sexo = input.Sexo,
                EstadoCivil = input.EstadoCivil,
                Ocupacao = input.Ocupacao,
                Telefone = input.Telefone,
                Renda = input.Renda
            };

            var success = await _parentesService.AtualizarAsync(dto);
            return success ? Ok("Parente atualizado com sucesso!") : BadRequest("Erro ao atualizar parente.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Remove(int id)
        {
            var success = await _parentesService.RemoverAsync(id);
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