FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/DaprShowcase.Services.ZipWorker/DaprShowcase.Services.ZipWorker.csproj", "Services/DaprShowcase.Services.ZipWorker/"]
COPY ["Common/DaprShowcase.Common/DaprShowcase.Common.csproj", "Common/DaprShowcase.Common/"]
RUN dotnet restore "Services/DaprShowcase.Services.ZipWorker/DaprShowcase.Services.ZipWorker.csproj"
COPY . .
WORKDIR "/src/Services/DaprShowcase.Services.ZipWorker"
RUN dotnet build "DaprShowcase.Services.ZipWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DaprShowcase.Services.ZipWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaprShowcase.Services.ZipWorker.dll"]