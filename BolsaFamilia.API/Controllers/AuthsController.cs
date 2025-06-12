using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BolsaFamilia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;        
        private readonly ILogger<AuthsController> _logger;
        private readonly IUsuarioService _usuarioService;

        public AuthsController(IAuthService authService,  ILogger<AuthsController> logger, IUsuarioService usuarioService)
        {
            _authService = authService;
            _logger = logger;
            _usuarioService = usuarioService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var result = await _authService.AutenticarAsync(login.Email, login.Senha);

            var user = await _usuarioService.BuscarByEmail(login.Email);

            if (!result.Success)
            {
                return Unauthorized(result.Message);
            }

            return Ok(new { Token = result.Data, IdUsuario = user.Data.Id, IsAdm = user.Data.IsAdmin, Message = result.Message });
        }
    }
}