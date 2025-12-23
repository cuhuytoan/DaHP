# Database Migration Guide: SQL Server to PostgreSQL

## Table of Contents

1. [Overview](#1-overview)
2. [Pre-Migration Assessment](#2-pre-migration-assessment)
3. [Schema Conversion](#3-schema-conversion)
4. [Data Type Mapping](#4-data-type-mapping)
5. [Stored Procedure Migration](#5-stored-procedure-migration)
6. [Data Migration Scripts](#6-data-migration-scripts)
7. [Entity Framework Configuration](#7-entity-framework-configuration)
8. [Validation and Testing](#8-validation-and-testing)

---

## 1. Overview

This guide provides detailed instructions for migrating the CMS database from SQL Server to PostgreSQL, including schema conversion, stored procedure migration, and data transfer.

### Migration Tools

| Tool | Purpose |
|------|---------|
| pgLoader | Automated data migration from SQL Server |
| EF Core Migrations | Schema management |
| pg_dump/pg_restore | Backup and restore |
| DataGrip/DBeaver | Database management and comparison |

---

## 2. Pre-Migration Assessment

### 2.1 Database Size Analysis

```sql
-- SQL Server: Get database size
SELECT 
    DB_NAME() AS DatabaseName,
    SUM(size * 8 / 1024) AS SizeMB
FROM sys.database_files;

-- Get table row counts
SELECT 
    t.NAME AS TableName,
    p.rows AS RowCounts
FROM sys.tables t
INNER JOIN sys.partitions p ON t.object_id = p.object_id
WHERE p.index_id IN (0, 1)
ORDER BY p.rows DESC;
```

### 2.2 Identify SQL Server Specific Features

```sql
-- Find tables with identity columns
SELECT 
    TABLE_NAME,
    COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE COLUMNPROPERTY(OBJECT_ID(TABLE_SCHEMA + '.' + TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1;

-- Find computed columns
SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    definition
FROM sys.computed_columns cc
JOIN sys.tables t ON cc.object_id = t.object_id;

-- Find triggers
SELECT name, type_desc FROM sys.triggers;
```

---

## 3. Schema Conversion

### 3.1 Core Tables Schema

#### Article Table

**SQL Server (Original):**
```sql
CREATE TABLE [dbo].[Article] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ArticleTypeId] INT NULL,
    [ArticleCategoryIds] NVARCHAR(200) NULL,
    [ProductBrandId] INT NULL,
    [ArticleStatusId] INT NULL,
    [Name] NVARCHAR(1000) NULL,
    [SubTitle] NVARCHAR(200) NULL,
    [Image] NVARCHAR(200) NULL,
    [ImageDescription] NVARCHAR(200) NULL,
    [BannerImage] NVARCHAR(200) NULL,
    [Description] NVARCHAR(MAX) NULL,
    [Content] NTEXT NULL,
    [Author] NVARCHAR(200) NULL,
    [StartDate] DATETIME NULL,
    [EndDate] DATETIME NULL,
    [Active] BIT NULL,
    [Counter] INT NULL,
    [CreateBy] NVARCHAR(450) NULL,
    [CreateDate] DATETIME NULL,
    [LastEditBy] NVARCHAR(450) NULL,
    [LastEditDate] DATETIME NULL,
    [Checked] INT NULL,
    [CheckBy] NVARCHAR(450) NULL,
    [CheckDate] DATETIME NULL,
    [Approved] INT NULL,
    [ApproveBy] NVARCHAR(450) NULL,
    [ApproveDate] DATETIME NULL,
    [URL] NVARCHAR(1000) NULL,
    [Tags] NVARCHAR(MAX) NULL,
    [CanCopy] BIT NULL,
    [CanComment] BIT NULL,
    [CanDelete] BIT NULL,
    [MetaTitle] NVARCHAR(500) NULL,
    [MetaDescription] NVARCHAR(500) NULL,
    [MetaKeywords] NVARCHAR(500) NULL,
    [DocumentRefer] NVARCHAR(500) NULL
);
```

**PostgreSQL (Converted):**
```sql
CREATE TABLE article (
    id SERIAL PRIMARY KEY,
    article_type_id INTEGER,
    article_category_ids VARCHAR(200),
    product_brand_id INTEGER,
    article_status_id INTEGER,
    name VARCHAR(1000),
    sub_title VARCHAR(200),
    image VARCHAR(200),
    image_description VARCHAR(200),
    banner_image VARCHAR(200),
    description TEXT,
    content TEXT,
    author VARCHAR(200),
    start_date TIMESTAMP,
    end_date TIMESTAMP,
    active BOOLEAN DEFAULT TRUE,
    counter INTEGER DEFAULT 0,
    create_by VARCHAR(450),
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP,
    checked INTEGER,
    check_by VARCHAR(450),
    check_date TIMESTAMP,
    approved INTEGER,
    approve_by VARCHAR(450),
    approve_date TIMESTAMP,
    url VARCHAR(1000),
    tags TEXT,
    can_copy BOOLEAN DEFAULT TRUE,
    can_comment BOOLEAN DEFAULT TRUE,
    can_delete BOOLEAN DEFAULT TRUE,
    meta_title VARCHAR(500),
    meta_description VARCHAR(500),
    meta_keywords VARCHAR(500),
    document_refer VARCHAR(500)
);

-- Create indexes
CREATE INDEX idx_article_status ON article(article_status_id);
CREATE INDEX idx_article_category ON article(article_category_ids);
CREATE INDEX idx_article_url ON article(url);
CREATE INDEX idx_article_create_date ON article(create_date DESC);
```

#### Product Table

**PostgreSQL (Converted):**
```sql
CREATE TABLE product (
    id SERIAL PRIMARY KEY,
    product_type_id INTEGER,
    product_category_ids VARCHAR(200),
    product_manufacture_id INTEGER,
    product_brand_id INTEGER,
    country_id INTEGER,
    product_status_id INTEGER,
    name VARCHAR(1000),
    bar_code VARCHAR(50),
    manufacture_sku VARCHAR(100),
    sku VARCHAR(100),
    qrcode_public VARCHAR(100),
    sub_title VARCHAR(200),
    image VARCHAR(200),
    image_description VARCHAR(200),
    banner_image VARCHAR(200),
    description TEXT,
    content TEXT,
    specification TEXT,
    product_certificate TEXT,
    legal_info VARCHAR(4000),
    price DECIMAL(19,4) DEFAULT 0,
    price_old DECIMAL(19,4) DEFAULT 0,
    price_wholesale DECIMAL(19,4) DEFAULT 0,
    wholesale_min INTEGER,
    discount DECIMAL(19,4) DEFAULT 0,
    discount_rate INTEGER DEFAULT 0,
    is_second_hand BOOLEAN DEFAULT FALSE,
    is_author BOOLEAN DEFAULT FALSE,
    is_best_sale BOOLEAN DEFAULT FALSE,
    is_sale_off BOOLEAN DEFAULT FALSE,
    is_new BOOLEAN DEFAULT FALSE,
    is_comming BOOLEAN DEFAULT FALSE,
    is_out_stock BOOLEAN DEFAULT FALSE,
    is_discontinue BOOLEAN DEFAULT FALSE,
    amount_default INTEGER,
    unit_id INTEGER,
    expiry_display VARCHAR(500),
    expiry_by_day INTEGER,
    warranty_display VARCHAR(500),
    warranty_by_month INTEGER DEFAULT 0,
    rate INTEGER,
    start_date TIMESTAMP,
    end_date TIMESTAMP,
    active BOOLEAN DEFAULT TRUE,
    counter INTEGER DEFAULT 0,
    like_count INTEGER DEFAULT 0,
    sell_count INTEGER DEFAULT 0,
    create_by VARCHAR(450),
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP,
    checked INTEGER,
    check_by VARCHAR(450),
    check_date TIMESTAMP,
    approved INTEGER,
    approve_by VARCHAR(450),
    approve_date TIMESTAMP,
    url VARCHAR(1000),
    tags TEXT,
    can_copy BOOLEAN DEFAULT TRUE,
    can_comment BOOLEAN DEFAULT TRUE,
    can_delete BOOLEAN DEFAULT TRUE,
    meta_title VARCHAR(500),
    meta_description VARCHAR(500),
    meta_keywords VARCHAR(500),
    document_refer VARCHAR(500)
);

-- Create indexes
CREATE INDEX idx_product_status ON product(product_status_id);
CREATE INDEX idx_product_category ON product(product_category_ids);
CREATE INDEX idx_product_brand ON product(product_brand_id);
CREATE INDEX idx_product_url ON product(url);
CREATE INDEX idx_product_price ON product(price);
CREATE INDEX idx_product_create_date ON product(create_date DESC);
```

#### ASP.NET Identity Tables

**PostgreSQL (Converted):**
```sql
-- AspNetUsers
CREATE TABLE asp_net_users (
    id VARCHAR(450) PRIMARY KEY,
    user_name VARCHAR(256),
    normalized_user_name VARCHAR(256),
    email VARCHAR(256),
    normalized_email VARCHAR(256),
    email_confirmed BOOLEAN NOT NULL DEFAULT FALSE,
    password_hash TEXT,
    security_stamp TEXT,
    concurrency_stamp TEXT,
    phone_number TEXT,
    phone_number_confirmed BOOLEAN NOT NULL DEFAULT FALSE,
    two_factor_enabled BOOLEAN NOT NULL DEFAULT FALSE,
    lockout_end TIMESTAMP WITH TIME ZONE,
    lockout_enabled BOOLEAN NOT NULL DEFAULT FALSE,
    access_failed_count INTEGER NOT NULL DEFAULT 0
);

CREATE UNIQUE INDEX idx_users_normalized_username ON asp_net_users(normalized_user_name);
CREATE INDEX idx_users_normalized_email ON asp_net_users(normalized_email);

-- AspNetRoles
CREATE TABLE asp_net_roles (
    id VARCHAR(450) PRIMARY KEY,
    name VARCHAR(256),
    normalized_name VARCHAR(256),
    concurrency_stamp TEXT
);

CREATE UNIQUE INDEX idx_roles_normalized_name ON asp_net_roles(normalized_name);

-- AspNetUserRoles
CREATE TABLE asp_net_user_roles (
    user_id VARCHAR(450) NOT NULL,
    role_id VARCHAR(450) NOT NULL,
    PRIMARY KEY (user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES asp_net_users(id) ON DELETE CASCADE,
    FOREIGN KEY (role_id) REFERENCES asp_net_roles(id) ON DELETE CASCADE
);

-- AspNetUserClaims
CREATE TABLE asp_net_user_claims (
    id SERIAL PRIMARY KEY,
    user_id VARCHAR(450) NOT NULL,
    claim_type TEXT,
    claim_value TEXT,
    FOREIGN KEY (user_id) REFERENCES asp_net_users(id) ON DELETE CASCADE
);

-- AspNetUserLogins
CREATE TABLE asp_net_user_logins (
    login_provider VARCHAR(128) NOT NULL,
    provider_key VARCHAR(128) NOT NULL,
    provider_display_name TEXT,
    user_id VARCHAR(450) NOT NULL,
    PRIMARY KEY (login_provider, provider_key),
    FOREIGN KEY (user_id) REFERENCES asp_net_users(id) ON DELETE CASCADE
);

-- AspNetUserTokens
CREATE TABLE asp_net_user_tokens (
    user_id VARCHAR(450) NOT NULL,
    login_provider VARCHAR(128) NOT NULL,
    name VARCHAR(128) NOT NULL,
    value TEXT,
    PRIMARY KEY (user_id, login_provider, name),
    FOREIGN KEY (user_id) REFERENCES asp_net_users(id) ON DELETE CASCADE
);

-- AspNetRoleClaims
CREATE TABLE asp_net_role_claims (
    id SERIAL PRIMARY KEY,
    role_id VARCHAR(450) NOT NULL,
    claim_type TEXT,
    claim_value TEXT,
    FOREIGN KEY (role_id) REFERENCES asp_net_roles(id) ON DELETE CASCADE
);

-- AspNetUserProfiles (Custom)
CREATE TABLE asp_net_user_profiles (
    id SERIAL PRIMARY KEY,
    user_id VARCHAR(450) NOT NULL,
    full_name VARCHAR(200),
    phone VARCHAR(50),
    address VARCHAR(500),
    avatar VARCHAR(200),
    product_brand_id INTEGER,
    department_id INTEGER,
    active BOOLEAN DEFAULT TRUE,
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES asp_net_users(id) ON DELETE CASCADE
);
```

### 3.2 Complete Schema Migration Script

```sql
-- =============================================
-- PostgreSQL Schema Migration Script
-- CMS Database
-- =============================================

-- Enable required extensions
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "citext";

-- =============================================
-- LOOKUP TABLES
-- =============================================

CREATE TABLE article_status (
    id INTEGER PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(500)
);

CREATE TABLE article_type (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE product_status (
    id INTEGER PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(500)
);

CREATE TABLE product_type (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE country (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    code VARCHAR(10)
);

CREATE TABLE unit (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL
);

CREATE TABLE bank (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    code VARCHAR(50),
    logo VARCHAR(200)
);

-- =============================================
-- CATEGORY TABLES
-- =============================================

CREATE TABLE article_category (
    id SERIAL PRIMARY KEY,
    parent_id INTEGER,
    name VARCHAR(200) NOT NULL,
    description TEXT,
    image VARCHAR(200),
    url VARCHAR(500),
    sort INTEGER DEFAULT 0,
    active BOOLEAN DEFAULT TRUE,
    create_by VARCHAR(450),
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP,
    meta_title VARCHAR(500),
    meta_description VARCHAR(500),
    meta_keywords VARCHAR(500),
    FOREIGN KEY (parent_id) REFERENCES article_category(id)
);

CREATE TABLE product_category (
    id SERIAL PRIMARY KEY,
    parent_id INTEGER,
    name VARCHAR(200) NOT NULL,
    description TEXT,
    image VARCHAR(200),
    url VARCHAR(500),
    sort INTEGER DEFAULT 0,
    active BOOLEAN DEFAULT TRUE,
    create_by VARCHAR(450),
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP,
    meta_title VARCHAR(500),
    meta_description VARCHAR(500),
    meta_keywords VARCHAR(500),
    FOREIGN KEY (parent_id) REFERENCES product_category(id)
);

-- =============================================
-- JUNCTION TABLES
-- =============================================

CREATE TABLE article_category_article (
    id SERIAL PRIMARY KEY,
    article_id INTEGER NOT NULL,
    article_category_id INTEGER NOT NULL,
    UNIQUE(article_id, article_category_id)
);

CREATE TABLE product_category_product (
    id SERIAL PRIMARY KEY,
    product_id INTEGER NOT NULL,
    product_category_id INTEGER NOT NULL,
    UNIQUE(product_id, product_category_id)
);

CREATE TABLE article_relation_article (
    id SERIAL PRIMARY KEY,
    article_id INTEGER NOT NULL,
    article_relation_id INTEGER NOT NULL,
    UNIQUE(article_id, article_relation_id)
);

CREATE TABLE product_relation_product (
    id SERIAL PRIMARY KEY,
    product_id INTEGER NOT NULL,
    product_relation_id INTEGER NOT NULL,
    UNIQUE(product_id, product_relation_id)
);

-- =============================================
-- BLOCK TABLES
-- =============================================

CREATE TABLE article_block (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    description VARCHAR(500),
    article_category_id INTEGER,
    sort INTEGER DEFAULT 0,
    active BOOLEAN DEFAULT TRUE,
    create_by VARCHAR(450),
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE article_block_article (
    id SERIAL PRIMARY KEY,
    article_block_id INTEGER NOT NULL,
    article_id INTEGER NOT NULL,
    sort INTEGER DEFAULT 0,
    UNIQUE(article_block_id, article_id)
);

CREATE TABLE product_block (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    description VARCHAR(500),
    product_category_id INTEGER,
    sort INTEGER DEFAULT 0,
    active BOOLEAN DEFAULT TRUE,
    create_by VARCHAR(450),
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE product_block_product (
    id SERIAL PRIMARY KEY,
    product_block_id INTEGER NOT NULL,
    product_id INTEGER NOT NULL,
    sort INTEGER DEFAULT 0,
    UNIQUE(product_block_id, product_id)
);

-- =============================================
-- COMMENT TABLES
-- =============================================

CREATE TABLE article_comment (
    id SERIAL PRIMARY KEY,
    article_id INTEGER NOT NULL,
    parent_id INTEGER,
    name VARCHAR(200),
    email VARCHAR(200),
    phone VARCHAR(50),
    content TEXT,
    active BOOLEAN DEFAULT FALSE,
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    create_by VARCHAR(450)
);

CREATE TABLE product_comment (
    id SERIAL PRIMARY KEY,
    product_id INTEGER NOT NULL,
    parent_id INTEGER,
    name VARCHAR(200),
    email VARCHAR(200),
    phone VARCHAR(50),
    content TEXT,
    active BOOLEAN DEFAULT FALSE,
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    create_by VARCHAR(450)
);

CREATE TABLE product_review (
    id SERIAL PRIMARY KEY,
    product_id INTEGER NOT NULL,
    user_id VARCHAR(450),
    rating INTEGER CHECK (rating >= 1 AND rating <= 5),
    title VARCHAR(200),
    content TEXT,
    active BOOLEAN DEFAULT FALSE,
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- =============================================
-- ATTACHMENT TABLES
-- =============================================

CREATE TABLE article_attach_file (
    id SERIAL PRIMARY KEY,
    article_id INTEGER,
    attach_file_name VARCHAR(500),
    file_type VARCHAR(50),
    file_size BIGINT,
    create_by VARCHAR(450),
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP
);

CREATE TABLE product_attach_file (
    id SERIAL PRIMARY KEY,
    product_id INTEGER,
    attach_file_name VARCHAR(500),
    file_type VARCHAR(50),
    file_size BIGINT,
    create_by VARCHAR(450),
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP
);

CREATE TABLE product_picture (
    id SERIAL PRIMARY KEY,
    product_id INTEGER,
    image VARCHAR(200),
    sort INTEGER DEFAULT 0,
    create_by VARCHAR(450),
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP
);

-- =============================================
-- PRODUCT BRAND TABLES
-- =============================================

CREATE TABLE product_brand_status (
    id INTEGER PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE product_brand_type (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE product_brand_level (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE product_brand (
    id SERIAL PRIMARY KEY,
    name VARCHAR(500) NOT NULL,
    description TEXT,
    content TEXT,
    image VARCHAR(200),
    banner_image VARCHAR(200),
    phone VARCHAR(50),
    email VARCHAR(200),
    address VARCHAR(500),
    website VARCHAR(200),
    facebook VARCHAR(200),
    youtube VARCHAR(200),
    product_brand_type_id INTEGER,
    product_brand_status_id INTEGER,
    product_brand_level_id INTEGER,
    location_id INTEGER,
    department_man_id INTEGER,
    qrcode_public VARCHAR(100),
    has_qrcode BOOLEAN DEFAULT FALSE,
    view_count INTEGER DEFAULT 0,
    view_page_count INTEGER DEFAULT 0,
    sell_count INTEGER DEFAULT 0,
    active BOOLEAN DEFAULT TRUE,
    create_by VARCHAR(450),
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP,
    url VARCHAR(500),
    meta_title VARCHAR(500),
    meta_description VARCHAR(500),
    meta_keywords VARCHAR(500)
);

-- =============================================
-- ORDER TABLES
-- =============================================

CREATE TABLE product_order_status (
    id INTEGER PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE product_order_payment_method (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE product_order_payment_status (
    id INTEGER PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE product_order (
    id SERIAL PRIMARY KEY,
    order_code VARCHAR(50),
    customer_name VARCHAR(200),
    customer_email VARCHAR(200),
    customer_phone VARCHAR(50),
    customer_address VARCHAR(500),
    customer_note TEXT,
    product_brand_id INTEGER,
    total_amount DECIMAL(19,4) DEFAULT 0,
    discount_amount DECIMAL(19,4) DEFAULT 0,
    shipping_fee DECIMAL(19,4) DEFAULT 0,
    final_amount DECIMAL(19,4) DEFAULT 0,
    order_status_id INTEGER,
    payment_method_id INTEGER,
    payment_status_id INTEGER,
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    create_by VARCHAR(450),
    last_edit_date TIMESTAMP,
    last_edit_by VARCHAR(450)
);

CREATE TABLE product_order_detail (
    id SERIAL PRIMARY KEY,
    product_order_id INTEGER NOT NULL,
    product_id INTEGER NOT NULL,
    product_name VARCHAR(500),
    product_image VARCHAR(200),
    quantity INTEGER DEFAULT 1,
    price DECIMAL(19,4) DEFAULT 0,
    discount DECIMAL(19,4) DEFAULT 0,
    total DECIMAL(19,4) DEFAULT 0,
    FOREIGN KEY (product_order_id) REFERENCES product_order(id) ON DELETE CASCADE
);

-- =============================================
-- ADVERTISING TABLES
-- =============================================

CREATE TABLE advertising_type (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE advertising_block (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    description VARCHAR(500),
    article_category_id INTEGER,
    device_display INTEGER DEFAULT 0,
    sort INTEGER DEFAULT 0,
    active BOOLEAN DEFAULT TRUE,
    create_by VARCHAR(450),
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

CREATE TABLE advertising (
    id SERIAL PRIMARY KEY,
    advertising_block_id INTEGER,
    advertising_type_id INTEGER,
    name VARCHAR(200),
    description VARCHAR(500),
    image VARCHAR(200),
    url VARCHAR(500),
    code TEXT,
    target VARCHAR(20) DEFAULT '_self',
    width INTEGER,
    height INTEGER,
    counter INTEGER DEFAULT 0,
    start_date TIMESTAMP,
    end_date TIMESTAMP,
    sort INTEGER DEFAULT 0,
    active BOOLEAN DEFAULT TRUE,
    create_by VARCHAR(450),
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP
);

-- =============================================
-- OTHER TABLES
-- =============================================

CREATE TABLE setting (
    id INTEGER PRIMARY KEY,
    setting_key VARCHAR(100),
    setting_value TEXT,
    description VARCHAR(500)
);

CREATE TABLE log_visit (
    id SERIAL PRIMARY KEY,
    ip_address VARCHAR(50),
    user_agent TEXT,
    url VARCHAR(1000),
    referrer VARCHAR(1000),
    user_id VARCHAR(450),
    visit_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE user_notify (
    id SERIAL PRIMARY KEY,
    user_id VARCHAR(450) NOT NULL,
    title VARCHAR(200),
    content TEXT,
    url VARCHAR(500),
    image VARCHAR(200),
    is_read BOOLEAN DEFAULT FALSE,
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE replace_char (
    id SERIAL PRIMARY KEY,
    char_from VARCHAR(10),
    char_to VARCHAR(10)
);

-- =============================================
-- LOCATION TABLES
-- =============================================

CREATE TABLE location (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    code VARCHAR(50)
);

CREATE TABLE district (
    id SERIAL PRIMARY KEY,
    location_id INTEGER,
    name VARCHAR(200) NOT NULL,
    code VARCHAR(50),
    FOREIGN KEY (location_id) REFERENCES location(id)
);

CREATE TABLE ward (
    id SERIAL PRIMARY KEY,
    district_id INTEGER,
    name VARCHAR(200) NOT NULL,
    code VARCHAR(50),
    FOREIGN KEY (district_id) REFERENCES district(id)
);

CREATE TABLE department (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    description VARCHAR(500)
);

CREATE TABLE department_man (
    id SERIAL PRIMARY KEY,
    department_id INTEGER,
    name VARCHAR(200) NOT NULL,
    phone VARCHAR(50),
    email VARCHAR(200),
    FOREIGN KEY (department_id) REFERENCES department(id)
);

-- =============================================
-- TOP/FEATURED TABLES
-- =============================================

CREATE TABLE article_top (
    id SERIAL PRIMARY KEY,
    article_id INTEGER NOT NULL,
    article_category_id INTEGER NOT NULL,
    sort INTEGER DEFAULT 0
);

CREATE TABLE product_top (
    id SERIAL PRIMARY KEY,
    product_id INTEGER NOT NULL,
    product_category_id INTEGER NOT NULL,
    sort INTEGER DEFAULT 0
);

-- =============================================
-- PRODUCT PROPERTIES TABLES
-- =============================================

CREATE TABLE product_property_category (
    id SERIAL PRIMARY KEY,
    parent_id INTEGER,
    name VARCHAR(200) NOT NULL,
    sort INTEGER DEFAULT 0,
    create_by VARCHAR(450),
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP
);

CREATE TABLE product_property_type (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE product_property (
    id SERIAL PRIMARY KEY,
    product_property_category_id INTEGER,
    product_property_type_id INTEGER,
    name VARCHAR(200) NOT NULL,
    sort INTEGER DEFAULT 0,
    create_by VARCHAR(450),
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP
);

CREATE TABLE product_property_value (
    id SERIAL PRIMARY KEY,
    product_id INTEGER NOT NULL,
    product_property_id INTEGER NOT NULL,
    value TEXT,
    create_by VARCHAR(450),
    create_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP
);

-- =============================================
-- ADD FOREIGN KEYS
-- =============================================

ALTER TABLE article ADD FOREIGN KEY (article_type_id) REFERENCES article_type(id);
ALTER TABLE article ADD FOREIGN KEY (article_status_id) REFERENCES article_status(id);
ALTER TABLE article ADD FOREIGN KEY (product_brand_id) REFERENCES product_brand(id);

ALTER TABLE product ADD FOREIGN KEY (product_type_id) REFERENCES product_type(id);
ALTER TABLE product ADD FOREIGN KEY (product_status_id) REFERENCES product_status(id);
ALTER TABLE product ADD FOREIGN KEY (product_brand_id) REFERENCES product_brand(id);
ALTER TABLE product ADD FOREIGN KEY (product_manufacture_id) REFERENCES product_manufacture(id);
ALTER TABLE product ADD FOREIGN KEY (country_id) REFERENCES country(id);
ALTER TABLE product ADD FOREIGN KEY (unit_id) REFERENCES unit(id);

ALTER TABLE article_category_article ADD FOREIGN KEY (article_id) REFERENCES article(id) ON DELETE CASCADE;
ALTER TABLE article_category_article ADD FOREIGN KEY (article_category_id) REFERENCES article_category(id) ON DELETE CASCADE;

ALTER TABLE product_category_product ADD FOREIGN KEY (product_id) REFERENCES product(id) ON DELETE CASCADE;
ALTER TABLE product_category_product ADD FOREIGN KEY (product_category_id) REFERENCES product_category(id) ON DELETE CASCADE;

-- =============================================
-- CREATE INDEXES
-- =============================================

CREATE INDEX idx_article_category_parent ON article_category(parent_id);
CREATE INDEX idx_product_category_parent ON product_category(parent_id);
CREATE INDEX idx_article_comment_article ON article_comment(article_id);
CREATE INDEX idx_product_comment_product ON product_comment(product_id);
CREATE INDEX idx_product_review_product ON product_review(product_id);
CREATE INDEX idx_product_order_status ON product_order(order_status_id);
CREATE INDEX idx_product_order_brand ON product_order(product_brand_id);
CREATE INDEX idx_log_visit_date ON log_visit(visit_date);
CREATE INDEX idx_user_notify_user ON user_notify(user_id);
```

---

## 4. Data Type Mapping

### 4.1 Complete Type Mapping Reference

| SQL Server Type | PostgreSQL Type | Notes |
|-----------------|-----------------|-------|
| `INT` | `INTEGER` | Same range |
| `BIGINT` | `BIGINT` | Same range |
| `SMALLINT` | `SMALLINT` | Same range |
| `TINYINT` | `SMALLINT` | PostgreSQL has no TINYINT |
| `BIT` | `BOOLEAN` | Direct mapping |
| `DECIMAL(p,s)` | `DECIMAL(p,s)` | Same syntax |
| `NUMERIC(p,s)` | `NUMERIC(p,s)` | Same syntax |
| `MONEY` | `DECIMAL(19,4)` | No native MONEY type |
| `FLOAT` | `DOUBLE PRECISION` | 8-byte floating point |
| `REAL` | `REAL` | 4-byte floating point |
| `CHAR(n)` | `CHAR(n)` | Fixed-length |
| `VARCHAR(n)` | `VARCHAR(n)` | Variable-length |
| `NCHAR(n)` | `CHAR(n)` | PostgreSQL is UTF-8 native |
| `NVARCHAR(n)` | `VARCHAR(n)` | PostgreSQL is UTF-8 native |
| `NVARCHAR(MAX)` | `TEXT` | Unlimited length |
| `NTEXT` | `TEXT` | Deprecated in SQL Server |
| `TEXT` | `TEXT` | Same |
| `DATE` | `DATE` | Same |
| `TIME` | `TIME` | Same |
| `DATETIME` | `TIMESTAMP` | Without timezone |
| `DATETIME2` | `TIMESTAMP` | Higher precision |
| `DATETIMEOFFSET` | `TIMESTAMP WITH TIME ZONE` | With timezone |
| `UNIQUEIDENTIFIER` | `UUID` | Requires uuid-ossp extension |
| `VARBINARY(MAX)` | `BYTEA` | Binary data |
| `IMAGE` | `BYTEA` | Deprecated in SQL Server |
| `XML` | `XML` | Same |
| `IDENTITY(1,1)` | `SERIAL` | Auto-increment |

### 4.2 Collation Handling

```sql
-- SQL Server collation
SQL_Latin1_General_CP1_CI_AS

-- PostgreSQL equivalent (case-insensitive)
-- Option 1: Use CITEXT extension
CREATE EXTENSION IF NOT EXISTS citext;
-- Then use CITEXT type for case-insensitive columns

-- Option 2: Use LOWER() in queries
SELECT * FROM users WHERE LOWER(email) = LOWER('User@Example.com');

-- Option 3: Create case-insensitive index
CREATE INDEX idx_users_email_lower ON users(LOWER(email));
```

---

## 5. Stored Procedure Migration

### 5.1 SpArticleSearch

**SQL Server (Original):**
```sql
CREATE PROCEDURE [dbo].[SpArticleSearch]
    @Keyword NVARCHAR(200) = NULL,
    @ArticleCategoryId INT = NULL,
    @ArticleStatusId INT = NULL,
    @ProductBrandId INT = NULL,
    @ArticleTypeId INT = NULL,
    @ExceptionId INT = NULL,
    @ExceptionArticleTop BIT = NULL,
    @FromDate DATETIME = NULL,
    @ToDate DATETIME = NULL,
    @Efficiency INT = NULL,
    @Active BIT = NULL,
    @AssignBy NVARCHAR(450) = NULL,
    @CreateBy NVARCHAR(450) = NULL,
    @PageSize INT = 20,
    @CurrentPage INT = 1,
    @ItemCount INT OUTPUT
AS
BEGIN
    -- Implementation
END
```

**PostgreSQL (Converted):**
```sql
CREATE OR REPLACE FUNCTION sp_article_search(
    p_keyword VARCHAR(200) DEFAULT NULL,
    p_article_category_id INTEGER DEFAULT NULL,
    p_article_status_id INTEGER DEFAULT NULL,
    p_product_brand_id INTEGER DEFAULT NULL,
    p_article_type_id INTEGER DEFAULT NULL,
    p_exception_id INTEGER DEFAULT NULL,
    p_exception_article_top BOOLEAN DEFAULT NULL,
    p_from_date TIMESTAMP DEFAULT NULL,
    p_to_date TIMESTAMP DEFAULT NULL,
    p_efficiency INTEGER DEFAULT NULL,
    p_active BOOLEAN DEFAULT NULL,
    p_assign_by VARCHAR(450) DEFAULT NULL,
    p_create_by VARCHAR(450) DEFAULT NULL,
    p_page_size INTEGER DEFAULT 20,
    p_current_page INTEGER DEFAULT 1,
    OUT p_item_count INTEGER
)
RETURNS TABLE (
    id INTEGER,
    article_type_id INTEGER,
    article_category_ids VARCHAR(200),
    article_category_name VARCHAR(200),
    product_brand_id INTEGER,
    article_status_id INTEGER,
    article_status_name VARCHAR(100),
    name VARCHAR(1000),
    image VARCHAR(200),
    description TEXT,
    author VARCHAR(200),
    start_date TIMESTAMP,
    active BOOLEAN,
    counter INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    url VARCHAR(1000)
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_offset INTEGER;
BEGIN
    v_offset := (p_current_page - 1) * p_page_size;
    
    -- Get total count
    SELECT COUNT(*) INTO p_item_count
    FROM article a
    LEFT JOIN article_category_article aca ON a.id = aca.article_id
    WHERE 
        (p_keyword IS NULL OR a.name ILIKE '%' || p_keyword || '%')
        AND (p_article_category_id IS NULL OR aca.article_category_id = p_article_category_id)
        AND (p_article_status_id IS NULL OR a.article_status_id = p_article_status_id)
        AND (p_product_brand_id IS NULL OR a.product_brand_id = p_product_brand_id)
        AND (p_article_type_id IS NULL OR a.article_type_id = p_article_type_id)
        AND (p_exception_id IS NULL OR a.id != p_exception_id)
        AND (p_from_date IS NULL OR a.create_date >= p_from_date)
        AND (p_to_date IS NULL OR a.create_date <= p_to_date)
        AND (p_active IS NULL OR a.active = p_active)
        AND (p_create_by IS NULL OR a.create_by = p_create_by);
    
    -- Return paginated results
    RETURN QUERY
    SELECT 
        a.id,
        a.article_type_id,
        a.article_category_ids,
        ac.name AS article_category_name,
        a.product_brand_id,
        a.article_status_id,
        ast.name AS article_status_name,
        a.name,
        a.image,
        a.description,
        a.author,
        a.start_date,
        a.active,
        a.counter,
        a.create_by,
        a.create_date,
        a.url
    FROM article a
    LEFT JOIN article_category_article aca ON a.id = aca.article_id
    LEFT JOIN article_category ac ON aca.article_category_id = ac.id
    LEFT JOIN article_status ast ON a.article_status_id = ast.id
    WHERE 
        (p_keyword IS NULL OR a.name ILIKE '%' || p_keyword || '%')
        AND (p_article_category_id IS NULL OR aca.article_category_id = p_article_category_id)
        AND (p_article_status_id IS NULL OR a.article_status_id = p_article_status_id)
        AND (p_product_brand_id IS NULL OR a.product_brand_id = p_product_brand_id)
        AND (p_article_type_id IS NULL OR a.article_type_id = p_article_type_id)
        AND (p_exception_id IS NULL OR a.id != p_exception_id)
        AND (p_from_date IS NULL OR a.create_date >= p_from_date)
        AND (p_to_date IS NULL OR a.create_date <= p_to_date)
        AND (p_active IS NULL OR a.active = p_active)
        AND (p_create_by IS NULL OR a.create_by = p_create_by)
    ORDER BY a.create_date DESC
    LIMIT p_page_size
    OFFSET v_offset;
END;
$$;
```

### 5.2 SpArticleCategoryTree

**PostgreSQL (Converted):**
```sql
CREATE OR REPLACE FUNCTION sp_article_category_tree()
RETURNS TABLE (
    id INTEGER,
    parent_id INTEGER,
    name VARCHAR(200),
    level INTEGER,
    have_child BOOLEAN
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    WITH RECURSIVE category_tree AS (
        -- Base case: root categories
        SELECT 
            ac.id,
            ac.parent_id,
            ac.name,
            0 AS level
        FROM article_category ac
        WHERE ac.parent_id IS NULL AND ac.active = TRUE
        
        UNION ALL
        
        -- Recursive case: child categories
        SELECT 
            ac.id,
            ac.parent_id,
            ac.name,
            ct.level + 1
        FROM article_category ac
        INNER JOIN category_tree ct ON ac.parent_id = ct.id
        WHERE ac.active = TRUE
    )
    SELECT 
        ct.id,
        ct.parent_id,
        ct.name,
        ct.level,
        EXISTS(SELECT 1 FROM article_category WHERE parent_id = ct.id AND active = TRUE) AS have_child
    FROM category_tree ct
    ORDER BY ct.level, ct.name;
END;
$$;
```

### 5.3 SpProductSearch

**PostgreSQL (Converted):**
```sql
CREATE OR REPLACE FUNCTION sp_product_search(
    p_keyword VARCHAR(200) DEFAULT NULL,
    p_product_category_id INTEGER DEFAULT NULL,
    p_product_manufacture_id INTEGER DEFAULT NULL,
    p_product_status_id INTEGER DEFAULT NULL,
    p_country_id INTEGER DEFAULT NULL,
    p_location_id INTEGER DEFAULT NULL,
    p_department_man_id INTEGER DEFAULT NULL,
    p_product_brand_id INTEGER DEFAULT NULL,
    p_product_type_id INTEGER DEFAULT NULL,
    p_exception_id INTEGER DEFAULT NULL,
    p_exception_product_top BOOLEAN DEFAULT NULL,
    p_from_price DECIMAL(19,4) DEFAULT NULL,
    p_to_price DECIMAL(19,4) DEFAULT NULL,
    p_from_date TIMESTAMP DEFAULT NULL,
    p_to_date TIMESTAMP DEFAULT NULL,
    p_efficiency INTEGER DEFAULT NULL,
    p_active BOOLEAN DEFAULT NULL,
    p_assign_by VARCHAR(450) DEFAULT NULL,
    p_create_by VARCHAR(450) DEFAULT NULL,
    p_order_by VARCHAR(50) DEFAULT 'create_date_desc',
    p_page_size INTEGER DEFAULT 20,
    p_current_page INTEGER DEFAULT 1,
    OUT p_item_count INTEGER
)
RETURNS TABLE (
    id INTEGER,
    product_type_id INTEGER,
    product_category_ids VARCHAR(200),
    product_category_name VARCHAR(200),
    product_brand_id INTEGER,
    product_brand_name VARCHAR(500),
    product_status_id INTEGER,
    product_status_name VARCHAR(100),
    name VARCHAR(1000),
    sku VARCHAR(100),
    image VARCHAR(200),
    description TEXT,
    price DECIMAL(19,4),
    price_old DECIMAL(19,4),
    discount_rate INTEGER,
    active BOOLEAN,
    counter INTEGER,
    like_count INTEGER,
    sell_count INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    url VARCHAR(1000)
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_offset INTEGER;
    v_order_clause TEXT;
BEGIN
    v_offset := (p_current_page - 1) * p_page_size;
    
    -- Determine order clause
    v_order_clause := CASE p_order_by
        WHEN 'price_asc' THEN 'p.price ASC'
        WHEN 'price_desc' THEN 'p.price DESC'
        WHEN 'name_asc' THEN 'p.name ASC'
        WHEN 'name_desc' THEN 'p.name DESC'
        WHEN 'sell_count_desc' THEN 'p.sell_count DESC'
        WHEN 'counter_desc' THEN 'p.counter DESC'
        ELSE 'p.create_date DESC'
    END;
    
    -- Get total count
    SELECT COUNT(DISTINCT p.id) INTO p_item_count
    FROM product p
    LEFT JOIN product_category_product pcp ON p.id = pcp.product_id
    LEFT JOIN product_brand pb ON p.product_brand_id = pb.id
    WHERE 
        (p_keyword IS NULL OR p.name ILIKE '%' || p_keyword || '%' OR p.sku ILIKE '%' || p_keyword || '%')
        AND (p_product_category_id IS NULL OR pcp.product_category_id = p_product_category_id)
        AND (p_product_manufacture_id IS NULL OR p.product_manufacture_id = p_product_manufacture_id)
        AND (p_product_status_id IS NULL OR p.product_status_id = p_product_status_id)
        AND (p_country_id IS NULL OR p.country_id = p_country_id)
        AND (p_product_brand_id IS NULL OR p.product_brand_id = p_product_brand_id)
        AND (p_product_type_id IS NULL OR p.product_type_id = p_product_type_id)
        AND (p_exception_id IS NULL OR p.id != p_exception_id)
        AND (p_from_price IS NULL OR p.price >= p_from_price)
        AND (p_to_price IS NULL OR p.price <= p_to_price)
        AND (p_from_date IS NULL OR p.create_date >= p_from_date)
        AND (p_to_date IS NULL OR p.create_date <= p_to_date)
        AND (p_active IS NULL OR p.active = p_active)
        AND (p_create_by IS NULL OR p.create_by = p_create_by);
    
    -- Return paginated results
    RETURN QUERY EXECUTE format('
    SELECT DISTINCT
        p.id,
        p.product_type_id,
        p.product_category_ids,
        pc.name AS product_category_name,
        p.product_brand_id,
        pb.name AS product_brand_name,
        p.product_status_id,
        ps.name AS product_status_name,
        p.name,
        p.sku,
        p.image,
        p.description,
        p.price,
        p.price_old,
        p.discount_rate,
        p.active,
        p.counter,
        p.like_count,
        p.sell_count,
        p.create_by,
        p.create_date,
        p.url
    FROM product p
    LEFT JOIN product_category_product pcp ON p.id = pcp.product_id
    LEFT JOIN product_category pc ON pcp.product_category_id = pc.id
    LEFT JOIN product_brand pb ON p.product_brand_id = pb.id
    LEFT JOIN product_status ps ON p.product_status_id = ps.id
    WHERE 
        ($1 IS NULL OR p.name ILIKE ''%%'' || $1 || ''%%'' OR p.sku ILIKE ''%%'' || $1 || ''%%'')
        AND ($2 IS NULL OR pcp.product_category_id = $2)
        AND ($3 IS NULL OR p.product_manufacture_id = $3)
        AND ($4 IS NULL OR p.product_status_id = $4)
        AND ($5 IS NULL OR p.country_id = $5)
        AND ($6 IS NULL OR p.product_brand_id = $6)
        AND ($7 IS NULL OR p.product_type_id = $7)
        AND ($8 IS NULL OR p.id != $8)
        AND ($9 IS NULL OR p.price >= $9)
        AND ($10 IS NULL OR p.price <= $10)
        AND ($11 IS NULL OR p.create_date >= $11)
        AND ($12 IS NULL OR p.create_date <= $12)
        AND ($13 IS NULL OR p.active = $13)
        AND ($14 IS NULL OR p.create_by = $14)
    ORDER BY %s
    LIMIT $15
    OFFSET $16',
    v_order_clause)
    USING p_keyword, p_product_category_id, p_product_manufacture_id, p_product_status_id,
          p_country_id, p_product_brand_id, p_product_type_id, p_exception_id,
          p_from_price, p_to_price, p_from_date, p_to_date, p_active, p_create_by,
          p_page_size, v_offset;
END;
$$;
```

### 5.4 SpArticleBreadcrumb

**PostgreSQL (Converted):**
```sql
CREATE OR REPLACE FUNCTION sp_article_breadcrumb(p_article_category_id INTEGER)
RETURNS TABLE (
    id INTEGER,
    name VARCHAR(200),
    url VARCHAR(500),
    level INTEGER
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    WITH RECURSIVE breadcrumb AS (
        -- Start with the given category
        SELECT 
            ac.id,
            ac.parent_id,
            ac.name,
            ac.url,
            0 AS level
        FROM article_category ac
        WHERE ac.id = p_article_category_id
        
        UNION ALL
        
        -- Get parent categories
        SELECT 
            ac.id,
            ac.parent_id,
            ac.name,
            ac.url,
            b.level + 1
        FROM article_category ac
        INNER JOIN breadcrumb b ON ac.id = b.parent_id
    )
    SELECT 
        bc.id,
        bc.name,
        bc.url,
        bc.level
    FROM breadcrumb bc
    ORDER BY bc.level DESC;
END;
$$;
```

---

## 6. Data Migration Scripts

### 6.1 Using pgLoader

Create a configuration file `migration.load`:

```
LOAD DATABASE
    FROM mssql://user:password@sqlserver-host/CmsDatabase
    INTO postgresql://user:password@postgres-host/cms_database

WITH
    include drop,
    create tables,
    create indexes,
    reset sequences,
    workers = 4,
    concurrency = 2

SET
    work_mem to '128MB',
    maintenance_work_mem to '512MB'

CAST
    type datetime to timestamp,
    type nvarchar to varchar,
    type ntext to text,
    type bit to boolean,
    type money to numeric

BEFORE LOAD DO
    $$ DROP SCHEMA IF EXISTS public CASCADE; $$,
    $$ CREATE SCHEMA public; $$

AFTER LOAD DO
    $$ UPDATE pg_index SET indisvalid = true WHERE NOT indisvalid; $$;
```

Run migration:
```bash
pgloader migration.load
```

### 6.2 Manual Data Migration Script

```sql
-- =============================================
-- Data Migration Script
-- Run after schema creation
-- =============================================

-- Step 1: Migrate lookup tables first
INSERT INTO article_status (id, name) VALUES
(0, 'Đã lưu'),
(1, 'Chờ duyệt'),
(2, 'Đã kiểm tra'),
(3, 'Đã sơ duyệt'),
(4, 'Đã đăng');

INSERT INTO product_status (id, name) VALUES
(0, 'Đã lưu'),
(1, 'Chờ duyệt'),
(2, 'Đã kiểm tra'),
(3, 'Đã sơ duyệt'),
(4, 'Đã đăng');

-- Step 2: Export data from SQL Server using BCP
-- bcp "SELECT * FROM Article" queryout article.csv -c -t"," -S sqlserver -d CmsDatabase -U user -P password

-- Step 3: Import data to PostgreSQL using COPY
-- \copy article FROM 'article.csv' WITH (FORMAT csv, HEADER true);

-- Step 4: Reset sequences after import
SELECT setval('article_id_seq', (SELECT MAX(id) FROM article));
SELECT setval('product_id_seq', (SELECT MAX(id) FROM product));
SELECT setval('article_category_id_seq', (SELECT MAX(id) FROM article_category));
SELECT setval('product_category_id_seq', (SELECT MAX(id) FROM product_category));
-- ... repeat for all tables with SERIAL columns

-- Step 5: Validate data counts
DO $$
DECLARE
    v_article_count INTEGER;
    v_product_count INTEGER;
    v_user_count INTEGER;
BEGIN
    SELECT COUNT(*) INTO v_article_count FROM article;
    SELECT COUNT(*) INTO v_product_count FROM product;
    SELECT COUNT(*) INTO v_user_count FROM asp_net_users;
    
    RAISE NOTICE 'Articles: %, Products: %, Users: %', 
        v_article_count, v_product_count, v_user_count;
END $$;
```

### 6.3 Python Migration Script

```python
#!/usr/bin/env python3
"""
CMS Database Migration Script
SQL Server to PostgreSQL
"""

import pyodbc
import psycopg2
from psycopg2.extras import execute_batch
import logging
from datetime import datetime

# Configure logging
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

# Database connections
SQLSERVER_CONN = {
    'driver': '{ODBC Driver 17 for SQL Server}',
    'server': 'sqlserver-host',
    'database': 'CmsDatabase',
    'uid': 'user',
    'pwd': 'password'
}

POSTGRES_CONN = {
    'host': 'postgres-host',
    'database': 'cms_database',
    'user': 'user',
    'password': 'password'
}

# Table mapping (SQL Server -> PostgreSQL)
TABLE_MAPPING = {
    'Article': 'article',
    'ArticleCategory': 'article_category',
    'ArticleCategoryArticle': 'article_category_article',
    'Product': 'product',
    'ProductCategory': 'product_category',
    'ProductCategoryProduct': 'product_category_product',
    'AspNetUsers': 'asp_net_users',
    'AspNetRoles': 'asp_net_roles',
    'AspNetUserRoles': 'asp_net_user_roles',
    # Add more tables...
}

# Column mapping for case conversion
def to_snake_case(name):
    import re
    s1 = re.sub('(.)([A-Z][a-z]+)', r'\1_\2', name)
    return re.sub('([a-z0-9])([A-Z])', r'\1_\2', s1).lower()

def get_sqlserver_connection():
    conn_str = ';'.join([f'{k}={v}' for k, v in SQLSERVER_CONN.items()])
    return pyodbc.connect(conn_str)

def get_postgres_connection():
    return psycopg2.connect(**POSTGRES_CONN)

def migrate_table(source_table, target_table, batch_size=1000):
    """Migrate a single table from SQL Server to PostgreSQL"""
    logger.info(f"Migrating {source_table} -> {target_table}")
    
    with get_sqlserver_connection() as sql_conn:
        sql_cursor = sql_conn.cursor()
        
        # Get column names
        sql_cursor.execute(f"SELECT TOP 1 * FROM [{source_table}]")
        columns = [column[0] for column in sql_cursor.description]
        pg_columns = [to_snake_case(col) for col in columns]
        
        # Get total count
        sql_cursor.execute(f"SELECT COUNT(*) FROM [{source_table}]")
        total_count = sql_cursor.fetchone()[0]
        logger.info(f"Total rows to migrate: {total_count}")
        
        # Fetch all data
        sql_cursor.execute(f"SELECT * FROM [{source_table}]")
        
        with get_postgres_connection() as pg_conn:
            pg_cursor = pg_conn.cursor()
            
            # Prepare insert statement
            placeholders = ','.join(['%s'] * len(pg_columns))
            insert_sql = f"""
                INSERT INTO {target_table} ({','.join(pg_columns)})
                VALUES ({placeholders})
                ON CONFLICT DO NOTHING
            """
            
            batch = []
            migrated = 0
            
            for row in sql_cursor:
                # Convert row data
                converted_row = []
                for i, value in enumerate(row):
                    if isinstance(value, datetime):
                        converted_row.append(value)
                    elif value is None:
                        converted_row.append(None)
                    else:
                        converted_row.append(value)
                
                batch.append(tuple(converted_row))
                
                if len(batch) >= batch_size:
                    execute_batch(pg_cursor, insert_sql, batch)
                    pg_conn.commit()
                    migrated += len(batch)
                    logger.info(f"Migrated {migrated}/{total_count} rows")
                    batch = []
            
            # Insert remaining rows
            if batch:
                execute_batch(pg_cursor, insert_sql, batch)
                pg_conn.commit()
                migrated += len(batch)
            
            logger.info(f"Completed: {migrated} rows migrated")

def reset_sequences():
    """Reset all sequences after data migration"""
    logger.info("Resetting sequences...")
    
    sequences = [
        ('article', 'id'),
        ('product', 'id'),
        ('article_category', 'id'),
        ('product_category', 'id'),
        ('product_brand', 'id'),
        ('product_order', 'id'),
        # Add more...
    ]
    
    with get_postgres_connection() as pg_conn:
        pg_cursor = pg_conn.cursor()
        
        for table, column in sequences:
            pg_cursor.execute(f"""
                SELECT setval(
                    pg_get_serial_sequence('{table}', '{column}'),
                    COALESCE((SELECT MAX({column}) FROM {table}), 1)
                )
            """)
        
        pg_conn.commit()
    
    logger.info("Sequences reset completed")

def validate_migration():
    """Validate data counts between source and target"""
    logger.info("Validating migration...")
    
    validation_results = []
    
    with get_sqlserver_connection() as sql_conn:
        sql_cursor = sql_conn.cursor()
        
        with get_postgres_connection() as pg_conn:
            pg_cursor = pg_conn.cursor()
            
            for source_table, target_table in TABLE_MAPPING.items():
                sql_cursor.execute(f"SELECT COUNT(*) FROM [{source_table}]")
                source_count = sql_cursor.fetchone()[0]
                
                pg_cursor.execute(f"SELECT COUNT(*) FROM {target_table}")
                target_count = pg_cursor.fetchone()[0]
                
                status = "✓" if source_count == target_count else "✗"
                validation_results.append({
                    'table': source_table,
                    'source': source_count,
                    'target': target_count,
                    'status': status
                })
    
    # Print results
    print("\n" + "="*60)
    print("Migration Validation Results")
    print("="*60)
    print(f"{'Table':<30} {'Source':<10} {'Target':<10} {'Status'}")
    print("-"*60)
    
    for result in validation_results:
        print(f"{result['table']:<30} {result['source']:<10} {result['target']:<10} {result['status']}")
    
    print("="*60)

def main():
    """Main migration function"""
    logger.info("Starting database migration...")
    
    # Migrate tables in order (respecting foreign keys)
    migration_order = [
        # Lookup tables first
        ('ArticleStatus', 'article_status'),
        ('ArticleType', 'article_type'),
        ('ProductStatus', 'product_status'),
        ('ProductType', 'product_type'),
        ('Country', 'country'),
        ('Unit', 'unit'),
        
        # Identity tables
        ('AspNetUsers', 'asp_net_users'),
        ('AspNetRoles', 'asp_net_roles'),
        ('AspNetUserRoles', 'asp_net_user_roles'),
        
        # Category tables
        ('ArticleCategory', 'article_category'),
        ('ProductCategory', 'product_category'),
        ('ProductBrand', 'product_brand'),
        
        # Main content tables
        ('Article', 'article'),
        ('Product', 'product'),
        
        # Junction tables
        ('ArticleCategoryArticle', 'article_category_article'),
        ('ProductCategoryProduct', 'product_category_product'),
        
        # Related tables
        ('ArticleComment', 'article_comment'),
        ('ProductComment', 'product_comment'),
        ('ProductReview', 'product_review'),
        ('ProductOrder', 'product_order'),
        ('ProductOrderDetail', 'product_order_detail'),
    ]
    
    for source_table, target_table in migration_order:
        try:
            migrate_table(source_table, target_table)
        except Exception as e:
            logger.error(f"Error migrating {source_table}: {e}")
            raise
    
    # Reset sequences
    reset_sequences()
    
    # Validate migration
    validate_migration()
    
    logger.info("Migration completed successfully!")

if __name__ == '__main__':
    main()
```

---

## 7. Entity Framework Configuration

### 7.1 Update NuGet Packages

```xml
<!-- Remove SQL Server package -->
<!-- <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="6.0.0" /> -->

<!-- Add PostgreSQL package -->
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="6.0.0" />
```

### 7.2 Update DbContext Configuration

```csharp
// CMS.Data/ModelEntity/CmsContext.cs
public partial class CmsContext : DbContext
{
    public CmsContext(DbContextOptions<CmsContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // PostgreSQL uses snake_case by default
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Convert table names to snake_case
            entity.SetTableName(ToSnakeCase(entity.GetTableName()));

            // Convert column names to snake_case
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.GetColumnName()));
            }

            // Convert key names to snake_case
            foreach (var key in entity.GetKeys())
            {
                key.SetName(ToSnakeCase(key.GetName()));
            }

            // Convert foreign key names to snake_case
            foreach (var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(ToSnakeCase(key.GetConstraintName()));
            }

            // Convert index names to snake_case
            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()));
            }
        }

        OnModelCreatingPartial(modelBuilder);
    }

    private static string ToSnakeCase(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;
        
        var result = new StringBuilder();
        for (int i = 0; i < name.Length; i++)
        {
            if (char.IsUpper(name[i]))
            {
                if (i > 0) result.Append('_');
                result.Append(char.ToLower(name[i]));
            }
            else
            {
                result.Append(name[i]);
            }
        }
        return result.ToString();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
```

### 7.3 Update Service Extensions

```csharp
// CMS.Website/Services/ServiceExtensions.cs
public static void ConfigureConnectDB(this IServiceCollection services, string connectStrings)
{
    services.AddDbContextFactory<CmsContext>(options =>
        options.UseNpgsql(connectStrings), ServiceLifetime.Transient);
}

public static void ConfigureConnectDBAuth(this IServiceCollection services, string connectStrings)
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectStrings));
}
```

### 7.4 Update Connection String

```json
// appsettings.json
{
  "ConnectionStrings": {
    "CmsConnection": "Host=localhost;Database=cms_database;Username=postgres;Password=password",
    "AuthConnection": "Host=localhost;Database=cms_database;Username=postgres;Password=password"
  }
}
```

### 7.5 Update DbContextExtensions

```csharp
// CMS.Data/ModelEntity/DbContextExtensions.cs
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (transaction != null)
    {
        optionsBuilder.UseNpgsql(connection);
    }
    else
    {
        optionsBuilder.UseNpgsql(connection, options => 
            options.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null));
    }
}
```

---

## 8. Validation and Testing

### 8.1 Data Integrity Checks

```sql
-- Check for orphaned records
SELECT 'article_category_article' AS table_name, COUNT(*) AS orphaned_count
FROM article_category_article aca
WHERE NOT EXISTS (SELECT 1 FROM article WHERE id = aca.article_id)
   OR NOT EXISTS (SELECT 1 FROM article_category WHERE id = aca.article_category_id)

UNION ALL

SELECT 'product_category_product', COUNT(*)
FROM product_category_product pcp
WHERE NOT EXISTS (SELECT 1 FROM product WHERE id = pcp.product_id)
   OR NOT EXISTS (SELECT 1 FROM product_category WHERE id = pcp.product_category_id);

-- Check for duplicate URLs
SELECT url, COUNT(*) as count
FROM article
WHERE url IS NOT NULL
GROUP BY url
HAVING COUNT(*) > 1;

-- Verify foreign key relationships
SELECT 
    tc.table_name, 
    kcu.column_name, 
    ccu.table_name AS foreign_table_name,
    ccu.column_name AS foreign_column_name 
FROM information_schema.table_constraints AS tc 
JOIN information_schema.key_column_usage AS kcu
    ON tc.constraint_name = kcu.constraint_name
JOIN information_schema.constraint_column_usage AS ccu
    ON ccu.constraint_name = tc.constraint_name
WHERE constraint_type = 'FOREIGN KEY';
```

### 8.2 Performance Testing

```sql
-- Test article search performance
EXPLAIN ANALYZE
SELECT * FROM sp_article_search(
    p_keyword := 'test',
    p_page_size := 20,
    p_current_page := 1
);

-- Test product search performance
EXPLAIN ANALYZE
SELECT * FROM sp_product_search(
    p_keyword := 'test',
    p_product_status_id := 4,
    p_page_size := 20,
    p_current_page := 1
);

-- Check index usage
SELECT 
    schemaname,
    tablename,
    indexname,
    idx_scan,
    idx_tup_read,
    idx_tup_fetch
FROM pg_stat_user_indexes
ORDER BY idx_scan DESC;
```

### 8.3 Application Testing Checklist

- [ ] User authentication works correctly
- [ ] Article CRUD operations function properly
- [ ] Product CRUD operations function properly
- [ ] Category tree displays correctly
- [ ] Search functionality returns correct results
- [ ] Pagination works as expected
- [ ] File uploads work correctly
- [ ] Comments and reviews save properly
- [ ] Order processing functions correctly
- [ ] Real-time notifications work (SignalR)

---

## Appendix: Quick Reference Commands

### PostgreSQL Administration

```bash
# Connect to database
psql -h localhost -U postgres -d cms_database

# Backup database
pg_dump -h localhost -U postgres cms_database > backup.sql

# Restore database
psql -h localhost -U postgres -d cms_database < backup.sql

# Check database size
psql -c "SELECT pg_size_pretty(pg_database_size('cms_database'));"

# List all tables
psql -c "\dt"

# Describe table
psql -c "\d article"
```

### Entity Framework Commands

```bash
# Add migration
dotnet ef migrations add InitialPostgreSQL --project CMS.Data

# Update database
dotnet ef database update --project CMS.Data

# Generate SQL script
dotnet ef migrations script --project CMS.Data -o migration.sql
```

---

*Document Version: 1.0*
*Last Updated: December 2024*