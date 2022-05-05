FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Services/DaprShowcase.Services.AuditingWorker/DaprShowcase.Services.AuditingWorker.csproj", "Services/DaprShowcase.Services.AuditingWorker/"]
COPY ["Common/DaprShowcase.Common/DaprShowcase.Common.csproj", "Common/DaprShowcase.Common/"]
RUN dotnet restore "Services/DaprShowcase.Services.AuditingWorker/DaprShowcase.Services.AuditingWorker.csproj"
COPY . .
WORKDIR "/src/Services/DaprShowcase.Services.AuditingWorker"
RUN dotnet build "DaprShowcase.Services.AuditingWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DaprShowcase.Services.AuditingWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaprShowcase.Services.AuditingWorker.dll"]