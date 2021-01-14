<div align="center">
<a href="https://github.com/alphayu/adnc" target="_blank" title="Adnc是一个微服务开发框架 代码改变世界 开源推动社区"><img src="https://aspdotnetcore.net/wp-content/uploads/2020/12/adnc-homepage-logo-3.webp" alt="Adnc是一个微服务开发框架 代码改变世界 开源推动社区"></a>
</div>

# consul集群部署
consul是微服务的中转中心(注册中心/配置中心)，最重要的组件。如果consul服务器挂了，系统也会奔溃。因为重要，所有我们必须要部署一个集群
consul分为server节点与client节点，server节点负责存储数据；client节点负责注册、发现、读写配置、健康监测。

## 自定义网络
自定义网络可以指定容器IP，这样服务器重启consul集群也可以正常运行。
```shell
docker network create --driver bridge --subnet=172.21.0.0/16 --gateway=172.21.0.16 adnc_consul
```
## 部署
``` shell
#拉取镜像
docker pull consul:1.8.0

#启动容器consul_server_1
docker run -d -p 8510:8500 --restart=always -v /root/data/consul/server1/data:/consul/data -v /root/data/consul/server1/config/:/consul/config --network=adnc_consul --ip 172.21.0.1 --privileged=true --name=consul_server_1 consul:1.8.0 agent -server -bootstrap-expect=3 -ui -node=consul_server_1 -client='0.0.0.0' -bind='172.21.0.1' -data-dir /consul/data -config-dir /consul/config -datacenter=adnc_dc

#为了让其他两个server节点加入集群，首先获取一下consul_server_1的IP地址。
JOIN_IP="172.21.0.1"

#启动容器consul_server_2并加入集群
docker run -d -p 8520:8500 --restart=always -v /root/data/consul/server2/data:/consul/data -v /root/data/consul/server2/config/:/consul/config --network=adnc_consul --ip 172.21.0.2 --privileged=true --name=consul_server_2 consul:1.8.0 agent -server -ui -node=consul_server_2 -client='0.0.0.0' -bind='172.21.0.2' -datacenter=adnc_dc -data-dir /consul/data -config-dir /consul/config -join=$JOIN_IP

#启动容器consul_server_3并加入集群
docker run -d -p 8530:8500 --restart=always -v /root/data/consul/server3/data:/consul/data -v /root/data/consul/server3/config/:/consul/config --network=adnc_consul --ip 172.21.0.3 --privileged=true --name=consul_server_3 consul:1.8.0 agent -server -ui -node=consul_server_3 -client='0.0.0.0' -bind='172.21.0.3' -datacenter=adnc_dc -data-dir /consul/data -config-dir /consul/config -join=$JOIN_IP

#验证2个server节点是否加入集群
docker exec consul_server_1 consul operator raft list-peers

#启动容器consul_client_1并加入集群
docker run -d -p 8550:8500 --restart=always -v /data/consul/client1/config:/consul/config --network=adnc_consul --ip 172.21.0.4 --name=consul_client_1 consul:1.8.0 agent -node=consul_client_1   -client='0.0.0.0' -bind='172.21.0.4' -datacenter=adnc_dc -config-dir /consul/config -join=$JOIN_IP
```

## 验证
```shell
#验证2个server节点是否加入集群
docker exec consul_server_1 consul operator raft list-peers
#显示信息如下，表示集群搭建成功。集群有3个server节点，consul_server_3是leader。
Node             ID                                    Address          State     Voter  RaftProtocol
consul_server_2  fc655a2d-556b-f6aa-1b17-d189da0081b4  172.21.0.2:8300  follower  true   3
consul_server_3  3cc9fc4e-a65c-1666-ab4c-d6cb5cfefd8a  172.21.0.3:8300  leader    true   3
consul_server_1  0026bb2d-d2e8-5681-a3ad-ada57036e2e1  172.21.0.1:8300  follower  true   3
```
client节点可以N多个，一般来说每台服务器上都需要部署一个client节点   
consul集群安装完成，我们在浏览器输入http://服务器IP:8510/ui/adnc_dc/nodes,应该可以看到4个节点

## fabio部署
``` shell
docker run --network=adnc_consul --ip 172.21.0.15 --name fabio1 -d -p 9998:9998 -p 9999:9999 --restart=always -v /root/data/fabio/fabio.properties:/etc/fabio/fabio.properties magiconair/fabio
```