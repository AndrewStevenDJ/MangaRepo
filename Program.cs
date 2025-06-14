using Microsoft.OpenApi.Models;
using MiMangaBot.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MangaAPI", Version = "v1" });
});

// Configura CORS para permitir llamadas desde el frontend/Swagger UI
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5188", policy =>
    {
        policy.WithOrigins("http://localhost:5188")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configura el contexto con MySQL y cadena de conexión
builder.Services.AddDbContext<MangaContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MangaAPI V1");
});

app.UseHttpsRedirection();

// Habilita CORS con la política definida
app.UseCors("AllowLocalhost5188");

app.UseAuthorization();

app.MapControllers();


// Bloque para generar mangas desde consola
if (args.Length >= 2 && args[0] == "generar")
{
    if (int.TryParse(args[1], out int cantidad))
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<MangaContext>();
            Seeder.InsertarMangas(db, cantidad);
            Console.WriteLine($"Se generaron {cantidad} mangas.");
        }
    }
    else
    {
        Console.WriteLine("Cantidad inválida. Usa: dotnet run generar 10");
    }
    return; // Termina la ejecución para no levantar el servidor
}

app.Run();
