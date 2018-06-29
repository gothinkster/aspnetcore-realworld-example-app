#build container
FROM microsoft/dotnet:2.1.301-sdk as build

#install unzip for Cake
RUN apt-get update
RUN apt-get install -y unzip

WORKDIR /build
COPY . .
RUN ./build.sh

#runtime container
FROM microsoft/dotnet:2.1.1-runtime

COPY --from=build /build/publish /app
WORKDIR /app

EXPOSE 5000

ENTRYPOINT ["dotnet", "Conduit.dll"]
