# build (client) container
FROM node:12.16.3-alpine3.11 as build-client

WORKDIR /build

COPY package.json yarn.lock ./
RUN yarn

COPY config ./config
COPY scripts ./scripts
COPY public ./public
COPY src/client ./src/client
RUN yarn build

# build (server) container
FROM mcr.microsoft.com/dotnet/core/sdk:3.1.201-alpine as build-server

WORKDIR /build

# Copy all .csproj files and solution
COPY Conduit.sln .
COPY src/Conduit/Conduit.csproj ./src/Conduit/Conduit.csproj
COPY build/build.csproj ./build/build.csproj
COPY tests/Conduit.IntegrationTests/Conduit.IntegrationTests.csproj ./tests/Conduit.IntegrationTests/Conduit.IntegrationTests.csproj

RUN dotnet restore

COPY tests ./tests
COPY build ./build
COPY src/Conduit ./src/Conduit

COPY --from=build-client /build/dist/Index.cshtml ./src/Conduit/Pages/Shared/Index.cshtml
COPY --from=build-client /build/dist/static ./src/Conduit/wwwroot/static

RUN dotnet run -p build/build.csproj

#runtime container
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.3-alpine

RUN apk add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

WORKDIR /app

COPY --from=build-server /build/publish .

ENTRYPOINT ["dotnet", "Conduit.dll"]
