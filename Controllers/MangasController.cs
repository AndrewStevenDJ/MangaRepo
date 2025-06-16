using Microsoft.AspNetCore.Mvc;
using MiMangaBot.Models;
using MiMangaBot.Services;

namespace MiMangaBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MangasController : ControllerBase
    {
        private readonly MangaService _mangaService;

        public MangasController(MangaService mangaService)
        {
            _mangaService = mangaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Manga>>> GetMangas(
            [FromQuery] int? id,
            [FromQuery] string? titulo,
            [FromQuery] string? autor,
            [FromQuery] int? anio)
        {
            var resultado = await _mangaService.ObtenerMangasAsync(id, titulo, autor, anio);

            if (resultado.Count == 0)
                return NotFound("No se encontraron mangas con esos criterios.");

            return resultado;
        }

        [HttpPost]
        public async Task<ActionResult<Manga>> PostManga(Manga manga)
        {
            var nuevoManga = await _mangaService.CrearMangaAsync(manga);
            return CreatedAtAction(nameof(GetMangas), new { id = nuevoManga.Id }, nuevoManga);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMangaByQuery(
            [FromQuery] int? id,
            [FromQuery] string? titulo)
        {
            var eliminados = await _mangaService.EliminarMangasAsync(id, titulo);

            if (eliminados.Count == 0)
                return NotFound("No se encontró ningún manga con esos criterios.");

            return Ok($"{eliminados.Count} manga(s) eliminado(s).");
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> UpdateManga(
            [FromQuery] int? id,
            [FromQuery] string? titulo,
            [FromBody] Manga mangaActualizado)
        {
            var actualizado = await _mangaService.ActualizarMangaAsync(id, titulo, mangaActualizado);

            if (actualizado == null)
                return NotFound("No se encontró un manga con los criterios proporcionados.");

            return Ok(actualizado);
        }
    }
}
