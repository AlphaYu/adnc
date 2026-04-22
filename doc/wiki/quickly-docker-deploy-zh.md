# 快速docker部署

## 1.系统要求

1. 服务器操作系统：`Ubuntu 22.04`
2. 服务器需预先安装 `Docker` 和 `Docker Compose`

## 2.创建目录

```bash
mkdir -p /opt/adnc/src
```

## 3.创建自定义网络

```bash
docker network create \
  --driver=bridge \
  --subnet=172.80.0.0/16 \
  --ip-range=172.80.5.0/24 \
  --gateway=172.80.5.254 \
  adnc_network_main
```

## 3.上传部署中间件的yml文件

上传 `adnc\doc\devops-staging` 文件夹到服务器 `/opt/adnc` 目录。

上传后服务器目录结构如下：

```bash
opt
└── adnc
	├── devops-staging
    └── src
```

## 4.执行docker-compose.yml

```bash
cd /opt/adnc/devops-staging
docker compose up -d
```

`docker-compose.yml` 文件将执行以下操作：

- 部署 `Consul` 集群并初始化配置信息

- 部署 `MariaDB` 并初始化数据库
- 部署 `Redis`
- 部署 `RabbitMQ`
- 部署 `Grafana`,`Loki`
- 部署`Nginx`

检查容器是否正常运行

```bash
docker container ls
```

## 5.安装 .NET 8 SDK

```bash
apt-get update && \
apt-get install -y dotnet-sdk-8.0
```

## 6.上传微服务代码

> 上传前先执行 `Delete-BIN-OBJ-Folders.bat`，该文件会清理 `bin` 和 `obj` 目录。

上传的以下文件夹与文件到服务器`opt/adnc/src`目录

- `Demo` 目录
- `Gateways` 目录
- `Directory.Build.props`
- `Directory.Packages.props`
- `deploy_demo.sh`
- `deploy_ocelot.sh`

上传后服务器目录结构如下：

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

## 7.执行微服务部署脚本

```bash
cd /opt/adnc/src
chmod +x deploy_demo.sh deploy_ocelot.sh
bash deploy_demo.sh
bash deploy_ocelot.sh
```

## 8.验证网关与微服务

- 访问`http://{服务器Ip}:8590`Consul UI,查看 `admin`,`maint`,`cust` 服务是否注册成功。
- 访问 `http://{服务器Ip}:5000` 检查网关是否正常工作。

## 9.部署前端

```bash
pnpm run build
```

- `build` 成功后，将 `dist` 目录内的文件上传到 `/opt/adnc/devops-staging/adnc-nginx/html`目录。
- 访问 `http://{服务器Ip}` 并登录，检查系统是否部署成功。

------

## 10.结语

🎉 恭喜您，`ADNC` 框架以及完成部署！
