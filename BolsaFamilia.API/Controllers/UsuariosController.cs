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
            return result is null ? NotFound($"Usuário id: {id} não encontrado em nossa base de dados.") : Ok(result);
        }

        [HttpGet("cpf/{cpf}")]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            var result = await _usuarioService.BuscarByCpf(cpf);
            return result is null ? NotFound($"Usuário cpf: {cpf} não encontrado em nossa base de dados.") : Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuarioDto dto)
        {
            var success = await _usuarioService.AdicionarAsync(dto);
            return success ? Ok("Usuário cadastrado com sucesso!") : BadRequest("Erro ao criar usuário.");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UsuarioDto dto)
        {
            var success = await _usuarioService.AtualizarAsync(dto);
            return success ? Ok("Usuário atualizado com sucesso!") : BadRequest("Erro ao atualizar usuário.");
        }

        [HttpDelete("{cpf}")]
        public async Task<IActionResult> Remove(string cpf)
        {
            var success = await _usuarioService.RemoverAsync(cpf);
            return success ? Ok("Usuário removido com sucesso!") : BadRequest("Erro ao remover usuário.");
        }
    }

}
