using BolsaFamilia.Application.Interfaces;
using BolsaFamilia.Application.Responses;
using BolsaFamilia.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

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
            try
            {                
                var list = Enum.GetValues(typeof(EstadoCivil)).Cast<EstadoCivil>().Select(e => new {
                    value = (int)e,
                    name = e.ToString()
                }).ToList();
                
                return Ok(Response<List<object>>.SuccessResult(list.Cast<object>().ToList(), "Lista de estados civis obtida com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<List<object>>.FailureResult("Erro ao obter lista de estados civis.", ex));
            }
        }

        [HttpGet("generos")]
        public IActionResult GetGeneros()
        {
            try
            {
                var list = Enum.GetValues(typeof(Sexo)).Cast<Sexo>().Select(e => new {
                    value = (int)e,
                    name = e.ToString()
                }).ToList();
                
                return Ok(Response<List<object>>.SuccessResult(list.Cast<object>().ToList(), "Lista de gêneros obtida com sucesso."));
            }
            catch (Exception ex)
            {                
                return BadRequest(Response<List<object>>.FailureResult("Erro ao obter lista de gêneros.", ex));
            }
        }

        [HttpGet("tipos-parentesco")]
        public async Task<IActionResult> GetTiposParentesco()
        {
            try
            {
                var infoResult = await _infoGeraisService.BuscaInfoGerais();
                if (!infoResult.Success)
                {
                    return BadRequest(Response<List<string>>.FailureResult(infoResult.Message));
                }

                var info = infoResult.Data;
                var list = string.IsNullOrWhiteSpace(info.TiposParentescoPermitidos) ? new List<string>() : info.TiposParentescoPermitidos.Split(',').Select(p => p.Trim()).ToList();
                return Ok(Response<List<string>>.SuccessResult(list, "Lista de tipos de parentesco obtida com sucesso."));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<List<string>>.FailureResult("Erro ao obter lista de tipos de parentesco.", ex));
            }
        }
    }
}