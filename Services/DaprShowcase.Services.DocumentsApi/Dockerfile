FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DaprShowcase.Services.DocumentsApi/DaprShowcase.Services.DocumentsApi.csproj", "DaprShowcase.Services.DocumentsApi/"]
RUN dotnet restore "DaprShowcase.Services.DocumentsApi/DaprShowcase.Services.DocumentsApi.csproj"
COPY . .
WORKDIR "/src/DaprShowcase.Services.DocumentsApi"
RUN dotnet build "DaprShowcase.Services.DocumentsApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DaprShowcase.Services.DocumentsApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaprShowcase.Services.DocumentsApi.dll"]