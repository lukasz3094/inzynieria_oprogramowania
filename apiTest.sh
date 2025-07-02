#!/bin/bash

set -e

echo "Running API integration tests with SQLite In-Memory..."

echo "Building solution..."
dotnet build

ASPNETCORE_ENVIRONMENT=Test dotnet test Tests/ApiTests/ApiTests.csproj

echo "Test run completed."
