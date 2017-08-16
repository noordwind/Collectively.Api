FROM microsoft/dotnet:1.1.2-runtime
WORKDIR /dotnetapp
COPY ./bin/Docker .
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker
ENTRYPOINT dotnet Collectively.Api.dll