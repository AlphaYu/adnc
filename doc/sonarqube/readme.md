```bash
--安装服务端
--https://www.cnblogs.com/7tiny/p/11342902.html
mkdir /root/sonarqube
cd  /root/sonarqube
docker run -d --name sonarqube \
-v /root/sonarqube/conf:/opt/sonarqube/conf \
-v /root/sonarqube/data:/opt/sonarqube/data \
-v /root/sonarqube/logs:/opt/sonarqube/logs \
-v /root/sonarqube/extensions:/opt/sonarqube/extensions \
-e SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true \
-p 19000:9000 \
sonarqube:8.9.0-community

--VS-NUGET
dotnet tool install --global dotnet-sonarscanner

dotnet sonarscanner begin /k:"adnc" /d:sonar.host.url="http://193.112.75.77:9000"  /d:sonar.login="a5649ec24aaa79f4934bbe959b346af6cc079aad"

dotnet build

dotnet sonarscanner end /d:sonar.login="a5649ec24aaa79f4934bbe959b346af6cc079aad"
```