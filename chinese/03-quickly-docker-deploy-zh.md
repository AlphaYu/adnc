# ADNC 快速 Docker 部署指南

[GitHub 仓库地址](https://github.com/alphayu/adnc)

## 1. 系统要求

1. 推荐服务器操作系统：`Ubuntu 22.04`
2. 服务器需预先安装 `Docker` 与 `Docker Compose`

## 2. 创建部署目录

```bash
mkdir -p /opt/adnc/src
```

## 3. 创建自定义 Docker 网络

```bash
docker network create \
  --driver=bridge \
  --subnet=172.80.0.0/16 \
  --ip-range=172.80.5.0/24 \
  --gateway=172.80.5.254 \
  adnc_network_main
```

## 4. 上传中间件部署 yml 文件

请将本地 `adnc\doc\devops-staging` 文件夹上传至服务器 `/opt/adnc` 目录。

上传完成后，服务器目录结构如下：

```bash
opt
└── adnc
    ├── devops-staging
    └── src
```

## 5. 启动中间件容器

```bash
cd /opt/adnc/devops-staging
docker compose up -d
```

`docker-compose.yml` 文件将完成以下操作：

- 部署 `Consul` 集群并初始化配置信息
- 部署 `MariaDB` 并初始化数据库
- 部署 `Redis`
- 部署 `RabbitMQ`
- 部署 `Grafana`、`Loki`
- 部署 `Nginx`

部署完成后，可通过以下命令检查容器运行状态：

```bash
docker container ls
```

## 6. 安装 .NET 8 SDK

```bash
apt-get update && \
apt-get install -y dotnet-sdk-8.0
```

## 7. 上传微服务代码

> 上传前请先在本地执行 `Delete-BIN-OBJ-Folders.bat`，以清理所有 `bin` 和 `obj` 目录。

请将以下文件夹及文件上传至服务器 `opt/adnc/src` 目录：

- `Demo` 目录
- `Gateways` 目录
- `Directory.Build.props`
- `Directory.Packages.props`
- `deploy_demo.sh`
- `deploy_ocelot.sh`

上传完成后，服务器目录结构如下：

```bash
adnc
├── src
│   ├── Demo
│   ├── Gateways
│   ├── deploy_demo.sh
│   ├── deploy_ocelot.sh
│   ├── Directory.Packages.props
│   └── Directory.Build.props
└── devops-staging
```

## 8. 执行微服务部署脚本

```bash
cd /opt/adnc/src
chmod +x deploy_demo.sh deploy_ocelot.sh
bash deploy_demo.sh
bash deploy_ocelot.sh
```

## 9. 验证网关与微服务

- 访问 `http://{服务器IP}:8590` 打开 Consul UI，检查 `admin`、`maint`、`cust` 服务是否注册成功。
- 访问 `http://{服务器IP}:5000` 检查网关是否正常工作。

## 10. 部署前端

```bash
pnpm run build
```

- `build` 成功后，将 `dist` 目录内的文件上传到 `/opt/adnc/devops-staging/adnc-nginx/html` 目录。
- 访问 `http://{服务器IP}` 并登录，检查系统是否部署成功。

------

## 11. 结语

至此，`ADNC` 已完成部署。
