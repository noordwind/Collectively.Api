#!/bin/bash
export ASPNETCORE_ENVIRONMENT=local
cd src/Collectively.Api
dotnet run --no-restore --urls "http://*:5000"