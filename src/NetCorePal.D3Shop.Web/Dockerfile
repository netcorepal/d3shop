#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/NetCorePal.D3Shop.Web/NetCorePal.D3Shop.Web.csproj"
WORKDIR "/src/src/NetCorePal.D3Shop.Web"
RUN dotnet build "NetCorePal.D3Shop.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetCorePal.D3Shop.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCorePal.D3Shop.Web.dll"]