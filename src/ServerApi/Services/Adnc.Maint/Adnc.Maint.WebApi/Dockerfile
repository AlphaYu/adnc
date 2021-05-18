#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Services/Adnc.Maint/Adnc.Maint.WebApi/Adnc.Maint.WebApi.csproj", "Services/Adnc.Maint/Adnc.Maint.WebApi/"]
COPY ["Infrastructures/Adnc.Infra.Common/Adnc.Infra.Common.csproj", "Infrastructures/Adnc.Infra.Common/"]
COPY ["Services/Shared/Adnc.WebApi.Shared/Adnc.WebApi.Shared.csproj", "Services/Shared/Adnc.WebApi.Shared/"]
COPY ["Infrastructures/Adnc.Infra.EfCore/Adnc.Infra.EfCore.csproj", "Infrastructures/Adnc.Infra.EfCore/"]
COPY ["Services/Shared/Adnc.Core.Shared/Adnc.Core.Shared.csproj", "Services/Shared/Adnc.Core.Shared/"]
COPY ["Infrastructures/Adnc.Infra.EventBus/Adnc.Infra.EventBus.csproj", "Infrastructures/Adnc.Infra.EventBus/"]
COPY ["Services/Shared/Adnc.Application.Shared/Adnc.Application.Shared.csproj", "Services/Shared/Adnc.Application.Shared/"]
COPY ["Infrastructures/Adnc.Infra.Mapper/Adnc.Infra.Mapper.csproj", "Infrastructures/Adnc.Infra.Mapper/"]
COPY ["Infrastructures/Adnc.Infra.Caching/Adnc.Infra.Caching.csproj", "Infrastructures/Adnc.Infra.Caching/"]
COPY ["Infrastructures/Adnc.Infra.Mongo/Adnc.Infra.Mongo.csproj", "Infrastructures/Adnc.Infra.Mongo/"]
COPY ["Infrastructures/Adnc.Infra.Consul/Adnc.Infra.Consul.csproj", "Infrastructures/Adnc.Infra.Consul/"]
COPY ["Services/Adnc.Maint/Adnc.Maint.Application/Adnc.Maint.Application.csproj", "Services/Adnc.Maint/Adnc.Maint.Application/"]
COPY ["Services/Adnc.Maint/Adnc.Maint.Core/Adnc.Maint.Core.csproj", "Services/Adnc.Maint/Adnc.Maint.Core/"]
COPY ["Services/Adnc.Maint/Adnc.Maint.Application.Contracts/Adnc.Maint.Application.Contracts.csproj", "Services/Adnc.Maint/Adnc.Maint.Application.Contracts/"]
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