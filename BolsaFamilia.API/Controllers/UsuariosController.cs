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

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [EndpointDescription("Listar um usuário cadastrados filtrado por ID.")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _usuarioService.BuscarById(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("cpf/{cpf}")]
        [Authorize(Roles = "Admin")]
        [EndpointDescription("[SOMENTE PARA USUÁRIO ADM] Listar um usuário cadastrados filtrado por CPF.")]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            var result = await _usuarioService.BuscarByCpf(cpf);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
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

            var result = await _usuarioService.AdicionarAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
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

            var result = await _usuarioService.AtualizarAsync(dto);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut("AlterarSenha")]
        [EndpointDescription("Valida por CPF e Email e altera a senha do usuário informado.")]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordInputDto input)
        {
            var result = await _usuarioService.AtualizarSenhaAsync(input);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [EndpointDescription("[SOMENTE PARA USUÁRIO ADM] Remove um usuário cadastrados filtrado por ID.")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _usuarioService.RemoverAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}