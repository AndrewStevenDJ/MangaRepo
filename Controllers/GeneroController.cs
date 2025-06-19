using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiMangaBot.Models;
using MiMangaBot.Services;

namespace MiMangaBot.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GeneroController : ControllerBase
    {
        private readonly GeneroService _generoService;

        public GeneroController(GeneroService generoService)
        {
            _generoService = generoService;
        }

        /// <summary>
        /// Obtiene todos los géneros.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<Genero>), 200)]
        public async Task<ActionResult<List<Genero>>> GetGeneros()
        {
            var generos = await _generoService.ObtenerGenerosAsync();
            return Ok(generos);
        }

        /// <summary>
        /// Obtiene un género por su ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Genero), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Genero>> GetGenero(int id)
        {
            var genero = await _generoService.ObtenerGeneroPorIdAsync(id);
            if (genero == null)
                return NotFound($"No se encontró el género con ID {id}");

            return Ok(genero);
        }

        /// <summary>
        /// Crea un nuevo género.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Genero), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Genero>> PostGenero([FromBody] Genero genero)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevoGenero = await _generoService.CrearGeneroAsync(genero);
            return CreatedAtAction(nameof(GetGenero), new { id = nuevoGenero.Id }, nuevoGenero);
        }

        /// <summary>
        /// Actualiza un género por ID.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Genero), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutGenero(int id, [FromBody] Genero generoActualizado)
        {
            if (id != generoActualizado.Id)
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var actualizado = await _generoService.ActualizarGeneroAsync(id, generoActualizado);
            if (actualizado == null)
                return NotFound($"No se encontró el género con ID {id} para actualizar.");

            return Ok(actualizado);
        }

        /// <summary>
        /// Elimina un género por ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteGenero(int id)
        {
            var eliminado = await _generoService.EliminarGeneroAsync(id);
            if (!eliminado)
                return NotFound($"No se encontró el género con ID {id} para eliminar.");

            return Ok($"Género con ID {id} eliminado correctamente.");
        }
    }
}
