#!/usr/bin/env bash
echo Triggering Docker Hub registry build using branch $TRAVIS_BRANCH
curl -H "Content-Type: application/json" --data '{"source_type": "Branch", "source_name": "'"$TRAVIS_BRANCH"'"}' -X POST $DOCKER_HUB_TRIGGER_URL