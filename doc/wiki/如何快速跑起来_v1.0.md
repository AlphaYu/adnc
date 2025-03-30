## 前言

`adnc` 是一个完全可落地的 `.NET 8` 微服务/分布式框架，集成了多种主流、稳定的微服务配套组件。在本地调试时，仅需安装必要的软件，包括 `MariaDB` 或 `MySQL`、`Redis` 和 `RabbitMQ`，可采用任意熟悉的方式进行安装，或使用已有的环境。

此外，服务的自动注册、发现、配置中心及链路跟踪等功能，在调试环境下不会启用，因为代码中已做了环境变量判断。安装完必备软件后，仅需修改配置文件中的相关连接字符串，即可运行项目。

- **所有服务的数据库脚本存放于 \**\*\*`adnc\doc\dbsql\adnc.sql`\*\**\*，请自行导入。**

## 1. 环境说明与配置

在开发环境下，`adnc` 所有服务的公共配置存放于 `adnc\src\Demo\Shared\resources\appsettings.shared.Development.json` 文件，例如 `Redis`、`RabbitMQ` 配置。

各服务的特有配置存放在对应 `API` 工程的 `appsettings.Development.json` 文件中，例如数据库和服务端口的配置。

1. Redis 配置

```json
"Redis": {
    "Provider": "StackExchange",
    "EnableLogging": true,
    "SerializerName": "json",
    "EnableBloomFilter": false,
    "Dbconfig": {
        "ConnectionString": "服务器IP地址:端口,password=密码,defaultDatabase=0,ssl=false,sslHost=null,connectTimeout=4000,allowAdmin=true"
    }
}
```

2. RabbitMQ 配置

```json
"RabbitMq": {
    "HostName": "服务器IP地址",
    "Port": "端口",
    "VirtualHost": "/",
    "UserName": "用户名",
    "Password": "密码"
}
```

3.  SysLogDb配置

```json
"SysLogDb": {
    "DbType": "mysql",
    "ConnectionString": "Server=服务器IP地址;Port=端口;database=adnc_syslog;uid=用户名;pwd=密码;connection timeout=30;"
}
```

4. MariaDB/MySQL 配置

```json
"Mysql": {
    "ConnectionString": "Server=服务器IP地址;Port=端口;database=adnc_admin;uid=用户名;pwd=密码;connection timeout=30;"
}
```

## 2. 启动后端

1. 在 `Visual Studio 2022` 中，右键解决方案 -> **属性** -> **项目启动** -> **多个启动项目**，选择以下 4 个项目：

   - `Adnc.Gateway.Ocelot`
   - `Adnc.Demo.Admin.Api`
   - `Adnc.Demo.Maint.Api`
   - `Adnc.Demo.Cust.Api`

   **注意**：实际开发过程中不必一次性启动所有服务，此处仅用于快速在本地运行项目。

2. 在 `Visual Studio 2022` 主界面，点击 **启动** 按钮，成功后 3 个服务和网关将会启动。

3. 若启动报错，优先查看 **控制台窗口** 的错误信息，常见问题如下：

   - **RabbitMQ 端口配置错误**：RabbitMQ 公开两个端口，一个用于 Web 管理页面，另一个用于数据通信，配置文件中应填写数据端口。
   - **服务端口冲突**：可能与企业微信端口或其他已占用端口冲突。

| 工程名              | 描述     | URL 地址                 |
| ------------------- | -------- | ------------------------ |
| Adnc.Gateway.Ocelot | 网关     | `http://localhost:5000`  |
| Adnc.Demo.Admin.Api | 系统管理 | `http://localhost:50010` |
| Adnc.Demo.Maint.Api | 运维管理 | `http://localhost:50020` |
| Adnc.Demo.Cust.Api  | 客户管理 | `http://localhost:50030` |

## 3. 启动前端

1. 使用 `Visual Studio Code` 打开前端项目 `adnc-vue-elementplus`，前端基于 `Vue 3` 开发，需要安装相关依赖。
2. 运行以下命令进行环境配置：

```bash
# 安装 pnpm
npm install pnpm -g

# 可选：设置国内镜像源
pnpm config set registry https://registry.npmmirror.com

# 安装依赖
pnpm install

# 启动前端项目
pnpm run dev
```

## 4. 结语

至此，`adnc` 框架已成功在本地运行！

如果觉得本项目对你有帮助，欢迎 `Star` & `Fork` 支持！

[ADNC](https://aspdotnetcore.net/) —— 一个可落地的 .NET 微服务/分布式开发框架。