FROM mcr.microsoft.com/azure-functions/dotnet:3.3.1-appservice AS base
ADD ./Certs/nscacert.pem /etc/ssl/certs/nscacert.pem
ADD ./Certs/nstenantcert.pem /etc/ssl/certs/nstenantcert.pem
RUN update-ca-certificates
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Services/DaprShowcase.Services.Orchestrator/DaprShowcase.Services.Orchestrator.csproj", "Services/DaprShowcase.Services.Orchestrator/"]
RUN dotnet restore "Services/DaprShowcase.Services.Orchestrator/DaprShowcase.Services.Orchestrator.csproj"
COPY . .
WORKDIR "/src/Services/DaprShowcase.Services.Orchestrator"
RUN dotnet build "DaprShowcase.Services.Orchestrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DaprShowcase.Services.Orchestrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true