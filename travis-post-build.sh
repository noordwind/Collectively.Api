#!/usr/bin/env bash
echo Triggering Docker Hub registry using branch $TRAVIS_BRANCH and webhook URL $DOCKER_HUB_TRIGGER_URL
curl -H "Content-Type: application/json" --data '{"source_type": "Branch", "source_name": "$TRAVIS_BRANCH"}' -X POST $DOCKER_HUB_TRIGGER_URL