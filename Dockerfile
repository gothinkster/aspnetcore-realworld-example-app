FROM microsoft/dotnet:1.1.1-runtime

COPY ./publish /app
WORKDIR /app

EXPOSE 5000

ENTRYPOINT ["dotnet", "RealWorld.dll"]
