#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Portal/Adnc.Warehouse/Adnc.Whse.WebApi/Adnc.Whse.WebApi.csproj", "Portal/Adnc.Warehouse/Adnc.Whse.WebApi/"]
COPY ["Adnc.Infra.Mongo/23.Adnc.Infra.Mongo.csproj", "Adnc.Infra.Mongo/"]
COPY ["Adnc.Core.Shared/03.Adnc.Core.Shared.csproj", "Adnc.Core.Shared/"]
COPY ["Adnc.Infra.Common/04.Adnc.Infra.Common.csproj", "Adnc.Infra.Common/"]
COPY ["Portal/Adnc.Warehouse/Adnc.Whse.Application/Adnc.Whse.Application.csproj", "Portal/Adnc.Warehouse/Adnc.Whse.Application/"]
COPY ["Adnc.Infra.Caching/21.Adnc.Infra.Caching.csproj", "Adnc.Infra.Caching/"]
COPY ["Portal/Adnc.Warehouse/Adnc.Whse.Core/Adnc.Whse.Core.csproj", "Portal/Adnc.Warehouse/Adnc.Whse.Core/"]
COPY ["Adnc.Application.Shared/02.Adnc.Application.Shared.csproj", "Adnc.Application.Shared/"]
COPY ["Adnc.Infra.Mq/24.Adnc.Infra.Mq.csproj", "Adnc.Infra.Mq/"]
COPY ["Adnc.Infra.EfCore/22.Adnc.Infra.EfCore.csproj", "Adnc.Infra.EfCore/"]
COPY ["Adnc.Infra.Consul/20.Adnc.Infra.Consul.csproj", "Adnc.Infra.Consul/"]
COPY ["Adnc.WebApi.Shared/01.Adnc.WebApi.Shared.csproj", "Adnc.WebApi.Shared/"]
RUN dotnet restore "Portal/Adnc.Warehouse/Adnc.Whse.WebApi/Adnc.Whse.WebApi.csproj"
COPY . .
WORKDIR "/src/Portal/Adnc.Warehouse/Adnc.Whse.WebApi"
RUN dotnet build "Adnc.Whse.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Adnc.Whse.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Adnc.Whse.WebApi.dll"]
