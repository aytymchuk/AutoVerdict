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

if ! command -v pnpm &>/dev/null; then
  echo "Error: pnpm is not installed. Install Node.js 22+ and run: npm install -g pnpm" >&2
  exit 1
fi

if [[ ! -f .env ]]; then
  echo "Warning: .env not found. Copy .env.example to .env and set VITE_CLERK_PUBLISHABLE_KEY." >&2
fi

echo "Starting API and infrastructure (MongoDB, Azurite, Seq) — web container skipped for HMR..."
"${DOCKER_COMPOSE_CMD[@]}" up -d --build

echo ""
echo "Backend ready at http://localhost:5065"
echo "Starting Vite dev server at http://localhost:5173 (HMR enabled)..."
echo ""

cd frontend
exec pnpm dev
