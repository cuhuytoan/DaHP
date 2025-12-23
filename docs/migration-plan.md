# CMS Migration Plan: Blazor to Next.js + .NET Core API + PostgreSQL

## Executive Summary

This document outlines a comprehensive migration strategy for transitioning the existing CMS system from a Blazor Server-Side application with SQL Server to a modern architecture featuring:
- **Frontend**: Next.js (React-based)
- **Backend**: .NET Core Web API
- **Database**: PostgreSQL

---

## 1. Current Architecture Analysis

### 1.1 Technology Stack

| Component | Current Technology | Version |
|-----------|-------------------|---------|
| Framework | .NET 6.0 | 6.0 |
| Frontend | Blazor Server-Side | 6.0 |
| Database | SQL Server | - |
| ORM | Entity Framework Core | 5.0.4 / 6.0 |
| UI Components | Telerik UI for Blazor | 2.18.0 |
| Authentication | ASP.NET Core Identity | 6.0 |
| Real-time | SignalR | 6.0 |

### 1.2 Project Structure

```
CMS.Website.sln
├── CMS.Common/          # Shared utilities
├── CMS.Data/            # Data layer (Entities, DTOs, Filters)
│   ├── ModelEntity/     # 70+ entity classes
│   ├── ModelDTO/        # Data Transfer Objects
│   ├── ModelFilter/     # Search filters
│   └── ValidationCustomize/
├── CMS.Services/        # Business logic layer
│   ├── Repositories/    # 20+ repository classes
│   └── RepositoriesBase/
└── CMS.Website/         # Blazor application
    ├── Areas/
    │   ├── Admin/       # Admin panel pages
    │   ├── Identity/    # Authentication pages
    │   ├── Member/      # Member area
    │   └── Shopman/     # Shop management
    ├── Controllers/     # API controllers (minimal)
    ├── Services/        # DI configuration
    └── wwwroot/         # Static assets
```

### 1.3 Key Features Identified

1. **Content Management**
   - Article management with categories, blocks, comments
   - Rich text editor with image upload
   - Article workflow (draft, review, publish)

2. **Product Management**
   - Product catalog with categories
   - Product properties and variants
   - Product images and attachments
   - Product reviews and comments

3. **E-commerce**
   - Product orders
   - Payment integration (VnPay)
   - Product brands management

4. **User Management**
   - ASP.NET Core Identity
   - Role-based access control
   - User profiles

5. **Additional Features**
   - Advertising management
   - Real-time notifications (SignalR)
   - Log visit tracking
   - Settings management

---

## 2. Database Migration: SQL Server to PostgreSQL

### 2.1 SQL Server Specific Features to Address

| Feature | SQL Server | PostgreSQL Equivalent |
|---------|------------|----------------------|
| Collation | `SQL_Latin1_General_CP1_CI_AS` | `en_US.UTF-8` with `CITEXT` extension |
| Data Types | `ntext` | `TEXT` |
| Data Types | `money` | `DECIMAL(19,4)` or `NUMERIC` |
| Data Types | `datetime` | `TIMESTAMP` |
| Identity | `IDENTITY(1,1)` | `SERIAL` or `GENERATED ALWAYS AS IDENTITY` |
| Stored Procedures | T-SQL | PL/pgSQL |

### 2.2 Stored Procedures to Migrate

The following stored procedures need to be converted to PL/pgSQL:

1. `SpArticleSearch` - Article search with pagination
2. `SpArticleBreadcrumb` - Article breadcrumb generation
3. `SpArticleCategoryTree` - Category tree structure
4. `SpArticleGetByBlockId` - Articles by block
5. `SpArticleGetByCategoryId` - Articles by category
6. `SpArticleGetTopByCategoryId` - Top articles
7. `SpProductSearch` - Product search with pagination
8. `SpProductBreadcrumb` - Product breadcrumb
9. `SpProductCategoryTree` - Product category tree
10. `SpAccountSearch` - User account search
11. `SpLogVisitSearch` - Visit log search
12. And more...

### 2.3 Migration Steps

```mermaid
flowchart TD
    A[Export SQL Server Schema] --> B[Convert Schema to PostgreSQL]
    B --> C[Create PostgreSQL Database]
    C --> D[Migrate Stored Procedures]
    D --> E[Export Data from SQL Server]
    E --> F[Transform Data Types]
    F --> G[Import Data to PostgreSQL]
    G --> H[Validate Data Integrity]
    H --> I[Update Connection Strings]
    I --> J[Test Application]
```

---

## 3. Backend Migration: Create .NET Core Web API

### 3.1 New API Project Structure

```
CMS.API/
├── Controllers/
│   ├── ArticlesController.cs
│   ├── ProductsController.cs
│   ├── CategoriesController.cs
│   ├── UsersController.cs
│   ├── OrdersController.cs
│   ├── SettingsController.cs
│   └── UploadController.cs
├── Models/
│   ├── Requests/
│   └── Responses/
├── Services/
├── Middleware/
│   ├── AuthenticationMiddleware.cs
│   └── ErrorHandlingMiddleware.cs
└── Program.cs
```

### 3.2 API Endpoints to Create

#### Articles API
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/articles` | List articles with pagination |
| GET | `/api/articles/{id}` | Get article by ID |
| GET | `/api/articles/url/{url}` | Get article by URL |
| POST | `/api/articles` | Create article |
| PUT | `/api/articles/{id}` | Update article |
| DELETE | `/api/articles/{id}` | Delete article |
| PUT | `/api/articles/{id}/status` | Update article status |
| GET | `/api/articles/{id}/related` | Get related articles |
| GET | `/api/articles/category/{categoryId}` | Get articles by category |
| GET | `/api/articles/top/{categoryId}` | Get top articles |

#### Products API
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | List products with pagination |
| GET | `/api/products/{id}` | Get product by ID |
| GET | `/api/products/url/{url}` | Get product by URL |
| POST | `/api/products` | Create product |
| PUT | `/api/products/{id}` | Update product |
| DELETE | `/api/products/{id}` | Delete product |
| PUT | `/api/products/{id}/status` | Update product status |
| POST | `/api/products/{id}/like` | Like product |
| DELETE | `/api/products/{id}/like` | Unlike product |
| GET | `/api/products/{id}/pictures` | Get product pictures |

#### Categories API
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/article-categories` | List article categories |
| GET | `/api/article-categories/tree` | Get category tree |
| GET | `/api/product-categories` | List product categories |
| GET | `/api/product-categories/tree` | Get category tree |

#### Authentication API
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/login` | User login |
| POST | `/api/auth/register` | User registration |
| POST | `/api/auth/refresh` | Refresh token |
| POST | `/api/auth/logout` | User logout |
| GET | `/api/auth/profile` | Get user profile |
| PUT | `/api/auth/profile` | Update profile |
| POST | `/api/auth/change-password` | Change password |

#### Orders API
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/orders` | List orders |
| GET | `/api/orders/{id}` | Get order by ID |
| POST | `/api/orders` | Create order |
| PUT | `/api/orders/{id}/status` | Update order status |

### 3.3 Authentication Strategy

Replace Blazor cookie-based authentication with JWT:

```csharp
// JWT Configuration
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Configuration["Jwt:Issuer"],
            ValidAudience = Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
        };
    });
```

---

## 4. Frontend Migration: Next.js Application

### 4.1 Next.js Project Structure

```
cms-frontend/
├── src/
│   ├── app/                    # App Router (Next.js 13+)
│   │   ├── (admin)/           # Admin layout group
│   │   │   ├── dashboard/
│   │   │   ├── articles/
│   │   │   ├── products/
│   │   │   └── settings/
│   │   ├── (public)/          # Public layout group
│   │   │   ├── page.tsx       # Home page
│   │   │   ├── articles/
│   │   │   └── products/
│   │   ├── api/               # API routes (if needed)
│   │   └── layout.tsx
│   ├── components/
│   │   ├── ui/                # Reusable UI components
│   │   ├── forms/             # Form components
│   │   ├── layouts/           # Layout components
│   │   └── features/          # Feature-specific components
│   ├── hooks/                 # Custom React hooks
│   ├── lib/                   # Utility functions
│   ├── services/              # API service layer
│   ├── store/                 # State management (Zustand/Redux)
│   └── types/                 # TypeScript types
├── public/
├── next.config.js
├── tailwind.config.js
└── package.json
```

### 4.2 Key Libraries

| Purpose | Library |
|---------|---------|
| UI Framework | Tailwind CSS + shadcn/ui |
| State Management | Zustand or TanStack Query |
| Forms | React Hook Form + Zod |
| Rich Text Editor | TipTap or Slate.js |
| Data Tables | TanStack Table |
| Charts | Recharts |
| Authentication | NextAuth.js |
| API Client | Axios or Fetch |
| Real-time | Socket.io-client |

### 4.3 Component Mapping

| Blazor Component | Next.js Equivalent |
|------------------|-------------------|
| Telerik Grid | TanStack Table |
| Telerik Editor | TipTap Editor |
| Telerik Upload | react-dropzone |
| Telerik Toast | react-hot-toast |
| Blazor Forms | React Hook Form |
| SignalR Hub | Socket.io |

---

## 5. Migration Timeline

### Phase 1: Foundation (Weeks 1-3)

```mermaid
gantt
    title Phase 1: Foundation
    dateFormat  YYYY-MM-DD
    section Database
    Schema Analysis           :a1, 2024-01-01, 3d
    PostgreSQL Schema Design  :a2, after a1, 4d
    Stored Procedure Migration:a3, after a2, 5d
    section API
    API Project Setup         :b1, 2024-01-01, 2d
    Authentication Setup      :b2, after b1, 3d
    Base Controllers          :b3, after b2, 5d
```

**Deliverables:**
- PostgreSQL database schema
- Migrated stored procedures
- Basic API project with authentication
- API documentation (Swagger)

### Phase 2: Core API Development (Weeks 4-7)

```mermaid
gantt
    title Phase 2: Core API Development
    dateFormat  YYYY-MM-DD
    section Articles
    Articles API              :a1, 2024-01-22, 5d
    Categories API            :a2, after a1, 3d
    Comments API              :a3, after a2, 2d
    section Products
    Products API              :b1, 2024-01-22, 5d
    Product Categories API    :b2, after b1, 3d
    Reviews API               :b3, after b2, 2d
    section Others
    Orders API                :c1, after a3, 4d
    Settings API              :c2, after c1, 2d
    Upload API                :c3, after c2, 3d
```

**Deliverables:**
- Complete REST API for all entities
- API integration tests
- Data migration scripts

### Phase 3: Frontend Development (Weeks 8-14)

```mermaid
gantt
    title Phase 3: Frontend Development
    dateFormat  YYYY-MM-DD
    section Setup
    Next.js Project Setup     :a1, 2024-02-19, 3d
    UI Component Library      :a2, after a1, 5d
    section Admin
    Dashboard                 :b1, after a2, 3d
    Article Management        :b2, after b1, 5d
    Product Management        :b3, after b2, 5d
    User Management           :b4, after b3, 3d
    Settings                  :b5, after b4, 2d
    section Public
    Home Page                 :c1, after a2, 3d
    Article Pages             :c2, after c1, 4d
    Product Pages             :c3, after c2, 4d
    Search                    :c4, after c3, 3d
```

**Deliverables:**
- Complete admin panel
- Public-facing pages
- Responsive design
- SEO optimization

### Phase 4: Integration & Testing (Weeks 15-17)

**Tasks:**
- End-to-end testing
- Performance optimization
- Security audit
- Data migration validation
- User acceptance testing

### Phase 5: Deployment & Go-Live (Weeks 18-20)

**Tasks:**
- Production environment setup
- CI/CD pipeline configuration
- Data migration execution
- DNS and SSL configuration
- Monitoring setup
- Go-live and support

---

## 6. Resource Requirements

### 6.1 Team Composition

| Role | Count | Responsibilities |
|------|-------|-----------------|
| Project Manager | 1 | Planning, coordination, stakeholder communication |
| Backend Developer | 2 | API development, database migration |
| Frontend Developer | 2 | Next.js development, UI/UX implementation |
| DevOps Engineer | 1 | Infrastructure, CI/CD, deployment |
| QA Engineer | 1 | Testing, quality assurance |
| Database Administrator | 1 (part-time) | PostgreSQL migration, optimization |

### 6.2 Infrastructure

| Component | Specification |
|-----------|--------------|
| API Server | 2 vCPU, 4GB RAM (scalable) |
| Database Server | PostgreSQL 15+, 4 vCPU, 8GB RAM |
| Frontend Hosting | Vercel or similar (Next.js optimized) |
| File Storage | AWS S3 or Azure Blob Storage |
| CDN | CloudFlare or AWS CloudFront |

### 6.3 Estimated Costs

| Item | Monthly Cost (USD) |
|------|-------------------|
| API Hosting | $50-100 |
| Database (managed) | $100-200 |
| Frontend Hosting | $20-50 |
| File Storage | $20-50 |
| CDN | $20-50 |
| **Total** | **$210-450** |

---

## 7. Potential Challenges and Mitigations

### 7.1 Technical Challenges

| Challenge | Risk Level | Mitigation Strategy |
|-----------|------------|---------------------|
| Stored Procedure Migration | High | Early analysis, incremental migration, thorough testing |
| Data Type Incompatibilities | Medium | Create mapping document, use migration scripts |
| Authentication Migration | Medium | Implement JWT alongside existing auth during transition |
| Real-time Features (SignalR) | Medium | Use Socket.io or SignalR for .NET Core |
| File Upload Handling | Low | Implement cloud storage early |
| SEO Preservation | Medium | Maintain URL structure, implement redirects |

### 7.2 Business Challenges

| Challenge | Risk Level | Mitigation Strategy |
|-----------|------------|---------------------|
| Downtime During Migration | High | Blue-green deployment, off-hours migration |
| Data Loss | High | Multiple backups, validation scripts |
| User Training | Medium | Documentation, training sessions |
| Feature Parity | Medium | Detailed feature checklist, UAT |

---

## 8. Success Criteria

### 8.1 Technical Metrics

- [ ] All API endpoints functional with <200ms response time
- [ ] Database queries optimized (<100ms for common operations)
- [ ] Frontend Lighthouse score >90
- [ ] Zero data loss during migration
- [ ] 99.9% uptime post-migration

### 8.2 Business Metrics

- [ ] All existing features available in new system
- [ ] User authentication working seamlessly
- [ ] Content preserved with correct formatting
- [ ] SEO rankings maintained
- [ ] User satisfaction score >4/5

---

## 9. Rollback Plan

In case of critical issues:

1. **Database**: Maintain SQL Server instance for 30 days post-migration
2. **Application**: Keep Blazor application deployable
3. **DNS**: Quick switch capability between old and new systems
4. **Data Sync**: Implement bidirectional sync during transition period

---

## 10. Post-Migration Support

### First 30 Days
- 24/7 monitoring
- Daily backup verification
- Performance optimization
- Bug fixes (priority)

### 30-90 Days
- Weekly performance reviews
- User feedback collection
- Feature enhancements
- Documentation updates

---

## Appendix A: Entity List

### Core Entities (70+)

**Articles Domain:**
- Article, ArticleCategory, ArticleBlock, ArticleBlockArticle
- ArticleCategoryArticle, ArticleCategoryAssign
- ArticleComment, ArticleCommentStaff
- ArticleRelationArticle, ArticleStatus, ArticleTop, ArticleType
- ArticleAttachFile

**Products Domain:**
- Product, ProductCategory, ProductBlock, ProductBlockProduct
- ProductCategoryProduct, ProductCategoryAssign
- ProductComment, ProductCommentStaff, ProductReview
- ProductRelationProduct, ProductStatus, ProductTop, ProductType
- ProductPicture, ProductAttachFile
- ProductProperty, ProductPropertyCategory, ProductPropertyType, ProductPropertyValue
- ProductBrand, ProductBrandAttachFile, ProductBrandCategory
- ProductBrandFollow, ProductBrandLevel, ProductBrandStatus, ProductBrandType
- ProductManufacture, ProductLike, ProductLogEdit

**E-commerce Domain:**
- ProductOrder, ProductOrderDetail
- ProductOrderPaymentMethod, ProductOrderPaymentStatus, ProductOrderStatus

**User Domain:**
- AspNetUsers, AspNetRoles, AspNetUserRoles
- AspNetUserClaims, AspNetRoleClaims
- AspNetUserLogins, AspNetUserTokens
- AspNetUserProfiles

**Supporting Entities:**
- Advertising, AdvertisingBlock, AdvertisingType
- Setting, Bank, Country, District, Ward, Location
- Department, DepartmentMan, Unit
- LogVisit, UserNotify, ReplaceChar

---

## Appendix B: API Response Format

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

---

## Appendix C: Environment Configuration

### Development
```env
DATABASE_URL=postgresql://user:pass@localhost:5432/cms_dev
API_URL=http://localhost:5000
NEXT_PUBLIC_API_URL=http://localhost:5000/api
JWT_SECRET=development-secret-key
```

### Production
```env
DATABASE_URL=postgresql://user:pass@prod-db:5432/cms_prod
API_URL=https://api.example.com
NEXT_PUBLIC_API_URL=https://api.example.com/api
JWT_SECRET=production-secret-key-from-vault
```

---

*Document Version: 1.0*
*Last Updated: December 2024*
*Author: Architecture Team*