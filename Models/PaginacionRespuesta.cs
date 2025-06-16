namespace MiMangaBot.Models
{
    public class PaginacionRespuesta<T>
    {
        public List<T> Datos { get; set; } = new();
        public int PaginaActual { get; set; }
        public int TamañoPagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
    }
}
