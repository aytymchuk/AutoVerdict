.PHONY: run dev clean test-backend up up-prod down

run:
	@bash scripts/run-local.sh

dev:
	@bash scripts/dev-frontend.sh

clean:
	@bash scripts/clean-all.sh

test-backend:
	@cd backend && dotnet test

# API + infra only (Vite on host via `make dev`)
up:
	docker compose up -d

# Full stack including production web image on :3000
up-prod:
	docker compose --profile production up -d --build

down:
	docker compose down

