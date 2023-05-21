```shell
#install
docker exec -it aef2acae87dd /bin/bash
cd /
mkdir dotmemory
cd dotmemory
mkdir bin
cd bin
apt-get update -y
apt-get install -y wget
wget -O dotMemoryclt.zip https://www.nuget.org/api/v2/package/JetBrains.dotMemory.Console.linux-x64/2021.3.2
apt-get install -y unzip && unzip dotMemoryclt.zip -d ./dotMemoryclt
chmod +x -R dotMemoryclt/*

#collecting
apt-get update && apt-get install procps
ps aux

cd /dotmemory/bin/dotMemoryclt/tools
./dotMemory.sh get-snapshot 1  --save-to-dir=Snapshots
exit

#move dump file
docker cp aef2acae87dd:/dotmemory/bin/dotMemoryclt/tools/Snapshots/[1]-dotnet.2023-05-19T22-54-53.166.dmw /dotmemory.dmw




```

