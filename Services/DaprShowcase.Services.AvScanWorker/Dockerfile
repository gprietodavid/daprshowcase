FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/DaprShowcase.Services.AvScanWorker/DaprShowcase.Services.AvScanWorker.csproj", "Services/DaprShowcase.Services.AvScanWorker/"]
RUN dotnet restore "Services/DaprShowcase.Services.AvScanWorker/DaprShowcase.Services.AvScanWorker.csproj"
COPY . .
WORKDIR "/src/Services/DaprShowcase.Services.AvScanWorker"
RUN dotnet build "DaprShowcase.Services.AvScanWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DaprShowcase.Services.AvScanWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaprShowcase.Services.AvScanWorker.dll"]