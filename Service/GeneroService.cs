using Microsoft.EntityFrameworkCore;
using MiMangaBot.Data;
using MiMangaBot.Models;

namespace MiMangaBot.Services
{
    public class GeneroService
    {
        private readonly MangaContext _context;

        public GeneroService(MangaContext context)
        {
            _context = context;
        }

        // Obtener todos los géneros
        public async Task<List<Genero>> ObtenerGenerosAsync()
        {
            return await _context.Generos.ToListAsync();
        }

        // Obtener género por ID
        public async Task<Genero?> ObtenerGeneroPorIdAsync(int id)
        {
            return await _context.Generos.FindAsync(id);
        }

        // Crear un nuevo género
        public async Task<Genero> CrearGeneroAsync(Genero genero)
        {
            // Validar que no exista un género con el mismo nombre (opcional)
            var existente = await _context.Generos
                .FirstOrDefaultAsync(g => g.Nombre.ToLower() == genero.Nombre.ToLower());

            if (existente != null)
                throw new Exception($"Ya existe un género con el nombre '{genero.Nombre}'.");

            _context.Generos.Add(genero);
            await _context.SaveChangesAsync();
            return genero;
        }

        // Eliminar un género por ID
        public async Task<bool> EliminarGeneroAsync(int id)
        {
            var genero = await _context.Generos.FindAsync(id);
            if (genero == null) return false;

            _context.Generos.Remove(genero);
            await _context.SaveChangesAsync();
            return true;
        }

        // Actualizar un género existente
        public async Task<Genero?> ActualizarGeneroAsync(int id, Genero generoActualizado)
        {
            var genero = await _context.Generos.FindAsync(id);
            if (genero == null) return null;

            genero.Nombre = generoActualizado.Nombre;
            await _context.SaveChangesAsync();
            return genero;
        }
    }
}
