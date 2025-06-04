using BolsaFamilia.Domain.Entities;
using BolsaFamilia.Domain.Interfaces;
using BolsaFamilia.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace BolsaFamilia.Infra.Repositories
{
    public class InfoGeraisRepository : IInfoGeraisRepository
    {
        private readonly AppDbContext _context;

        public InfoGeraisRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AtualizarAsync(InfoGerais info)
        {
            _context.InfoGerais.Update(info);
            await _context.SaveChangesAsync();
        }

        public async Task<InfoGerais> BuscaInfoGerais()
        {
            return await _context.InfoGerais.FirstOrDefaultAsync();
        }
    }
}
