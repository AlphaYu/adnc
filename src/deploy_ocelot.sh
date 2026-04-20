#!/bin/bash

# Environment variables
RUNNER_DEMO_SOURCE_ROOT="/opt/adnc/src/Gateways/Ocelot"
PUBLISH_PATH="bin/Release/net8.0/linux-x64/publish"

OCELOT_IMAGE_NAME="adnc-gateway-ocelot"
OCELOT_PROJECT_PATH=""
OCELOT_START_FILE="Adnc.Gateway.Ocelot.dll"

# Function to stop the script on failure
check_error() {
  if [ $? -ne 0 ]; then
    echo "Error: $1 failed!"
    exit 1
  fi
}

# Publish the solution
dotnet publish "${RUNNER_DEMO_SOURCE_ROOT}/Adnc.Ocelot.sln" --configuration Release --runtime linux-x64 --self-contained false
check_error "dotnet publish"

# Function to build the image
build_and_push_image() {
  local IMAGE_NAME=$1
  local PROJECT_PATH=$2
  local START_FILE=$3

  echo "--- Building image: ${IMAGE_NAME} ---"
  TARGET_DIR="${RUNNER_DEMO_SOURCE_ROOT}/${PROJECT_PATH}/${PUBLISH_PATH}"

  if [ ! -d "$TARGET_DIR" ]; then
    echo "Directory $TARGET_DIR does not exist, creating it..."
    mkdir -p "$TARGET_DIR"
    if [ $? -eq 0 ]; then
      echo "Directory $TARGET_DIR was created successfully!"
    else
      echo "Failed to create directory $TARGET_DIR!"
      exit 1
    fi
  fi

  cd "$TARGET_DIR"
  check_error "Enter directory $TARGET_DIR"

  echo "Current directory --- ${PWD}"

  # Create the Dockerfile
  cat <<EOF > Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY . /app
ENTRYPOINT [ "dotnet", "$START_FILE" ]
EOF

  echo "Building image: $IMAGE_NAME"
  docker build . --file Dockerfile --tag "$IMAGE_NAME"
  check_error "Docker build $IMAGE_NAME"

  IMAGE_ID=$(echo "$IMAGE_NAME" | tr '[:upper:]' '[:lower:]')
  echo "IMAGE_ID: $IMAGE_ID"

  VERSION=$(date +%s)
  docker tag "$IMAGE_NAME" "$IMAGE_ID:$VERSION"
  echo "Docker tag version $IMAGE_ID:$VERSION"
  check_error "Docker tag version $IMAGE_ID:$VERSION"

  # Stop and remove old containers
  CONTAINER_IDS=$(docker ps -a --filter "name=${IMAGE_ID}" --format "{{.ID}}")
  if [ -n "$CONTAINER_IDS" ]; then
    docker stop $CONTAINER_IDS
    echo "Stopped containers $CONTAINER_IDS"
    check_error "Stop containers $CONTAINER_IDS"

    docker rm $CONTAINER_IDS
    echo "Removed containers $CONTAINER_IDS"
    check_error "Remove containers $CONTAINER_IDS"
  fi

  docker rmi "${IMAGE_ID}:latest"

  docker tag "$IMAGE_ID:$VERSION" "${IMAGE_ID}:latest"
  echo "Docker tag latest $IMAGE_ID:latest"
  check_error "Docker tag $IMAGE_ID:latest"
}

# Function to remove containers/images
remove_container_image(){
  local IMAGE_NAME=$1
  local IMAGE_ID=$(echo "$IMAGE_NAME" | tr '[:upper:]' '[:lower:]')

  # Remove old images
  # Get image IDs and tags
  # IMAGE_LIST=$(docker images --filter=reference="${IMAGE_ID}" --format "{{.ID}} {{.Repository}}:{{.Tag}}")
  IMAGE_LIST=$(docker images --filter=reference="${IMAGE_NAME}" --format "{{.Tag}}")
  # If images exist, inspect and remove them
  if [ -n "$IMAGE_LIST" ]; then
    echo "Checking and deleting old images, excluding those tagged :latest..."

    # Iterate through each image
    for IMAGE in $IMAGE_LIST; do
    echo "-----IMAGE--------- $IMAGE"
      IMAGE_TAG=$(echo $IMAGE | awk '{print $1}')  # Extract the image tag

      IMAGE_FULL_NAME="${IMAGE_ID}:${IMAGE_TAG}"
      echo "-----IMAGE_FULL_NAME-----: $IMAGE_FULL_NAME"

      # Check whether the image tag is :latest
      if [[ "$IMAGE_FULL_NAME" != *":latest" ]]; then
        echo "Deleting image $IMAGE_FULL_NAME ..."
        docker rmi -f $IMAGE_FULL_NAME  # Force delete
        check_error "Delete old image $IMAGE_FULL_NAME"
      else
        echo "Skipping image $IMAGE_FULL_NAME because it has the :latest tag"
      fi
    done
  else
    echo "Image $IMAGE_FULL_NAME does not exist, skipping deletion."
  fi
}

# Function to deploy the image
deploy_image() {
  local IMAGE_NAME=$1
  local IMAGE_ID=$(echo "$IMAGE_NAME" | tr '[:upper:]' '[:lower:]')

  echo "--- Deploying image: ${IMAGE_NAME} ---"

  # Run the new container
  # -e ASPNETCORE_HOSTINGSTARTUPASSEMBLIES=SkyAPM.Agent.AspNetCore
  # -e SKYWALKING__SERVICENAME="${IMAGE_ID}"
  # -m 100M \
  docker run \
    -d \
    --name="${IMAGE_ID}-${RANDOM}" \
    --network adnc_network_main \
    -p 5000:80 \
    -e ASPNETCORE_ENVIRONMENT=Staging \
    -e TZ=Asia/Shanghai \
    "${IMAGE_ID}:latest"
  echo "Started new container $IMAGE_ID"
  check_error "Start new container $IMAGE_ID"
}

# adnc-gateway-ocelot
build_and_push_image "$OCELOT_IMAGE_NAME" "$OCELOT_PROJECT_PATH" "$OCELOT_START_FILE"
remove_container_image "$OCELOT_IMAGE_NAME"
deploy_image "$OCELOT_IMAGE_NAME"
