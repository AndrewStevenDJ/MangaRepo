using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiMangaBot.Models;
using MiMangaBot.Services;

namespace MiMangaBot.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MangasController : ControllerBase
    {
        private readonly MangaService _mangaService;

        public MangasController(MangaService mangaService)
        {
            _mangaService = mangaService;
        }

        /// <summary>
        /// Obtiene una lista paginada de mangas filtrados.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PaginacionRespuesta<MangaDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PaginacionRespuesta<MangaDto>>> GetMangas(
            [FromQuery] int? id,
            [FromQuery] string? titulo,
            [FromQuery] string? autor,
            [FromQuery] int? anio,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var resultado = await _mangaService.ObtenerMangasAsync(id, titulo, autor, anio, page, pageSize);

            if (resultado.Datos.Count == 0)
                return NotFound("No se encontraron mangas con esos criterios.");

            return Ok(resultado);
        }

        /// <summary>
        /// Crea un nuevo manga.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Manga), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Manga>> PostManga([FromBody] Manga manga)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevoManga = await _mangaService.CrearMangaAsync(manga);
            return CreatedAtAction(nameof(GetMangas), new { id = nuevoManga.Id }, nuevoManga);
        }

        /// <summary>
        /// Elimina mangas por ID o título.
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteMangaByQuery(
            [FromQuery] int? id,
            [FromQuery] string? titulo)
        {
            var eliminados = await _mangaService.EliminarMangasAsync(id, titulo);

            if (eliminados.Count == 0)
                return NotFound("No se encontró ningún manga con esos criterios.");

            return Ok($"{eliminados.Count} manga(s) eliminado(s).");
        }

        /// <summary>
        /// Actualiza un manga por ID o título.
        /// </summary>
        [HttpPut("actualizar")]
        [ProducesResponseType(typeof(Manga), 200)]
        [ProducesResponseType(404)]
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
