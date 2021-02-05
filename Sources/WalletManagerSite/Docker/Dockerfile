FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS base
WORKDIR /app
EXPOSE 7501

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS build
WORKDIR /src
COPY ["./WalletManagerSite.csproj", "WalletManagerSite/"]
WORKDIR "/src/WalletManagerSite"
RUN dotnet restore "./WalletManagerSite.csproj"
COPY . .
RUN dotnet build "WalletManagerSite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WalletManagerSite.csproj" -c Release -o /app/publish


FROM base AS final
ENV ASPNETCORE_URLS=http://+:7501
ENV ASPNETCORE_HTTPS_PORT=7501
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "WalletManagerSite.dll"]