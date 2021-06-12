#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Adnc.Usr/Adnc.Usr.WebApi/Adnc.Usr.WebApi.csproj", "Services/Adnc.Usr/Adnc.Usr.WebApi/"]
COPY ["Services/Adnc.Usr/Adnc.Usr.Application/Adnc.Usr.Application.csproj", "Services/Adnc.Usr/Adnc.Usr.Application/"]
COPY ["Services/Adnc.Usr/Adnc.Usr.Core/Adnc.Usr.Core.csproj", "Services/Adnc.Usr/Adnc.Usr.Core/"]
COPY ["Services/Shared/Adnc.Core.Shared/Adnc.Core.Shared.csproj", "Services/Shared/Adnc.Core.Shared/"]
COPY ["Infrastructures/Adnc.Infra.Common/Adnc.Infra.Common.csproj", "Infrastructures/Adnc.Infra.Common/"]
COPY ["Infrastructures/Adnc.Infra.EventBus/Adnc.Infra.EventBus.csproj", "Infrastructures/Adnc.Infra.EventBus/"]
COPY ["Services/Adnc.Usr/Adnc.Usr.Application.Contracts/Adnc.Usr.Application.Contracts.csproj", "Services/Adnc.Usr/Adnc.Usr.Application.Contracts/"]
COPY ["Services/Shared/Adnc.Application.Shared/Adnc.Application.Shared.csproj", "Services/Shared/Adnc.Application.Shared/"]
COPY ["Services/Shared/Adnc.Application.RpcService/Adnc.Application.RpcService.csproj", "Services/Shared/Adnc.Application.RpcService/"]
COPY ["Infrastructures/Adnc.Infra.Mapper/Adnc.Infra.Mapper.csproj", "Infrastructures/Adnc.Infra.Mapper/"]
COPY ["Infrastructures/Adnc.Infra.Caching/Adnc.Infra.Caching.csproj", "Infrastructures/Adnc.Infra.Caching/"]
COPY ["Services/Shared/Adnc.WebApi.Shared/Adnc.WebApi.Shared.csproj", "Services/Shared/Adnc.WebApi.Shared/"]
COPY ["Infrastructures/Adnc.Infra.EfCore/Adnc.Infra.EfCore.csproj", "Infrastructures/Adnc.Infra.EfCore/"]
COPY ["Infrastructures/Adnc.Infra.Mongo/Adnc.Infra.Mongo.csproj", "Infrastructures/Adnc.Infra.Mongo/"]
COPY ["Infrastructures/Adnc.Infra.Consul/Adnc.Infra.Consul.csproj", "Infrastructures/Adnc.Infra.Consul/"]
RUN dotnet restore "Services/Adnc.Usr/Adnc.Usr.WebApi/Adnc.Usr.WebApi.csproj"
COPY . .
WORKDIR "/src/Services/Adnc.Usr/Adnc.Usr.WebApi"
RUN dotnet build "Adnc.Usr.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Adnc.Usr.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Adnc.Usr.WebApi.dll"]