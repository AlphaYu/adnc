# ADNC repository instructions

## Build and test commands

Use the solution that matches the area you are changing instead of rebuilding everything by default:

```powershell
dotnet build src\Adnc.sln -v minimal
dotnet build src\Infrastructures\Adnc.Infra.sln -v minimal
dotnet build src\ServiceShared\Adnc.Shared.sln -v minimal
dotnet build src\Demo\Adnc.Demo.sln -v minimal
dotnet build src\Gateways\Ocelot\Adnc.Ocelot.sln -v minimal
```

Infrastructure tests live under `test\Adnc.Infra.Unittest.sln`:

```powershell
dotnet build test\Adnc.Infra.Unittest.sln -v minimal
dotnet test test\Adnc.Infra.Unittest.sln -v minimal
```

Run a single test project or test case with `--filter`:

```powershell
dotnet test test\Unittest.Helper\Adnc.Infra.Unittest.Helper.csproj -v minimal
dotnet test test\Unittest.Helper\Adnc.Infra.Unittest.Helper.csproj --filter "FullyQualifiedName~Adnc.Infra.Unittest.Helper.TestCases.HelperTests.TestAes" -v minimal
dotnet test test\Unittest.Redis\Adnc.Infra.Unittest.Redis.csproj --filter "FullyQualifiedName~RedisCahceTests" -v minimal
```

`Unittest.Helper` is self-contained. The Redis, Consul, MySQL, and MongoDB test projects read connection settings from their local `appsettings.json` files and expect those dependencies to be running first. For local infrastructure, initialize the `doc\devops` submodule and use `doc\devops\docker-compose\README.md`.

## High-level architecture

`src\Adnc.sln` is the top-level solution. The codebase is split into four major areas:

1. `src\Infrastructures`: reusable packages published as `Adnc.Infra.*` (`Core`, `Consul`, `EventBus`, `Redis`, `Repository`, `Repository.EfCore.*`, `Repository.Dapper`, `IdGenerater`).
2. `src\ServiceShared`: the shared application, domain, repository, remote-call, and Web API abstractions used by services (`Adnc.Shared.*`).
3. `src\Demo`: five demo microservices plus shared cross-service contracts.
4. `src\Gateways\Ocelot`: the Ocelot gateway that fronts the demo services.

The demo services intentionally show different service shapes:

- `Admin`: classic layered service with a separate `Application.Contracts` project.
- `Maint`: classic layered service with contracts merged into `Application`.
- `Cust`: single-project microservice; API, application, repository, RPC clients, and event subscribers live together.
- `Ord` and `Whse`: DDD-style services with explicit `Domain` and `Migrations` projects.

All demo services share `src\Demo\Shared` for cross-service constants, CAP events, gRPC contracts, Refit clients, shared proto files, and shared configuration.

The gateway is configured separately from the services. `src\Gateways\Ocelot\Program.cs` switches between file-backed routing (`Config\ocelot.direct.json`) and Consul-backed routing based on `ConfigurationType`.

## Key codebase conventions

### Service bootstrap pattern

Every API service follows the same startup flow in `Program.cs`:

1. Compute the migrations assembly name from the API assembly name.
2. Create `ServiceInfo`.
3. Call `WebApplication.CreateBuilder(args).AddConfiguration(serviceInfo)`.
4. Register dependencies with `builder.Services.AddAdnc(serviceInfo, builder.Configuration)`.
5. Build the app, then call `app.UseAdnc().ChangeThreadPoolSettings().UseRegistrationCenter()`.

`AddAdnc` and `UseAdnc` are convention-based. They discover the service's `DependencyRegistrar` and `MiddlewareRegistrar` types at runtime, so new services should keep following that naming and registration pattern.

### Registrar split

Each service has:

- an API-layer registrar inheriting from `AbstractWebApiDependencyRegistrar`
- an application-layer registrar inheriting from `AbstractApplicationDependencyRegistrar`

API registrars typically call `AddWebApiDefaultServices()`, then add health checks and any API-specific extras like gRPC or the CAP dashboard. Application registrars set the three key assemblies (`ApplicationLayerAssembly`, `ContractsLayerAssembly`, `RepositoryOrDomainLayerAssembly`) and then call `AddApplicaitonDefaultServices()` before layering in domain services, Refit clients, gRPC clients, CAP, or EF Core specifics.

### Shared service behavior comes from `ServiceShared`

Do not reimplement common service plumbing in feature projects. The shared projects already provide:

- controller base types and result shaping (`AdncControllerBase`, `ServiceResult`, `ProblemDetails`)
- authentication and authorization plumbing, including hybrid bearer/basic support
- automatic registration of validators from the contracts assembly
- interceptor-based application service proxies with `OperateLogInterceptor`, `CachingInterceptor`, and `UowInterceptor`
- common remote-call wiring for Refit and gRPC

Controllers are expected to inherit from `AdncControllerBase` and return shared result types instead of hand-rolling HTTP responses.

### Configuration model

Configuration is environment-driven and uses shared placeholders. `AddConfiguration` loads shared config, replaces `$SERVICENAME`, `$SHORTNAME`, and `$RELATIVEROOTPATH`, and can switch from local file config to Consul config.

In practice:

- development appsettings use `ConfigurationType = File`
- test/staging/production appsettings switch to `ConfigurationType = Consul`
- service registration and RPC resolution are controlled separately via `RegisterType`

The shared development defaults in `src\Demo\Shared\resources\appsettings.shared.Development.json` are the reference point for local infrastructure, RPC addresses, JWT/basic auth, thread pool settings, logging, Redis, RabbitMQ, and Consul.

### Data-access conventions

Infrastructure and demo services rely heavily on the ADNC repository extensions instead of raw DI setup:

- MySQL services call `AddAdncInfraEfCoreMySql(...)`
- SQL Server services call `AddAdncInfraEfCoreSQLServer(...)`
- MongoDB support comes through `AddAdncInfraEfCoreMongoDb(...)`
- Dapper is commonly registered alongside EF Core

Where MySQL or MongoDB EF Core contexts are configured, the repo consistently enables lowercase naming conventions. Audit information is populated through the shared repository/base-entity infrastructure, so preserve the existing aggregate/entity base types instead of bypassing them.

### RPC and cross-service integration

Cross-service calls are centralized through `src\Demo\Shared`:

- Refit interfaces live in `Remote.Http`
- gRPC contracts and clients live in `Remote.Grpc` and `protos`
- event contracts live in `Remote.Event`

When a service needs another service, prefer registering the existing shared client/event contract in the application registrar instead of adding ad hoc HTTP code.

### Repo-wide C# style

The repo-wide style rules live in `src\.editorconfig`. Follow the existing conventions when editing C#:

- file-scoped namespaces
- `var` preferred in most cases
- private/internal fields use `_camelCase`
- `using System.*` directives sort first
- braces are required

Analyzer behavior is also driven from that file, so `dotnet build` is the normal style/analyzer feedback loop in this repository.
