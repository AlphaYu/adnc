```bash
docker pull registry:2.7.1

docker run -d \
  -p 5000:5000 \
  --restart=always \
  --name registry \
  -v /root/data/docker-registry:/var/lib/registry \
  registry:2.7.1
```