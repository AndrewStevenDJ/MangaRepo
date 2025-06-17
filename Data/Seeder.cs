using Bogus;
using MiMangaBot.Models;
using Microsoft.EntityFrameworkCore;

namespace MiMangaBot.Data
{
    public class Seeder
    {
        private readonly MangaContext _context;

        public Seeder(MangaContext context)
        {
            _context = context;
        }

        public async Task SeedAsync(int cantidad)
        {
            // 1. Crear géneros si no existen
            List<Genero> generos;
            if (!await _context.Generos.AnyAsync())
            {
                generos = new List<Genero>
                {
                    new Genero { Nombre = "Acción" },
                    new Genero { Nombre = "Comedia" },
                    new Genero { Nombre = "Drama" },
                    new Genero { Nombre = "Fantasía" },
                    new Genero { Nombre = "Romance" },
                    new Genero { Nombre = "Terror" }
                };

                _context.Generos.AddRange(generos);
                await _context.SaveChangesAsync();
            }

            // 2. Cargar géneros existentes desde la base de datos
            generos = await _context.Generos.ToListAsync();

            // 3. Si ya hay mangas, no hacemos nada
            if (await _context.Mangas.AnyAsync())
                return;

            // 4. Faker para mangas
            var mangaFaker = new Faker<Manga>()
                .RuleFor(m => m.Titulo, f => f.UniqueIndex.ToString() + " - " + f.Lorem.Sentence(3).TrimEnd('.'))
                .RuleFor(m => m.Autor, f => f.Person.FullName)
                .RuleFor(m => m.Anio, f => f.Date.Past(20).Year)
                .RuleFor(m => m.GeneroId, f => f.PickRandom(generos).Id);

            var mangas = mangaFaker.Generate(cantidad);

            _context.Mangas.AddRange(mangas);
            await _context.SaveChangesAsync();

            Console.WriteLine($"✅ Se insertaron {cantidad} mangas correctamente.");
        }
    }
}
