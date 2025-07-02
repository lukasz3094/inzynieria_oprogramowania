#!/bin/bash

set -e

echo "Building solution..."
dotnet build

ASPNETCORE_ENVIRONMENT=Test dotnet test Tests/UnitTests/UnitTests.csproj

echo "Test run completed."
