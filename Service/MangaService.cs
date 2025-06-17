using Microsoft.EntityFrameworkCore;
using MiMangaBot.Data;
using MiMangaBot.Models;

namespace MiMangaBot.Services
{
    public class MangaService
    {
        private readonly MangaContext _context;

        public MangaService(MangaContext context)
        {
            _context = context;
        }

        // Método con paginación y metadatos
        public async Task<PaginacionRespuesta<Manga>> ObtenerMangasAsync(
            int? id,
            string? titulo,
            string? autor,
            int? anio,
            int page = 1,
            int pageSize = 10)
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

            var totalRegistros = await query.CountAsync();
            var totalPaginas = (int)Math.Ceiling((double)totalRegistros / pageSize);

            var datos = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginacionRespuesta<Manga>
            {
                Datos = datos,
                PaginaActual = page,
                TamañoPagina = pageSize,
                TotalRegistros = totalRegistros,
                TotalPaginas = totalPaginas
            };
        }

        public async Task<Manga> CrearMangaAsync(Manga manga)
        {
            _context.Mangas.Add(manga);
            await _context.SaveChangesAsync();
            return manga;
        }

        public async Task<List<Manga>> EliminarMangasAsync(int? id, string? titulo)
        {
            var query = _context.Mangas.AsQueryable();

            if (id.HasValue)
                query = query.Where(m => m.Id == id.Value);

            if (!string.IsNullOrEmpty(titulo))
                query = query.Where(m => EF.Functions.Like(m.Titulo, $"%{titulo}%"));

            var mangasAEliminar = await query.ToListAsync();

            if (mangasAEliminar.Count > 0)
            {
                _context.Mangas.RemoveRange(mangasAEliminar);
                await _context.SaveChangesAsync();
            }

            return mangasAEliminar;
        }

        public async Task<Manga?> ActualizarMangaAsync(int? id, string? titulo, Manga mangaActualizado)
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

            if (mangaExistente == null)
                return null;

            mangaExistente.Titulo = mangaActualizado.Titulo;
            mangaExistente.Autor = mangaActualizado.Autor;
            mangaExistente.GeneroId = mangaActualizado.GeneroId;
            mangaExistente.Anio = mangaActualizado.Anio;

            await _context.SaveChangesAsync();

            return mangaExistente;
        }
    }
}
