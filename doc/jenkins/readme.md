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