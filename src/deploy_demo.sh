#!/bin/bash

# 环境变量
RUNNER_DEMO_SOURCE_ROOT="/adnc/src/Demo"
PUBLISH_PATH="bin/Release/net8.0/linux-x64/publish"

ADMIN_IMAGE_NAME="adnc-demo-admin-api"
ADMIN_PROJECT_PATH="Admin/Admin.Api"
ADMIN_START_FILE="Adnc.Demo.Admin.Api.dll"

MAINT_IMAGE_NAME="adnc-demo-maint-api"
MAINT_PROJECT_PATH="Maint/Maint.Api"
MAINT_START_FILE="Adnc.Demo.Maint.Api.dll"

CUST_IMAGE_NAME="adnc-demo-cust-api"
CUST_PROJECT_PATH="Cust/Cust.Api"
CUST_START_FILE="Adnc.Demo.Cust.Api.dll"

# 失败时终止脚本的函数
check_error() {
  if [ $? -ne 0 ]; then
    echo "错误: $1 失败!"
    exit 1
  fi
}

# 发布解决方案
dotnet publish "${RUNNER_DEMO_SOURCE_ROOT}/Adnc.Demo.sln" --configuration Release --runtime linux-x64 --self-contained false
check_error "dotnet publish"

# 构建镜像的函数
build_and_push_image() {
  local IMAGE_NAME=$1
  local PROJECT_PATH=$2
  local START_FILE=$3

  echo "--- 构建镜像: ${IMAGE_NAME} ---"
  TARGET_DIR="${RUNNER_DEMO_SOURCE_ROOT}/${PROJECT_PATH}/${PUBLISH_PATH}"
  
  if [ ! -d "$TARGET_DIR" ]; then
    echo "目录 $TARGET_DIR 不存在，正在创建..."
    mkdir -p "$TARGET_DIR"
    if [ $? -eq 0 ]; then
      echo "目录 $TARGET_DIR 创建成功！"
    else
      echo "创建目录 $TARGET_DIR 失败！"
      exit 1
    fi
  fi

  cd "$TARGET_DIR"
  check_error "进入目录 $TARGET_DIR"

  echo "当前目录---${PWD}"

  # 创建 Dockerfile
  cat <<EOF > Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY . /app
ENTRYPOINT [ "dotnet", "$START_FILE" ]
EOF

  echo "正在构建镜像: $IMAGE_NAME"
  docker build . --file Dockerfile --tag "$IMAGE_NAME"
  check_error "Docker 构建 $IMAGE_NAME"

  IMAGE_ID=$(echo "$IMAGE_NAME" | tr '[:upper:]' '[:lower:]')
  echo "IMAGE_ID: $IMAGE_ID"

  VERSION=$(date +%s)
  docker tag "$IMAGE_NAME" "$IMAGE_ID:$VERSION"
  echo "Docker tag version $IMAGE_ID:$VERSION"
  check_error "Docker tag version $IMAGE_ID:$VERSION"
  
  # 停止并删除旧容器
  CONTAINER_IDS=$(docker ps -a --filter "name=${IMAGE_ID}" --format "{{.ID}}")
  if [ -n "$CONTAINER_IDS" ]; then
    docker stop $CONTAINER_IDS
    echo "停止容器 $CONTAINER_IDS"
    check_error "停止容器 $CONTAINER_IDS"
    
    docker rm $CONTAINER_IDS
    echo "删除容器 $CONTAINER_IDS"
    check_error "删除容器 $CONTAINER_IDS"
  fi
  
  docker rmi "${IMAGE_ID}:latest"

  docker tag "$IMAGE_ID:$VERSION" "${IMAGE_ID}:latest"
  echo "Docker tag latest $IMAGE_ID:latest"
  check_error "Docker tag $IMAGE_ID:latest"
}

#删除容器/镜像函数
remove_container_image(){
  local IMAGE_NAME=$1
  local IMAGE_ID=$(echo "$IMAGE_NAME" | tr '[:upper:]' '[:lower:]')

  #删除旧镜像
  #获取镜像ID和标签
  # IMAGE_LIST=$(docker images --filter=reference="${IMAGE_ID}" --format "{{.ID}} {{.Repository}}:{{.Tag}}")
  IMAGE_LIST=$(docker images --filter=reference="${IMAGE_NAME}" --format "{{.Tag}}")
  # 如果镜像存在，检查并删除
  if [ -n "$IMAGE_LIST" ]; then
    echo "检查并删除旧镜像，排除 :latest 标签的镜像..."
    
    # 遍历每个镜像
    for IMAGE in $IMAGE_LIST; do
    echo "-----IMAGE--------- $IMAGE"
      IMAGE_TAG=$(echo $IMAGE | awk '{print $1}')  # 提取镜像标签

      IMAGE_FULL_NAME="${IMAGE_ID}:${IMAGE_TAG}"
      echo "-----IMAGE_FULL_NAME-----: $IMAGE_FULL_NAME"

      # 判断镜像标签是否为 :latest
      if [[ "$IMAGE_FULL_NAME" != *":latest" ]]; then
        echo "删除镜像 $IMAGE_FULL_NAME ..."
        docker rmi -f $IMAGE_FULL_NAME  # 强制删除
        check_error "删除旧镜像 $IMAGE_FULL_NAME"
      else
        echo "跳过删除镜像 $IMAGE_FULL_NAME - 带有 :latest 标签"
      fi
    done
  else
    echo "镜像 $IMAGE_FULL_NAME 不存在，跳过删除。"
  fi
}

# 部署镜像的函数
deploy_image() {
  local IMAGE_NAME=$1
  local IMAGE_ID=$(echo "$IMAGE_NAME" | tr '[:upper:]' '[:lower:]')

  echo "--- 部署镜像: ${IMAGE_NAME} ---"

  # 运行新容器
  #-e ASPNETCORE_HOSTINGSTARTUPASSEMBLIES=SkyAPM.Agent.AspNetCore
  #-e SKYWALKING__SERVICENAME="${IMAGE_ID}"
  #-m 150M 
  docker run \
    -d \
    --name="${IMAGE_ID}-${RANDOM}" \
    -e ASPNETCORE_ENVIRONMENT=Staging \
    -e TZ=Asia/Shanghai \
    "${IMAGE_ID}:latest"
  echo "启动新容器 $IMAGE_ID"
  check_error "启动新容器 $IMAGE_ID"
}

# admin-demo-admin-api
build_and_push_image "$ADMIN_IMAGE_NAME" "$ADMIN_PROJECT_PATH" "$ADMIN_START_FILE"
remove_container_image "$ADMIN_IMAGE_NAME"
deploy_image "$ADMIN_IMAGE_NAME"

# admin-demo-maint-api
build_and_push_image "$MAINT_IMAGE_NAME" "$MAINT_PROJECT_PATH" "$MAINT_START_FILE"
remove_container_image "$MAINT_IMAGE_NAME"
deploy_image "$MAINT_IMAGE_NAME"

# admin-demo-cust-api
build_and_push_image "$CUST_IMAGE_NAME" "$CUST_PROJECT_PATH" "$CUST_START_FILE"
remove_container_image "$CUST_IMAGE_NAME"
deploy_image "$CUST_IMAGE_NAME"