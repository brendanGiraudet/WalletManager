FROM mcr.microsoft.com/dotnet/core/aspnet:2.1

RUN mkdir /app
WORKDIR /app

COPY . .

EXPOSE 7501/tcp
CMD ["dotnet", "WalletManagerSite.dll"]