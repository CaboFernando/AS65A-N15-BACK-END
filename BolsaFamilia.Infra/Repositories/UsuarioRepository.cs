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
            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Usuario user)
        {
            _context.Usuarios.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Usuario> BuscarByCpf(string cpf)
        {
            return await _context.Usuarios
                .Include(u => u.Parentes)
                .AsSplitQuery()
                .FirstOrDefaultAsync(s => s.Cpf == cpf);
        }

        public async Task<Usuario> BuscarByEmail(string email)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);

            return user;
        }


        public async Task<Usuario> BuscarById(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Parentes)
                .AsSplitQuery()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Usuario>> ListarTodos()
        {
            return await _context.Usuarios
                .Include(u => u.Parentes)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task RemoverAsync(Usuario user)
        {
            _context.Usuarios.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
