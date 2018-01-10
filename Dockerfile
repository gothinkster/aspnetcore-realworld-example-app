FROM microsoft/dotnet:2.0.5-runtime

COPY ./publish /app
WORKDIR /app

EXPOSE 5000

ENTRYPOINT ["dotnet", "Conduit.dll"]
