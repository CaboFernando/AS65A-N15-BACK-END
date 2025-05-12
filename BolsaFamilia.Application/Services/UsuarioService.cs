using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BolsaFamilia.Application.Interfaces;
using BolsaFamilia.Domain.Entities;
using BolsaFamilia.Domain.Interfaces;

namespace BolsaFamilia.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task AdicionarAsync(Usuario user)
        {
            throw new NotImplementedException();
        }

        public Task AtualizarAsync(Usuario user)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> BuscarByCpf(string cpf)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> BuscarById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> ListarTodos()
        {
            throw new NotImplementedException();
        }

        public Task RemoverAsync(string cpf)
        {
            throw new NotImplementedException();
        }
    }
}
