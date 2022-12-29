#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Adnc.Usr/Adnc.Usr.WebApi/Adnc.Usr.WebApi.csproj", "Services/Adnc.Usr/Adnc.Usr.WebApi/"]
COPY ["Services/Adnc.Usr/Adnc.Usr.Application/Adnc.Usr.Application.csproj", "Services/Adnc.Usr/Adnc.Usr.Application/"]
COPY ["Services/Adnc.Usr/Adnc.Usr.Repository/Adnc.Usr.Repository.csproj", "Services/Adnc.Usr/Adnc.Usr.Repository/"]
COPY ["Infrastructures/Adnc.Infra.Repository/Adnc.Infra.Repository.csproj", "Infrastructures/Adnc.Infra.Repository/"]
COPY ["Infrastructures/Adnc.Infra.Core/Adnc.Infra.Core.csproj", "Infrastructures/Adnc.Infra.Core/"]
COPY ["Services/Shared/Adnc.Shared/Adnc.Shared.csproj", "Services/Shared/Adnc.Shared/"]
COPY ["Services/Shared/Adnc.Shared.Rpc/Adnc.Shared.Rpc.csproj", "Services/Shared/Adnc.Shared.Rpc/"]
COPY ["Services/Shared/Adnc.Shared.Application/Adnc.Shared.Application.csproj", "Services/Shared/Adnc.Shared.Application/"]
COPY ["Infrastructures/Adnc.Infra.Mapper/Adnc.Infra.Mapper.csproj", "Infrastructures/Adnc.Infra.Mapper/"]
COPY ["Infrastructures/Adnc.Infra.Helper/Adnc.Infra.Helper.csproj", "Infrastructures/Adnc.Infra.Helper/"]
COPY ["Infrastructures/Adnc.Infra.EventBus/Adnc.Infra.EventBus.csproj", "Infrastructures/Adnc.Infra.EventBus/"]
COPY ["Infrastructures/Adnc.Infra.EfCore.MySql/Adnc.Infra.EfCore.MySql.csproj", "Infrastructures/Adnc.Infra.EfCore.MySql/"]
COPY ["Infrastructures/Adnc.Infra.Dapper/Adnc.Infra.Dapper.csproj", "Infrastructures/Adnc.Infra.Dapper/"]
COPY ["Infrastructures/Adnc.Infra.Mongo/Adnc.Infra.Mongo.csproj", "Infrastructures/Adnc.Infra.Mongo/"]
COPY ["Services/Shared/Adnc.Shared.Application.Contracts/Adnc.Shared.Application.Contracts.csproj", "Services/Shared/Adnc.Shared.Application.Contracts/"]
COPY ["Infrastructures/Adnc.Infra.Caching/Adnc.Infra.Caching.csproj", "Infrastructures/Adnc.Infra.Caching/"]
COPY ["Infrastructures/Adnc.Infra.IdGenerater/Adnc.Infra.IdGenerater.csproj", "Infrastructures/Adnc.Infra.IdGenerater/"]
COPY ["Infrastructures/Adnc.Infra.Consul/Adnc.Infra.Consul.csproj", "Infrastructures/Adnc.Infra.Consul/"]
COPY ["Services/Adnc.Usr/Adnc.Usr.Application.Contracts/Adnc.Usr.Application.Contracts.csproj", "Services/Adnc.Usr/Adnc.Usr.Application.Contracts/"]
COPY ["Services/Shared/Adnc.Shared.WebApi/Adnc.Shared.WebApi.csproj", "Services/Shared/Adnc.Shared.WebApi/"]
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