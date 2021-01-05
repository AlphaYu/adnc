<div align="center">
<a href="https://github.com/alphayu/adnc" target="_blank" title="Adnc是一个微服务开发框架 代码改变世界 开源推动社区"><img src="https://aspdotnetcore.net/wp-content/uploads/2020/12/adnc-homepage-logo-3.webp" alt="Adnc是一个微服务开发框架 代码改变世界 开源推动社区"></a>
</div>

# maxscale安装
maxscale安装前必须已经部署好mariadb集群。
如何部署，请参考<a href="https://github.com/AlphaYu/Adnc/tree/master/doc/mariadb" target="_blank" alt="mariadb基于GTID主从复制搭建">mariadb基于GTID主从复制搭建</a>。

## 创建maxscale默认账号在master
```
docker exec -it mariadb01 /bin/bash
mysql -uroot -palpha.abc

CREATE USER 'maxscale'@'%' IDENTIFIED BY 'maxscale_pw';
GRANT SELECT ON mysql.user TO 'maxscale'@'%';
GRANT SELECT ON mysql.db TO 'maxscale'@'%';
GRANT SELECT ON mysql.tables_priv TO 'maxscale'@'%';
GRANT SELECT ON mysql.columns_priv TO 'maxscale'@'%';
GRANT SELECT ON mysql.proxies_priv TO 'maxscale'@'%';
GRANT SELECT ON mysql.roles_mapping TO 'maxscale'@'%';
GRANT SHOW DATABASES ON *.* TO 'maxscale'@'%';
GRANT SELECT ON mysql.* TO 'maxscale'@'%';
```


## 创建maxscale监控账号在master
```
CREATE USER 'mariadb_monitor'@'%' IDENTIFIED BY '123abc';
GRANT REPLICATION CLIENT on *.* to 'mariadb_monitor'@'%';
GRANT SUPER, RELOAD on *.* to 'mariadb_monitor'@'%';
GRANT SHOW DATABASES ON *.* TO mariadb_monitor@'%';
```

## 创建maxscale路由账号在master
```
create user 'mariadb_router'@'%' identified by '123abc';
grant select on mysql.* to mariadb_router@'%';
GRANT SHOW DATABASES ON *.* TO mariadb_router@'%';
```
## 创建maxscale连接账号在master
该账号是你C#代码连接maxscale的账号。
```
create user 'adnc'@'%' identified by '123abc';
GRANT ALL PRIVILEGES on *.* to adnc@'%';
FLUSH  PRIVILEGES; 
```

## 下载配置文件
```
mkdir /root/data/maxscale
cd /root/data/maxscale

#下载已经配置好的配置文件到/root/data/maxscale目录，如果不能下载成功，请手工处理
wget https://raw.githubusercontent.com/AlphaYu/Adnc/master/doc/maxscale/my.cnf
```
## 拉取镜像文件并运行容器
```
docker pull mariadb/maxscale
# 14006是你C#代码连接maxscale的端口
# 18989是maxscale的web配置界面端口
docker run -d -p 18989:8989 -p 14006:4006 -e TZ="Asia/Shanghai" --name mxs -v /root/data/maxscale:/etc/maxscale.cnf.d --network=adnc_net --ip 172.20.0.14 mariadb/maxscale
```
##验证
访问 http://你maxscale外网ip:18989 
账号:admin
密码:mariadb 

配置正确如下图
<img src="https://aspdotnetcore.net/wp-content/uploads/2021/01/mariadb-web.jpg" alt="Adnc是一个微服务开发框架 代码改变世界 开源推动社区">