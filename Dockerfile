FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Kopiér PUBLISH-output fra workflowet
COPY ./publish ./

ENTRYPOINT ["dotnet", "Median.Intranet.dll"]