using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BolsaFamilia.Domain.Entities;
using BolsaFamilia.Domain.Interfaces;
using BolsaFamilia.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace BolsaFamilia.Infra.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Usuario user)
        {
            throw new NotImplementedException();
        }

        public async Task AtualizarAsync(Usuario user)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario> BuscarByCpf(string cpf)
        {
            return _context.Usuarios.FirstOrDefaultAsync(s => s.Cpf == cpf);
        }

        public Task<Usuario> BuscarById(int id)
        {
            return _context.Usuarios.FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task<Usuario> ListarTodos()
        {
            throw new NotImplementedException();
        }

        public async Task RemoverAsync(string cpf)
        {
            throw new NotImplementedException();
        }
    }
}
