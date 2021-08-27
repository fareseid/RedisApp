FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
EXPOSE 6778

#RUN apt update -y
#RUN apt install lsof -y

COPY bin/Release/net5.0/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "RedisServer.dll"]