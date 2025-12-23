.PHONY: help dev-up dev-down dev-logs dev-ps db-connect redis-connect clean migrate build api-run api-watch

help: ## Show this help message
	@echo 'Usage: make [target]'
	@echo ''
	@echo 'Available targets:'
	@awk 'BEGIN {FS = ":.*?## "} /^[a-zA-Z_-]+:.*?## / {printf "  %-20s %s\n", $$1, $$2}' $(MAKEFILE_LIST)

# Development Environment Commands

dev-up: ## Start development services (PostgreSQL + Redis)
	docker-compose -f docker-compose.dev.yml up -d

dev-down: ## Stop development services
	docker-compose -f docker-compose.dev.yml down

dev-logs: ## View logs from development services
	docker-compose -f docker-compose.dev.yml logs -f

dev-ps: ## Show status of development services
	docker-compose -f docker-compose.dev.yml ps

# Database Commands

db-connect: ## Connect to PostgreSQL database
	docker-compose -f docker-compose.dev.yml exec postgres psql -U cms_user -d cms_db

db-backup: ## Backup database to backup.sql
	docker-compose -f docker-compose.dev.yml exec postgres pg_dump -U cms_user cms_db > backup_$(shell date +%Y%m%d_%H%M%S).sql
	@echo "Database backup created: backup_$(shell date +%Y%m%d_%H%M%S).sql"

db-restore: ## Restore database from backup.sql (Usage: make db-restore FILE=backup.sql)
	docker-compose -f docker-compose.dev.yml exec -T postgres psql -U cms_user -d cms_db < $(FILE)

# Redis Commands

redis-connect: ## Connect to Redis CLI
	docker-compose -f docker-compose.dev.yml exec redis redis-cli -a redis_password_dev

redis-flush: ## Flush all Redis data (CAUTION: This will delete all cached data)
	docker-compose -f docker-compose.dev.yml exec redis redis-cli -a redis_password_dev FLUSHALL

# Backend API Commands

api-run: ## Run backend API locally
	cd backend/CMS.API && dotnet run

api-watch: ## Run backend API with hot reload
	cd backend/CMS.API && dotnet watch run

api-build: ## Build backend API
	cd backend/CMS.API && dotnet build

api-test: ## Run backend tests
	cd backend/CMS.API && dotnet test

# Database Migration Commands

migrate: ## Apply database migrations
	cd backend/CMS.API && dotnet ef database update

migrate-create: ## Create new migration (Usage: make migrate-create NAME=MigrationName)
	cd backend/CMS.API && dotnet ef migrations add $(NAME)

migrate-rollback: ## Rollback last migration
	cd backend/CMS.API && dotnet ef database update 0

migrate-list: ## List all migrations
	cd backend/CMS.API && dotnet ef migrations list

# Cleanup Commands

clean: ## Remove all containers, volumes, and images
	docker-compose -f docker-compose.dev.yml down -v
	docker system prune -f

clean-all: ## Remove everything including images (CAUTION: Complete cleanup)
	docker-compose -f docker-compose.dev.yml down -v --rmi all
	docker system prune -af --volumes

# Production Commands

prod-up: ## Start production services (Full stack)
	docker-compose up -d

prod-down: ## Stop production services
	docker-compose down

prod-logs: ## View production logs
	docker-compose logs -f

prod-build: ## Rebuild production images
	docker-compose build --no-cache

# Full Stack Commands

build: ## Build all Docker images
	docker-compose -f docker-compose.dev.yml build
	docker-compose build

up-all: ## Start all services (Dev + Production)
	$(MAKE) dev-up
	@echo "Waiting for database to be ready..."
	@sleep 5
	$(MAKE) migrate
	$(MAKE) api-run
