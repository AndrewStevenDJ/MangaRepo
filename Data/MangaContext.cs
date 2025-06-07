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
    }
}
