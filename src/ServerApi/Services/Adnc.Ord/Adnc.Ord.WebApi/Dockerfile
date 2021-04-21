#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Portal/Adnc.Ord/Adnc.Ord.WebApi/Adnc.Ord.WebApi.csproj", "Portal/Adnc.Ord/Adnc.Ord.WebApi/"]
COPY ["Adnc.Infr.Mongo/23.Adnc.Infr.Mongo.csproj", "Adnc.Infr.Mongo/"]
COPY ["Adnc.Core.Shared/03.Adnc.Core.Shared.csproj", "Adnc.Core.Shared/"]
COPY ["Adnc.Infr.Common/04.Adnc.Infr.Common.csproj", "Adnc.Infr.Common/"]
COPY ["Portal/Adnc.Ord/Adnc.Ord.Application/Adnc.Ord.Application.csproj", "Portal/Adnc.Ord/Adnc.Ord.Application/"]
COPY ["Adnc.Infr.EasyCaching/21.Adnc.Infr.EasyCaching.csproj", "Adnc.Infr.EasyCaching/"]
COPY ["Portal/Adnc.Ord/Adnc.Ord.Core/Adnc.Ord.Core.csproj", "Portal/Adnc.Ord/Adnc.Ord.Core/"]
COPY ["Adnc.Application.Shared/02.Adnc.Application.Shared.csproj", "Adnc.Application.Shared/"]ss
COPY ["Adnc.Infr.Mq/24.Adnc.Infr.Mq.csproj", "Adnc.Infr.Mq/"]
COPY ["Adnc.Infr.EfCore/22.Adnc.Infr.EfCore.csproj", "Adnc.Infr.EfCore/"]
COPY ["Adnc.Infr.Consul/20.Adnc.Infr.Consul.csproj", "Adnc.Infr.Consul/"]
COPY ["Adnc.WebApi.Shared/01.Adnc.WebApi.Shared.csproj", "Adnc.WebApi.Shared/"]
RUN dotnet restore "Portal/Adnc.Ord/Adnc.Ord.WebApi/Adnc.Ord.WebApi.csproj"ss
COPY . .
WORKDIR "/src/Portal/Adnc.Ord/Adnc.Ord.WebApi"
RUN dotnet build "Adnc.Ord.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Adnc.Ord.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Adnc.Ord.WebApi.dll"]
