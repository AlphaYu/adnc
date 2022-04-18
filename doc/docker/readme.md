<div align="center">
<a href="https://github.com/alphayu/adnc" target="_blank" title="Adnc是一个微服务开发框架 代码改变世界 开源推动社区"><img src="https://aspdotnetcore.net/wp-content/uploads/2020/12/adnc-homepage-logo-3.webp" alt="Adnc是一个微服务开发框架 代码改变世界 开源推动社区"></a>
</div>

# docker实用命令记录
```shell
# 查看已下载的Docker镜像latest具体版本
# docker image inspect redis:latest|grep -i version
docker image inspect 镜像名:latest|grep -i version
```

```shell
# 容器日志过大处理
# Docker 的日志文件存在 /var/lib/docker/containers 目录中，通过下面的命令可以将日志文件夹根据升序的方式罗列出来。
du -d1 -h /var/lib/docker/containers | sort -h

# 不过已存在的容器不会生效，需要重建才可以
# 创建或修改文件 /etc/docker/daemon.json，并增加以下配置控制日志大小
{
    "log-driver":"json-file",
    "log-opts":{
        "max-size" :"500m","max-file":"3"
    }
}
# 随后重启 Docker 服务
systemctl daemon-reload
systemctl restart docker


# docker安装ping
apt-get update
apt install iputils-ping

# portainer
docker run -d -p 49111:9000 --name portainer --restart=always  -e TZ=Asia/Shanghai -v /var/run/docker.sock:/var/run/docker.sock -v  /root/data/portainer:/data portainer/portainer
```
