using Microsoft.AspNetCore.Mvc;
using MiMangaBot.Data;
using MiMangaBot.Models;
using Microsoft.EntityFrameworkCore;

namespace MiMangaBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MangasController : ControllerBase
    {
        private readonly MangaContext _context;

        public MangasController(MangaContext context)
        {
            _context = context;
        }

        // GET api/mangas?id=1&titulo=naruto&autor=toriyama&anio=1999
        [HttpGet]
        public ActionResult<IEnumerable<Manga>> GetMangas(
            [FromQuery] int? id,
            [FromQuery] string? titulo,
            [FromQuery] string? autor,
            [FromQuery] int? anio)
        {
            var query = _context.Mangas.AsQueryable();

            if (id.HasValue)
                query = query.Where(m => m.Id == id.Value);

            if (!string.IsNullOrEmpty(titulo))
                query = query.Where(m => EF.Functions.Like(m.Titulo, $"%{titulo}%"));

            if (!string.IsNullOrEmpty(autor))
                query = query.Where(m => EF.Functions.Like(m.Autor, $"%{autor}%"));

            if (anio.HasValue)
                query = query.Where(m => m.Anio == anio.Value);

            var resultado = query.ToList();

            if (resultado.Count == 0)
                return NotFound("No se encontraron mangas con esos criterios.");

            return resultado;
        }

        [HttpPost]
        public ActionResult<Manga> PostManga(Manga manga)
        {
            _context.Mangas.Add(manga);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetMangas), new { id = manga.Id }, manga);
        }
    }
}
