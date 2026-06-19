#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT"

if ! command -v docker &>/dev/null; then
  echo "Error: docker is not installed." >&2
  exit 1
fi

if docker compose version &>/dev/null; then
  DOCKER_COMPOSE_CMD=(docker compose)
elif command -v docker-compose &>/dev/null; then
  DOCKER_COMPOSE_CMD=(docker-compose)
else
  echo "Error: neither 'docker compose' nor 'docker-compose' is available." >&2
  exit 1
fi

if [[ ! -f .env ]]; then
  echo "Warning: .env not found. Copy .env.example to .env and set VITE_CLERK_PUBLISHABLE_KEY." >&2
fi

echo "Starting full dev stack (API, infra, Vite web with HMR)..."
"${DOCKER_COMPOSE_CMD[@]}" up -d --build

echo ""
echo "Dev stack ready:"
echo "  Frontend (Vite): http://localhost:5173"
echo "  API:             http://localhost:5065"
echo "  Seq:             http://localhost:5341"
echo ""
echo "Logs: ${DOCKER_COMPOSE_CMD[*]} logs -f web"
echo "Stop: ${DOCKER_COMPOSE_CMD[*]} down"
