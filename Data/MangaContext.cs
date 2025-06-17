using Microsoft.EntityFrameworkCore;
using MiMangaBot.Models;

namespace MiMangaBot.Data
{
    public class MangaContext : DbContext
    {
        public MangaContext(DbContextOptions<MangaContext> options) : base(options)
        {
        }

        public DbSet<Manga> Mangas { get; set; }
        public DbSet<Genero> Generos { get; set; } // 👈 NUEVO

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la relación Manga - Genero
            modelBuilder.Entity<Manga>()
                .HasOne(m => m.Genero)
                .WithMany(g => g.Mangas)
                .HasForeignKey(m => m.GeneroId)
                .OnDelete(DeleteBehavior.Restrict); // Puedes usar Cascade o Restrict
        }
    }
}
