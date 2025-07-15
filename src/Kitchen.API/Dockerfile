# FASTTECHFOODS.KITCHEN
# BASE
FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# BUILD
FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Kitchen.API/Kitchen.API.csproj", "src/Kitchen.API/"]
COPY ["src/Kitchen.Application/Kitchen.Application.csproj", "src/Kitchen.Application/"]
COPY ["src/Kitchen.Domain/Kitchen.Domain.csproj", "src/Kitchen.Domain/"]
COPY ["src/Kitchen.Infrastructure/Kitchen.Infrastructure.csproj", "src/Kitchen.Infrastructure/"]
RUN dotnet restore "./src/Kitchen.API/Kitchen.API.csproj"
COPY . .
WORKDIR "/src/src/Kitchen.API"
RUN dotnet build "./Kitchen.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# PUBLISH
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Kitchen.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# RUN
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kitchen.API.dll"]