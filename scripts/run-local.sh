#!/usr/bin/env bash
set -e

echo "Checking prerequisites..."
if ! command -v docker &> /dev/null; then
    echo "Error: docker is not installed." >&2
    exit 1
fi

if docker compose version &> /dev/null; then
    DOCKER_COMPOSE_CMD="docker compose"
elif command -v docker-compose &> /dev/null; then
    DOCKER_COMPOSE_CMD="docker-compose"
else
    echo "Error: neither 'docker compose' nor 'docker-compose' is available." >&2
    exit 1
fi

echo "Starting AutoVerdikt local environment..."
# Docker-first orchestration to bring up the local stack
$DOCKER_COMPOSE_CMD up -d --build

echo "Local environment initialization complete."
