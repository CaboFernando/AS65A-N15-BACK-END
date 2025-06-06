﻿using BolsaFamilia.Domain.Entities;
using BolsaFamilia.Domain.Interfaces;
using BolsaFamilia.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace BolsaFamilia.Infra.Repositories
{
    public class ParenteRepository : IParenteRepository
    {
        private readonly AppDbContext _context;

        public ParenteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Parente parent)
        {
            _context.Parentes.Add(parent);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Parente parent)
        {
            _context.Parentes.Update(parent);
            await _context.SaveChangesAsync();
        }

        public async Task<Parente> BuscarByCpf(string cpf, int userId)
        {
            return await _context.Parentes.FirstOrDefaultAsync(s => s.Cpf == cpf && s.UsuarioId == userId);
        }

        public async Task<Parente> BuscarByCpf(string cpf)
        {
            return await _context.Parentes.FirstOrDefaultAsync(s => s.Cpf == cpf);
        }


        public async Task<Parente> BuscarById(int id, int userId)
        {
            return await _context.Parentes.FirstOrDefaultAsync(s => s.Id == id && s.UsuarioId == userId);
        }

        public async Task<IEnumerable<Parente>> ListarTodos(int userId)
        {
            var list = await _context.Parentes.ToListAsync();

            return list.Where(s => s.UsuarioId == userId);
        }

        public async Task RemoverAsync(Parente parent)
        {
            _context.Parentes.Remove(parent);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Parente>> ObterPorUsuarioIdAsync(int usuarioId)
        {
            return await _context.Parentes
                .Where(p => p.UsuarioId == usuarioId)
                .ToListAsync();
        }
    }
}
