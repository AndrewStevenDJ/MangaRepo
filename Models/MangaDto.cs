namespace MiMangaBot.Models
{
    public class MangaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Autor { get; set; } = null!;
        public int GeneroId { get; set; }
        public string GeneroNombre { get; set; } = null!;
        public int Anio { get; set; }
    }
}
