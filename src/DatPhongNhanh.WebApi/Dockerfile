#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DatPhongNhanh.WebApi/DatPhongNhanh.WebApi.csproj", "DatPhongNhanh.WebApi/"]
COPY ["DatPhongNhanh.Application/DatPhongNhanh.Application.csproj", "DatPhongNhanh.Application/"]
COPY ["DatPhongNhanh.Domain/DatPhongNhanh.Domain.csproj", "DatPhongNhanh.Domain/"]
COPY ["DatPhongNhanh.Shared/DatPhongNhanh.Shared.csproj", "DatPhongNhanh.Shared/"]
COPY ["DatPhongNhanh.Infrastructure/DatPhongNhanh.Infrastructure.csproj", "DatPhongNhanh.Infrastructure/"]
RUN dotnet restore "DatPhongNhanh.WebApi/DatPhongNhanh.WebApi.csproj"
COPY . .
WORKDIR "/src/DatPhongNhanh.WebApi"
RUN dotnet build "./DatPhongNhanh.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DatPhongNhanh.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DatPhongNhanh.WebApi.dll"]