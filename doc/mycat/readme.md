<div align="center">
<a href="https://github.com/alphayu/adnc" target="_blank" title="Adnc是一个微服务开发框架 代码改变世界 开源推动社区"><img src="https://aspdotnetcore.net/wp-content/uploads/2020/12/adnc-homepage-logo-3.webp" alt="Adnc是一个微服务开发框架 代码改变世界 开源推动社区"></a>
</div>

# docker安装mycat
mycat目前稳定版本是1.6.7.x版本，本文选择了1.6.7.6。
mycat安装前必须已经部署好mariadb集群。
如何部署，请参考<a href="https://github.com/AlphaYu/Adnc/tree/master/doc/mariadb" target="_blank" alt="mariadb基于GTID主从复制搭建">mariadb基于GTID主从复制搭建</a>。
## 下载mycat安装包
```
mkdir /root/data/mycat
cd /root/data/mycat
#下载mycat release1.6.7.6到当前目录
wget http://dl.mycat.org.cn/1.6.7.6/20201126013625/Mycat-server-1.6.7.6-release-20201126013625-linux.tar.gz
mv Mycat-server-1.6.7.6-release-20201126013625-linux.tar.gz mycat1.6.7.6.tar.gz
#解压conf目录到当前目录，因为使用docker直接挂载conf目录会报错，mycat启动时需要依赖conf目录中的文件。
tar -zxvf mycat1.6.7.6.tar.gz -C /root/data/ mycat/conf
```

## 替换配置文件
从 https://github.com/AlphaYu/Adnc/tree/master/doc/mycat 下载已经配置好的`server.xml`与`schema.xml`替换`/root/data/mycat/conf`目录中的同名文件。

- server.xml 关键节点介绍
```xml
  <!-- mycat的账号 -->
  <user name="root" defaultAccount="true"> 
    <!-- 密码 -->
    <property name="password">alpha.mycat</property>  
    <!-- 该账号可以访问的逻辑表,对应schema.xml文件的schema节点的name-->
    <property name="schemas">adnc_usr,adnc_maint,adnc_cus</property> 
  </user> 
```
- schema.xml 关键节点介绍
```xml
<?xml version="1.0"?>
<!DOCTYPE mycat:schema SYSTEM "schema.dtd">
<mycat:schema xmlns:mycat="http://io.mycat/">
    <!-- 配置3个逻辑库-->
	<schema name="adnc_usr" checkSQLschema="true" sqlMaxLimit="100" dataNode="dn_usr"></schema>
	<schema name="adnc_maint" checkSQLschema="true" sqlMaxLimit="100" dataNode="dn_maint"></schema>
	<schema name="adnc_cus" checkSQLschema="true" sqlMaxLimit="100" dataNode="dn_cus"></schema>

    <!-- 逻辑库对应的真实数据库-->
	<dataNode name="dn_usr" dataHost="dh_adnc" database="adnc_usr" />
	<dataNode name="dn_maint" dataHost="dh_adnc" database="adnc_maint" />
	<dataNode name="dn_cus" dataHost="dh_adnc" database="adnc_cus" />

    <!--真实数据库所在的服务器地址，这里配置了1主2从。主服务器(hostM1)宕机会自动切换到(hostS1) -->
	<dataHost name="dh_adnc" maxCon="1000" minCon="10" balance="1" writeType="0" dbType="mysql" dbDriver="native">
		<heartbeat>select user()</heartbeat>
		<writeHost host="hostM1" url="172.20.0.11:3306" user="root" password="alpha.abc" >
			<readHost host="hostS2" url="172.20.0.13:3306" user="root" password="alpha.abc" />
		</writeHost>
		<writeHost host="hostS1" url="172.20.0.12:3306" user="root" password="alpha.abc" />
	</dataHost>

</mycat:schema>
```
- 每个节点详细介绍请参考 <a href="http://www.mycat.org.cn/document/mycat-definitive-guide.pdf" target="_blank">mycat权威指南</a>

## 下载dockerfile
由于mycat官方并没有提供docker镜像，我们需要自己编写dockerfile文件打包镜像。当然你也可以不采用docker部署，直接部署在linux上。
```
#下载dockerfile文件到当前目录
wget https://raw.githubusercontent.com/AlphaYu/Adnc/master/doc/mycat/Dockerfile
#如果下载失败，请手动下载并上传到/root/data/mycat目录，文件地址如下
#https://github.com/AlphaYu/Adnc/tree/master/doc/mycat
```
最终目录结构如下图
![.NET微服务开源框架-mycat部署](https://aspdotnetcore.net/wp-content/uploads/2020/12/mycat_dir.jpg)
## 创建mycat镜像与容器

```
#创建镜像文件
docker build -t mycat:1.6.7.6 .
#创建容器并挂载配置文件目录与日志目录
docker run --privileged=true -p 18066:8066 -p 19066:9066 --name mycat -v /root/data/mycat/conf:/usr/local/mycat/conf -v /root/data/mycat/logs:/usr/local/mycat/logs --network=adnc_net --ip 172.20.0.16 -d mycat:1.6.7.6
```
## 验证
```
docker exec -it mariadb01 /bin/bash
# 登录mycat
mysql -uroot -palpha.mycat -P8066 -h172.20.0.16
# 显示所有数据库
show databases;
#结果如下
+------------+
| DATABASE   |
+------------+
| adnc_cus   |
| adnc_maint |
| adnc_usr   |
+------------+
```
## mycat-web安装
```
mkdir /root/data/mycat-web
cd /root/data/mycat-web
wget http://dl.mycat.org.cn/mycat-web-1.0/Mycat-web-1.0-SNAPSHOT.war

# 运行
docker run --name mycat-web -d -p 18082:8082 -e TZ="Asia/Shanghai" --restart=always --network=adnc_net --ip 172.20.0.15 registry.cn-hangzhou.aliyuncs.com/zhengqing/mycat-web
```