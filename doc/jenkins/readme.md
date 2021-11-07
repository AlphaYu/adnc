# JEKINS
## Jekins自动部署SHELL脚本
```
#!/bin/bash
#version
#-------------------------------------------------------------------------------------------------------
adnc_version_tag=0.8.6.0
#adnc_version_tag=$(date "+%s")
#-------------------------------------------------------------------------------------------------------

#publish && docker build
#-------------------------------------------------------------------------------------------------------
echo "adnc.healthui publish && docker build"
cd src/ServerApi/Infrastructures/Adnc.Infra.HealthCheckUI
dotnet restore
dotnet publish --configuration Release --runtime linux-x64 --self-contained false
cd bin/Release/net5.0/linux-x64/publish
docker build -f /root/data/dockerfiles/healthui -t adnc.healthui:$adnc_version_tag .

echo "adnc.gateway publish && docker build"
cd $WORKSPACE
cd src/ServerApi/Infrastructures/Adnc.Infra.Gateway
dotnet restore
dotnet publish --configuration Release --runtime linux-x64 --self-contained false
cd bin/Release/net5.0/linux-x64/publish
docker build -f /root/data/dockerfiles/gateway -t adnc.gateway:$adnc_version_tag .

echo "adnc.usr publish && docker build"
cd $WORKSPACE
cd src/ServerApi/Services/Adnc.Usr/Adnc.Usr.WebApi
dotnet restore
dotnet publish --configuration Release --runtime linux-x64 --self-contained false
cd bin/Release/net5.0/linux-x64/publish
docker build -f /root/data/dockerfiles/usr -t adnc.usr.webapi:$adnc_version_tag .

echo "adnc.maint publish && docker build"
cd $WORKSPACE
cd src/ServerApi/Services/Adnc.Maint/Adnc.Maint.WebApi
dotnet restore
dotnet publish --configuration Release --runtime linux-x64 --self-contained false
cd bin/Release/net5.0/linux-x64/publish
docker build -f /root/data/dockerfiles/maint -t adnc.maint.webapi:$adnc_version_tag .

echo "adnc.cus publish && docker build"
cd $WORKSPACE
cd src/ServerApi/Services/Adnc.Cus/Adnc.Cus.WebApi
dotnet restore
dotnet publish --configuration Release --runtime linux-x64 --self-contained false
cd bin/Release/net5.0/linux-x64/publish
docker build -f /root/data/dockerfiles/cus -t adnc.cus.webapi:$adnc_version_tag .
#-------------------------------------------------------------------------------------------------------

#remove docker container
#-------------------------------------------------------------------------------------------------------
dockercontainers=`docker ps -a | grep 'healthui' |awk '{print $1}'`
dockerimages=`docker ps -a | grep 'healthui' |awk '{print $2}'`
if test -n "$dockercontainers"; 
then 
docker container stop $dockercontainers;
docker container rm $dockercontainers;
fi;

if test -n "$dockerimages"
then
docker rmi -f $dockerimages
fi;

dockercontainers=`docker ps -a | grep 'gateway' |awk '{print $1}'`
dockerimages=`docker ps -a | grep 'gateway' |awk '{print $2}'`
if test -n "$dockercontainers"; 
then 
docker container stop $dockercontainers;
docker container rm $dockercontainers;
fi;

if test -n "$dockerimages"
then
docker rmi -f $dockerimages
fi;

dockercontainers=`docker ps -a | grep 'adnc.usr' |awk '{print $1}'`
dockerimages=`docker ps -a | grep 'adnc.usr' |awk '{print $2}'`
if test -n "$dockercontainers"; 
then 
docker container stop $dockercontainers;
docker container rm $dockercontainers;
fi;

if test -n "$dockerimages"
then
docker rmi -f $dockerimages
fi;

dockercontainers=`docker ps -a | grep 'adnc.maint' |awk '{print $1}'`
dockerimages=`docker ps -a | grep 'adnc.maint' |awk '{print $2}'`
if test -n "$dockercontainers"; 
then 
docker container stop $dockercontainers;
docker container rm $dockercontainers;
fi;

if test -n "$dockerimages"
then
docker rmi -f $dockerimages
fi;

dockercontainers=`docker ps -a | grep 'adnc.cus' |awk '{print $1}'`
dockerimages=`docker ps -a | grep 'adnc.cus' |awk '{print $2}'`
if test -n "$dockercontainers"; 
then 
docker container stop $dockercontainers;
docker container rm $dockercontainers;
fi;

if test -n "$dockerimages"
then
docker rmi -f $dockerimages
fi;
#-------------------------------------------------------------------------------------------------------

#run docker container
#-------------------------------------------------------------------------------------------------------
#docker tag adnc-healthui:$healthui_tag 172.16.101.220:5000/adnc-healthui:$adnc_version_tag
#docker push 172.16.101.220:5000/adnc-healthui:$adnc_version_tag
docker run --name adnc.usr.webapi.01 -d -e ASPNETCORE_ENVIRONMENT=production adnc.usr.webapi:$adnc_version_tag
docker run --name adnc.usr.webapi.02 -d -e ASPNETCORE_ENVIRONMENT=production adnc.usr.webapi:$adnc_version_tag

docker run --name adnc.maint.webapi.01 -d -e ASPNETCORE_ENVIRONMENT=production adnc.maint.webapi:$adnc_version_tag
docker run --name adnc.maint.webapi.02 -d -e ASPNETCORE_ENVIRONMENT=production adnc.maint.webapi:$adnc_version_tag

docker run --name adnc.cus.webapi.01 -d -e ASPNETCORE_ENVIRONMENT=production adnc.cus.webapi:$adnc_version_tag
docker run --name adnc.cus.webapi.02 -d -e ASPNETCORE_ENVIRONMENT=production adnc.cus.webapi:$adnc_version_tag

docker run --name adnc.healthui -d -p 8666:80 -e ASPNETCORE_ENVIRONMENT=production adnc.healthui:$adnc_version_tag
docker run --name adnc.gateway -d -p 8888:80 -e ASPNETCORE_ENVIRONMENT=production adnc.gateway:$adnc_version_tag
#-------------------------------------------------------------------------------------------------------
#end
```

## jekkins 按照脚本
```
yum -y install git
#按照提示，按三次回车，即可生成sshkey.
ssh-keygen -t rsa -C "xyz199809@gmail.com"
#查看公钥，复制生成后的sshkey，配置到代码仓库(gitee)的公钥中。
cat ~/.ssh/id_rsa.pub
#添加主机到本机SSH可信列表,输入yes
ssh -T git@gitee.com
#安装sdk与运行时
sudo yum install -y dotnet-sdk-5.0
sudo yum install -y aspnetcore-runtime-5.0


#安装java sdk
yum install java-1.8.0-openjdk* -y
```

## jekins docker 按照脚本
```
mkdir jenkins
cd jenkis
docker build -t adnc-jenkis .
mkdir jenkins_home
// 创建容器
docker run -d --name jenkins_01 -p 8080:8080 -v /root/jenkins/jenkins_home:/home/jenkins_01 adnc-jenkis
docker run -u root --rm -d -p 8080:8080 --name jenkins \
-v /etc/localtime:/etc/localtime \
-v /usr/bin/docker:/usr/bin/docker \
-v /var/run/docker.sock:/var/run/docker.sock \
-v /root/jenkins/jenkins_home:/var/jenkins_home \
adnc-jenkis
// 进入容器 
docker exec -it jenkins bash 
//进入jenkins界面
//http://你的ip:8080
// 查看密码 
cat /var/jenkins_home/secrets/initialAdminPassword
//选择推荐需要安装的组件

```