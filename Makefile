.PHONY: run clean test-backend

run:
	@bash scripts/run-local.sh

clean:
	@bash scripts/clean-all.sh

test-backend:
	@cd backend && dotnet test

