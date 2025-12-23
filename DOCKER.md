# Docker Setup Guide

This guide explains how to run the CMS project using Docker Compose.

## Prerequisites

- Docker Desktop installed (version 20.10+)
- Docker Compose (comes with Docker Desktop)
- At least 2GB RAM available for Docker
- .NET 8 SDK (for running backend API locally)

## Development Setup (Recommended)

For active development, use `docker-compose.dev.yml` which only runs PostgreSQL and Redis. You'll run the backend API locally for faster iteration.

### 1. Environment Setup

Copy the example environment file and customize if needed:

```bash
cp .env.example .env
```

Edit `.env` file to update passwords and secrets for production use.

### 2. Start Database Services Only

Start PostgreSQL and Redis:

```bash
docker-compose -f docker-compose.dev.yml up -d
```

This will:
- Pull required Docker images (first time only)
- Start PostgreSQL on port 5432
- Start Redis on port 6379

### 3. Run Backend API Locally

In a separate terminal, run the backend API:

```bash
cd backend/CMS.API
dotnet run
```

Or with watch mode (auto-reload on changes):

```bash
cd backend/CMS.API
dotnet watch run
```

The API will be available at `http://localhost:5000`

### 4. Run Database Migrations

Apply database migrations:

```bash
cd backend/CMS.API
dotnet ef database update
```

## Full Stack Setup (Production-like)

Use `docker-compose.yml` when backend implementation is complete and you want to run everything in containers.

**Note**: Currently commented out in docker-compose.yml. Uncomment the API service when ready.

```bash
docker-compose up -d
```

### 3. Verify Services

Check if all services are running:

```bash
docker-compose ps
```

Check logs:

```bash
# All services
docker-compose logs

# Specific service
docker-compose logs api
docker-compose logs postgres
docker-compose logs redis

# Follow logs (real-time)
docker-compose logs -f api
```

### 4. Access the Application

- **API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger
- **Health Check**: http://localhost:5000/health

### 5. Default Admin Credentials

After seeding, you can login with:
- **Email**: Check the logs for seeded admin credentials
- **Password**: Check the logs for seeded admin credentials

## Common Commands

### Stop All Services

```bash
docker-compose down
```

### Stop and Remove Volumes (Clean Reset)

```bash
docker-compose down -v
```

**Warning**: This will delete all data in the database!

### Rebuild API After Code Changes

```bash
docker-compose up -d --build api
```

### View Real-time Logs

```bash
docker-compose logs -f
```

### Execute Commands in Containers

```bash
# Access PostgreSQL
docker-compose exec postgres psql -U cms_user -d cms_db

# Access Redis CLI
docker-compose exec redis redis-cli -a redis_password_dev

# Access API container bash
docker-compose exec api bash
```

## Database Management

### Run Migrations Manually

If you need to run migrations manually:

```bash
cd backend/CMS.API
dotnet ef database update
```

Or inside Docker container:

```bash
docker-compose exec api dotnet ef database update
```

### Create New Migration

```bash
cd backend/CMS.API
dotnet ef migrations add MigrationName
```

### Backup Database

```bash
docker-compose exec postgres pg_dump -U cms_user cms_db > backup.sql
```

### Restore Database

```bash
docker-compose exec -T postgres psql -U cms_user -d cms_db < backup.sql
```

## Troubleshooting

### Port Already in Use

If ports 5000, 5432, or 6379 are already in use, update the `.env` file:

```env
API_PORT=5001
POSTGRES_PORT=5433
REDIS_PORT=6380
```

Then restart:

```bash
docker-compose down
docker-compose up -d
```

### Database Connection Issues

1. Check if PostgreSQL is healthy:
   ```bash
   docker-compose exec postgres pg_isready -U cms_user
   ```

2. Check connection string in logs:
   ```bash
   docker-compose logs api | grep "Connection"
   ```

### API Not Starting

1. Check API logs:
   ```bash
   docker-compose logs api
   ```

2. Verify PostgreSQL is running:
   ```bash
   docker-compose ps postgres
   ```

3. Rebuild the API image:
   ```bash
   docker-compose up -d --build api
   ```

### Clear Everything and Start Fresh

```bash
# Stop all containers
docker-compose down -v

# Remove all Docker resources
docker system prune -a --volumes

# Start again
docker-compose up -d
```

## Production Deployment

For production deployment:

1. Update `.env` with secure passwords and secrets
2. Set `ASPNETCORE_ENVIRONMENT=Production`
3. Use a reverse proxy (nginx, Caddy, Traefik)
4. Enable HTTPS
5. Use managed database services instead of containerized PostgreSQL
6. Implement proper backup strategies
7. Set up monitoring and logging

## Architecture

```
┌─────────────────┐
│   Frontend      │
│   (Next.js)     │
│   Port: 3000    │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│   Backend API   │
│   (.NET 8)      │
│   Port: 5000    │
└────┬────────┬───┘
     │        │
     ▼        ▼
┌─────────┐ ┌──────┐
│PostgreSQL│ │Redis │
│Port: 5432│ │6379  │
└─────────┘ └──────┘
```

## Volumes

- `postgres_data`: PostgreSQL data persistence
- `redis_data`: Redis data persistence
- `./backend/CMS.API/logs`: API logs (mounted from host)
- `./backend/CMS.API/wwwroot/uploads`: Uploaded files (mounted from host)

## Network

All services communicate through a bridge network called `cms_network`.
