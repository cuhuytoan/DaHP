-- PostgreSQL Functions for Product Management
-- Converted from SQL Server Stored Procedures
-- CMS Database Migration

-- =============================================
-- Function: sp_product_search
-- Description: Search products with pagination
-- =============================================
CREATE OR REPLACE FUNCTION sp_product_search(
    p_keyword VARCHAR(4000) DEFAULT NULL,
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
    p_efficiency BOOLEAN DEFAULT NULL,
    p_active BOOLEAN DEFAULT NULL,
    p_assign_by VARCHAR(900) DEFAULT NULL,
    p_create_by VARCHAR(900) DEFAULT NULL,
    p_order_by VARCHAR(200) DEFAULT 'create_date_desc',
    p_page_size INTEGER DEFAULT 20,
    p_current_page INTEGER DEFAULT 1,
    OUT p_item_count INTEGER
)
RETURNS TABLE (
    id INTEGER,
    product_type_id INTEGER,
    product_category_ids VARCHAR(200),
    product_manufacture_id INTEGER,
    product_brand_id INTEGER,
    country_id INTEGER,
    product_status_id INTEGER,
    name VARCHAR(1000),
    bar_code VARCHAR(50),
    sku VARCHAR(100),
    sub_title VARCHAR(200),
    image VARCHAR(200),
    description TEXT,
    price DECIMAL(19,4),
    price_old DECIMAL(19,4),
    discount DECIMAL(19,4),
    discount_rate INTEGER,
    is_new BOOLEAN,
    is_best_sale BOOLEAN,
    is_sale_off BOOLEAN,
    is_out_stock BOOLEAN,
    start_date TIMESTAMP,
    end_date TIMESTAMP,
    active BOOLEAN,
    counter INTEGER,
    sell_count INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    last_edit_by VARCHAR(450),
    last_edit_date TIMESTAMP,
    checked INTEGER,
    approved INTEGER,
    url VARCHAR(1000),
    product_status_name VARCHAR(200),
    product_type_name VARCHAR(200),
    product_brand_name VARCHAR(1000),
    product_manufacture_name VARCHAR(500),
    country_name VARCHAR(200),
    row_num BIGINT
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_offset INTEGER;
    v_order_clause TEXT;
BEGIN
    -- Calculate offset
    v_offset := (p_current_page - 1) * p_page_size;
    
    -- Get total count
    SELECT COUNT(*)
    INTO p_item_count
    FROM product p
    WHERE (p_keyword IS NULL OR p.name ILIKE '%' || p_keyword || '%' OR p.description ILIKE '%' || p_keyword || '%' OR p.sku ILIKE '%' || p_keyword || '%')
      AND (p_product_category_id IS NULL OR p.product_category_ids LIKE '%' || p_product_category_id::TEXT || '%')
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
      AND (p_create_by IS NULL OR p.create_by = p_create_by)
      AND (p_efficiency IS NULL OR (p_efficiency = true AND p.start_date <= NOW() AND (p.end_date IS NULL OR p.end_date >= NOW())));

    -- Return paginated results with dynamic ordering
    RETURN QUERY
    SELECT 
        p.id,
        p.product_type_id,
        p.product_category_ids,
        p.product_manufacture_id,
        p.product_brand_id,
        p.country_id,
        p.product_status_id,
        p.name,
        p.bar_code,
        p.sku,
        p.sub_title,
        p.image,
        p.description,
        p.price,
        p.price_old,
        p.discount,
        p.discount_rate,
        p.is_new,
        p.is_best_sale,
        p.is_sale_off,
        p.is_out_stock,
        p.start_date,
        p.end_date,
        p.active,
        p.counter,
        p.sell_count,
        p.create_by,
        p.create_date,
        p.last_edit_by,
        p.last_edit_date,
        p.checked,
        p.approved,
        p.url,
        ps.name AS product_status_name,
        pt.name AS product_type_name,
        pb.name AS product_brand_name,
        pm.name AS product_manufacture_name,
        c.name AS country_name,
        ROW_NUMBER() OVER (
            ORDER BY 
                CASE WHEN p_order_by = 'price_asc' THEN p.price END ASC,
                CASE WHEN p_order_by = 'price_desc' THEN p.price END DESC,
                CASE WHEN p_order_by = 'name_asc' THEN p.name END ASC,
                CASE WHEN p_order_by = 'name_desc' THEN p.name END DESC,
                CASE WHEN p_order_by = 'sell_count_desc' THEN p.sell_count END DESC,
                CASE WHEN p_order_by = 'counter_desc' THEN p.counter END DESC,
                CASE WHEN p_order_by = 'create_date_asc' THEN p.create_date END ASC,
                p.create_date DESC
        ) AS row_num
    FROM product p
    LEFT JOIN product_status ps ON p.product_status_id = ps.id
    LEFT JOIN product_type pt ON p.product_type_id = pt.id
    LEFT JOIN product_brand pb ON p.product_brand_id = pb.id
    LEFT JOIN product_manufacture pm ON p.product_manufacture_id = pm.id
    LEFT JOIN country c ON p.country_id = c.id
    WHERE (p_keyword IS NULL OR p.name ILIKE '%' || p_keyword || '%' OR p.description ILIKE '%' || p_keyword || '%' OR p.sku ILIKE '%' || p_keyword || '%')
      AND (p_product_category_id IS NULL OR p.product_category_ids LIKE '%' || p_product_category_id::TEXT || '%')
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
      AND (p_create_by IS NULL OR p.create_by = p_create_by)
      AND (p_efficiency IS NULL OR (p_efficiency = true AND p.start_date <= NOW() AND (p.end_date IS NULL OR p.end_date >= NOW())))
    ORDER BY 
        CASE WHEN p_order_by = 'price_asc' THEN p.price END ASC,
        CASE WHEN p_order_by = 'price_desc' THEN p.price END DESC,
        CASE WHEN p_order_by = 'name_asc' THEN p.name END ASC,
        CASE WHEN p_order_by = 'name_desc' THEN p.name END DESC,
        CASE WHEN p_order_by = 'sell_count_desc' THEN p.sell_count END DESC,
        CASE WHEN p_order_by = 'counter_desc' THEN p.counter END DESC,
        CASE WHEN p_order_by = 'create_date_asc' THEN p.create_date END ASC,
        p.create_date DESC
    LIMIT p_page_size
    OFFSET v_offset;
END;
$$;

-- =============================================
-- Function: sp_product_breadcrumb
-- Description: Get product category breadcrumb
-- =============================================
CREATE OR REPLACE FUNCTION sp_product_breadcrumb(
    p_product_category_id INTEGER
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
            pc.id,
            pc.parent_id,
            pc.name,
            pc.url,
            1 AS level
        FROM product_category pc
        WHERE pc.id = p_product_category_id
        
        UNION ALL
        
        -- Recursive case: get parent categories
        SELECT 
            pc.id,
            pc.parent_id,
            pc.name,
            pc.url,
            ct.level + 1
        FROM product_category pc
        INNER JOIN category_tree ct ON pc.id = ct.parent_id
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
-- Function: sp_product_category_tree
-- Description: Get product category tree structure
-- =============================================
CREATE OR REPLACE FUNCTION sp_product_category_tree(
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
            pc.id,
            pc.parent_id,
            pc.name,
            pc.url,
            0 AS level,
            pc.name::VARCHAR(1000) AS display_name,
            pc.sort,
            pc.active
        FROM product_category pc
        WHERE (p_root IS NULL AND pc.parent_id IS NULL) OR pc.id = p_root
        
        UNION ALL
        
        -- Recursive case: child categories
        SELECT 
            pc.id,
            pc.parent_id,
            pc.name,
            pc.url,
            ct.level + 1,
            (REPEAT('--- ', ct.level + 1) || pc.name)::VARCHAR(1000) AS display_name,
            pc.sort,
            pc.active
        FROM product_category pc
        INNER JOIN category_tree ct ON pc.parent_id = ct.id
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
-- Function: sp_product_property_category_tree
-- Description: Get product property category tree structure
-- =============================================
CREATE OR REPLACE FUNCTION sp_product_property_category_tree(
    p_root INTEGER DEFAULT NULL,
    p_first_line VARCHAR(400) DEFAULT NULL
)
RETURNS TABLE (
    id INTEGER,
    parent_id INTEGER,
    product_category_id INTEGER,
    name VARCHAR(500),
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
            ppc.id,
            ppc.parent_id,
            ppc.product_category_id,
            ppc.name,
            0 AS level,
            ppc.name::VARCHAR(1000) AS display_name,
            ppc.sort,
            ppc.active
        FROM product_property_category ppc
        WHERE (p_root IS NULL AND ppc.parent_id IS NULL) OR ppc.id = p_root
        
        UNION ALL
        
        -- Recursive case: child categories
        SELECT 
            ppc.id,
            ppc.parent_id,
            ppc.product_category_id,
            ppc.name,
            ct.level + 1,
            (REPEAT('--- ', ct.level + 1) || ppc.name)::VARCHAR(1000) AS display_name,
            ppc.sort,
            ppc.active
        FROM product_property_category ppc
        INNER JOIN category_tree ct ON ppc.parent_id = ct.id
    )
    SELECT 
        ct.id,
        ct.parent_id,
        ct.product_category_id,
        ct.name,
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
-- Function: sp_get_lst_product_properties
-- Description: Get product properties list
-- =============================================
CREATE OR REPLACE FUNCTION sp_get_lst_product_properties(
    p_product_id INTEGER,
    p_product_property_category INTEGER DEFAULT NULL
)
RETURNS TABLE (
    id INTEGER,
    product_id INTEGER,
    product_property_category_id INTEGER,
    product_property_type_id INTEGER,
    name VARCHAR(500),
    value VARCHAR(500),
    sort INTEGER,
    property_category_name VARCHAR(500),
    property_type_name VARCHAR(200)
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT 
        pp.id,
        pp.product_id,
        pp.product_property_category_id,
        pp.product_property_type_id,
        pp.name,
        pp.value,
        pp.sort,
        ppc.name AS property_category_name,
        ppt.name AS property_type_name
    FROM product_property pp
    LEFT JOIN product_property_category ppc ON pp.product_property_category_id = ppc.id
    LEFT JOIN product_property_type ppt ON pp.product_property_type_id = ppt.id
    WHERE pp.product_id = p_product_id
      AND (p_product_property_category IS NULL OR pp.product_property_category_id = p_product_property_category)
    ORDER BY pp.sort, pp.name;
END;
$$;

-- =============================================
-- Function: sp_product_comment_search
-- Description: Search product comments with pagination
-- =============================================
CREATE OR REPLACE FUNCTION sp_product_comment_search(
    p_keyword VARCHAR(4000) DEFAULT NULL,
    p_product_id INTEGER DEFAULT NULL,
    p_active BOOLEAN DEFAULT NULL,
    p_create_by VARCHAR(200) DEFAULT NULL,
    p_page_size INTEGER DEFAULT 20,
    p_current_page INTEGER DEFAULT 1,
    OUT p_item_count INTEGER
)
RETURNS TABLE (
    id INTEGER,
    product_id INTEGER,
    parent_id INTEGER,
    user_id VARCHAR(450),
    name VARCHAR(200),
    email VARCHAR(200),
    phone VARCHAR(50),
    content TEXT,
    active BOOLEAN,
    create_date TIMESTAMP,
    product_name VARCHAR(1000),
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
    FROM product_comment pc
    WHERE (p_keyword IS NULL OR pc.content ILIKE '%' || p_keyword || '%' OR pc.name ILIKE '%' || p_keyword || '%')
      AND (p_product_id IS NULL OR pc.product_id = p_product_id)
      AND (p_active IS NULL OR pc.active = p_active);

    -- Return paginated results
    RETURN QUERY
    SELECT 
        pc.id,
        pc.product_id,
        pc.parent_id,
        pc.user_id,
        pc.name,
        pc.email,
        pc.phone,
        pc.content,
        pc.active,
        pc.create_date,
        p.name AS product_name,
        u.user_name,
        ROW_NUMBER() OVER (ORDER BY pc.create_date DESC) AS row_num
    FROM product_comment pc
    LEFT JOIN product p ON pc.product_id = p.id
    LEFT JOIN asp_net_users u ON pc.user_id = u.id
    WHERE (p_keyword IS NULL OR pc.content ILIKE '%' || p_keyword || '%' OR pc.name ILIKE '%' || p_keyword || '%')
      AND (p_product_id IS NULL OR pc.product_id = p_product_id)
      AND (p_active IS NULL OR pc.active = p_active)
    ORDER BY pc.create_date DESC
    LIMIT p_page_size
    OFFSET v_offset;
END;
$$;

-- =============================================
-- Function: sp_product_comment_staff_search
-- Description: Search product staff comments with pagination
-- =============================================
CREATE OR REPLACE FUNCTION sp_product_comment_staff_search(
    p_keyword VARCHAR(4000) DEFAULT NULL,
    p_product_id INTEGER DEFAULT NULL,
    p_active BOOLEAN DEFAULT NULL,
    p_create_by VARCHAR(200) DEFAULT NULL,
    p_page_size INTEGER DEFAULT 20,
    p_current_page INTEGER DEFAULT 1,
    OUT p_item_count INTEGER
)
RETURNS TABLE (
    id INTEGER,
    product_comment_id INTEGER,
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
    FROM product_comment_staff pcs
    INNER JOIN product_comment pc ON pcs.product_comment_id = pc.id
    WHERE (p_keyword IS NULL OR pcs.content ILIKE '%' || p_keyword || '%')
      AND (p_product_id IS NULL OR pc.product_id = p_product_id)
      AND (p_active IS NULL OR pcs.active = p_active);

    -- Return paginated results
    RETURN QUERY
    SELECT 
        pcs.id,
        pcs.product_comment_id,
        pcs.user_id,
        pcs.content,
        pcs.active,
        pcs.create_date,
        u.user_name,
        ROW_NUMBER() OVER (ORDER BY pcs.create_date DESC) AS row_num
    FROM product_comment_staff pcs
    INNER JOIN product_comment pc ON pcs.product_comment_id = pc.id
    LEFT JOIN asp_net_users u ON pcs.user_id = u.id
    WHERE (p_keyword IS NULL OR pcs.content ILIKE '%' || p_keyword || '%')
      AND (p_product_id IS NULL OR pc.product_id = p_product_id)
      AND (p_active IS NULL OR pcs.active = p_active)
    ORDER BY pcs.create_date DESC
    LIMIT p_page_size
    OFFSET v_offset;
END;
$$;

-- =============================================
-- Function: sp_product_review_search
-- Description: Search product reviews with pagination
-- =============================================
CREATE OR REPLACE FUNCTION sp_product_review_search(
    p_keyword VARCHAR(4000) DEFAULT NULL,
    p_product_review_type_id INTEGER DEFAULT NULL,
    p_location_id INTEGER DEFAULT NULL,
    p_department_man_id INTEGER DEFAULT NULL,
    p_product_brand_id INTEGER DEFAULT NULL,
    p_product_id INTEGER DEFAULT NULL,
    p_active BOOLEAN DEFAULT NULL,
    p_from_date TIMESTAMP DEFAULT NULL,
    p_to_date TIMESTAMP DEFAULT NULL,
    p_create_by UUID DEFAULT NULL,
    p_page_size INTEGER DEFAULT 20,
    p_current_page INTEGER DEFAULT 1,
    OUT p_item_count INTEGER
)
RETURNS TABLE (
    id INTEGER,
    product_id INTEGER,
    user_id VARCHAR(450),
    name VARCHAR(200),
    email VARCHAR(200),
    phone VARCHAR(50),
    title VARCHAR(500),
    content TEXT,
    rate INTEGER,
    active BOOLEAN,
    create_date TIMESTAMP,
    product_name VARCHAR(1000),
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
    FROM product_review pr
    LEFT JOIN product p ON pr.product_id = p.id
    WHERE (p_keyword IS NULL OR pr.content ILIKE '%' || p_keyword || '%' OR pr.title ILIKE '%' || p_keyword || '%')
      AND (p_product_id IS NULL OR pr.product_id = p_product_id)
      AND (p_product_brand_id IS NULL OR p.product_brand_id = p_product_brand_id)
      AND (p_active IS NULL OR pr.active = p_active)
      AND (p_from_date IS NULL OR pr.create_date >= p_from_date)
      AND (p_to_date IS NULL OR pr.create_date <= p_to_date);

    -- Return paginated results
    RETURN QUERY
    SELECT 
        pr.id,
        pr.product_id,
        pr.user_id,
        pr.name,
        pr.email,
        pr.phone,
        pr.title,
        pr.content,
        pr.rate,
        pr.active,
        pr.create_date,
        p.name AS product_name,
        u.user_name,
        ROW_NUMBER() OVER (ORDER BY pr.create_date DESC) AS row_num
    FROM product_review pr
    LEFT JOIN product p ON pr.product_id = p.id
    LEFT JOIN asp_net_users u ON pr.user_id = u.id
    WHERE (p_keyword IS NULL OR pr.content ILIKE '%' || p_keyword || '%' OR pr.title ILIKE '%' || p_keyword || '%')
      AND (p_product_id IS NULL OR pr.product_id = p_product_id)
      AND (p_product_brand_id IS NULL OR p.product_brand_id = p_product_brand_id)
      AND (p_active IS NULL OR pr.active = p_active)
      AND (p_from_date IS NULL OR pr.create_date >= p_from_date)
      AND (p_to_date IS NULL OR pr.create_date <= p_to_date)
    ORDER BY pr.create_date DESC
    LIMIT p_page_size
    OFFSET v_offset;
END;
$$;

-- =============================================
-- Function: sp_product_brand_search
-- Description: Search product brands with pagination
-- =============================================
CREATE OR REPLACE FUNCTION sp_product_brand_search(
    p_keyword VARCHAR(4000) DEFAULT NULL,
    p_product_brand_category_id INTEGER DEFAULT NULL,
    p_product_brand_status_id INTEGER DEFAULT NULL,
    p_product_brand_type_id INTEGER DEFAULT NULL,
    p_department_man_id INTEGER DEFAULT NULL,
    p_country_id INTEGER DEFAULT NULL,
    p_location_id INTEGER DEFAULT NULL,
    p_district_id INTEGER DEFAULT NULL,
    p_ward_id INTEGER DEFAULT NULL,
    p_exception_id INTEGER DEFAULT NULL,
    p_from_date TIMESTAMP DEFAULT NULL,
    p_to_date TIMESTAMP DEFAULT NULL,
    p_efficiency BOOLEAN DEFAULT NULL,
    p_active BOOLEAN DEFAULT NULL,
    p_create_by VARCHAR(900) DEFAULT NULL,
    p_page_size INTEGER DEFAULT 20,
    p_current_page INTEGER DEFAULT 1,
    OUT p_item_count INTEGER
)
RETURNS TABLE (
    id INTEGER,
    product_brand_type_id INTEGER,
    product_brand_status_id INTEGER,
    product_brand_level_id INTEGER,
    name VARCHAR(1000),
    code VARCHAR(100),
    sub_title VARCHAR(200),
    image VARCHAR(200),
    description TEXT,
    location_id INTEGER,
    district_id INTEGER,
    ward_id INTEGER,
    address VARCHAR(1000),
    phone VARCHAR(100),
    email VARCHAR(200),
    website VARCHAR(500),
    start_date TIMESTAMP,
    end_date TIMESTAMP,
    active BOOLEAN,
    counter INTEGER,
    view_count INTEGER,
    sell_count INTEGER,
    create_by VARCHAR(450),
    create_date TIMESTAMP,
    approved INTEGER,
    url VARCHAR(1000),
    product_brand_status_name VARCHAR(200),
    product_brand_type_name VARCHAR(200),
    location_name VARCHAR(200),
    district_name VARCHAR(200),
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
    FROM product_brand pb
    WHERE (p_keyword IS NULL OR pb.name ILIKE '%' || p_keyword || '%' OR pb.description ILIKE '%' || p_keyword || '%')
      AND (p_product_brand_status_id IS NULL OR pb.product_brand_status_id = p_product_brand_status_id)
      AND (p_product_brand_type_id IS NULL OR pb.product_brand_type_id = p_product_brand_type_id)
      AND (p_location_id IS NULL OR pb.location_id = p_location_id)
      AND (p_district_id IS NULL OR pb.district_id = p_district_id)
      AND (p_ward_id IS NULL OR pb.ward_id = p_ward_id)
      AND (p_exception_id IS NULL OR pb.id != p_exception_id)
      AND (p_from_date IS NULL OR pb.create_date >= p_from_date)
      AND (p_to_date IS NULL OR pb.create_date <= p_to_date)
      AND (p_active IS NULL OR pb.active = p_active)
      AND (p_create_by IS NULL OR pb.create_by = p_create_by)
      AND (p_efficiency IS NULL OR (p_efficiency = true AND pb.start_date <= NOW() AND (pb.end_date IS NULL OR pb.end_date >= NOW())));

    -- Return paginated results
    RETURN QUERY
    SELECT 
        pb.id,
        pb.product_brand_type_id,
        pb.product_brand_status_id,
        pb.product_brand_level_id,
        pb.name,
        pb.code,
        pb.sub_title,
        pb.image,
        pb.description,
        pb.location_id,
        pb.district_id,
        pb.ward_id,
        pb.address,
        pb.phone,
        pb.email,
        pb.website,
        pb.start_date,
        pb.end_date,
        pb.active,
        pb.counter,
        pb.view_count,
        pb.sell_count,
        pb.create_by,
        pb.create_date,
        pb.approved,
        pb.url,
        pbs.name AS product_brand_status_name,
        pbt.name AS product_brand_type_name,
        l.name AS location_name,
        d.name AS district_name,
        ROW_NUMBER() OVER (ORDER BY pb.create_date DESC) AS row_num
    FROM product_brand pb
    LEFT JOIN product_brand_status pbs ON pb.product_brand_status_id = pbs.id
    LEFT JOIN product_brand_type pbt ON pb.product_brand_type_id = pbt.id
    LEFT JOIN location l ON pb.location_id = l.id
    LEFT JOIN district d ON pb.district_id = d.id
    WHERE (p_keyword IS NULL OR pb.name ILIKE '%' || p_keyword || '%' OR pb.description ILIKE '%' || p_keyword || '%')
      AND (p_product_brand_status_id IS NULL OR pb.product_brand_status_id = p_product_brand_status_id)
      AND (p_product_brand_type_id IS NULL OR pb.product_brand_type_id = p_product_brand_type_id)
      AND (p_location_id IS NULL OR pb.location_id = p_location_id)
      AND (p_district_id IS NULL OR pb.district_id = p_district_id)
      AND (p_ward_id IS NULL OR pb.ward_id = p_ward_id)
      AND (p_exception_id IS NULL OR pb.id != p_exception_id)
      AND (p_from_date IS NULL OR pb.create_date >= p_from_date)
      AND (p_to_date IS NULL OR pb.create_date <= p_to_date)
      AND (p_active IS NULL OR pb.active = p_active)
      AND (p_create_by IS NULL OR pb.create_by = p_create_by)
      AND (p_efficiency IS NULL OR (p_efficiency = true AND pb.start_date <= NOW() AND (pb.end_date IS NULL OR pb.end_date >= NOW())))
    ORDER BY pb.create_date DESC
    LIMIT p_page_size
    OFFSET v_offset;
END;
$$;

-- =============================================
-- Function: sp_product_order_search
-- Description: Search product orders with pagination
-- =============================================
CREATE OR REPLACE FUNCTION sp_product_order_search(
    p_product_order_id INTEGER DEFAULT NULL,
    p_create_by VARCHAR(900) DEFAULT NULL,
    p_product_order_status_id INTEGER DEFAULT NULL,
    p_order_by VARCHAR(200) DEFAULT 'create_date_desc',
    p_page_size INTEGER DEFAULT 20,
    p_current_page INTEGER DEFAULT 1,
    OUT p_item_count INTEGER
)
RETURNS TABLE (
    id INTEGER,
    order_code VARCHAR(50),
    product_brand_id INTEGER,
    product_order_status_id INTEGER,
    product_order_payment_status_id INTEGER,
    product_order_payment_method_id INTEGER,
    user_id VARCHAR(450),
    customer_name VARCHAR(200),
    customer_email VARCHAR(200),
    customer_phone VARCHAR(50),
    customer_address VARCHAR(1000),
    shipping_fee DECIMAL(19,4),
    sub_total DECIMAL(19,4),
    discount DECIMAL(19,4),
    total DECIMAL(19,4),
    note TEXT,
    create_date TIMESTAMP,
    product_order_status_name VARCHAR(200),
    product_order_payment_status_name VARCHAR(200),
    product_order_payment_method_name VARCHAR(200),
    product_brand_name VARCHAR(1000),
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
    FROM product_order po
    WHERE (p_product_order_id IS NULL OR po.id = p_product_order_id)
      AND (p_create_by IS NULL OR po.user_id = p_create_by)
      AND (p_product_order_status_id IS NULL OR po.product_order_status_id = p_product_order_status_id);

    -- Return paginated results
    RETURN QUERY
    SELECT 
        po.id,
        po.order_code,
        po.product_brand_id,
        po.product_order_status_id,
        po.product_order_payment_status_id,
        po.product_order_payment_method_id,
        po.user_id,
        po.customer_name,
        po.customer_email,
        po.customer_phone,
        po.customer_address,
        po.shipping_fee,
        po.sub_total,
        po.discount,
        po.total,
        po.note,
        po.create_date,
        pos.name AS product_order_status_name,
        pops.name AS product_order_payment_status_name,
        popm.name AS product_order_payment_method_name,
        pb.name AS product_brand_name,
        ROW_NUMBER() OVER (
            ORDER BY 
                CASE WHEN p_order_by = 'total_asc' THEN po.total END ASC,
                CASE WHEN p_order_by = 'total_desc' THEN po.total END DESC,
                CASE WHEN p_order_by = 'create_date_asc' THEN po.create_date END ASC,
                po.create_date DESC
        ) AS row_num
    FROM product_order po
    LEFT JOIN product_order_status pos ON po.product_order_status_id = pos.id
    LEFT JOIN product_order_payment_status pops ON po.product_order_payment_status_id = pops.id
    LEFT JOIN product_order_payment_method popm ON po.product_order_payment_method_id = popm.id
    LEFT JOIN product_brand pb ON po.product_brand_id = pb.id
    WHERE (p_product_order_id IS NULL OR po.id = p_product_order_id)
      AND (p_create_by IS NULL OR po.user_id = p_create_by)
      AND (p_product_order_status_id IS NULL OR po.product_order_status_id = p_product_order_status_id)
    ORDER BY 
        CASE WHEN p_order_by = 'total_asc' THEN po.total END ASC,
        CASE WHEN p_order_by = 'total_desc' THEN po.total END DESC,
        CASE WHEN p_order_by = 'create_date_asc' THEN po.create_date END ASC,
        po.create_date DESC
    LIMIT p_page_size
    OFFSET v_offset;
END;
$$;