FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Matchmaker/Matchmaker.csproj", "src/Matchmaker/"]
COPY ["src/Matchmaker.Api/Matchmaker.Api.csproj", "src/Matchmaker.Api/"]
COPY ["src/Matchmaker.Domain/Matchmaker.Domain.csproj", "src/Matchmaker.Domain/"]
COPY ["src/Matchmaker.Infra.Data/Matchmaker.Infra.Data.csproj", "src/Matchmaker.Infra.Data/"]
COPY ["src/Matchmaker.Infra.Http/Matchmaker.Infra.Http.csproj", "src/Matchmaker.Infra.Http/"]
RUN dotnet restore "src/Matchmaker/Matchmaker.csproj"
COPY . .
WORKDIR "/src/src/Matchmaker"
RUN dotnet build "Matchmaker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Matchmaker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Matchmaker.dll"]
