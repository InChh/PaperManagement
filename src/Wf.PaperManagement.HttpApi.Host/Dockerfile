#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/Wf.PaperManagement.HttpApi.Host/Wf.PaperManagement.HttpApi.Host.csproj", "src/Wf.PaperManagement.HttpApi.Host/"]
COPY ["src/Wf.PaperManagement.Application/Wf.PaperManagement.Application.csproj", "src/Wf.PaperManagement.Application/"]
COPY ["src/Wf.PaperManagement.Domain/Wf.PaperManagement.Domain.csproj", "src/Wf.PaperManagement.Domain/"]
COPY ["src/Wf.PaperManagement.Domain.Shared/Wf.PaperManagement.Domain.Shared.csproj", "src/Wf.PaperManagement.Domain.Shared/"]
COPY ["src/Wf.PaperManagement.Application.Contracts/Wf.PaperManagement.Application.Contracts.csproj", "src/Wf.PaperManagement.Application.Contracts/"]
COPY ["src/Wf.PaperManagement.EntityFrameworkCore/Wf.PaperManagement.EntityFrameworkCore.csproj", "src/Wf.PaperManagement.EntityFrameworkCore/"]
COPY ["src/Wf.PaperManagement.HttpApi/Wf.PaperManagement.HttpApi.csproj", "src/Wf.PaperManagement.HttpApi/"]
RUN dotnet restore "./src/Wf.PaperManagement.HttpApi.Host/Wf.PaperManagement.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/src/Wf.PaperManagement.HttpApi.Host"
RUN dotnet build "./Wf.PaperManagement.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wf.PaperManagement.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wf.PaperManagement.HttpApi.Host.dll"]