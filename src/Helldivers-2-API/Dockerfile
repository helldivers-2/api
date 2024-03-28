FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Helldivers-2-API/Helldivers-2-API.csproj", "src/Helldivers-2-API/"]
RUN dotnet restore "src/Helldivers-2-API/Helldivers-2-API.csproj"
COPY . .
WORKDIR "/src/src/Helldivers-2-API"
RUN dotnet build "Helldivers-2-API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Helldivers-2-API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Helldivers-2-API.dll"]
