#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DaprShowcase.Services.EntitiesApi/DaprShowcase.Services.EntitiesApi.csproj", "DaprShowcase.Services.EntitiesApi/"]
RUN dotnet restore "DaprShowcase.Services.EntitiesApi/DaprShowcase.Services.EntitiesApi.csproj"
COPY . .
WORKDIR "/src/DaprShowcase.Services.EntitiesApi"
RUN dotnet build "DaprShowcase.Services.EntitiesApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DaprShowcase.Services.EntitiesApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaprShowcase.Services.EntitiesApi.dll"]