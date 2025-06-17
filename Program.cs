using Microsoft.OpenApi.Models;
using MiMangaBot.Data;
using MiMangaBot.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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

// Base de datos
builder.Services.AddDbContext<MangaContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Servicios
builder.Services.AddScoped<MangaService>();

var app = builder.Build();

// ✅ Ejecutar el seeder automático al iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MangaContext>();
    await Seeder.SeedAsync(db);
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
