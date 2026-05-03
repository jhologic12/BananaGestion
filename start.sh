#!/bin/sh
PORT=${PORT:-8080}
echo "Starting BananaGestion API on port $PORT"
exec dotnet BananaGestion.Api.dll --urls="http://+:$PORT"
