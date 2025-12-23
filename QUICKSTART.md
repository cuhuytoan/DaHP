# Quick Start Guide

Get started with the CMS project in 3 simple steps!

## Prerequisites

- Docker Desktop
- .NET 8 SDK
- Make (optional, for convenience commands)

## Quick Start Commands

### Using Make (Recommended)

```bash
# 1. Start database services
make dev-up

# 2. Apply database migrations
make migrate

# 3. Run backend API
make api-watch

# View all available commands
make help
```

### Using Docker Compose Directly

```bash
# 1. Start database services
docker-compose -f docker-compose.dev.yml up -d

# 2. Apply migrations
cd backend/CMS.API
dotnet ef database update

# 3. Run backend API
dotnet watch run
```

## Verify Setup

After starting services:

1. **Check services status:**
   ```bash
   make dev-ps
   # or
   docker-compose -f docker-compose.dev.yml ps
   ```

2. **Access API:**
   - API: http://localhost:5000
   - Swagger: http://localhost:5000/swagger

3. **Check logs:**
   ```bash
   make dev-logs
   # or
   docker-compose -f docker-compose.dev.yml logs -f
   ```

## Common Tasks

### Database Management

```bash
# Connect to database
make db-connect

# Backup database
make db-backup

# Restore from backup
make db-restore FILE=backup_20231203_120000.sql
```

### Redis Management

```bash
# Connect to Redis CLI
make redis-connect

# Flush all cached data
make redis-flush
```

### Migration Management

```bash
# Create new migration
make migrate-create NAME=AddUserProfileTable

# Apply migrations
make migrate

# List all migrations
make migrate-list

# Rollback last migration
make migrate-rollback
```

## Stop Services

```bash
# Stop services (keep data)
make dev-down

# Stop and remove all data
make clean
```

## Troubleshooting

### Port already in use

Edit `.env` file and change ports:
```env
POSTGRES_PORT=5433
REDIS_PORT=6380
```

### Database connection error

1. Ensure PostgreSQL is running: `make dev-ps`
2. Check logs: `make dev-logs`
3. Restart services: `make dev-down && make dev-up`

### Migration errors

1. Check database connection in `appsettings.json`
2. Ensure connection string matches `.env` settings:
   ```json
   "DefaultConnection": "Host=localhost;Port=5432;Database=cms_db;Username=cms_user;Password=cms_password_dev"
   ```

## Architecture Overview

```
┌─────────────────────────────────────────┐
│  Development Environment                │
├─────────────────────────────────────────┤
│                                         │
│  Backend API (Local)                    │
│  └─ .NET 8 Web API                      │
│     └─ Port: 5000                       │
│        └─ Swagger: /swagger             │
│                                         │
│  ┌─────────────────┐  ┌──────────────┐ │
│  │  PostgreSQL     │  │   Redis      │ │
│  │  (Docker)       │  │   (Docker)   │ │
│  │  Port: 5432     │  │   Port: 6379 │ │
│  └─────────────────┘  └──────────────┘ │
└─────────────────────────────────────────┘
```

## Environment Variables

Key environment variables in `.env`:

```env
# Database
POSTGRES_DB=cms_db
POSTGRES_USER=cms_user
POSTGRES_PASSWORD=cms_password_dev

# Redis
REDIS_PASSWORD=redis_password_dev

# JWT (configured in appsettings.json)
JWT_SECRET_KEY=dev-secret-key-with-at-least-32-characters...
```

## Next Steps

1. Read [DOCKER.md](./DOCKER.md) for detailed Docker documentation
2. Read [CLAUDE.md](./CLAUDE.md) for project architecture and guidelines
3. Check [docs/migration-plan.md](./docs/migration-plan.md) for migration strategy

## Support

- **Issues**: Create an issue in the repository
- **Documentation**: Check [DOCKER.md](./DOCKER.md) for comprehensive guide
- **Make commands**: Run `make help` to see all available commands
