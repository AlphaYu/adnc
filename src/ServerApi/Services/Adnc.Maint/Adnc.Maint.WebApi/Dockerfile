#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Adnc.Maint/Adnc.Maint.WebApi/Adnc.Maint.WebApi.csproj", "Services/Adnc.Maint/Adnc.Maint.WebApi/"]
COPY ["Services/Shared/Adnc.Shared.WebApi/Adnc.Shared.WebApi.csproj", "Services/Shared/Adnc.Shared.WebApi/"]
COPY ["Infrastructures/Adnc.Infra.Helper/Adnc.Infra.Helper.csproj", "Infrastructures/Adnc.Infra.Helper/"]
COPY ["Infrastructures/Adnc.Infra.Core/Adnc.Infra.Core.csproj", "Infrastructures/Adnc.Infra.Core/"]
COPY ["Infrastructures/Adnc.Infra.EventBus/Adnc.Infra.EventBus.csproj", "Infrastructures/Adnc.Infra.EventBus/"]
COPY ["Services/Shared/Adnc.Shared.Application.Contracts/Adnc.Shared.Application.Contracts.csproj", "Services/Shared/Adnc.Shared.Application.Contracts/"]
COPY ["Services/Shared/Adnc.Shared.Rpc/Adnc.Shared.Rpc.csproj", "Services/Shared/Adnc.Shared.Rpc/"]
COPY ["Services/Shared/Adnc.Shared/Adnc.Shared.csproj", "Services/Shared/Adnc.Shared/"]
COPY ["Infrastructures/Adnc.Infra.Caching/Adnc.Infra.Caching.csproj", "Infrastructures/Adnc.Infra.Caching/"]
COPY ["Infrastructures/Adnc.Infra.Consul/Adnc.Infra.Consul.csproj", "Infrastructures/Adnc.Infra.Consul/"]
COPY ["Services/Adnc.Maint/Adnc.Maint.Application/Adnc.Maint.Application.csproj", "Services/Adnc.Maint/Adnc.Maint.Application/"]
COPY ["Services/Shared/Adnc.Shared.Application/Adnc.Shared.Application.csproj", "Services/Shared/Adnc.Shared.Application/"]
COPY ["Infrastructures/Adnc.Infra.Mapper/Adnc.Infra.Mapper.csproj", "Infrastructures/Adnc.Infra.Mapper/"]
COPY ["Infrastructures/Adnc.Infra.EfCore.MySQL/Adnc.Infra.EfCore.MySQL.csproj", "Infrastructures/Adnc.Infra.EfCore.MySQL/"]
COPY ["Infrastructures/Adnc.Infra.Repository/Adnc.Infra.Repository.csproj", "Infrastructures/Adnc.Infra.Repository/"]
COPY ["Infrastructures/Adnc.Infra.Dapper/Adnc.Infra.Dapper.csproj", "Infrastructures/Adnc.Infra.Dapper/"]
COPY ["Infrastructures/Adnc.Infra.Mongo/Adnc.Infra.Mongo.csproj", "Infrastructures/Adnc.Infra.Mongo/"]
COPY ["Infrastructures/Adnc.Infra.IdGenerater/Adnc.Infra.IdGenerater.csproj", "Infrastructures/Adnc.Infra.IdGenerater/"]
COPY ["Services/Adnc.Maint/Adnc.Maint.Application.Contracts/Adnc.Maint.Application.Contracts.csproj", "Services/Adnc.Maint/Adnc.Maint.Application.Contracts/"]
COPY ["Services/Adnc.Maint/Adnc.Maint.Repository/Adnc.Maint.Repository.csproj", "Services/Adnc.Maint/Adnc.Maint.Repository/"]
RUN dotnet restore "Services/Adnc.Maint/Adnc.Maint.WebApi/Adnc.Maint.WebApi.csproj"
COPY . .
WORKDIR "/src/Services/Adnc.Maint/Adnc.Maint.WebApi"
RUN dotnet build "Adnc.Maint.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Adnc.Maint.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Adnc.Maint.WebApi.dll"]