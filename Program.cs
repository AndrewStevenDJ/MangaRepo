using Microsoft.OpenApi.Models;
using MiMangaBot.Data;
using MiMangaBot.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MangaAPI", Version = "v1" });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5188", policy =>
    {
        policy.WithOrigins("http://localhost:5188")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// DbContext
builder.Services.AddDbContext<MangaContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Servicios adicionales
builder.Services.AddScoped<MangaService>();
builder.Services.AddScoped<GeneroService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MangaContext>();
    
    context.Database.Migrate();

    if (args.Length > 0 && int.TryParse(args[0], out int cantidadMangasParaAgregar))
    {
        var seeder = new Seeder(context);
        await seeder.SeedAsync(cantidadMangasParaAgregar);
        Console.WriteLine($"Se agregaron {cantidadMangasParaAgregar} mangas.");
    }
    else
    {
        Console.WriteLine("No se agregaron mangas porque no se proporcionó una cantidad válida.");
    }
}

// Middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MangaAPI V1");
});

app.UseHttpsRedirection();
app.UseCors("AllowLocalhost5188");
app.UseAuthorization();
app.MapControllers();

app.Run();
