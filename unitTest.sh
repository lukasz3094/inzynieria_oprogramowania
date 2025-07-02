#!/bin/bash

set -e

echo "Building solution..."
dotnet build

ASPNETCORE_ENVIRONMENT=Test dotnet test Tests/UnitTest/UnitTest.csproj

echo "Test run completed."
