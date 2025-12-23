-- PostgreSQL Schema Migration Script
-- CMS Database Migration from SQL Server to PostgreSQL
-- Phase 2: Database Migration - Indexes
-- Generated: 2024

-- =============================================
-- Create Indexes for Performance Optimization
-- =============================================

-- ASP.NET Identity Indexes
CREATE INDEX idx_asp_net_users_normalized_email ON asp_net_users(normalized_email);
CREATE INDEX idx_asp_net_users_normalized_user_name ON asp_net_users(normalized_user_name);
CREATE INDEX idx_asp_net_roles_normalized_name ON asp_net_roles(normalized_name);
CREATE INDEX idx_asp_net_user_roles_role_id ON asp_net_user_roles(role_id);
CREATE INDEX idx_asp_net_user_claims_user_id ON asp_net_user_claims(user_id);
CREATE INDEX idx_asp_net_user_logins_user_id ON asp_net_user_logins(user_id);
CREATE INDEX idx_asp_net_role_claims_role_id ON asp_net_role_claims(role_id);
CREATE INDEX idx_asp_net_user_profiles_user_id ON asp_net_user_profiles(user_id);

-- Article Indexes
CREATE INDEX idx_article_article_type_id ON article(article_type_id);
CREATE INDEX idx_article_article_status_id ON article(article_status_id);
CREATE INDEX idx_article_product_brand_id ON article(product_brand_id);
CREATE INDEX idx_article_active ON article(active);
CREATE INDEX idx_article_create_date ON article(create_date DESC);
CREATE INDEX idx_article_start_date ON article(start_date);
CREATE INDEX idx_article_end_date ON article(end_date);
CREATE INDEX idx_article_url ON article(url);
CREATE INDEX idx_article_name ON article(name);
CREATE INDEX idx_article_create_by ON article(create_by);
CREATE INDEX idx_article_approved ON article(approved);

-- Article Category Indexes
CREATE INDEX idx_article_category_parent_id ON article_category(parent_id);
CREATE INDEX idx_article_category_active ON article_category(active);
CREATE INDEX idx_article_category_url ON article_category(url);
CREATE INDEX idx_article_category_sort ON article_category(sort);

-- Article Category Article Indexes
CREATE INDEX idx_article_category_article_article_id ON article_category_article(article_id);
CREATE INDEX idx_article_category_article_category_id ON article_category_article(article_category_id);

-- Article Block Article Indexes
CREATE INDEX idx_article_block_article_article_id ON article_block_article(article_id);
CREATE INDEX idx_article_block_article_block_id ON article_block_article(article_block_id);

-- Article Comment Indexes
CREATE INDEX idx_article_comment_article_id ON article_comment(article_id);
CREATE INDEX idx_article_comment_user_id ON article_comment(user_id);
CREATE INDEX idx_article_comment_parent_id ON article_comment(parent_id);
CREATE INDEX idx_article_comment_active ON article_comment(active);
CREATE INDEX idx_article_comment_create_date ON article_comment(create_date DESC);

-- Product Indexes
CREATE INDEX idx_product_product_type_id ON product(product_type_id);
CREATE INDEX idx_product_product_status_id ON product(product_status_id);
CREATE INDEX idx_product_product_brand_id ON product(product_brand_id);
CREATE INDEX idx_product_product_manufacture_id ON product(product_manufacture_id);
CREATE INDEX idx_product_country_id ON product(country_id);
CREATE INDEX idx_product_active ON product(active);
CREATE INDEX idx_product_create_date ON product(create_date DESC);
CREATE INDEX idx_product_start_date ON product(start_date);
CREATE INDEX idx_product_end_date ON product(end_date);
CREATE INDEX idx_product_url ON product(url);
CREATE INDEX idx_product_name ON product(name);
CREATE INDEX idx_product_sku ON product(sku);
CREATE INDEX idx_product_bar_code ON product(bar_code);
CREATE INDEX idx_product_create_by ON product(create_by);
CREATE INDEX idx_product_approved ON product(approved);
CREATE INDEX idx_product_price ON product(price);
CREATE INDEX idx_product_is_new ON product(is_new);
CREATE INDEX idx_product_is_best_sale ON product(is_best_sale);
CREATE INDEX idx_product_is_sale_off ON product(is_sale_off);
CREATE INDEX idx_product_sell_count ON product(sell_count DESC);
CREATE INDEX idx_product_counter ON product(counter DESC);

-- Product Category Indexes
CREATE INDEX idx_product_category_parent_id ON product_category(parent_id);
CREATE INDEX idx_product_category_active ON product_category(active);
CREATE INDEX idx_product_category_url ON product_category(url);
CREATE INDEX idx_product_category_sort ON product_category(sort);

-- Product Category Product Indexes
CREATE INDEX idx_product_category_product_product_id ON product_category_product(product_id);
CREATE INDEX idx_product_category_product_category_id ON product_category_product(product_category_id);

-- Product Block Product Indexes
CREATE INDEX idx_product_block_product_product_id ON product_block_product(product_id);
CREATE INDEX idx_product_block_product_block_id ON product_block_product(product_block_id);

-- Product Comment Indexes
CREATE INDEX idx_product_comment_product_id ON product_comment(product_id);
CREATE INDEX idx_product_comment_user_id ON product_comment(user_id);
CREATE INDEX idx_product_comment_parent_id ON product_comment(parent_id);
CREATE INDEX idx_product_comment_active ON product_comment(active);
CREATE INDEX idx_product_comment_create_date ON product_comment(create_date DESC);

-- Product Review Indexes
CREATE INDEX idx_product_review_product_id ON product_review(product_id);
CREATE INDEX idx_product_review_user_id ON product_review(user_id);
CREATE INDEX idx_product_review_active ON product_review(active);
CREATE INDEX idx_product_review_rate ON product_review(rate);
CREATE INDEX idx_product_review_create_date ON product_review(create_date DESC);

-- Product Like Indexes
CREATE INDEX idx_product_like_product_id ON product_like(product_id);
CREATE INDEX idx_product_like_user_id ON product_like(user_id);

-- Product Picture Indexes
CREATE INDEX idx_product_picture_product_id ON product_picture(product_id);
CREATE INDEX idx_product_picture_sort ON product_picture(sort);

-- Product Property Indexes
CREATE INDEX idx_product_property_product_id ON product_property(product_id);
CREATE INDEX idx_product_property_category_id ON product_property(product_property_category_id);
CREATE INDEX idx_product_property_type_id ON product_property(product_property_type_id);

-- Product Brand Indexes
CREATE INDEX idx_product_brand_product_brand_type_id ON product_brand(product_brand_type_id);
CREATE INDEX idx_product_brand_product_brand_status_id ON product_brand(product_brand_status_id);
CREATE INDEX idx_product_brand_active ON product_brand(active);
CREATE INDEX idx_product_brand_create_date ON product_brand(create_date DESC);
CREATE INDEX idx_product_brand_url ON product_brand(url);
CREATE INDEX idx_product_brand_name ON product_brand(name);
CREATE INDEX idx_product_brand_approved ON product_brand(approved);
CREATE INDEX idx_product_brand_location_id ON product_brand(location_id);

-- Product Order Indexes
CREATE INDEX idx_product_order_order_code ON product_order(order_code);
CREATE INDEX idx_product_order_product_brand_id ON product_order(product_brand_id);
CREATE INDEX idx_product_order_user_id ON product_order(user_id);
CREATE INDEX idx_product_order_status_id ON product_order(product_order_status_id);
CREATE INDEX idx_product_order_payment_status_id ON product_order(product_order_payment_status_id);
CREATE INDEX idx_product_order_create_date ON product_order(create_date DESC);
CREATE INDEX idx_product_order_customer_email ON product_order(customer_email);
CREATE INDEX idx_product_order_customer_phone ON product_order(customer_phone);

-- Product Order Detail Indexes
CREATE INDEX idx_product_order_detail_order_id ON product_order_detail(product_order_id);
CREATE INDEX idx_product_order_detail_product_id ON product_order_detail(product_id);

-- Advertising Indexes
CREATE INDEX idx_advertising_block_id ON advertising(advertising_block_id);
CREATE INDEX idx_advertising_type_id ON advertising(advertising_type_id);
CREATE INDEX idx_advertising_active ON advertising(active);
CREATE INDEX idx_advertising_start_date ON advertising(start_date);
CREATE INDEX idx_advertising_end_date ON advertising(end_date);
CREATE INDEX idx_advertising_sort ON advertising(sort);

-- Advertising Block Indexes
CREATE INDEX idx_advertising_block_article_category_id ON advertising_block(article_category_id);
CREATE INDEX idx_advertising_block_active ON advertising_block(active);
CREATE INDEX idx_advertising_block_sort ON advertising_block(sort);

-- Log Visit Indexes
CREATE INDEX idx_log_visit_user_id ON log_visit(user_id);
CREATE INDEX idx_log_visit_create_date ON log_visit(create_date DESC);
CREATE INDEX idx_log_visit_ip_address ON log_visit(ip_address);

-- User Notify Indexes
CREATE INDEX idx_user_notify_user_id ON user_notify(user_id);
CREATE INDEX idx_user_notify_is_read ON user_notify(is_read);
CREATE INDEX idx_user_notify_create_date ON user_notify(create_date DESC);

-- Location Indexes
CREATE INDEX idx_location_country_id ON location(country_id);
CREATE INDEX idx_district_location_id ON district(location_id);
CREATE INDEX idx_ward_district_id ON ward(district_id);

-- Department Indexes
CREATE INDEX idx_department_parent_id ON department(parent_id);
CREATE INDEX idx_department_active ON department(active);
CREATE INDEX idx_department_man_department_id ON department_man(department_id);
CREATE INDEX idx_department_man_user_id ON department_man(user_id);

-- =============================================
-- Create Full-Text Search Indexes (GIN)
-- =============================================

-- Enable pg_trgm extension for fuzzy search
CREATE EXTENSION IF NOT EXISTS pg_trgm;

-- Article full-text search
CREATE INDEX idx_article_name_trgm ON article USING gin(name gin_trgm_ops);
CREATE INDEX idx_article_description_trgm ON article USING gin(description gin_trgm_ops);

-- Product full-text search
CREATE INDEX idx_product_name_trgm ON product USING gin(name gin_trgm_ops);
CREATE INDEX idx_product_description_trgm ON product USING gin(description gin_trgm_ops);

-- Product Brand full-text search
CREATE INDEX idx_product_brand_name_trgm ON product_brand USING gin(name gin_trgm_ops);

-- Category full-text search
CREATE INDEX idx_article_category_name_trgm ON article_category USING gin(name gin_trgm_ops);
CREATE INDEX idx_product_category_name_trgm ON product_category USING gin(name gin_trgm_ops);

-- =============================================
-- Create Composite Indexes for Common Queries
-- =============================================

-- Article listing with status and date
CREATE INDEX idx_article_listing ON article(active, article_status_id, start_date DESC);

-- Product listing with status and date
CREATE INDEX idx_product_listing ON product(active, product_status_id, start_date DESC);

-- Product with price range
CREATE INDEX idx_product_price_range ON product(active, price, product_status_id);

-- Product brand listing
CREATE INDEX idx_product_brand_listing ON product_brand(active, product_brand_status_id, create_date DESC);

-- Order listing by status
CREATE INDEX idx_order_listing ON product_order(product_order_status_id, create_date DESC);

-- Comment listing
CREATE INDEX idx_article_comment_listing ON article_comment(article_id, active, create_date DESC);
CREATE INDEX idx_product_comment_listing ON product_comment(product_id, active, create_date DESC);

-- =============================================
-- Create Unique Indexes
-- =============================================

-- Unique constraints
CREATE UNIQUE INDEX idx_article_url_unique ON article(url) WHERE url IS NOT NULL AND url != '';
CREATE UNIQUE INDEX idx_product_url_unique ON product(url) WHERE url IS NOT NULL AND url != '';
CREATE UNIQUE INDEX idx_product_sku_unique ON product(sku) WHERE sku IS NOT NULL AND sku != '';
CREATE UNIQUE INDEX idx_product_brand_url_unique ON product_brand(url) WHERE url IS NOT NULL AND url != '';
CREATE UNIQUE INDEX idx_article_category_url_unique ON article_category(url) WHERE url IS NOT NULL AND url != '';
CREATE UNIQUE INDEX idx_product_category_url_unique ON product_category(url) WHERE url IS NOT NULL AND url != '';