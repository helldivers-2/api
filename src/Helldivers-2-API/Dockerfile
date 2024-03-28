FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-alpine-extra AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
RUN apk add --upgrade --no-cache \
        build-base \
        clang \
        zlib-dev
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Helldivers-2-API/Helldivers-2-API.csproj", "Helldivers-2-API/"]
COPY ["src/Helldivers-2-Models/Helldivers-2-Models.csproj", "Helldivers-2-Models/"]
COPY ["src/Helldivers-2-Sync/Helldivers-2-Sync.csproj", "Helldivers-2-Sync/"]
COPY ["src/Helldivers-2-Core/Helldivers-2-Core.csproj", "Helldivers-2-Core/"]
COPY ["src/Helldivers-2-SourceGen/Helldivers-2-SourceGen.csproj", "Helldivers-2-SourceGen/"]
COPY ["Directory.Build.props", "."]

RUN dotnet restore "Helldivers-2-API/Helldivers-2-API.csproj"
COPY ./src .
WORKDIR "/src/Helldivers-2-API"
RUN dotnet build "Helldivers-2-API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Helldivers-2-API.csproj" --self-contained -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
COPY --from=publish /app/publish .
ENTRYPOINT ["./Helldivers-2-API"]
