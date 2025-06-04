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
        [EndpointDescription("[SOMENTE PARA USUÁRIO ADM] Listar todos os usuários cadastrados.")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _usuarioService.ListarTodos();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        [EndpointDescription("[SOMENTE PARA USUÁRIO ADM] Listar um usuário cadastrados filtrado por ID.")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _usuarioService.BuscarById(id);
            return result is null ? NotFound($"Usuário id: {id} não encontrado em nossa base de dados.") : Ok(result);
        }

        [HttpGet("cpf/{cpf}")]
        [Authorize(Roles = "Admin")]
        [EndpointDescription("[SOMENTE PARA USUÁRIO ADM] Listar um usuário cadastrados filtrado por CPF.")]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            var result = await _usuarioService.BuscarByCpf(cpf);
            return result is null ? NotFound($"Usuário cpf: {cpf} não encontrado em nossa base de dados.") : Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [EndpointDescription("Cadastra um usuário.")]
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
        [EndpointDescription("Altera um usuário cadastrados filtrado por ID.")]
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

        [AllowAnonymous]
        [HttpPut("AlterarSenha")]
        [EndpointDescription("Valida por CPF e Email e altera a senha do usuário informado.")]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordInputDto input)
        {
            var success = await _usuarioService.AtualizarSenhaAsync(input);
            return success ? Ok("Password atualizado com sucesso!") : BadRequest("Erro ao atualizar password.");
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [EndpointDescription("[SOMENTE PARA USUÁRIO ADM] Remove um usuário cadastrados filtrado por ID.")]
        public async Task<IActionResult> Remove(int id)
        {
            var success = await _usuarioService.RemoverAsync(id);
            return success ? Ok("Usuário removido com sucesso!") : BadRequest("Erro ao remover usuário.");
        }
    }
}