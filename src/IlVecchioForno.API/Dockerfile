FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["src/IlVecchioForno.API/IlVecchioForno.API.csproj", "src/IlVecchioForno.API/"]
COPY ["src/IlVecchioForno.Infrastructure/IlVecchioForno.Infrastructure.csproj", "src/IlVecchioForno.Infrastructure/"]
COPY ["src/IlVecchioForno.Application/IlVecchioForno.Application.csproj", "src/IlVecchioForno.Application/"]
COPY ["src/IlVecchioForno.Domain/IlVecchioForno.Domain.csproj", "src/IlVecchioForno.Domain/"]
RUN dotnet restore "src/IlVecchioForno.API/IlVecchioForno.API.csproj"
COPY . .
WORKDIR "/src/src/IlVecchioForno.API"
RUN dotnet build "./IlVecchioForno.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IlVecchioForno.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IlVecchioForno.API.dll"]
