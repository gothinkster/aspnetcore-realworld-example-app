#build container
FROM microsoft/dotnet:2.0.7-sdk-2.1.200 as build

#install unzip for Cake
RUN apt-get update
RUN apt-get install -y unzip

WORKDIR /build
COPY . .
RUN ./build.sh

#runtime container
FROM microsoft/dotnet:2.0.7-runtime

COPY --from=build /build/publish /app
WORKDIR /app

EXPOSE 5000

ENTRYPOINT ["dotnet", "Conduit.dll"]
