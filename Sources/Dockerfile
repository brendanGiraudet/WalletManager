FROM mcr.microsoft.com/dotnet/core/aspnet:2.1 AS base
WORKDIR /app
EXPOSE 7501

FROM mcr.microsoft.com/dotnet/sdk:2.1 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
ENV ASPNETCORE_URLS=https://+:7501
ENV ASPNETCORE_HTTPS_PORT=7501
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "WalletManagerSite.dll"]