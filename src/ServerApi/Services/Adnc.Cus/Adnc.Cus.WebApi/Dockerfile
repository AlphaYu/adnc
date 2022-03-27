#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Services/Adnc.Cus/Adnc.Cus.WebApi/Adnc.Cus.WebApi.csproj", "Services/Adnc.Cus/Adnc.Cus.WebApi/"]
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
COPY ["Services/Adnc.Cus/Adnc.Cus.Application/Adnc.Cus.Application.csproj", "Services/Adnc.Cus/Adnc.Cus.Application/"]
COPY ["Services/Adnc.Cus/Adnc.Cus.Core/Adnc.Cus.Core.csproj", "Services/Adnc.Cus/Adnc.Cus.Core/"]
COPY ["Services/Adnc.Cus/Adnc.Cus.Application.Contracts/Adnc.Cus.Application.Contracts.csproj", "Services/Adnc.Cus/Adnc.Cus.Application.Contracts/"]
RUN dotnet restore "Services/Adnc.Cus/Adnc.Cus.WebApi/Adnc.Cus.WebApi.csproj"
COPY . .
WORKDIR "/src/Services/Adnc.Cus/Adnc.Cus.WebApi"
RUN dotnet build "Adnc.Cus.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Adnc.Cus.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Adnc.Cus.WebApi.dll"]