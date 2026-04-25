# ADNC 快速上手指南

[GitHub 仓库地址](https://github.com/alphayu/adnc)

## 1. 配置文件修改

在开发环境中，`adnc` 各服务的通用配置统一集中在 `adnc\src\Demo\Shared\resources\appsettings.shared.Development.json` 文件中（例如 Redis、RabbitMQ 等）。

各服务的专有配置位于对应 API 工程的 `appsettings.Development.json` 文件中（例如数据库连接字符串、服务端口等）。

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

3. SysLogDb 配置

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

## 2. 导入数据库数据

所有服务的数据库脚本统一存放于 `adnc\doc\dbsql\adnc.sql` 文件，可一次性导入全部数据。

## 3. 启动后端服务

1. 在 `Visual Studio 2022` 中，右键解决方案 → **属性** → **项目启动** → **多个启动项目**，勾选以下 4 个项目：

   - `Adnc.Gateway.Ocelot`
   - `Adnc.Demo.Admin.Api`
   - `Adnc.Demo.Maint.Api`
   - `Adnc.Demo.Cust.Api`

   **提示**：实际开发时无需同时启动所有服务，此处仅为快速本地体验。

2. 在 `Visual Studio 2022` 主界面，点击 **启动** 按钮，启动上述 3 个服务以及网关（共 4 个项目）。

3. 若启动报错，优先查看 **控制台窗口** 的错误信息，常见问题如下：

   - **RabbitMQ 端口配置错误**：RabbitMQ 公开两个端口，一个用于 Web 管理页面，另一个用于数据通信，配置文件中应填写数据端口。
   - **服务端口冲突**：可能与企业微信等应用占用的端口冲突。

| 工程名              | 描述     | URL 地址                 |
| ------------------- | -------- | ------------------------ |
| Adnc.Gateway.Ocelot | 网关     | `http://localhost:5000`  |
| Adnc.Demo.Admin.Api | 系统管理 | `http://localhost:50010` |
| Adnc.Demo.Maint.Api | 运维管理 | `http://localhost:50020` |
| Adnc.Demo.Cust.Api  | 客户管理 | `http://localhost:50030` |

## 4. 启动前端

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

## 5. 结语

至此，`adnc` 已可在本地正常运行。

如果本项目对你有帮助，欢迎 `Star` & `Fork` 支持！

[ADNC](https://aspdotnetcore.net/) —— 一个可落地的 .NET 微服务/分布式开发框架。
