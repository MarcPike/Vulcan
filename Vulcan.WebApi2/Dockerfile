FROM microsoft/dotnet-framework:4.61
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "Vulcan.WebApi2.dll"]
