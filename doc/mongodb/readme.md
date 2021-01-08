<div align="center">
<a href="https://github.com/alphayu/adnc" target="_blank" title="Adnc是一个微服务开发框架 代码改变世界 开源推动社区"><img src="https://aspdotnetcore.net/wp-content/uploads/2020/12/adnc-homepage-logo-3.webp" alt="Adnc是一个微服务开发框架 代码改变世界 开源推动社区"></a>
</div>

# mongodb安装与配置

## 拉取镜像并运行容器
```
docker pull mongo:4.4.3
docker run --name mongo -p 13017:27017 -e TZ="Asia/Shanghai" -e MONGO_INITDB_ROOT_USERNAME=admin -e MONGO_INITDB_ROOT_PASSWORD=football -v /root/data/mongo:/data/db -v /root/data/mongo/backup:/data/backup -d mongo:4.4.3 --auth
```

## 新建数据库
```
进入mongo容器
docker exec -it mongo mongo admin
# 新建Logs数据库
use Logs
# 创建用户
db.createUser({user:'alpha',pwd:'football',roles:[{role:'readWrite',db:'Logs'}]})
```