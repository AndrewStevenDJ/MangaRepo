# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia archivos del proyecto
COPY ["MiMangaBot.csproj", "./"]
RUN dotnet restore "./MiMangaBot.csproj"

COPY . .
RUN dotnet publish "./MiMangaBot.csproj" -c Release -o /app/publish

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expone el puerto por defecto de ASP.NET
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "MiMangaBot.dll"]
