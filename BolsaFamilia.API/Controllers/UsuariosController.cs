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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _usuarioService.ListarTodos();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _usuarioService.BuscarById(id);
            return result is null ? NotFound($"Usuário id: {id} não encontrado em nossa base de dados.") : Ok(result);
        }

        [HttpGet("cpf/{cpf}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            var result = await _usuarioService.BuscarByCpf(cpf);
            return result is null ? NotFound($"Usuário cpf: {cpf} não encontrado em nossa base de dados.") : Ok(result);
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
            return success ? Ok("Usuário cadastrado com sucesso!") : BadRequest("Erro ao criar usuário.");
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
            return success ? Ok("Usuário atualizado com sucesso!") : BadRequest("Erro ao atualizar usuário.");
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int id)
        {
            var success = await _usuarioService.RemoverAsync(id);
            return success ? Ok("Usuário removido com sucesso!") : BadRequest("Erro ao remover usuário.");
        }
    }
}