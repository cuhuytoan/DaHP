-- PostgreSQL Functions for Article Management
-- Converted from SQL Server Stored Procedures
-- CMS Database Migration

-- =============================================
-- Function: sp_article_search
-- Description: Search articles with pagination
-- =============================================
CREATE OR REPLACE FUNCTION sp_article_search(
    p_keyword VARCHAR(4000) DEFAULT NULL,
    p_article_category_id INTEGER DEFAULT NULL,
    p_article_status_id INTEGER DEFAULT NULL,
    p_product_brand_id INTEGER DEFAULT NULL,
    p_article_type_id INTEGER DEFAULT NULL,
    p_exception_id INTEGER DEFAULT NULL,
    p_exception_article_top BOOLEAN DEFAULT NULL,
    p_from_date TIMESTAMP DEFAULT NULL,
    p_to_date TIMESTAMP DEFAULT NULL,
    p_efficiency BOOLEAN DEFAULT NULL,
    p_active BOOLEAN DEFAULT NULL,
    p_assign_by VARCHAR(900) DEFAULT NULL,
    p_create_by VARCHAR(900) DEFAULT NULL,
    p_page_size INTEGER DEFAULT 20,
    p_current_page INTEGER DEFAULT 1,
    OUT p_item_count INTEGER
)
RETURNS TABLE (
    id INTEGER,
    article_type_id INTEGER,
    article_category_ids VARCHAR(200),
    product_brand_id INTEGER,
    article_status_id INTEGER,
    name VARCHAR(1000),
    sub_title VARCHAR(200),
    image VARCHAR(200),
    description TEXT,
    author VARCHAR(200),
    start_date TIMESTAMP,
    end_date TIMESTAMP,
    active BOOLEAN,
    counter INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP,
    checked INTEGER,
    approved INTEGER,
    url VARCHAR(1000),
    article_status_name VARCHAR(200),
    article_type_name VARCHAR(200),
    product_brand_name VARCHAR(1000),
    row_num BIGINT
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_offset INTEGER;
BEGIN
    -- Calculate offset
    v_offset := (p_current_page - 1) * p_page_size;
    
    -- Get total count
    SELECT COUNT(*)
    INTO p_item_count
    FROM article a
    WHERE (p_keyword IS NULL OR a.name ILIKE '%' || p_keyword || '%' OR a.description ILIKE '%' || p_keyword || '%')
      AND (p_article_category_id IS NULL OR a.article_category_ids LIKE '%' || p_article_category_id::TEXT || '%')
      AND (p_article_status_id IS NULL OR a.article_status_id = p_article_status_id)
      AND (p_product_brand_id IS NULL OR a.product_brand_id = p_product_brand_id)
      AND (p_article_type_id IS NULL OR a.article_type_id = p_article_type_id)
      AND (p_exception_id IS NULL OR a.id != p_exception_id)
      AND (p_from_date IS NULL OR a.create_date >= p_from_date)
      AND (p_to_date IS NULL OR a.create_date <= p_to_date)
      AND (p_active IS NULL OR a.active = p_active)
      AND (p_create_by IS NULL OR a.create_by = p_create_by)
      AND (p_efficiency IS NULL OR (p_efficiency = true AND a.start_date <= NOW() AND (a.end_date IS NULL OR a.end_date >= NOW())));

    -- Return paginated results
    RETURN QUERY
    SELECT 
        a.id,
        a.article_type_id,
        a.article_category_ids,
        a.product_brand_id,
        a.article_status_id,
        a.name,
        a.sub_title,
        a.image,
        a.description,
        a.author,
        a.start_date,
        a.end_date,
        a.active,
        a.counter,
        a.create_by,
        a.create_date,
        a.last_edit_by,
        a.last_edit_date,
        a.checked,
        a.approved,
        a.url,
        ast.name AS article_status_name,
        at.name AS article_type_name,
        pb.name AS product_brand_name,
        ROW_NUMBER() OVER (ORDER BY a.create_date DESC) AS row_num
    FROM article a
    LEFT JOIN article_status ast ON a.article_status_id = ast.id
    LEFT JOIN article_type at ON a.article_type_id = at.id
    LEFT JOIN product_brand pb ON a.product_brand_id = pb.id
    WHERE (p_keyword IS NULL OR a.name ILIKE '%' || p_keyword || '%' OR a.description ILIKE '%' || p_keyword || '%')
      AND (p_article_category_id IS NULL OR a.article_category_ids LIKE '%' || p_article_category_id::TEXT || '%')
      AND (p_article_status_id IS NULL OR a.article_status_id = p_article_status_id)
      AND (p_product_brand_id IS NULL OR a.product_brand_id = p_product_brand_id)
      AND (p_article_type_id IS NULL OR a.article_type_id = p_article_type_id)
      AND (p_exception_id IS NULL OR a.id != p_exception_id)
      AND (p_from_date IS NULL OR a.create_date >= p_from_date)
      AND (p_to_date IS NULL OR a.create_date <= p_to_date)
      AND (p_active IS NULL OR a.active = p_active)
      AND (p_create_by IS NULL OR a.create_by = p_create_by)
      AND (p_efficiency IS NULL OR (p_efficiency = true AND a.start_date <= NOW() AND (a.end_date IS NULL OR a.end_date >= NOW())))
    ORDER BY a.create_date DESC
    LIMIT p_page_size
    OFFSET v_offset;
END;
$$;

-- =============================================
-- Function: sp_article_breadcrumb
-- Description: Get article category breadcrumb
-- =============================================
CREATE OR REPLACE FUNCTION sp_article_breadcrumb(
    p_article_category_id INTEGER
)
RETURNS TABLE (
    id INTEGER,
    parent_id INTEGER,
    name VARCHAR(500),
    url VARCHAR(500),
    level INTEGER
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    WITH RECURSIVE category_tree AS (
        -- Base case: start with the given category
        SELECT 
            ac.id,
            ac.parent_id,
            ac.name,
            ac.url,
            1 AS level
        FROM article_category ac
        WHERE ac.id = p_article_category_id
        
        UNION ALL
        
        -- Recursive case: get parent categories
        SELECT 
            ac.id,
            ac.parent_id,
            ac.name,
            ac.url,
            ct.level + 1
        FROM article_category ac
        INNER JOIN category_tree ct ON ac.id = ct.parent_id
    )
    SELECT 
        ct.id,
        ct.parent_id,
        ct.name,
        ct.url,
        ct.level
    FROM category_tree ct
    ORDER BY ct.level DESC;
END;
$$;

-- =============================================
-- Function: sp_article_category_tree
-- Description: Get article category tree structure
-- =============================================
CREATE OR REPLACE FUNCTION sp_article_category_tree(
    p_root INTEGER DEFAULT NULL,
    p_first_line VARCHAR(400) DEFAULT NULL
)
RETURNS TABLE (
    id INTEGER,
    parent_id INTEGER,
    name VARCHAR(500),
    url VARCHAR(500),
    level INTEGER,
    display_name VARCHAR(1000),
    sort INTEGER,
    active BOOLEAN
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
            ac.url,
            0 AS level,
            ac.name::VARCHAR(1000) AS display_name,
            ac.sort,
            ac.active
        FROM article_category ac
        WHERE (p_root IS NULL AND ac.parent_id IS NULL) OR ac.id = p_root
        
        UNION ALL
        
        -- Recursive case: child categories
        SELECT 
            ac.id,
            ac.parent_id,
            ac.name,
            ac.url,
            ct.level + 1,
            (REPEAT('--- ', ct.level + 1) || ac.name)::VARCHAR(1000) AS display_name,
            ac.sort,
            ac.active
        FROM article_category ac
        INNER JOIN category_tree ct ON ac.parent_id = ct.id
    )
    SELECT 
        ct.id,
        ct.parent_id,
        ct.name,
        ct.url,
        ct.level,
        CASE 
            WHEN p_first_line IS NOT NULL AND ct.level = 0 THEN p_first_line
            ELSE ct.display_name
        END AS display_name,
        ct.sort,
        ct.active
    FROM category_tree ct
    ORDER BY ct.level, ct.sort, ct.name;
END;
$$;

-- =============================================
-- Function: sp_article_get_by_block_id
-- Description: Get articles by block ID
-- =============================================
CREATE OR REPLACE FUNCTION sp_article_get_by_block_id(
    p_article_block_id INTEGER
)
RETURNS TABLE (
    id INTEGER,
    name VARCHAR(1000),
    sub_title VARCHAR(200),
    image VARCHAR(200),
    description TEXT,
    author VARCHAR(200),
    start_date TIMESTAMP,
    url VARCHAR(1000),
    counter INTEGER,
    sort INTEGER
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        a.id,
        a.name,
        a.sub_title,
        a.image,
        a.description,
        a.author,
        a.start_date,
        a.url,
        a.counter,
        aba.sort
    FROM article a
    INNER JOIN article_block_article aba ON a.id = aba.article_id
    WHERE aba.article_block_id = p_article_block_id
      AND a.active = true
      AND a.start_date <= NOW()
      AND (a.end_date IS NULL OR a.end_date >= NOW())
    ORDER BY aba.sort, a.start_date DESC;
END;
$$;

-- =============================================
-- Function: sp_article_get_by_category_id
-- Description: Get articles by category ID with pagination
-- =============================================
CREATE OR REPLACE FUNCTION sp_article_get_by_category_id(
    p_article_category_id INTEGER,
    p_page_size INTEGER DEFAULT 20,
    p_current_page INTEGER DEFAULT 1,
    OUT p_item_count INTEGER
)
RETURNS TABLE (
    id INTEGER,
    name VARCHAR(1000),
    sub_title VARCHAR(200),
    image VARCHAR(200),
    description TEXT,
    author VARCHAR(200),
    start_date TIMESTAMP,
    url VARCHAR(1000),
    counter INTEGER,
    row_num BIGINT
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_offset INTEGER;
BEGIN
    v_offset := (p_current_page - 1) * p_page_size;
    
    -- Get total count
    SELECT COUNT(*)
    INTO p_item_count
    FROM article a
    WHERE a.article_category_ids LIKE '%' || p_article_category_id::TEXT || '%'
      AND a.active = true
      AND a.start_date <= NOW()
      AND (a.end_date IS NULL OR a.end_date >= NOW());

    -- Return paginated results
    RETURN QUERY
    SELECT 
        a.id,
        a.name,
        a.sub_title,
        a.image,
        a.description,
        a.author,
        a.start_date,
        a.url,
        a.counter,
        ROW_NUMBER() OVER (ORDER BY a.start_date DESC) AS row_num
    FROM article a
    WHERE a.article_category_ids LIKE '%' || p_article_category_id::TEXT || '%'
      AND a.active = true
      AND a.start_date <= NOW()
      AND (a.end_date IS NULL OR a.end_date >= NOW())
    ORDER BY a.start_date DESC
    LIMIT p_page_size
    OFFSET v_offset;
END;
$$;

-- =============================================
-- Function: sp_article_get_new_by_category_id
-- Description: Get newest articles by category ID
-- =============================================
CREATE OR REPLACE FUNCTION sp_article_get_new_by_category_id(
    p_article_category_id INTEGER,
    p_number INTEGER DEFAULT 10
)
RETURNS TABLE (
    id INTEGER,
    name VARCHAR(1000),
    sub_title VARCHAR(200),
    image VARCHAR(200),
    description TEXT,
    author VARCHAR(200),
    start_date TIMESTAMP,
    url VARCHAR(1000),
    counter INTEGER
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        a.id,
        a.name,
        a.sub_title,
        a.image,
        a.description,
        a.author,
        a.start_date,
        a.url,
        a.counter
    FROM article a
    WHERE a.article_category_ids LIKE '%' || p_article_category_id::TEXT || '%'
      AND a.active = true
      AND a.start_date <= NOW()
      AND (a.end_date IS NULL OR a.end_date >= NOW())
    ORDER BY a.start_date DESC
    LIMIT p_number;
END;
$$;

-- =============================================
-- Function: sp_article_get_top_by_category_id
-- Description: Get top/featured articles by category ID
-- =============================================
CREATE OR REPLACE FUNCTION sp_article_get_top_by_category_id(
    p_article_category_id INTEGER
)
RETURNS TABLE (
    id INTEGER,
    name VARCHAR(1000),
    sub_title VARCHAR(200),
    image VARCHAR(200),
    description TEXT,
    author VARCHAR(200),
    start_date TIMESTAMP,
    url VARCHAR(1000),
    counter INTEGER,
    article_top_id INTEGER,
    article_top_name VARCHAR(200)
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        a.id,
        a.name,
        a.sub_title,
        a.image,
        a.description,
        a.author,
        a.start_date,
        a.url,
        a.counter,
        at.id AS article_top_id,
        at.name AS article_top_name
    FROM article a
    INNER JOIN article_top at ON true  -- Assuming there's a relationship
    WHERE a.article_category_ids LIKE '%' || p_article_category_id::TEXT || '%'
      AND a.active = true
      AND a.start_date <= NOW()
      AND (a.end_date IS NULL OR a.end_date >= NOW())
    ORDER BY a.counter DESC, a.start_date DESC
    LIMIT 10;
END;
$$;

-- =============================================
-- Function: sp_article_comment_search
-- Description: Search article comments with pagination
-- =============================================
CREATE OR REPLACE FUNCTION sp_article_comment_search(
    p_keyword VARCHAR(4000) DEFAULT NULL,
    p_article_id INTEGER DEFAULT NULL,
    p_active BOOLEAN DEFAULT NULL,
    p_create_by VARCHAR(200) DEFAULT NULL,
    p_page_size INTEGER DEFAULT 20,
    p_current_page INTEGER DEFAULT 1,
    OUT p_item_count INTEGER
)
RETURNS TABLE (
    id INTEGER,
    article_id INTEGER,
    parent_id INTEGER,
    user_id VARCHAR(450),
    name VARCHAR(200),
    email VARCHAR(200),
    phone VARCHAR(50),
    content TEXT,
    active BOOLEAN,
    create_date TIMESTAMP,
    article_name VARCHAR(1000),
    user_name VARCHAR(256),
    row_num BIGINT
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_offset INTEGER;
BEGIN
    v_offset := (p_current_page - 1) * p_page_size;
    
    -- Get total count
    SELECT COUNT(*)
    INTO p_item_count
    FROM article_comment ac
    WHERE (p_keyword IS NULL OR ac.content ILIKE '%' || p_keyword || '%' OR ac.name ILIKE '%' || p_keyword || '%')
      AND (p_article_id IS NULL OR ac.article_id = p_article_id)
      AND (p_active IS NULL OR ac.active = p_active);

    -- Return paginated results
    RETURN QUERY
    SELECT 
        ac.id,
        ac.article_id,
        ac.parent_id,
        ac.user_id,
        ac.name,
        ac.email,
        ac.phone,
        ac.content,
        ac.active,
        ac.create_date,
        a.name AS article_name,
        u.user_name,
        ROW_NUMBER() OVER (ORDER BY ac.create_date DESC) AS row_num
    FROM article_comment ac
    LEFT JOIN article a ON ac.article_id = a.id
    LEFT JOIN asp_net_users u ON ac.user_id = u.id
    WHERE (p_keyword IS NULL OR ac.content ILIKE '%' || p_keyword || '%' OR ac.name ILIKE '%' || p_keyword || '%')
      AND (p_article_id IS NULL OR ac.article_id = p_article_id)
      AND (p_active IS NULL OR ac.active = p_active)
    ORDER BY ac.create_date DESC
    LIMIT p_page_size
    OFFSET v_offset;
END;
$$;

-- =============================================
-- Function: sp_article_comment_staff_search
-- Description: Search article staff comments with pagination
-- =============================================
CREATE OR REPLACE FUNCTION sp_article_comment_staff_search(
    p_keyword VARCHAR(4000) DEFAULT NULL,
    p_article_id INTEGER DEFAULT NULL,
    p_active BOOLEAN DEFAULT NULL,
    p_create_by VARCHAR(200) DEFAULT NULL,
    p_page_size INTEGER DEFAULT 20,
    p_current_page INTEGER DEFAULT 1,
    OUT p_item_count INTEGER
)
RETURNS TABLE (
    id INTEGER,
    article_comment_id INTEGER,
    user_id VARCHAR(450),
    content TEXT,
    active BOOLEAN,
    create_date TIMESTAMP,
    user_name VARCHAR(256),
    row_num BIGINT
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_offset INTEGER;
BEGIN
    v_offset := (p_current_page - 1) * p_page_size;
    
    -- Get total count
    SELECT COUNT(*)
    INTO p_item_count
    FROM article_comment_staff acs
    INNER JOIN article_comment ac ON acs.article_comment_id = ac.id
    WHERE (p_keyword IS NULL OR acs.content ILIKE '%' || p_keyword || '%')
      AND (p_article_id IS NULL OR ac.article_id = p_article_id)
      AND (p_active IS NULL OR acs.active = p_active);

    -- Return paginated results
    RETURN QUERY
    SELECT 
        acs.id,
        acs.article_comment_id,
        acs.user_id,
        acs.content,
        acs.active,
        acs.create_date,
        u.user_name,
        ROW_NUMBER() OVER (ORDER BY acs.create_date DESC) AS row_num
    FROM article_comment_staff acs
    INNER JOIN article_comment ac ON acs.article_comment_id = ac.id
    LEFT JOIN asp_net_users u ON acs.user_id = u.id
    WHERE (p_keyword IS NULL OR acs.content ILIKE '%' || p_keyword || '%')
      AND (p_article_id IS NULL OR ac.article_id = p_article_id)
      AND (p_active IS NULL OR acs.active = p_active)
    ORDER BY acs.create_date DESC
    LIMIT p_page_size
    OFFSET v_offset;
END;
$$;