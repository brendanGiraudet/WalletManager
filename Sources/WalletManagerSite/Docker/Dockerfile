FROM mcr.microsoft.com/dotnet/core/aspnet:2.1
WORKDIR /app
EXPOSE 7501

COPY ./Source .

ENV ASPNETCORE_URLS=http://+:7501

EXPOSE 7501/tcp
CMD ["dotnet", "WalletManagerSite.dll"]