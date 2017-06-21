FROM microsoft/dotnet:1.1.2-runtime

COPY ./publish /app
WORKDIR /app

EXPOSE 5000

ENTRYPOINT ["dotnet", "Conduit.dll"]
