#!/bin/bash
DOCKER_ENV=''
case "$TRAVIS_BRANCH" in
  "master")
    DOCKER_ENV=production
    ;;
  "develop")
    DOCKER_ENV=development
    ;;    
esac

docker login -u $DOCKER_USERNAME -p $DOCKER_PASSWORD
docker build -f ./src/Collectively.Api/Dockerfile.$DOCKER_ENV -t collectively.api:$DOCKER_ENV ./src/Collectively.Api
docker tag collectively.api:$DOCKER_ENV $DOCKER_USERNAME/collectively.api:$DOCKER_ENV
docker push $DOCKER_USERNAME/collectively.api:$DOCKER_ENV