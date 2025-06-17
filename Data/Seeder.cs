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

            // 3. Cargar títulos existentes para evitar duplicados
            var titulosExistentes = new HashSet<string>(await _context.Mangas.Select(m => m.Titulo).ToListAsync());

            // 4. Faker para mangas
            var faker = new Faker();

            var mangasParaAgregar = new List<Manga>();

            while (mangasParaAgregar.Count < cantidad)
            {
                var titulo = faker.UniqueIndex + " - " + faker.Lorem.Sentence(3).TrimEnd('.');

                if (titulosExistentes.Contains(titulo))
                    continue;

                titulosExistentes.Add(titulo);

                var genero = faker.PickRandom(generos);

                mangasParaAgregar.Add(new Manga
                {
                    Titulo = titulo,
                    Autor = faker.Name.FullName(),
                    Anio = faker.Date.Past(20).Year,
                    GeneroId = genero.Id,
                    Genero = genero // 🔧 Aquí está la línea clave
                });
            }

            _context.Mangas.AddRange(mangasParaAgregar);
            await _context.SaveChangesAsync();

            Console.WriteLine($"✅ Se insertaron {cantidad} mangas correctamente.");
        }
    }
}
