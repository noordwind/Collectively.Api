#!/usr/bin/env bash
MYGET_ENV=""
case "$TRAVIS_BRANCH" in
  "develop")
    MYGET_ENV="-dev"
    ;;
esac

dotnet restore --source "https://api.nuget.org/v3/index.json" --source "https://www.myget.org/F/coolector$MYGET_ENV/api/v3/index.json" --no-cache
dotnet build **/project.json
