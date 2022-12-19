#build container
FROM mcr.microsoft.com/dotnet/sdk:6.0.100-focal as build

WORKDIR /build
COPY .. .
RUN dotnet run --project build/build.csproj

#runtime container
FROM mcr.microsoft.com/dotnet/aspnet:6.0.12-alpine3.17-amd64
RUN apk add --no-cache tzdata

COPY --from=build /backend/build/publish /app
WORKDIR /app

EXPOSE 5000

ENTRYPOINT ["dotnet", "Conduit.dll"]
