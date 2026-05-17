.PHONY: run clean test-backend up down

run:
	@bash scripts/run-local.sh

clean:
	@bash scripts/clean-all.sh

test-backend:
	@cd backend && dotnet test

up:
	docker compose up -d

down:
	docker compose down

