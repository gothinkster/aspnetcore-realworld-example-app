#build container
FROM microsoft/dotnet:2.2.104-sdk-alpine as build

WORKDIR /build
COPY . .
RUN dotnet tool install -g Cake.Tool
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet cake build.cake --runtime=alpine-x64

#runtime container
FROM microsoft/dotnet:2.2.2-runtime-alpine

COPY --from=build /build/publish /app
WORKDIR /app

EXPOSE 5000

RUN dotnet --list-runtimes
ENTRYPOINT ["dotnet", "Conduit.dll"]
