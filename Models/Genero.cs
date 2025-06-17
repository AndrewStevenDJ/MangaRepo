using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MiMangaBot.Models
{
    public class Genero
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del género es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede tener más de 50 caracteres.")]
        public required string Nombre { get; set; }

        [JsonIgnore]
        public ICollection<Manga> Mangas { get; set; } = new List<Manga>();
    }
}
