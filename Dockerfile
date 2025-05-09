
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API-Challenge-Visual-Software/API-Challenge-Visual-Software.csproj", "API-Challenge-Visual-Software/"]
RUN dotnet restore "./API-Challenge-Visual-Software/API-Challenge-Visual-Software.csproj"
COPY . .
WORKDIR "/src/API-Challenge-Visual-Software"
RUN dotnet build "./API-Challenge-Visual-Software.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API-Challenge-Visual-Software.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API-Challenge-Visual-Software.dll"]