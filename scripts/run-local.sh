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

echo "Starting AutoVerdikt local environment (API + infra; no Docker web)..."
# Default stack: API + databases. Use `make up-prod` to include the production web image.
$DOCKER_COMPOSE_CMD up -d --build

echo ""
echo "Local environment ready."
echo "  API:  http://localhost:5065"
echo "  Seq:  http://localhost:5341"
echo ""
echo "For frontend hot reload, run: make dev"
echo "  (or: cd frontend && pnpm dev → http://localhost:5173)"
