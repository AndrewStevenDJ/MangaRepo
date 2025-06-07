using Microsoft.AspNetCore.Mvc;
using MiMangaBot.Data;
using MiMangaBot.Models;

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

        [HttpGet]
        public ActionResult<IEnumerable<Manga>> GetMangas()
        {
            return _context.Mangas.ToList();
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
