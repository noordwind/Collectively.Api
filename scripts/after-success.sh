#!/bin/bash
echo Building and pushing Docker images on branch $TRAVIS_BRANCH
./scripts/docker-publish-ci.sh