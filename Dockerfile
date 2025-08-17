# ---------- build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1) Solution file
COPY ["E-Ticaret Project.sln", "./"]

# 2) Project csproj files
COPY ["src/Presentation/E-Ticaret Project.WebApi/E-Ticaret Project.WebApi.csproj", "src/Presentation/E-Ticaret Project.WebApi/"]
COPY ["src/Core/E-Ticaret Project.Application/E-Ticaret Project.Application.csproj", "src/Core/E-Ticaret Project.Application/"]
COPY ["src/Core/E-Ticaret Project.Domain/E-Ticaret Project.Domain.csproj", "src/Core/E-Ticaret Project.Domain/"]
COPY ["src/Infrastructure/E-Ticaret Project.Infrastructure/E-Ticaret Project.Infrastructure.csproj", "src/Infrastructure/E-Ticaret Project.Infrastructure/"]
COPY ["src/Infrastructure/E-Ticaret Project.Persistence/E-Ticaret Project.Persistence.csproj", "src/Infrastructure/E-Ticaret Project.Persistence/"]

RUN dotnet restore "./E-Ticaret Project.sln"

COPY . .
RUN dotnet publish "src/Presentation/E-Ticaret Project.WebApi/E-Ticaret Project.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---------- runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Lokal test ucun 8080-i gosteririk (Render PORT verir)
EXPOSE 8080

COPY --from=build /app/publish .

# Burada bash runtime-da $PORT varsa onu istifade edir, yoxdursa 8080
ENTRYPOINT ["bash", "-c", "dotnet 'E-Ticaret Project.WebApi.dll' --urls http://0.0.0.0:${PORT:-8080}"]