# AGENTS.md — AutoVerdikt Monorepo Root

## Project Overview
AutoVerdikt is an AI investigator for used car buyers in Poland. It cross-verifies listing claims against CEPiK records, VIN databases, and user documents to surface fraud and hidden defects.

## Repository Layout
- frontend/   React 19 + TypeScript + Vite + Tailwind v4 + shadcn/ui
- backend/    .NET 10 Minimal API, Clean Architecture
- landing/    Static HTML/CSS/JS landing page
- ascode/     Terraform for Azure West Europe
- .github/    CI/CD workflows
- scripts/    Local automation scripts

## Core Rules & Policies (Non-Negotiable)
1. **Language Policy:** All user-facing text defaults to English. Supported: en, uk, pl. Russian is NEVER supported (no text, comments, or translation keys). If browser language is 'ru', fallback to 'en'.
2. **AI Grounding:** Every flag or claim in a generated report must cite its source (e.g., "CEPiK, p. 2"). No citation = no flag. No AI speculation.
3. **Secrets:** Never hardcode keys. Use `.env.local` for local development and Azure Key Vault for production.

## Local Development Stack
- **Frontend:** Run `cd frontend && pnpm dev` (Vite dev server).
- **Backend:** Run `cd backend && dotnet run` (.NET 10 Minimal API).
- **Databases:** MongoDB, Azurite, Seq are pre-configured in `docker-compose.yml`. Use `docker-compose up -d` to start the local stack.

## Tooling Reference
- **Code Formatting & Linting:** Run `pnpm lint` in the `frontend/` directory to lint and format code.
- **Testing:** Run `dotnet test` in `backend/` for backend verification, and `pnpm test` in `frontend/` for frontend testing.
- **Builds:** Run `dotnet build` in `backend/` and `pnpm build` in `frontend/` to compile the respective projects.