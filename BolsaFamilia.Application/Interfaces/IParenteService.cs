using BolsaFamilia.Application.DTOs;
using BolsaFamilia.Application.Responses;
using BolsaFamilia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BolsaFamilia.Application.Interfaces
{
    public interface IParenteService
    {
        Task<Response<IEnumerable<ParenteDto>>> ListarTodos();
        Task<Response<ParenteDto>> BuscarByCpf(string cpf);
        Task<Response<bool>> AdicionarAsync(ParenteDto dto);
        Task<Response<bool>> AtualizarAsync(ParenteDto dto);
        Task<Response<bool>> RemoverAsync(int id);
        Task<Response<string>> CalcularRendaFamiliarAsync();
    }
}
