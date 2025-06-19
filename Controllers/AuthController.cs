using Microsoft.AspNetCore.Mvc;
using MiMangaBot.Services;

namespace MiMangaBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthController(JwtTokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Para este ejemplo, validamos contra credenciales fijas (puedes cambiar esto a una base de datos después).
            if (request.Username == "admin" && request.Password == "1234")
            {
                var token = _tokenGenerator.GenerarToken(request.Username);
                return Ok(new { token });
            }

            return Unauthorized("Credenciales inválidas");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
