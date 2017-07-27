#!/bin/bash
export ASPNETCORE_ENVIRONMENT=local
cd src/Collectively.Api
dotnet run --urls "http://*:5000"