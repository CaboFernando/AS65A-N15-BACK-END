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
        [EndpointDescription("Listar todos os parentes cadastrados pelo usuário logado.")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _parentesService.ListarTodos();

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("cpf/{cpf}")]
        [EndpointDescription("Listar o parente cadastrado pelo usuário logado, filtrado pelo CPF.")]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            var result = await _parentesService.BuscarByCpf(cpf);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost]
        [EndpointDescription("Cadastra um parente, vinculado pelo usuário logado.")]
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

            var result = await _parentesService.AdicionarAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [EndpointDescription("Atualiza um parente cadastrado pelo usuário logado, filtrado pelo ID.")]
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

            var result = await _parentesService.AtualizarAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [EndpointDescription("Remove um parente cadastrado pelo usuário logado, filtrado pelo ID.")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _parentesService.RemoverAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("renda")]
        [EndpointDescription("Calcula a renda familiar com base nos parentes cadastrados pelo usuário logado.")]
        public async Task<IActionResult> CalcularRenda()
        {
            var result = await _parentesService.CalcularRendaFamiliarAsync();

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}