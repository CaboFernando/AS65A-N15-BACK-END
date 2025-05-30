using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace BolsaFamilia.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _usuarioService.ListarTodos();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _usuarioService.BuscarById(id);
            return result is null ? NotFound($"Usu�rio id: {id} n�o encontrado em nossa base de dados.") : Ok(result);
        }

        [HttpGet("cpf/{cpf}")]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            var result = await _usuarioService.BuscarByCpf(cpf);
            return result is null ? NotFound($"Usu�rio cpf: {cpf} n�o encontrado em nossa base de dados.") : Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuarioInputDto input)
        {
            var dto = new UsuarioDto
            {
                Nome = input.Nome,
                Cpf = input.Cpf,
                Email = input.Email,
                Senha = input.Senha
            };

            var success = await _usuarioService.AdicionarAsync(dto);
            return success ? Ok("Usu�rio cadastrado com sucesso!") : BadRequest("Erro ao criar usu�rio.");
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UsuarioInputDto input)
        {
            var dto = new UsuarioDto
            {
                Id = id,
                Nome = input.Nome,
                Cpf = input.Cpf,
                Email = input.Email,
                Senha = input.Senha
            };

            var success = await _usuarioService.AtualizarAsync(dto);
            return success ? Ok("Usu�rio atualizado com sucesso!") : BadRequest("Erro ao atualizar usu�rio.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Remove(int id)
        {
            var success = await _usuarioService.RemoverAsync(id);
            return success ? Ok("Usu�rio removido com sucesso!") : BadRequest("Erro ao remover usu�rio.");
        }
    }
}