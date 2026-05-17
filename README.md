# AutoVerdikt

[![CodeRabbit Pull Request Reviews](https://img.shields.io/coderabbit/prs/github/aytymchuk/AutoVerdict?utm_source=oss&utm_medium=github&utm_campaign=aytymchuk%2FAutoVerdict&labelColor=171717&color=FF570A&link=https%3A%2F%2Fcoderabbit.ai&label=CodeRabbit+Reviews)](https://coderabbit.ai)

AI investigator for used car buyers in Poland. Cross-verifies listing claims against CEPiK records, VIN databases, and user documents to surface fraud and hidden defects.

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | React 19 · TypeScript · Vite 8 · Tailwind CSS v4 · Clerk |
| Backend | .NET 10 Minimal API · Clean Architecture |
| Observability | OpenTelemetry (frontend) · Seq (structured logs) |
| Storage | MongoDB · Azure Blob Storage (Azurite locally) |
| Infrastructure | Docker Compose · Terraform (Azure West Europe) |

## Repository Structure

```
AutoVerdikt/
├── frontend/        React 19 SPA — Feature-Sliced Design architecture
├── backend/
│   ├── src/
│   │   ├── AutoVerdikt.WebApi          Minimal API host, auth, CORS
│   │   ├── AutoVerdikt.Application     Use cases & application logic
│   │   ├── AutoVerdikt.Infrastructure  External integrations
│   │   ├── AutoVerdikt.Store           Data access layer (MongoDB)
│   │   └── AutoVerdikt.Agents          AI agent orchestration
│   └── tests/                          Unit & architecture tests per layer
├── landing/         Static marketing page
├── ascode/          Terraform for Azure West Europe
├── scripts/         Local automation scripts
├── docker-compose.yml
└── .env.example     Environment variable reference
```

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 10 SDK](https://dotnet.microsoft.com/download) — for running the backend locally
- [Node.js 22+](https://nodejs.org/) — for running the frontend locally

## Getting Started

### 1. Configure environment

```bash
cp .env.example .env
# Fill in VITE_CLERK_PUBLISHABLE_KEY and Clerk__ backend vars
```

### 2. Run via Docker (recommended)

Starts all services: API, frontend, MongoDB, Azurite, Seq.

```bash
make up
# or: docker compose up -d
```

| Service | URL |
|---|---|
| Frontend | http://localhost:3000 |
| API | http://localhost:5065 |
| Seq (logs) | http://localhost:5341 |
| MongoDB | mongodb://localhost:27017 |
| Azurite Blob | http://localhost:10000 |

### 3. Run services individually (local dev)

**Frontend** (Vite dev server with HMR):
```bash
cd frontend && npm run dev
```

**Backend** (.NET hot reload):
```bash
cd backend && dotnet run --project src/AutoVerdikt.WebApi
```

> When running the backend locally against `npm run dev`, set `Clerk__AuthorizedParty=http://localhost:5173` in your shell or `.env`.

## Development Commands

```bash
make up              # Start all Docker services (detached)
make down            # Stop all Docker services
make run             # Run via local orchestration script
make clean           # Tear down containers and volumes
make test-backend    # Run all backend test projects
```

## Testing

```bash
# Backend — all layers
cd backend && dotnet test

# Frontend — lint
cd frontend && npm run lint
```

## Environment Variables

See [`.env.example`](.env.example) for a full reference with descriptions. Key variables:

| Variable | Where used | Purpose |
|---|---|---|
| `VITE_CLERK_PUBLISHABLE_KEY` | Frontend (build-time) | Clerk authentication |
| `Clerk__Authority` | Backend (runtime) | Clerk JWT issuer URL |
| `Clerk__AuthorizedParty` | Backend (runtime) | CORS allowed origin |
| `ASPNETCORE_ENVIRONMENT` | Backend | Runtime environment |
| `ACCEPT_EULA=Y` | Seq container | Required to start Seq |