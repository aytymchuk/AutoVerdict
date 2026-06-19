.PHONY: run dev clean build-full build-backend build-frontend test-full test-backend test-frontend up down

run:
	@bash scripts/run-local.sh

dev:
	@bash scripts/dev-frontend.sh

clean:
	@bash scripts/clean-all.sh

build-full: build-backend build-frontend

build-backend:
	@cd backend && dotnet build

build-frontend:
	@cd frontend && pnpm lint && pnpm build

test-full: test-backend test-frontend

test-backend:
	@cd backend && dotnet test

test-frontend:
	@cd frontend && pnpm test

# API + infra + Vite dev server (HMR on :5173)
up:
	docker compose up -d --build

down:
	docker compose down

