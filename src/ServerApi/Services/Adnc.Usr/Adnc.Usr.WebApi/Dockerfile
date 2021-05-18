#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Infrastructures/Adnc.Infra.Common/Adnc.Infra.Common.csproj", "Infrastructures/Adnc.Infra.Common/"]
COPY ["Infrastructures/Adnc.Infra.EventBus/Adnc.Infra.EventBus.csproj", "Infrastructures/Adnc.Infra.EventBus/"]
COPY ["Infrastructures/Adnc.Infra.Mapper/Adnc.Infra.Mapper.csproj", "Infrastructures/Adnc.Infra.Mapper/"]
COPY ["Infrastructures/Adnc.Infra.Caching/Adnc.Infra.Caching.csproj", "Infrastructures/Adnc.Infra.Caching/"]
COPY ["Infrastructures/Adnc.Infra.EfCore/Adnc.Infra.EfCore.csproj", "Infrastructures/Adnc.Infra.EfCore/"]
COPY ["Infrastructures/Adnc.Infra.Mongo/Adnc.Infra.Mongo.csproj", "Infrastructures/Adnc.Infra.Mongo/"]
COPY ["Infrastructures/Adnc.Infra.Consul/Adnc.Infra.Consul.csproj", "Infrastructures/Adnc.Infra.Consul/"]
COPY ["Services/Shared/Adnc.Application.Shared/Adnc.Application.Shared.csproj", "Services/Shared/Adnc.Application.Shared/"]
COPY ["Services/Shared/Adnc.WebApi.Shared/Adnc.WebApi.Shared.csproj", "Services/Shared/Adnc.WebApi.Shared/"]
COPY ["Services/Shared/Adnc.Core.Shared/Adnc.Core.Shared.csproj", "Services/Shared/Adnc.Core.Shared/"]
COPY ["Services/Adnc.Usr/Adnc.Usr.Core/Adnc.Usr.Core.csproj", "Services/Adnc.Usr/Adnc.Usr.Core
COPY ["Services/Adnc.Usr/Adnc.Usr.Application.Contracts/Adnc.Usr.Application.Contracts.csproj", "Services/Adnc.Usr/Adnc.Usr.Application.Contracts/"]
COPY ["Services/Adnc.Usr/Adnc.Usr.Application/Adnc.Usr.Application.csproj", "Services/Adnc.Usr/Adnc.Usr.Application/"]
COPY ["Services/Adnc.Usr/Adnc.Usr.WebApi/Adnc.Usr.WebApi.csproj", "Services/Adnc.Usr/Adnc.Usr.WebApi/"]
RUN dotnet restore "Services/Adnc.Usr/Adnc.Usr.WebApi/Adnc.Usr.WebApi.csproj"
COPY . .
WORKDIR "/src/Services/Adnc.Usr/Adnc.Usr.WebApi"
RUN dotnet build "Adnc.Usr.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Adnc.Usr.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#配置本服务在skywalking名称
ENV SKYWALKING__SERVICENAME=adnc-usr-webapi
ENV ASPNETCORE_HOSTINGSTARTUPASSEMBLIES=SkyAPM.Agent.AspNetCore
#设置时区为东八区
ENV TZ Asia/Shanghai
ENTRYPOINT ["dotnet", "Adnc.Usr.WebApi.dll"]