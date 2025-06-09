using Bogus;
using MiMangaBot.Data; 
using MiMangaBot.Models; // Reemplaza con el namespace real donde está tu clase Manga
// Asegúrate que este `using` tenga tu espacio de nombres real

public static class Seeder
{
    public static void InsertarMangas(MangaContext context, int cantidad)
    {
        var generos = new[] { "Shonen", "Shojo", "Seinen", "Josei", "Aventura", "Misterio", "Terror", "Acción", "Fantasía", "Comedia" };

        var faker = new Faker<Manga>()
            .RuleFor(m => m.Titulo, f => f.Lorem.Sentence(3))
            .RuleFor(m => m.Autor, f => f.Name.FullName())
            .RuleFor(m => m.Genero, f => f.PickRandom(generos))
            .RuleFor(m => m.Anio, f => f.Date.Past(30).Year);

        var mangas = faker.Generate(cantidad);

        context.Mangas.AddRange(mangas);
        context.SaveChanges();
    }
}
