# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the project files first (for layer caching)
COPY Applications/Excuses.WebApi.Server/Excuses.WebApi.Server.csproj Applications/Excuses.WebApi.Server/
COPY Libraries/Excuses.Persistence.EFCore/Excuses.Persistence.EFCore.csproj Libraries/Excuses.Persistence.EFCore/
COPY Libraries/Excuses.Persistence.Shared/Excuses.Persistence.Shared.csproj Libraries/Excuses.Persistence.Shared/

# Restore dependencies
RUN dotnet restore Applications/Excuses.WebApi.Server/Excuses.WebApi.Server.csproj

# Copy everything else and build
COPY . .
WORKDIR /src/Applications/Excuses.WebApi.Server
RUN dotnet build "Excuses.WebApi.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Excuses.WebApi.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Entrypoint
ENTRYPOINT ["dotnet", "Excuses.WebApi.Server.dll"]