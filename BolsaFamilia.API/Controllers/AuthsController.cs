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

        public AuthsController(IAuthService authService, ILogger<AuthsController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            _logger.LogInformation("Iniciando login de usuário.");
            var result = await _authService.AutenticarAsync(login.Email, login.Senha);

            if (!result.Success)
            {
                return Unauthorized(result.Message);
            }

            return Ok(new { Token = result.Data, Message = result.Message });
        }
    }
}