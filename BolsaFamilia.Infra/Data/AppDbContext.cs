using BolsaFamilia.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BolsaFamilia.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Parente> Parentes { get; set; }
        public DbSet<InfoGerais> InfoGerais { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Usuários
            modelBuilder.Entity<Usuario>()
                .HasIndex(s => s.Cpf)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .Property(s => s.Nome)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Usuario>()
                .Property(s => s.Cpf)
                .IsRequired()
                .HasMaxLength(11);

            modelBuilder.Entity<Parente>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Parentes)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
