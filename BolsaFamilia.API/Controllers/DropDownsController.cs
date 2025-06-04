using BolsaFamilia.Application.Interfaces;
using BolsaFamilia.Domain.Enums;
using BolsaFamilia.Infra.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BolsaFamilia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropDownsController : ControllerBase
    {
        private readonly IInfoGeraisService _infoGeraisService;
        
        public DropDownsController(IInfoGeraisService infoGeraisService)
        {
            _infoGeraisService = infoGeraisService;
        }

        [HttpGet("estados-civis")]
        public IActionResult GetEstadosCivis()
        {
            var list = Enum.GetValues(typeof(EstadoCivil)).Cast<EstadoCivil>().Select(e => new {
                value = (int)e,
                name = e.ToString()
            }).ToList();
            return Ok(list);
        }

        [HttpGet("generos")]
        public IActionResult GetGeneros()
        {
            var list = Enum.GetValues(typeof(Sexo)).Cast<Sexo>().Select(e => new {
                value = (int)e,
                name = e.ToString()
            }).ToList();
            return Ok(list);
        }

        [HttpGet("tipos-parentesco")]
        public async Task<IActionResult> GetTiposParentesco()
        {
            var info = await _infoGeraisService.BuscaInfoGerais();
            if (info == null || string.IsNullOrWhiteSpace(info.TiposParentescoPermitidos))
            {
                return Ok(new List<string>());
            }
            var list = info.TiposParentescoPermitidos.Split(',').Select(p => p.Trim()).ToList();
            return Ok(list);
        }
    }
}
