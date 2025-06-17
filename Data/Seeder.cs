using Bogus;
using MiMangaBot.Models;
using Microsoft.EntityFrameworkCore;

namespace MiMangaBot.Data
{
    public static class Seeder
    {
        public static async Task SeedAsync(MangaContext context)
        {
            // Asegúrate de aplicar migraciones antes de sembrar
            await context.Database.MigrateAsync();

            // Solo insertar si no hay géneros ni mangas
            if (!context.Generos.Any() && !context.Mangas.Any())
            {
                // 1. Crear lista de géneros
                var generos = new List<Genero>
                {
                    new Genero { Nombre = "Acción" },
                    new Genero { Nombre = "Comedia" },
                    new Genero { Nombre = "Drama" },
                    new Genero { Nombre = "Fantasía" },
                    new Genero { Nombre = "Terror" }
                };

                context.Generos.AddRange(generos);
                await context.SaveChangesAsync();

                // 2. Crear mangas con Faker y asignarles un GeneroId aleatorio
                var mangaFaker = new Faker<Manga>()
                    .RuleFor(m => m.Titulo, f => f.Lorem.Sentence(3))
                    .RuleFor(m => m.Autor, f => f.Person.FullName)
                    .RuleFor(m => m.Anio, f => f.Date.Past(20).Year)
                    .RuleFor(m => m.GeneroId, f => f.PickRandom(generos).Id);

                var mangas = mangaFaker.Generate(20); // Puedes cambiar el número

                context.Mangas.AddRange(mangas);
                await context.SaveChangesAsync();
            }
        }
    }
}
