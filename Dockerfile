#build container
FROM microsoft/dotnet:2.1.401-sdk as build

WORKDIR /build
COPY . .
RUN dotnet tool install -g Cake.Tool
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet cake build.cake 

#runtime container
FROM microsoft/dotnet:2.1.3-runtime

COPY --from=build /build/publish /app
WORKDIR /app

EXPOSE 5000

ENTRYPOINT ["dotnet", "Conduit.dll"]
