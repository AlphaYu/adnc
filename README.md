# <div align="center"><img src="https://aspdotnetcore.net/wp-content/uploads/2023/04/adnc-github.png" alt="ADNC - Open Source Microservice Framework Based on the .NET Platform" style="zoom:50%;" /></div>
<div align='center'>
<a href="./LICENSE">
<img alt="GitHub license" src="https://img.shields.io/github/license/AlphaYu/Adnc"/>
</a>
<a href="https://github.com/AlphaYu/Adnc/stargazers">
<img alt="GitHub stars" src="https://img.shields.io/github/stars/AlphaYu/Adnc"/>
</a>
<a href="https://github.com/AlphaYu/Adnc/network">
<img alt="GitHub forks" src="https://img.shields.io/github/forks/AlphaYu/Adnc"/>
</a>
<img alt="Visitors" src="https://komarev.com/ghpvc/?username=alphayu&color=red&label=Visitors"/>
</div>

###### <div align="center">Code changes the world, and open source drives the community</div>

[中文](./README_ZH.md)  [English](./README.md)

## Introduction

### What is ADNC?

`ADNC` is an open-source distributed/microservice framework based on `.NET 8`, and it also works well for monolithic applications. It provides a practical set of infrastructure and engineering practices around service registration and discovery, configuration management, distributed tracing, load balancing, circuit breaking and fault tolerance, distributed transactions, distributed caching, message queues, `RPC` communication (`HTTP` / `gRPC`), authentication and authorization, read/write splitting, and logging. The repository also includes supporting documentation and sample code to help you understand the framework design and get started quickly.

### Why choose ADNC?

- Supports multiple service styles: classic layered architecture, `DDD`, and compact single-project service structures.
- Ready-to-use infrastructure: pre-integrated solutions for configuration, service discovery, caching, messaging, authentication, and logging.
- Good for both learning and real projects: the repository includes a full demo, supporting documentation, and a front-end example.
- Open and extensible: released under the `MIT` license, so it can be customized, extended, and integrated into existing systems as needed.

Whether you are building a new system from scratch or refactoring and evolving an existing one, ADNC can serve as a reusable engineering foundation and reference implementation.

## Quick Start

It is recommended to start in this order:

1. Read the [Quick Start documentation](https://aspdotnetcore.net/docs/quickstart/)
2. Open the solution with `src/Adnc.sln` or `src/Demo/Adnc.Demo.sln`
3. If you need the front-end project, see the links at the end of this document
4. If you need seed data, see the database script link at the end of this document

Before running the demo, prepare the `.NET 8 SDK` and the required infrastructure described in the quick start guide. For complete setup and local run instructions, refer directly to the quick start documentation.

## Repository Layout / Architecture

### Directory Structure

```
adnc
├── .github
│   └── workflows CI/CD scripts (GitHub Actions)
├── doc Technical documentation
├── src Source code
│   ├── Infrastructures Infrastructure layer projects
│   ├── ServiceShared Shared service layer projects
│   ├── Gateways Ocelot gateway projects
│   └── Demo Demo projects
├── test Test-related projects
├── .gitignore
├── README.md
└── LICENSE
```

### Important Files

| Path | Description |
| --- | --- |
| `src/Adnc.sln` | The solution containing all `adnc` projects |
| `src/Infrastructures/Adnc.Infra.sln` | The solution containing only infrastructure-related projects |
| `src/ServiceShared/Adnc.Shared.sln` | The solution containing only shared service layer projects |
| `src/Demo/Adnc.Demo.sln` | The solution containing only demo-related projects |
| `src/.editorconfig` | Cross-editor configuration used to keep code style consistent across Visual Studio, VS Code, and JetBrains Rider |
| `src/Directory.Build.props` | Centralized common build properties such as target frameworks, language version, and output paths |
| `src/Directory.Packages.props` | Central Package Management (CPM) file used to manage NuGet package versions across the solution |

### Overall Architecture Diagram

<img src="https://aspdotnetcore.net/wp-content/uploads/2023/04/adnc_framework-e1682145003197.png" alt="adnc_framework"/>

#### Adnc.Infra.*

[NuGet Gallery | Packages matching adnc.infra](https://www.nuget.org/packages?q=adnc.infra)

![adnc-framework-2](https://aspdotnetcore.net/wp-content/uploads/2023/04/adnc-framework-2.png)

#### Adnc.Shared.*

[NuGet Gallery | Packages matching adnc.shared](https://www.nuget.org/packages?q=adnc.shared)

<img src="https://aspdotnetcore.net/wp-content/uploads/2023/04/adnc-framework-3.png" alt="adnc-framework-3" style="zoom:80%;" />

### Tech Stack

| Name | Description |
| --- | --- |
| <a target="_blank" href="https://github.com/ThreeMammals/Ocelot">Ocelot</a> | Open-source gateway built on .NET |
| <a target="_blank" href="https://github.com/hashicorp/consul">Consul</a> | Configuration center and service registry |
| <a target="_blank" href="https://github.com/reactiveui/refit">Refit</a> | Declarative and type-safe REST client library |
| <a target="_blank" href="https://github.com/grpc/grpc-dotnet">Grpc.Net.ClientFactory</a><br />Grpc.Tools | gRPC communication framework |
| <a target="_blank" href="https://github.com/SkyAPM/SkyAPM-dotnet">SkyAPM.Agent.AspNetCore</a> | SkyWalking .NET agent for tracing and performance monitoring |
| <a target="_blank" href="https://github.com/castleproject/Core">Castle DynamicProxy</a> | Dynamic proxy and AOP component |
| <a target="_blank" href="https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql">Pomelo.EntityFrameworkCore.MySql</a> | EF Core ORM provider |
| <a target="_blank" href="https://github.com/StackExchange/Dapper">Dapper</a> | Lightweight ORM |
| <a target="_blank" href="https://github.com/NLog/NLog">NLog</a><br />NLog.MongoDB<br />NLog.Loki | Logging components |
| <a target="_blank" href="https://github.com/AutoMapper/AutoMapper">AutoMapper</a> | Object-object mapping library |
| <a target="_blank" href="https://github.com/domaindrivendev/Swashbuckle.AspNetCore">Swashbuckle.AspNetCore</a> | Swagger-based API documentation generator |
| <a target="_blank" href="https://github.com/StackExchange/StackExchange.Redis">StackExchange.Redis</a> | Redis client SDK |
| <a target="_blank" href="https://github.com/dotnetcore/CAP">CAP</a> | Event bus and eventual consistency / distributed transaction component |
| <a target="_blank" href="https://github.com/rabbitmq/rabbitmq-dotnet-client">RabbitMQ</a> | Asynchronous message queue component |
| <a target="_blank" href="https://github.com/App-vNext/Polly">Polly</a> | .NET resilience and transient-fault-handling library |
| <a target="_blank" href="https://github.com/FluentValidation">FluentValidation</a> | .NET validation framework |
| <a target="_blank" href="https://github.com/mariadb-corporation/MaxScale">MaxScale</a> | Mature, high-performance, open-source database middleware from MariaDB |
| <a target="_blank" href="https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks">AspNetCore.HealthChecks</a> | Health check component that can work together with Consul health checks |

## Demo Service Overview

The demo includes five related microservices, each showing a different service decomposition and project organization style.

| Service | Description | Architecture Style |
| --- | --- | --- |
| Admin | System management (organization, users, roles, permissions, dictionaries, configuration) | Classic layered architecture with separate contracts |
| Maint | Operations management (logs, audits) | Classic layered architecture with merged contracts |
| Cust | Customer management | Minimal single-project structure |
| Ord | Order management | Domain-driven design (DDD) with a domain layer |
| Whse | Warehouse management | Domain-driven design (DDD) with a domain layer |

These demos show how to organize code for different business sizes and complexity levels while keeping the overall framework consistent.

##### :white_check_mark: Shared

> Shared demo projects reused by all demo services.

```
Shared/
├── Remote.Event/ - Event contracts for cross-service communication
├── Remote.Grpc/ - gRPC client definitions
├── Remote.Http/ - HTTP client definitions
├── protos/ - gRPC protocol definitions
└── resources/ - Shared configuration and resources
```

##### :white_check_mark: Adnc.Demo.Admin

> Admin is the system management service. It uses a classic three-layer structure and places application service contracts in a separate `Adnc.Demo.Admin.Application.Contracts` project. This layout is clear and well suited to back-office scenarios with well-defined boundaries and more modules.

```
Admin/
├── Api/ - Controllers and API endpoints
├── Application/ - Business logic implementations
├── Application.Contracts/ - DTOs and service interfaces
└── Repository/ - Data access layer
```

##### :white_check_mark: Adnc.Demo.Maint

> Maint is the operations center service. It uses a more compact three-layer structure, with both contracts and implementations living in `Adnc.Demo.Maint.Application`. This reduces the number of projects while keeping responsibilities clear.

```
Maint/
├── Api/ - Controllers and endpoints
├── Application/ - Contracts and implementations
└── Repository/ - Data access layer
```

##### :white_check_mark: Adnc.Demo.Cust

> Cust is the customer center service. It uses a single-project structure, with controllers, application services, contracts, and repositories all living in one project. This approach is better suited to smaller services with focused responsibilities and clear boundaries.

```
Cust/
└── Api/ - Controllers, application logic, and repositories
```

##### :white_check_mark: Adnc.Demo.Ord

> Ord is the order center service. It uses a DDD structure with an independent domain layer to emphasize business rules and domain models while separating them from application-layer concerns.

```
Ord/
├── Api/ - API endpoints
├── Application/ - Application services
├── Domain/ - Domain entities, aggregates, and domain services
└── Migrations/ - Database migrations
```

##### :white_check_mark: Adnc.Demo.Whse

> Whse is the warehouse center service. Its structure is the same as Ord and also uses a DDD organization with an independent domain layer.

```
Whse/
├── Api/ - API endpoints
├── Application/ - Application services
├── Domain/ - Domain entities, aggregates, and domain services
└── Migrations/ - Database migrations
```

## Documentation Links

### Available Documentation

#### Configuration file reference

Explains each configuration section and what it does: [View documentation](https://aspdotnetcore.net/docs/appsettings/)

#### How to manually deploy to containers

Covers how to use Docker to install and configure a Consul cluster, the SkyWalking stack, how to write Dockerfiles for the related projects, and how to deploy multiple services: [View documentation](https://aspdotnetcore.net/docs/deploy-docker/)

#### How to implement read/write splitting

Explains why middleware is used for read/write splitting and how to write EF Core code under this approach: [View documentation](https://aspdotnetcore.net/docs/maxscale-readwritesplit/)

#### How to use Cache, Redis, distributed locks, and Bloom filters

Includes usage patterns for Cache, Redis, distributed locks, and Bloom filters, as well as strategies to avoid cache avalanche, breakdown, penetration, and synchronization issues: [View documentation](https://aspdotnetcore.net/docs/cache-redis-distributedlock-bloomfilter/)

#### How to dynamically assign Snowflake workerId

Introduces the Yitter Snowflake algorithm, its features, configuration, and how to get a workerId dynamically: [View documentation](https://aspdotnetcore.net/docs/snowflake-max_value-workerid/)

#### How authentication and authorization work

Explains why a hybrid JwtBearer + Basic authentication model is used, along with the corresponding implementation and configuration: [View documentation](https://aspdotnetcore.net/docs/claims-based-authentication/)

#### How to use the EF Core repository

Covers repository basics, unit of work, Code First, native SQL, and related demo code and SQL examples.

1. [How to use the repository (1) - Basic features](https://aspdotnetcore.net/docs/efcore-pemelo-grud/)<br/>
1. [How to use the repository (2) - Distributed transactions / local transactions](https://aspdotnetcore.net/docs/efcore-pemolo-unitofwork/)<br/>
1. [How to use the repository (3) - Code First](https://aspdotnetcore.net/docs/efcore-pemelo-codefirst/)<br/>
1. [How to use the repository (4) - Writing raw SQL](https://aspdotnetcore.net/docs/efcore-pemelo-sql/)<br/>
1. [How to use the repository (5) - Switching database types](https://aspdotnetcore.net/docs/efcore-pemelo-sqlserver/)<br/>

### Planned Documentation

#### How to use Jenkins + Shell scripts for automated deployment

- Documentation is being prepared

#### How to deploy to Kubernetes

- Documentation is being prepared

#### How to build business services from scratch

- Documentation is being prepared

#### How to call microservices

- Documentation is being prepared

#### How to configure the gateway

- Documentation is being prepared

#### How to use the service registry / configuration center

- Documentation is being prepared

#### How to configure distributed tracing

- Documentation is being prepared

#### How to configure health checks

- Documentation is being prepared

## Screenshots / JMeter / Website

### JMeter Testing

> Six test cases cover the gateway, service discovery, configuration center, synchronous service calls, database CRUD, local transactions, distributed transactions, caching, Bloom filters, SkyAPM tracing, NLog logging, and operation logs.

- ECS server configuration: 4 vCPU, 8 GB RAM, 8 Mbps bandwidth. The server was also running many other components, with about 50% CPU and 50% memory still available.
- Due to bandwidth limits, throughput was around 1000 requests/second.
- Simulated concurrency: 1200 threads/second
- Read/write ratio: 7:3

### Front-end

An out-of-the-box admin front-end template based on Vue 3, Vite, TypeScript, and Element Plus.

#### Project Repository

- [adnc-vue3: ADNC's Vue3 front-end](https://github.com/alphayu/adnc-vue-elementplus)

#### UI Screenshots

![.NET open-source microservice framework - exception log page](https://aspdotnetcore.net/wp-content/uploads/2021/11/adnc-dashboard-nlog.png)
![.NET open-source microservice framework - role management page](https://aspdotnetcore.net/wp-content/uploads/2021/11/adnc-dashboard-role.png)

### Related Links

#### Official Website

- [https://aspdotnetcore.net](https://aspdotnetcore.net)

#### Online Demo

- [https://online.aspdotnetcore.net](https://online.aspdotnetcore.net)

#### Code Generator

- [https://code.aspdotnetcore.net](https://code.aspdotnetcore.net)

#### Database Scripts

- [adnc/doc/dbsql at develop · AlphaYu/adnc](https://github.com/AlphaYu/adnc/tree/develop/doc/dbsql)

### Community

- QQ Group: `780634162`

- If you made it this far, please give the project a `star`!

## License

This project is open sourced under the **MIT License**. See [LICENSE](./LICENSE) for details.
