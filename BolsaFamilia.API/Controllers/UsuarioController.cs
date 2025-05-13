using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BolsaFamilia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioService usuarioService, ILogger<UsuarioController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Iniciando listagem de todos os usu�rios.");
            var result = await _usuarioService.ListarTodos();
            return Ok(result);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Buscando usu�rio por ID: {Id}", id);
            var result = await _usuarioService.BuscarById(id);
            return Ok(result);
        }

        [HttpGet("get-by-cpf/{cpf}")]
        public async Task<IActionResult> GetByCpf(string cpf)
        {
            _logger.LogInformation("Buscando usu�rio por CPF: {Cpf}", cpf);
            var result = await _usuarioService.BuscarByCpf(cpf);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuarioDto dto)
        {
            try
            {
                _logger.LogInformation("Tentando criar usu�rio com CPF: {Cpf}", dto.Cpf);
                await _usuarioService.AdicionarAsync(dto);
                _logger.LogInformation("Usu�rio criado com sucesso: {Cpf}", dto.Cpf);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao criar usu�rio com CPF: {Cpf}", dto.Cpf);
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UsuarioDto dto)
        {
            try
            {
                _logger.LogInformation("Tentando atualizar usu�rio com CPF: {Cpf}", dto.Cpf);
                await _usuarioService.AtualizarAsync(dto);
                _logger.LogInformation("Usu�rio atualizado com sucesso: {Cpf}", dto.Cpf);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao atualizar usu�rio com CPF: {Cpf}", dto.Cpf);
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(string cpf)
        {
            try
            {
                _logger.LogInformation("Tentando remover usu�rio com CPF: {Cpf}", cpf);
                await _usuarioService.RemoverAsync(cpf);
                _logger.LogInformation("Usu�rio removido com sucesso: {Cpf}", cpf);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao remover usu�rio com CPF: {Cpf}", cpf);
                return BadRequest(e.Message);
            }
        }
    }
}
