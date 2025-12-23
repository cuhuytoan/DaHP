-- PostgreSQL Schema Migration Script
-- CMS Database Migration from SQL Server to PostgreSQL
-- Phase 2: Database Migration - Core Tables
-- Generated: 2024

-- =============================================
-- Drop existing tables if they exist (for clean migration)
-- =============================================

-- Drop tables in reverse dependency order
DROP TABLE IF EXISTS user_notify CASCADE;
DROP TABLE IF EXISTS log_visit CASCADE;
DROP TABLE IF EXISTS product_review CASCADE;
DROP TABLE IF EXISTS product_comment_staff CASCADE;
DROP TABLE IF EXISTS product_comment CASCADE;
DROP TABLE IF EXISTS product_like CASCADE;
DROP TABLE IF EXISTS product_log_edit CASCADE;
DROP TABLE IF EXISTS product_order_detail CASCADE;
DROP TABLE IF EXISTS product_order CASCADE;
DROP TABLE IF EXISTS product_property_value CASCADE;
DROP TABLE IF EXISTS product_property CASCADE;
DROP TABLE IF EXISTS product_picture CASCADE;
DROP TABLE IF EXISTS product_relation_product CASCADE;
DROP TABLE IF EXISTS product_category_product CASCADE;
DROP TABLE IF EXISTS product_block_product CASCADE;
DROP TABLE IF EXISTS product_attach_file CASCADE;
DROP TABLE IF EXISTS product CASCADE;
DROP TABLE IF EXISTS article_comment_staff CASCADE;
DROP TABLE IF EXISTS article_comment CASCADE;
DROP TABLE IF EXISTS article_relation_article CASCADE;
DROP TABLE IF EXISTS article_category_article CASCADE;
DROP TABLE IF EXISTS article_block_article CASCADE;
DROP TABLE IF EXISTS article_attach_file CASCADE;
DROP TABLE IF EXISTS article CASCADE;
DROP TABLE IF EXISTS advertising CASCADE;
DROP TABLE IF EXISTS asp_net_user_profiles CASCADE;
DROP TABLE IF EXISTS asp_net_user_tokens CASCADE;
DROP TABLE IF EXISTS asp_net_user_logins CASCADE;
DROP TABLE IF EXISTS asp_net_user_claims CASCADE;
DROP TABLE IF EXISTS asp_net_user_roles CASCADE;
DROP TABLE IF EXISTS asp_net_users CASCADE;
DROP TABLE IF EXISTS asp_net_role_claims CASCADE;
DROP TABLE IF EXISTS asp_net_roles CASCADE;
DROP TABLE IF EXISTS product_category_assign CASCADE;
DROP TABLE IF EXISTS product_category CASCADE;
DROP TABLE IF EXISTS product_property_category CASCADE;
DROP TABLE IF EXISTS product_property_type CASCADE;
DROP TABLE IF EXISTS product_brand_attach_file CASCADE;
DROP TABLE IF EXISTS product_brand_follow CASCADE;
DROP TABLE IF EXISTS product_brand_category CASCADE;
DROP TABLE IF EXISTS product_brand_model_management CASCADE;
DROP TABLE IF EXISTS product_brand CASCADE;
DROP TABLE IF EXISTS article_category_assign CASCADE;
DROP TABLE IF EXISTS article_category CASCADE;
DROP TABLE IF EXISTS article_block CASCADE;
DROP TABLE IF EXISTS article_top CASCADE;
DROP TABLE IF EXISTS article_type CASCADE;
DROP TABLE IF EXISTS article_status CASCADE;
DROP TABLE IF EXISTS advertising_block CASCADE;
DROP TABLE IF EXISTS advertising_type CASCADE;
DROP TABLE IF EXISTS product_block CASCADE;
DROP TABLE IF EXISTS product_top CASCADE;
DROP TABLE IF EXISTS product_type CASCADE;
DROP TABLE IF EXISTS product_status CASCADE;
DROP TABLE IF EXISTS product_brand_status CASCADE;
DROP TABLE IF EXISTS product_brand_type CASCADE;
DROP TABLE IF EXISTS product_brand_level CASCADE;
DROP TABLE IF EXISTS product_brand_qrcode_create_type CASCADE;
DROP TABLE IF EXISTS product_order_status CASCADE;
DROP TABLE IF EXISTS product_order_payment_status CASCADE;
DROP TABLE IF EXISTS product_order_payment_method CASCADE;
DROP TABLE IF EXISTS product_manufacture CASCADE;
DROP TABLE IF EXISTS department_man CASCADE;
DROP TABLE IF EXISTS department CASCADE;
DROP TABLE IF EXISTS ward CASCADE;
DROP TABLE IF EXISTS district CASCADE;
DROP TABLE IF EXISTS location CASCADE;
DROP TABLE IF EXISTS country CASCADE;
DROP TABLE IF EXISTS bank CASCADE;
DROP TABLE IF EXISTS unit CASCADE;
DROP TABLE IF EXISTS setting CASCADE;
DROP TABLE IF EXISTS replace_char CASCADE;

-- =============================================
-- Create Lookup/Reference Tables
-- =============================================

-- Article Status
CREATE TABLE article_status (
    id INTEGER PRIMARY KEY,
    name VARCHAR(200)
);

-- Article Type
CREATE TABLE article_type (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    active BOOLEAN DEFAULT true,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- Article Top
CREATE TABLE article_top (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    active BOOLEAN DEFAULT true,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- Product Status
CREATE TABLE product_status (
    id INTEGER PRIMARY KEY,
    name VARCHAR(200)
);

-- Product Type
CREATE TABLE product_type (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    active BOOLEAN DEFAULT true,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- Product Top
CREATE TABLE product_top (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    active BOOLEAN DEFAULT true,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- Product Brand Status
CREATE TABLE product_brand_status (
    id INTEGER PRIMARY KEY,
    name VARCHAR(200)
);

-- Product Brand Type
CREATE TABLE product_brand_type (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    active BOOLEAN DEFAULT true,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- Product Brand Level
CREATE TABLE product_brand_level (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    active BOOLEAN DEFAULT true,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- Product Brand QRCode Create Type
CREATE TABLE product_brand_qrcode_create_type (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    active BOOLEAN DEFAULT true,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- Product Order Status
CREATE TABLE product_order_status (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    active BOOLEAN DEFAULT true,
    sort INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- Product Order Payment Status
CREATE TABLE product_order_payment_status (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    active BOOLEAN DEFAULT true,
    sort INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- Product Order Payment Method
CREATE TABLE product_order_payment_method (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    active BOOLEAN DEFAULT true,
    sort INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- Product Manufacture
CREATE TABLE product_manufacture (
    id SERIAL PRIMARY KEY,
    name VARCHAR(500),
    url VARCHAR(500),
    image VARCHAR(500),
    description TEXT,
    sort INTEGER,
    active BOOLEAN DEFAULT true,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- Advertising Type
CREATE TABLE advertising_type (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200)
);

-- Unit
CREATE TABLE unit (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    active BOOLEAN DEFAULT true,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- Country
CREATE TABLE country (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    code VARCHAR(10),
    active BOOLEAN DEFAULT true
);

-- Location
CREATE TABLE location (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    code VARCHAR(50),
    country_id INTEGER REFERENCES country(id),
    active BOOLEAN DEFAULT true
);

-- District
CREATE TABLE district (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    code VARCHAR(50),
    location_id INTEGER REFERENCES location(id),
    active BOOLEAN DEFAULT true
);

-- Ward
CREATE TABLE ward (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    code VARCHAR(50),
    district_id INTEGER REFERENCES district(id),
    active BOOLEAN DEFAULT true
);

-- Department
CREATE TABLE department (
    id SERIAL PRIMARY KEY,
    parent_id INTEGER,
    name VARCHAR(500),
    url VARCHAR(500),
    image VARCHAR(500),
    description TEXT,
    sort INTEGER,
    active BOOLEAN DEFAULT true,
    can_delete BOOLEAN DEFAULT true,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- Department Man
CREATE TABLE department_man (
    id SERIAL PRIMARY KEY,
    department_id INTEGER REFERENCES department(id),
    user_id VARCHAR(450),
    create_by VARCHAR(450),
    create_date TIMESTAMP
);

-- Bank
CREATE TABLE bank (
    id SERIAL PRIMARY KEY,
    name VARCHAR(500),
    code VARCHAR(50),
    logo VARCHAR(500),
    active BOOLEAN DEFAULT true
);

-- Setting
CREATE TABLE setting (
    id INTEGER PRIMARY KEY,
    setting_key VARCHAR(200),
    setting_value TEXT,
    setting_comment VARCHAR(500)
);

-- Replace Char
CREATE TABLE replace_char (
    id SERIAL PRIMARY KEY,
    char_from VARCHAR(10),
    char_to VARCHAR(10)
);

-- =============================================
-- Create ASP.NET Identity Tables
-- =============================================

-- ASP.NET Roles
CREATE TABLE asp_net_roles (
    id VARCHAR(450) PRIMARY KEY,
    name VARCHAR(256),
    normalized_name VARCHAR(256),
    concurrency_stamp TEXT
);

-- ASP.NET Role Claims
CREATE TABLE asp_net_role_claims (
    id SERIAL PRIMARY KEY,
    role_id VARCHAR(450) NOT NULL REFERENCES asp_net_roles(id) ON DELETE CASCADE,
    claim_type TEXT,
    claim_value TEXT
);

-- ASP.NET Users
CREATE TABLE asp_net_users (
    id VARCHAR(450) PRIMARY KEY,
    user_name VARCHAR(256),
    normalized_user_name VARCHAR(256),
    email VARCHAR(256),
    normalized_email VARCHAR(256),
    email_confirmed BOOLEAN NOT NULL DEFAULT false,
    password_hash TEXT,
    security_stamp TEXT,
    concurrency_stamp TEXT,
    phone_number TEXT,
    phone_number_confirmed BOOLEAN NOT NULL DEFAULT false,
    two_factor_enabled BOOLEAN NOT NULL DEFAULT false,
    lockout_end TIMESTAMPTZ,
    lockout_enabled BOOLEAN NOT NULL DEFAULT false,
    access_failed_count INTEGER NOT NULL DEFAULT 0
);

-- ASP.NET User Roles
CREATE TABLE asp_net_user_roles (
    user_id VARCHAR(450) NOT NULL REFERENCES asp_net_users(id) ON DELETE CASCADE,
    role_id VARCHAR(450) NOT NULL REFERENCES asp_net_roles(id) ON DELETE CASCADE,
    PRIMARY KEY (user_id, role_id)
);

-- ASP.NET User Claims
CREATE TABLE asp_net_user_claims (
    id SERIAL PRIMARY KEY,
    user_id VARCHAR(450) NOT NULL REFERENCES asp_net_users(id) ON DELETE CASCADE,
    claim_type TEXT,
    claim_value TEXT
);

-- ASP.NET User Logins
CREATE TABLE asp_net_user_logins (
    login_provider VARCHAR(128) NOT NULL,
    provider_key VARCHAR(128) NOT NULL,
    provider_display_name TEXT,
    user_id VARCHAR(450) NOT NULL REFERENCES asp_net_users(id) ON DELETE CASCADE,
    PRIMARY KEY (login_provider, provider_key)
);

-- ASP.NET User Tokens
CREATE TABLE asp_net_user_tokens (
    user_id VARCHAR(450) NOT NULL REFERENCES asp_net_users(id) ON DELETE CASCADE,
    login_provider VARCHAR(128) NOT NULL,
    name VARCHAR(128) NOT NULL,
    value TEXT,
    PRIMARY KEY (user_id, login_provider, name)
);

-- ASP.NET User Profiles (Custom extension)
CREATE TABLE asp_net_user_profiles (
    id SERIAL PRIMARY KEY,
    user_id VARCHAR(450) REFERENCES asp_net_users(id) ON DELETE CASCADE,
    full_name VARCHAR(500),
    avatar VARCHAR(500),
    phone VARCHAR(50),
    address VARCHAR(1000),
    location_id INTEGER REFERENCES location(id),
    district_id INTEGER REFERENCES district(id),
    ward_id INTEGER REFERENCES ward(id),
    birthday DATE,
    gender INTEGER,
    department_id INTEGER REFERENCES department(id),
    active BOOLEAN DEFAULT true,
    create_date TIMESTAMP,
    last_edited_date TIMESTAMP
);

-- =============================================
-- Create Category Tables
-- =============================================

-- Article Category
CREATE TABLE article_category (
    id SERIAL PRIMARY KEY,
    parent_id INTEGER,
    name VARCHAR(500),
    url VARCHAR(500),
    image VARCHAR(500),
    description TEXT,
    sort INTEGER,
    counter INTEGER DEFAULT 0,
    display_menu BOOLEAN DEFAULT true,
    menu_color VARCHAR(50),
    active BOOLEAN DEFAULT true,
    can_delete BOOLEAN DEFAULT true,
    create_by VARCHAR(500),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(500),
    last_edited_date TIMESTAMP
);

-- Article Category Assign
CREATE TABLE article_category_assign (
    id SERIAL PRIMARY KEY,
    article_category_id INTEGER REFERENCES article_category(id),
    user_id VARCHAR(450) REFERENCES asp_net_users(id),
    create_by VARCHAR(450),
    create_date TIMESTAMP
);

-- Product Category
CREATE TABLE product_category (
    id SERIAL PRIMARY KEY,
    parent_id INTEGER,
    name VARCHAR(500),
    url VARCHAR(500),
    image VARCHAR(500),
    description TEXT,
    sort INTEGER,
    counter INTEGER DEFAULT 0,
    display_menu BOOLEAN DEFAULT true,
    display_menu_horizontal BOOLEAN DEFAULT false,
    menu_color VARCHAR(50),
    active BOOLEAN DEFAULT true,
    can_delete BOOLEAN DEFAULT true,
    create_by VARCHAR(500),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(500),
    last_edited_date TIMESTAMP,
    meta_description VARCHAR(500),
    meta_keywords VARCHAR(500)
);

-- Product Category Assign
CREATE TABLE product_category_assign (
    id SERIAL PRIMARY KEY,
    product_category_id INTEGER REFERENCES product_category(id),
    user_id VARCHAR(450) REFERENCES asp_net_users(id),
    create_by VARCHAR(450),
    create_date TIMESTAMP
);

-- Product Property Type
CREATE TABLE product_property_type (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200),
    active BOOLEAN DEFAULT true,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- Product Property Category
CREATE TABLE product_property_category (
    id SERIAL PRIMARY KEY,
    parent_id INTEGER,
    product_category_id INTEGER REFERENCES product_category(id),
    name VARCHAR(500),
    url VARCHAR(500),
    description TEXT,
    sort INTEGER,
    active BOOLEAN DEFAULT true,
    can_delete BOOLEAN DEFAULT true,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- =============================================
-- Create Block Tables
-- =============================================

-- Article Block
CREATE TABLE article_block (
    id SERIAL PRIMARY KEY,
    name VARCHAR(500),
    description TEXT,
    sort INTEGER,
    active BOOLEAN DEFAULT true,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- Product Block
CREATE TABLE product_block (
    id SERIAL PRIMARY KEY,
    name VARCHAR(500),
    description TEXT,
    sort INTEGER,
    active BOOLEAN DEFAULT true,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- Advertising Block
CREATE TABLE advertising_block (
    id SERIAL PRIMARY KEY,
    article_category_id INTEGER REFERENCES article_category(id),
    name VARCHAR(500),
    description TEXT,
    device_display INTEGER DEFAULT 0,
    sort INTEGER,
    active BOOLEAN DEFAULT true,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edited_by VARCHAR(450),
    last_edited_date TIMESTAMP
);

-- =============================================
-- Create Product Brand Tables
-- =============================================

-- Product Brand
CREATE TABLE product_brand (
    id SERIAL PRIMARY KEY,
    product_brand_type_id INTEGER REFERENCES product_brand_type(id),
    product_brand_status_id INTEGER REFERENCES product_brand_status(id),
    product_brand_level_id INTEGER REFERENCES product_brand_level(id),
    product_brand_qrcode_create_type_id INTEGER REFERENCES product_brand_qrcode_create_type(id),
    name VARCHAR(1000),
    code VARCHAR(100),
    qrcode_public VARCHAR(100),
    sub_title VARCHAR(200),
    image VARCHAR(200),
    image_description VARCHAR(200),
    banner_image VARCHAR(200),
    description TEXT,
    content TEXT,
    location_id INTEGER REFERENCES location(id),
    district_id INTEGER REFERENCES district(id),
    ward_id INTEGER REFERENCES ward(id),
    address VARCHAR(1000),
    phone VARCHAR(100),
    fax VARCHAR(100),
    email VARCHAR(200),
    website VARCHAR(500),
    facebook VARCHAR(500),
    youtube VARCHAR(500),
    zalo VARCHAR(100),
    tax_code VARCHAR(50),
    bank_account VARCHAR(50),
    bank_id INTEGER REFERENCES bank(id),
    representative VARCHAR(200),
    representative_phone VARCHAR(50),
    representative_email VARCHAR(200),
    contact_name VARCHAR(200),
    contact_phone VARCHAR(50),
    contact_email VARCHAR(200),
    has_qrcode BOOLEAN DEFAULT false,
    view_count INTEGER DEFAULT 0,
    view_page_count INTEGER DEFAULT 0,
    sell_count INTEGER DEFAULT 0,
    rate INTEGER,
    start_date TIMESTAMP,
    end_date TIMESTAMP,
    active BOOLEAN DEFAULT true,
    counter INTEGER DEFAULT 0,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
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
    can_copy BOOLEAN DEFAULT true,
    can_comment BOOLEAN DEFAULT true,
    can_delete BOOLEAN DEFAULT true,
    meta_title VARCHAR(500),
    meta_description VARCHAR(500),
    meta_keywords VARCHAR(500)
);

-- Product Brand Category
CREATE TABLE product_brand_category (
    id SERIAL PRIMARY KEY,
    product_brand_id INTEGER REFERENCES product_brand(id),
    product_category_id INTEGER REFERENCES product_category(id),
    create_by VARCHAR(450),
    create_date TIMESTAMP
);

-- Product Brand Follow
CREATE TABLE product_brand_follow (
    id SERIAL PRIMARY KEY,
    product_brand_id INTEGER REFERENCES product_brand(id),
    user_id VARCHAR(450) REFERENCES asp_net_users(id),
    create_date TIMESTAMP
);

-- Product Brand Attach File
CREATE TABLE product_brand_attach_file (
    id SERIAL PRIMARY KEY,
    product_brand_id INTEGER REFERENCES product_brand(id),
    name VARCHAR(500),
    file_path VARCHAR(500),
    file_type VARCHAR(50),
    file_size INTEGER,
    sort INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP
);

-- Product Brand Model Management
CREATE TABLE product_brand_model_management (
    id SERIAL PRIMARY KEY,
    product_brand_id INTEGER REFERENCES product_brand(id),
    user_id VARCHAR(450) REFERENCES asp_net_users(id),
    create_by VARCHAR(450),
    create_date TIMESTAMP
);

-- =============================================
-- Create Article Tables
-- =============================================

-- Article
CREATE TABLE article (
    id SERIAL PRIMARY KEY,
    article_type_id INTEGER REFERENCES article_type(id),
    article_category_ids VARCHAR(200),
    product_brand_id INTEGER REFERENCES product_brand(id),
    article_status_id INTEGER REFERENCES article_status(id),
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
    active BOOLEAN DEFAULT true,
    counter INTEGER DEFAULT 0,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
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
    can_copy BOOLEAN DEFAULT true,
    can_comment BOOLEAN DEFAULT true,
    can_delete BOOLEAN DEFAULT true,
    meta_title VARCHAR(500),
    meta_description VARCHAR(500),
    meta_keywords VARCHAR(500),
    document_refer VARCHAR(500)
);

-- Article Attach File
CREATE TABLE article_attach_file (
    id SERIAL PRIMARY KEY,
    article_id INTEGER REFERENCES article(id),
    name VARCHAR(500),
    file_path VARCHAR(500),
    file_type VARCHAR(50),
    file_size INTEGER,
    sort INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP
);

-- Article Block Article
CREATE TABLE article_block_article (
    id SERIAL PRIMARY KEY,
    article_block_id INTEGER REFERENCES article_block(id),
    article_id INTEGER REFERENCES article(id),
    sort INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP
);

-- Article Category Article
CREATE TABLE article_category_article (
    id SERIAL PRIMARY KEY,
    article_category_id INTEGER REFERENCES article_category(id),
    article_id INTEGER REFERENCES article(id),
    sort INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP
);

-- Article Relation Article
CREATE TABLE article_relation_article (
    id SERIAL PRIMARY KEY,
    article_id INTEGER REFERENCES article(id),
    article_relation_id INTEGER REFERENCES article(id),
    sort INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP
);

-- Article Comment
CREATE TABLE article_comment (
    id SERIAL PRIMARY KEY,
    article_id INTEGER REFERENCES article(id),
    parent_id INTEGER,
    user_id VARCHAR(450) REFERENCES asp_net_users(id),
    name VARCHAR(200),
    email VARCHAR(200),
    phone VARCHAR(50),
    content TEXT,
    active BOOLEAN DEFAULT true,
    create_date TIMESTAMP
);

-- Article Comment Staff
CREATE TABLE article_comment_staff (
    id SERIAL PRIMARY KEY,
    article_comment_id INTEGER REFERENCES article_comment(id),
    user_id VARCHAR(450) REFERENCES asp_net_users(id),
    content TEXT,
    active BOOLEAN DEFAULT true,
    create_date TIMESTAMP
);

-- =============================================
-- Create Product Tables
-- =============================================

-- Product
CREATE TABLE product (
    id SERIAL PRIMARY KEY,
    product_type_id INTEGER REFERENCES product_type(id),
    product_category_ids VARCHAR(200),
    product_manufacture_id INTEGER REFERENCES product_manufacture(id),
    product_brand_id INTEGER REFERENCES product_brand(id),
    country_id INTEGER REFERENCES country(id),
    product_status_id INTEGER REFERENCES product_status(id),
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
    is_second_hand BOOLEAN DEFAULT false,
    is_author BOOLEAN DEFAULT false,
    is_best_sale BOOLEAN DEFAULT false,
    is_sale_off BOOLEAN DEFAULT false,
    is_new BOOLEAN DEFAULT false,
    is_comming BOOLEAN DEFAULT false,
    is_out_stock BOOLEAN DEFAULT false,
    is_discontinue BOOLEAN DEFAULT false,
    amount_default INTEGER,
    unit_id INTEGER REFERENCES unit(id),
    expiry_display VARCHAR(500),
    expiry_by_day INTEGER,
    warranty_display VARCHAR(500),
    warranty_by_month INTEGER DEFAULT 0,
    rate INTEGER,
    start_date TIMESTAMP,
    end_date TIMESTAMP,
    active BOOLEAN DEFAULT true,
    counter INTEGER DEFAULT 0,
    like_count INTEGER DEFAULT 0,
    sell_count INTEGER DEFAULT 0,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
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
    can_copy BOOLEAN DEFAULT true,
    can_comment BOOLEAN DEFAULT true,
    can_delete BOOLEAN DEFAULT true,
    meta_title VARCHAR(500),
    meta_description VARCHAR(500),
    meta_keywords VARCHAR(500),
    document_refer VARCHAR(500)
);

-- Product Attach File
CREATE TABLE product_attach_file (
    id SERIAL PRIMARY KEY,
    product_id INTEGER REFERENCES product(id),
    name VARCHAR(500),
    file_path VARCHAR(500),
    file_type VARCHAR(50),
    file_size INTEGER,
    sort INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP
);

-- Product Picture
CREATE TABLE product_picture (
    id SERIAL PRIMARY KEY,
    product_id INTEGER REFERENCES product(id),
    image VARCHAR(200),
    sort INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP
);

-- Product Property
CREATE TABLE product_property (
    id SERIAL PRIMARY KEY,
    product_id INTEGER REFERENCES product(id),
    product_property_category_id INTEGER REFERENCES product_property_category(id),
    product_property_type_id INTEGER REFERENCES product_property_type(id),
    name VARCHAR(500),
    value VARCHAR(500),
    sort INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP
);

-- Product Property Value
CREATE TABLE product_property_value (
    id SERIAL PRIMARY KEY,
    product_property_id INTEGER REFERENCES product_property(id),
    value VARCHAR(500),
    sort INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP
);

-- Product Block Product
CREATE TABLE product_block_product (
    id SERIAL PRIMARY KEY,
    product_block_id INTEGER REFERENCES product_block(id),
    product_id INTEGER REFERENCES product(id),
    sort INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP
);

-- Product Category Product
CREATE TABLE product_category_product (
    id SERIAL PRIMARY KEY,
    product_category_id INTEGER REFERENCES product_category(id),
    product_id INTEGER REFERENCES product(id),
    sort INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP
);

-- Product Relation Product
CREATE TABLE product_relation_product (
    id SERIAL PRIMARY KEY,
    product_id INTEGER REFERENCES product(id),
    product_relation_id INTEGER REFERENCES product(id),
    sort INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP
);

-- Product Comment
CREATE TABLE product_comment (
    id SERIAL PRIMARY KEY,
    product_id INTEGER REFERENCES product(id),
    parent_id INTEGER,
    user_id VARCHAR(450) REFERENCES asp_net_users(id),
    name VARCHAR(200),
    email VARCHAR(200),
    phone VARCHAR(50),
    content TEXT,
    active BOOLEAN DEFAULT true,
    create_date TIMESTAMP
);

-- Product Comment Staff
CREATE TABLE product_comment_staff (
    id SERIAL PRIMARY KEY,
    product_comment_id INTEGER REFERENCES product_comment(id),
    user_id VARCHAR(450) REFERENCES asp_net_users(id),
    content TEXT,
    active BOOLEAN DEFAULT true,
    create_date TIMESTAMP
);

-- Product Like
CREATE TABLE product_like (
    id SERIAL PRIMARY KEY,
    product_id INTEGER REFERENCES product(id),
    user_id VARCHAR(450) REFERENCES asp_net_users(id),
    create_date TIMESTAMP
);

-- Product Log Edit
CREATE TABLE product_log_edit (
    id SERIAL PRIMARY KEY,
    product_id INTEGER REFERENCES product(id),
    user_id VARCHAR(450) REFERENCES asp_net_users(id),
    action VARCHAR(100),
    description TEXT,
    create_date TIMESTAMP
);

-- Product Review
CREATE TABLE product_review (
    id SERIAL PRIMARY KEY,
    product_id INTEGER REFERENCES product(id),
    user_id VARCHAR(450) REFERENCES asp_net_users(id),
    name VARCHAR(200),
    email VARCHAR(200),
    phone VARCHAR(50),
    title VARCHAR(500),
    content TEXT,
    rate INTEGER,
    active BOOLEAN DEFAULT true,
    create_date TIMESTAMP
);

-- =============================================
-- Create Order Tables
-- =============================================

-- Product Order
CREATE TABLE product_order (
    id SERIAL PRIMARY KEY,
    order_code VARCHAR(50),
    product_brand_id INTEGER REFERENCES product_brand(id),
    product_order_status_id INTEGER REFERENCES product_order_status(id),
    product_order_payment_status_id INTEGER REFERENCES product_order_payment_status(id),
    product_order_payment_method_id INTEGER REFERENCES product_order_payment_method(id),
    user_id VARCHAR(450) REFERENCES asp_net_users(id),
    customer_name VARCHAR(200),
    customer_email VARCHAR(200),
    customer_phone VARCHAR(50),
    customer_address VARCHAR(1000),
    location_id INTEGER REFERENCES location(id),
    district_id INTEGER REFERENCES district(id),
    ward_id INTEGER REFERENCES ward(id),
    shipping_name VARCHAR(200),
    shipping_phone VARCHAR(50),
    shipping_address VARCHAR(1000),
    shipping_location_id INTEGER REFERENCES location(id),
    shipping_district_id INTEGER REFERENCES district(id),
    shipping_ward_id INTEGER REFERENCES ward(id),
    shipping_fee DECIMAL(19,4) DEFAULT 0,
    sub_total DECIMAL(19,4) DEFAULT 0,
    discount DECIMAL(19,4) DEFAULT 0,
    total DECIMAL(19,4) DEFAULT 0,
    note TEXT,
    create_date TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP
);

-- Product Order Detail
CREATE TABLE product_order_detail (
    id SERIAL PRIMARY KEY,
    product_order_id INTEGER REFERENCES product_order(id),
    product_id INTEGER REFERENCES product(id),
    product_name VARCHAR(1000),
    product_image VARCHAR(200),
    product_url VARCHAR(1000),
    price DECIMAL(19,4) DEFAULT 0,
    quantity INTEGER DEFAULT 1,
    discount DECIMAL(19,4) DEFAULT 0,
    total DECIMAL(19,4) DEFAULT 0,
    create_date TIMESTAMP
);

-- =============================================
-- Create Advertising Table
-- =============================================

-- Advertising
CREATE TABLE advertising (
    id SERIAL PRIMARY KEY,
    advertising_block_id INTEGER REFERENCES advertising_block(id),
    advertising_type_id INTEGER REFERENCES advertising_type(id),
    name VARCHAR(500),
    description TEXT,
    image VARCHAR(500),
    url VARCHAR(1000),
    target VARCHAR(50),
    code TEXT,
    width INTEGER,
    height INTEGER,
    counter INTEGER DEFAULT 0,
    start_date TIMESTAMP,
    end_date TIMESTAMP,
    sort INTEGER,
    active BOOLEAN DEFAULT true,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP
);

-- =============================================
-- Create Log and Notification Tables
-- =============================================

-- Log Visit
CREATE TABLE log_visit (
    id SERIAL PRIMARY KEY,
    user_id VARCHAR(450) REFERENCES asp_net_users(id),
    ip_address VARCHAR(50),
    user_agent TEXT,
    url VARCHAR(1000),
    referrer VARCHAR(1000),
    create_date TIMESTAMP
);

-- User Notify
CREATE TABLE user_notify (
    id SERIAL PRIMARY KEY,
    user_id VARCHAR(450) REFERENCES asp_net_users(id),
    title VARCHAR(500),
    content TEXT,
    url VARCHAR(1000),
    is_read BOOLEAN DEFAULT false,
    create_date TIMESTAMP
);

-- =============================================
-- Add Comments to Tables
-- =============================================

COMMENT ON TABLE article IS 'Bảng lưu trữ bài viết';
COMMENT ON TABLE product IS 'Bảng lưu trữ sản phẩm';
COMMENT ON TABLE product_brand IS 'Bảng lưu trữ thương hiệu/doanh nghiệp';
COMMENT ON TABLE article_category IS 'Bảng lưu trữ danh mục bài viết';
COMMENT ON TABLE product_category IS 'Bảng lưu trữ danh mục sản phẩm';
COMMENT ON TABLE asp_net_users IS 'Bảng lưu trữ người dùng';
COMMENT ON TABLE product_order IS 'Bảng lưu trữ đơn hàng';