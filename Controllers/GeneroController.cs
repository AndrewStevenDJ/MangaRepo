using Microsoft.AspNetCore.Mvc;
using MiMangaBot.Models;
using MiMangaBot.Services;

namespace MiMangaBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeneroController : ControllerBase
    {
        private readonly GeneroService _generoService;

        public GeneroController(GeneroService generoService)
        {
            _generoService = generoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Genero>>> GetGeneros()
        {
            var generos = await _generoService.ObtenerGenerosAsync();
            return Ok(generos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Genero>> GetGenero(int id)
        {
            var genero = await _generoService.ObtenerGeneroPorIdAsync(id);
            if (genero == null)
                return NotFound($"No se encontró el género con ID {id}");

            return Ok(genero);
        }

        [HttpPost]
        public async Task<ActionResult<Genero>> PostGenero(Genero genero)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevoGenero = await _generoService.CrearGeneroAsync(genero);
            return CreatedAtAction(nameof(GetGenero), new { id = nuevoGenero.Id }, nuevoGenero);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenero(int id, Genero generoActualizado)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenero(int id)
        {
            var eliminado = await _generoService.EliminarGeneroAsync(id);
            if (!eliminado)
                return NotFound($"No se encontró el género con ID {id} para eliminar.");

            return Ok($"Género con ID {id} eliminado correctamente.");
        }
    }
}
