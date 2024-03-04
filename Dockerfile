#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["build/build.csproj", "build/"]
RUN dotnet restore "build/build.csproj"
COPY . .
WORKDIR "/src/build"
RUN dotnet build "build.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "build.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
EXPOSE 5000

ENTRYPOINT ["dotnet", "Conduit.dll"]
