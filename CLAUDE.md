# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a CMS (Content Management System) migration project transitioning from:
- **Legacy**: Blazor Server-Side + SQL Server
- **New Stack**: Next.js + .NET Core Web API + PostgreSQL

The repository contains both legacy code (in CMS.Website, CMS.Data, CMS.Services) and new architecture components (backend/, frontend/, database/).

## Architecture

### Backend API (backend/CMS.API)
- **Framework**: .NET 8.0 Web API
- **Database**: PostgreSQL with Npgsql.EntityFrameworkCore.PostgreSQL 8.0
- **Authentication**: JWT Bearer authentication with ASP.NET Core Identity
- **Structure**:
  - `Controllers/` - REST API controllers (Auth, Articles, Products, Categories, Comments, Orders, Settings, Upload)
  - `Services/` - Business logic layer with interface-based design
  - `Data/` - ApplicationDbContext and entities
  - `Models/` - DTOs, Requests, and Responses
- **Features**: Serilog logging, FluentValidation, AutoMapper, Redis caching, rate limiting, health checks

### Frontend (frontend/cms-admin)
- **Framework**: Next.js 14 with App Router and TypeScript
- **UI**: Tailwind CSS + Radix UI components
- **State Management**: Zustand + TanStack Query (React Query)
- **Forms**: React Hook Form + Zod validation
- **Rich Text**: TipTap editor
- **Charts**: Recharts
- **Structure**:
  - `src/app/` - App Router pages with route groups
    - `(admin)/` - Admin panel (dashboard, articles, products, settings)
    - `(public)/` - Public-facing pages (home, articles, products, search)
    - `(auth)/` - Authentication pages
  - `src/lib/` - API clients (api.ts, public-api.ts), services, utilities
  - `src/components/` - Reusable UI components
  - `src/stores/` - Zustand state stores
  - `src/types/` - TypeScript type definitions

### Database
- **ORM**: Entity Framework Core with Code First approach
- **Provider**: Npgsql.EntityFrameworkCore.PostgreSQL 8.0
- **Migration Strategy**: All database changes managed through EF Core migrations
- **Entities**: Migrated from legacy `CMS.Data/ModelEntity/` (70+ entities)
  - Core entities: Articles, Products, Categories, Orders, Users (ASP.NET Identity), Comments, Advertising, etc.
  - Entities starting with `sp` prefix are excluded (legacy stored procedure result types)
- **Reference Files**: `database/` directory contains reference SQL schemas for understanding legacy structure

## Common Commands

### Backend API Development

```bash
# Navigate to API directory
cd backend/CMS.API

# Build the API
dotnet build

# Run the API (development)
dotnet run

# Run with watch mode
dotnet watch run

# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Run tests (if tests exist)
dotnet test
```

### Frontend Development

```bash
# Navigate to frontend directory
cd frontend/cms-admin

# Install dependencies
npm install

# Run development server (http://localhost:3000)
npm run dev

# Build for production
npm run build

# Start production server
npm start

# Run linting
npm run lint

# Run tests
npm test

# Run tests with coverage
npm test:coverage
```

## Key Architectural Patterns

### API Response Format
All API endpoints return a consistent response format:
```json
{
  "success": true,
  "data": {},
  "message": "Operation successful",
  "errors": [],
  "pagination": {
    "currentPage": 1,
    "pageSize": 20,
    "totalItems": 100,
    "totalPages": 5
  }
}
```

### Authentication Flow
1. User logs in via `/api/auth/login` with credentials
2. API returns JWT access token and refresh token
3. Frontend stores tokens and includes Bearer token in subsequent requests
4. Tokens expire after configured duration (60 minutes for access, 7 days for refresh)
5. Use `/api/auth/refresh` to obtain new access token

### Authorization Policies
- `AdminOnly` - Requires Admin role
- `EditorOrAdmin` - Requires Admin or Editor role
- `AuthorOrAbove` - Requires Admin, Editor, or Author role

### Frontend API Integration
- `src/lib/api.ts` - Admin API client with authentication
- `src/lib/public-api.ts` - Public API client without authentication
- `src/lib/services/` - Feature-specific service layers (articles, products, categories)
- API base URL configured via `NEXT_PUBLIC_API_URL` environment variable

### Database Design
- Identity tables: `asp_net_users`, `asp_net_roles`, etc.
- Core entities: `article`, `product`, `article_category`, `product_category`
- Junction tables for many-to-many: `article_category_article`, `product_category_product`
- Lookup tables: `article_status`, `article_type`, `product_status`, `product_type`
- All tables use snake_case naming convention in PostgreSQL

### Entity Framework Core Patterns
- **DbContext**: `ApplicationDbContext` in `backend/CMS.API/Data/`
- **Entities**: Located in `backend/CMS.API/Data/Entities/`
  - Entities mirror those in legacy `CMS.Data/ModelEntity/`
  - **IMPORTANT**: Exclude any entities starting with `sp` prefix - these were SQL Server stored procedure result types and are NOT needed
  - All entities should have proper navigation properties defined
- **Querying**: Use LINQ queries with `.AsNoTracking()` for read-only operations
- **Eager Loading**: Use `.Include()` and `.ThenInclude()` to avoid N+1 queries
- **Async Operations**: Always use async methods (`ToListAsync()`, `FirstOrDefaultAsync()`, etc.)
- **Transactions**: Use `BeginTransactionAsync()` for multi-step operations
- **Naming Convention**: Entity properties use PascalCase, database columns use snake_case (configured via conventions)

## Important Development Notes

### Working with Legacy Code
- **Do NOT modify** files in CMS.Website/, CMS.Data/, CMS.Services/ - these are legacy Blazor components
- Focus development on backend/CMS.API/, frontend/cms-admin/, and database/ directories
- Refer to `docs/migration-plan.md` for understanding the migration strategy

### API Development
- Controllers inherit from `BaseController` which provides helper methods (ApiSuccess, ApiError, etc.)
- All services use dependency injection via interfaces
- Use FluentValidation for input validation
- Apply `[Authorize]` attributes with appropriate policies for protected endpoints
- All database operations handled through Entity Framework Core (no stored procedures)
- Business logic previously in SQL Server stored procedures should be implemented in service layer using LINQ queries
- Complex queries should use IQueryable composition for optimal performance

### Frontend Development
- Use Next.js App Router conventions (not Pages Router)
- Server Components by default, add 'use client' only when needed
- API calls should use the api-client wrapper which handles authentication
- Forms should use React Hook Form + Zod schema validation
- UI components should use Radix UI primitives with Tailwind styling

### Database Migrations
- All schema changes managed exclusively through Entity Framework Core migrations
- Use `dotnet ef migrations add MigrationName` to create new migrations in backend/CMS.API
- Use `dotnet ef database update` to apply migrations to the database
- Always review generated migration code before applying
- Configure entity relationships and constraints in `ApplicationDbContext` using Fluent API
- Add indexes using `HasIndex()` in entity configurations for foreign keys and frequently queried columns
- Database seeding handled in `DbInitializer` class, executed on application startup

### Environment Configuration
Backend requires: `DefaultConnection` (PostgreSQL), `JwtSettings`, `Cors:AllowedOrigins`
Frontend requires: `NEXT_PUBLIC_API_URL` pointing to backend API

## Migration Context

The system is migrating from a monolithic Blazor application to a decoupled architecture. Key differences:
- **Architecture**: Monolithic Blazor Server → Separated API + Frontend
- **Database**: SQL Server → PostgreSQL
- **ORM Approach**: Database First with stored procedures → Code First with Entity Framework Core
- **Data Model**: Entities migrated from `CMS.Data/ModelEntity/` (excluding `sp*` stored procedure types)
- **Authentication**: Cookie-based → JWT tokens
- **UI Components**: Telerik UI for Blazor → Custom components with Radix UI + Tailwind
- **Real-time**: SignalR (in migration)
- **File Upload**: Local storage → Cloud storage strategy TBD
- **SEO**: Server-side Blazor → Next.js SSR/SSG with proper meta tags

Refer to `docs/migration-plan.md` for complete migration strategy and `docs/database-migration-guide.md` for database-specific migration details.
