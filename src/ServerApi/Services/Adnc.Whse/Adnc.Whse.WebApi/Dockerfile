#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Services/Adnc.Whse/Adnc.Whse.WebApi/Adnc.Whse.WebApi.csproj", "Services/Adnc.Whse/Adnc.Whse.WebApi/"]
COPY ["Services/Shared/Adnc.Shared.WebApi/Adnc.Shared.WebApi.csproj", "Services/Shared/Adnc.Shared.WebApi/"]
COPY ["Infrastructures/Adnc.Infra.EfCore/Adnc.Infra.EfCore.csproj", "Infrastructures/Adnc.Infra.EfCore/"]
COPY ["Services/Shared/Adnc.Core.Shared/Adnc.Core.Shared.csproj", "Services/Shared/Adnc.Core.Shared/"]
COPY ["Infrastructures/Adnc.Infra.Common/Adnc.Infra.Common.csproj", "Infrastructures/Adnc.Infra.Common/"]
COPY ["Infrastructures/Adnc.Infra.EventBus/Adnc.Infra.EventBus.csproj", "Infrastructures/Adnc.Infra.EventBus/"]
COPY ["Services/Shared/Adnc.Shared.Application/Adnc.Shared.Application.csproj", "Services/Shared/Adnc.Shared.Application/"]
COPY ["Infrastructures/Adnc.Infra.Mapper/Adnc.Infra.Mapper.csproj", "Infrastructures/Adnc.Infra.Mapper/"]
COPY ["Infrastructures/Adnc.Infra.Caching/Adnc.Infra.Caching.csproj", "Infrastructures/Adnc.Infra.Caching/"]
COPY ["Infrastructures/Adnc.Infra.Mongo/Adnc.Infra.Mongo.csproj", "Infrastructures/Adnc.Infra.Mongo/"]
COPY ["Infrastructures/Adnc.Infra.Consul/Adnc.Infra.Consul.csproj", "Infrastructures/Adnc.Infra.Consul/"]
COPY ["Services/Adnc.Whse/Adnc.Whse.Application/Adnc.Whse.Application.csproj", "Services/Adnc.Whse/Adnc.Whse.Application/"]
COPY ["Services/Adnc.Whse/Adnc.Whse.Application.Contracts/Adnc.Whse.Application.Contracts.csproj", "Services/Adnc.Whse/Adnc.Whse.Application.Contracts/"]
COPY ["Services/Adnc.Whse/Adnc.Whse.Core/Adnc.Whse.Core.csproj", "Services/Adnc.Whse/Adnc.Whse.Core/"]
RUN dotnet restore "Services/Adnc.Whse/Adnc.Whse.WebApi/Adnc.Whse.WebApi.csproj"
COPY . .
WORKDIR "/src/Services/Adnc.Whse/Adnc.Whse.WebApi"
RUN dotnet build "Adnc.Whse.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Adnc.Whse.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Adnc.Whse.WebApi.dll"]