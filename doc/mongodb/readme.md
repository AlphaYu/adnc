<div align="center">
<a href="https://github.com/alphayu/adnc" target="_blank" title="Adnc是一个微服务开发框架 代码改变世界 开源推动社区"><img src="https://aspdotnetcore.net/wp-content/uploads/2020/12/adnc-homepage-logo-3.webp" alt="Adnc是一个微服务开发框架 代码改变世界 开源推动社区"></a>
</div>

# mongodb安装与配置

```
docker pull mongo:4.4.3
docker run --name mongo -p 13017:27017 -e MONGO_INITDB_ROOT_USERNAME=admin -e MONGO_INITDB_ROOT_PASSWORD=football -e TZ=Asia/Shanghai -v /root/data/mongo:/data/db -v /root/data/mongo/backup:/data/backup -d mongo:4.4.3 --auth
进入mongo容器
docker exec -it mongo mongo admin
use admin
db.auth("admin","football")
# 新建logs_dev数据库
use logs_dev
# 创建用户
db.createUser({user:'alpha',pwd:'football',roles:[{role:'readWrite',db:'logs_dev'}]})
```