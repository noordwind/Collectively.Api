#!/bin/bash
echo Building and pushing Docker images on branch $TRAVIS_BRANCH
./docker-publish-ci.sh