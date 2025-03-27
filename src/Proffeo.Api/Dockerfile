FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Proffeo.Api/Proffeo.Api.csproj", "src/Proffeo.Api/"]
COPY ["src/Proffeo.Infrastructure/Proffeo.Infrastructure.csproj", "src/Proffeo.Infrastructure/"]
COPY ["src/Proffeo.Models/Proffeo.Models.csproj", "src/Proffeo.Models/"]
COPY ["src/Proffeo.Services/Proffeo.Services.csproj", "src/Proffeo.Services/"]
RUN dotnet restore "src/Proffeo.Api/Proffeo.Api.csproj"

COPY . .
WORKDIR "/src/src/Proffeo.Api"
RUN dotnet build "Proffeo.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Proffeo.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Proffeo.Api.dll"]
