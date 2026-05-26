FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy directory build/package properties
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]

# Copy project files
COPY ["src/AutoVerdikt.Domain/AutoVerdikt.Domain.csproj", "src/AutoVerdikt.Domain/"]
COPY ["src/AutoVerdikt.WebApi/AutoVerdikt.WebApi.csproj", "src/AutoVerdikt.WebApi/"]
COPY ["src/AutoVerdikt.Application/AutoVerdikt.Application.csproj", "src/AutoVerdikt.Application/"]
COPY ["src/AutoVerdikt.Infrastructure/AutoVerdikt.Infrastructure.csproj", "src/AutoVerdikt.Infrastructure/"]
COPY ["src/AutoVerdikt.Store/AutoVerdikt.Store.csproj", "src/AutoVerdikt.Store/"]
COPY ["src/AutoVerdikt.Agents/AutoVerdikt.Agents.csproj", "src/AutoVerdikt.Agents/"]

RUN dotnet restore "./src/AutoVerdikt.WebApi/AutoVerdikt.WebApi.csproj"

# Copy all source code
COPY . .
WORKDIR "/src/src/AutoVerdikt.WebApi"
RUN dotnet build "./AutoVerdikt.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AutoVerdikt.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
USER root
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID
HEALTHCHECK --interval=30s --timeout=5s --start-period=15s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutoVerdikt.WebApi.dll"]
