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
        [EndpointDescription("[SOMENTE PARA USU�RIO ADM] Listar todos os usu�rios cadastrados.")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _usuarioService.ListarTodos();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        [EndpointDescription("[SOMENTE PARA USU�RIO ADM] Listar um usu�rio cadastrados filtrado por ID.")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _usuarioService.BuscarById(id);
            return result is null ? NotFound($"Usu�rio id: {id} n�o encontrado em nossa base de dados.") : Ok(result);
        }

        [HttpGet("cpf/{cpf}")]
        [Authorize(Roles = "Admin")]
        [EndpointDescription("[SOMENTE PARA USU�RIO ADM] Listar um usu�rio cadastrados filtrado por CPF.")]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            var result = await _usuarioService.BuscarByCpf(cpf);
            return result is null ? NotFound($"Usu�rio cpf: {cpf} n�o encontrado em nossa base de dados.") : Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [EndpointDescription("Cadastra um usu�rio.")]
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
        [EndpointDescription("Altera um usu�rio cadastrados filtrado por ID.")]
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

        [AllowAnonymous]
        [HttpPut("AlterarSenha")]
        [EndpointDescription("Valida por CPF e Email e altera a senha do usu�rio informado.")]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordInputDto input)
        {
            var success = await _usuarioService.AtualizarSenhaAsync(input);
            return success ? Ok("Password atualizado com sucesso!") : BadRequest("Erro ao atualizar password.");
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [EndpointDescription("[SOMENTE PARA USU�RIO ADM] Remove um usu�rio cadastrados filtrado por ID.")]
        public async Task<IActionResult> Remove(int id)
        {
            var success = await _usuarioService.RemoverAsync(id);
            return success ? Ok("Usu�rio removido com sucesso!") : BadRequest("Erro ao remover usu�rio.");
        }
    }
}