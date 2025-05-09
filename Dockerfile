# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . . 
RUN dotnet restore "API-Challenge-Visual-Software.csproj"
RUN dotnet build "API-Challenge-Visual-Software.csproj" -c Release -o /app/build

# Etapa 2: Publish
FROM build AS publish
RUN dotnet publish "API-Challenge-Visual-Software.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 3: Imagem de Execução
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API-Challenge-Visual-Software.dll"]
