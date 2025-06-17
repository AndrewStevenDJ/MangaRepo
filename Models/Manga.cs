namespace MiMangaBot.Models
{
    public class Manga
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Autor { get; set; }
        public int Anio { get; set; }

        public int GeneroId { get; set; }                  // Clave foránea
        public required Genero Genero { get; set; }        // Navegación con 'required' ✅
    }
}
