using System.Collections.Generic;

namespace MiMangaBot.Models
{
    public class Genero
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }

        public ICollection<Manga> Mangas { get; set; } = new List<Manga>();
    }
}
