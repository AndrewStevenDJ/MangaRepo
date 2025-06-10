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

        // DELETE api/mangas?id=5&titulo=naruto
        [HttpDelete]
        public async Task<IActionResult> DeleteMangaByQuery(
            [FromQuery] int? id,
            [FromQuery] string? titulo)
        {
            var query = _context.Mangas.AsQueryable();

            if (id.HasValue)
                query = query.Where(m => m.Id == id.Value);

            if (!string.IsNullOrEmpty(titulo))
                query = query.Where(m => EF.Functions.Like(m.Titulo, $"%{titulo}%"));

            var mangasAEliminar = await query.ToListAsync();

            if (mangasAEliminar.Count == 0)
                return NotFound("No se encontró ningún manga con esos criterios.");

            _context.Mangas.RemoveRange(mangasAEliminar);
            await _context.SaveChangesAsync();

            return Ok($"{mangasAEliminar.Count} manga(s) eliminado(s).");
        }

        // PUT api/mangas/actualizar?id=3
        // PUT api/mangas/actualizar?titulo=Naruto
        [HttpPut("actualizar")]
        public async Task<IActionResult> UpdateManga(
            [FromQuery] int? id,
            [FromQuery] string? titulo,
            [FromBody] Manga mangaActualizado)
        {
            Manga? mangaExistente = null;

            if (id.HasValue)
            {
                mangaExistente = await _context.Mangas.FindAsync(id.Value);
            }
            else if (!string.IsNullOrWhiteSpace(titulo))
            {
                mangaExistente = await _context.Mangas.FirstOrDefaultAsync(m => m.Titulo.ToLower() == titulo.ToLower());
            }
            else
            {
                return BadRequest("Debes proporcionar un 'id' o un 'titulo' para actualizar el manga.");
            }

            if (mangaExistente == null)
            {
                return NotFound("No se encontró un manga con los criterios proporcionados.");
            }

            // Actualizar los datos
            mangaExistente.Titulo = mangaActualizado.Titulo;
            mangaExistente.Autor = mangaActualizado.Autor;
            mangaExistente.Genero = mangaActualizado.Genero;
            mangaExistente.Anio = mangaActualizado.Anio;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Error al actualizar el manga.");
            }

            return Ok(mangaExistente);
        }
    }
}
