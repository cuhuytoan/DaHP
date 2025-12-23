-- PostgreSQL Utility Functions
-- CMS Database Migration

-- =============================================
-- Function: sp_account_search
-- Description: Search user accounts with pagination
-- =============================================
CREATE OR REPLACE FUNCTION sp_account_search(
    p_keyword VARCHAR(4000) DEFAULT NULL,
    p_role_id UUID DEFAULT NULL,
    p_product_brand_id INTEGER DEFAULT NULL,
    p_active BOOLEAN DEFAULT NULL,
    p_page_size INTEGER DEFAULT 20,
    p_current_page INTEGER DEFAULT 1,
    OUT p_item_count INTEGER
)
RETURNS TABLE (
    id VARCHAR(450),
    user_name VARCHAR(256),
    email VARCHAR(256),
    phone_number TEXT,
    email_confirmed BOOLEAN,
    lockout_enabled BOOLEAN,
    lockout_end TIMESTAMPTZ,
    full_name VARCHAR(500),
    avatar VARCHAR(500),
    department_id INTEGER,
    active BOOLEAN,
    role_id VARCHAR(450),
    role_name VARCHAR(256),
    product_brand_id INTEGER,
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
    SELECT COUNT(DISTINCT u.id)
    INTO p_item_count
    FROM asp_net_users u
    LEFT JOIN asp_net_user_profiles up ON u.id = up.user_id
    LEFT JOIN asp_net_user_roles ur ON u.id = ur.user_id
    LEFT JOIN product_brand_model_management pbmm ON u.id = pbmm.user_id
    WHERE (p_keyword IS NULL OR u.user_name ILIKE '%' || p_keyword || '%' OR u.email ILIKE '%' || p_keyword || '%' OR up.full_name ILIKE '%' || p_keyword || '%')
      AND (p_role_id IS NULL OR ur.role_id = p_role_id::TEXT)
      AND (p_product_brand_id IS NULL OR pbmm.product_brand_id = p_product_brand_id)
      AND (p_active IS NULL OR up.active = p_active);

    -- Return paginated results
    RETURN QUERY
    SELECT DISTINCT
        u.id,
        u.user_name,
        u.email,
        u.phone_number,
        u.email_confirmed,
        u.lockout_enabled,
        u.lockout_end,
        up.full_name,
        up.avatar,
        up.department_id,
        up.active,
        ur.role_id,
        r.name AS role_name,
        pbmm.product_brand_id,
        pb.name AS product_brand_name,
        ROW_NUMBER() OVER (ORDER BY u.user_name) AS row_num
    FROM asp_net_users u
    LEFT JOIN asp_net_user_profiles up ON u.id = up.user_id
    LEFT JOIN asp_net_user_roles ur ON u.id = ur.user_id
    LEFT JOIN asp_net_roles r ON ur.role_id = r.id
    LEFT JOIN product_brand_model_management pbmm ON u.id = pbmm.user_id
    LEFT JOIN product_brand pb ON pbmm.product_brand_id = pb.id
    WHERE (p_keyword IS NULL OR u.user_name ILIKE '%' || p_keyword || '%' OR u.email ILIKE '%' || p_keyword || '%' OR up.full_name ILIKE '%' || p_keyword || '%')
      AND (p_role_id IS NULL OR ur.role_id = p_role_id::TEXT)
      AND (p_product_brand_id IS NULL OR pbmm.product_brand_id = p_product_brand_id)
      AND (p_active IS NULL OR up.active = p_active)
    ORDER BY u.user_name
    LIMIT p_page_size
    OFFSET v_offset;
END;
$$;

-- =============================================
-- Function: sp_log_visit_search
-- Description: Search log visits with pagination
-- =============================================
CREATE OR REPLACE FUNCTION sp_log_visit_search(
    p_page_size INTEGER DEFAULT 20,
    p_current_page INTEGER DEFAULT 1,
    OUT p_item_count INTEGER
)
RETURNS TABLE (
    id INTEGER,
    user_id VARCHAR(450),
    ip_address VARCHAR(50),
    user_agent TEXT,
    url VARCHAR(1000),
    referrer VARCHAR(1000),
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
    FROM log_visit;

    -- Return paginated results
    RETURN QUERY
    SELECT 
        lv.id,
        lv.user_id,
        lv.ip_address,
        lv.user_agent,
        lv.url,
        lv.referrer,
        lv.create_date,
        u.user_name,
        ROW_NUMBER() OVER (ORDER BY lv.create_date DESC) AS row_num
    FROM log_visit lv
    LEFT JOIN asp_net_users u ON lv.user_id = u.id
    ORDER BY lv.create_date DESC
    LIMIT p_page_size
    OFFSET v_offset;
END;
$$;

-- =============================================
-- Function: sp_user_notify_search
-- Description: Search user notifications with pagination
-- =============================================
CREATE OR REPLACE FUNCTION sp_user_notify_search(
    p_user_notify_type_id INTEGER DEFAULT NULL,
    p_asp_net_users_id UUID DEFAULT NULL,
    p_readed BOOLEAN DEFAULT NULL,
    p_page_size INTEGER DEFAULT 20,
    p_current_page INTEGER DEFAULT 1,
    OUT p_item_count INTEGER
)
RETURNS TABLE (
    id INTEGER,
    user_id VARCHAR(450),
    title VARCHAR(500),
    content TEXT,
    url VARCHAR(1000),
    is_read BOOLEAN,
    create_date TIMESTAMP,
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
    FROM user_notify un
    WHERE (p_asp_net_users_id IS NULL OR un.user_id = p_asp_net_users_id::TEXT)
      AND (p_readed IS NULL OR un.is_read = p_readed);

    -- Return paginated results
    RETURN QUERY
    SELECT 
        un.id,
        un.user_id,
        un.title,
        un.content,
        un.url,
        un.is_read,
        un.create_date,
        ROW_NUMBER() OVER (ORDER BY un.create_date DESC) AS row_num
    FROM user_notify un
    WHERE (p_asp_net_users_id IS NULL OR un.user_id = p_asp_net_users_id::TEXT)
      AND (p_readed IS NULL OR un.is_read = p_readed)
    ORDER BY un.create_date DESC
    LIMIT p_page_size
    OFFSET v_offset;
END;
$$;

-- =============================================
-- Function: fn_generate_url_slug
-- Description: Generate URL-friendly slug from text
-- =============================================
CREATE OR REPLACE FUNCTION fn_generate_url_slug(
    p_text VARCHAR(1000)
)
RETURNS VARCHAR(1000)
LANGUAGE plpgsql
AS $$
DECLARE
    v_result VARCHAR(1000);
BEGIN
    -- Convert to lowercase
    v_result := LOWER(p_text);
    
    -- Replace Vietnamese characters
    v_result := TRANSLATE(v_result, 
        'àáạảãâầấậẩẫăằắặẳẵèéẹẻẽêềếệểễìíịỉĩòóọỏõôồốộổỗơờớợởỡùúụủũưừứựửữỳýỵỷỹđ',
        'aaaaaaaaaaaaaaaaaeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyd');
    
    -- Replace special characters with hyphen
    v_result := REGEXP_REPLACE(v_result, '[^a-z0-9\-]', '-', 'g');
    
    -- Replace multiple hyphens with single hyphen
    v_result := REGEXP_REPLACE(v_result, '-+', '-', 'g');
    
    -- Remove leading and trailing hyphens
    v_result := TRIM(BOTH '-' FROM v_result);
    
    RETURN v_result;
END;
$$;

-- =============================================
-- Function: fn_get_category_children_ids
-- Description: Get all child category IDs recursively
-- =============================================
CREATE OR REPLACE FUNCTION fn_get_article_category_children_ids(
    p_parent_id INTEGER
)
RETURNS TABLE (id INTEGER)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    WITH RECURSIVE category_tree AS (
        SELECT ac.id
        FROM article_category ac
        WHERE ac.id = p_parent_id
        
        UNION ALL
        
        SELECT ac.id
        FROM article_category ac
        INNER JOIN category_tree ct ON ac.parent_id = ct.id
    )
    SELECT ct.id FROM category_tree ct;
END;
$$;

CREATE OR REPLACE FUNCTION fn_get_product_category_children_ids(
    p_parent_id INTEGER
)
RETURNS TABLE (id INTEGER)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    WITH RECURSIVE category_tree AS (
        SELECT pc.id
        FROM product_category pc
        WHERE pc.id = p_parent_id
        
        UNION ALL
        
        SELECT pc.id
        FROM product_category pc
        INNER JOIN category_tree ct ON pc.parent_id = ct.id
    )
    SELECT ct.id FROM category_tree ct;
END;
$$;

-- =============================================
-- Function: fn_update_article_counter
-- Description: Increment article view counter
-- =============================================
CREATE OR REPLACE FUNCTION fn_update_article_counter(
    p_article_id INTEGER
)
RETURNS VOID
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE article
    SET counter = COALESCE(counter, 0) + 1
    WHERE id = p_article_id;
END;
$$;

-- =============================================
-- Function: fn_update_product_counter
-- Description: Increment product view counter
-- =============================================
CREATE OR REPLACE FUNCTION fn_update_product_counter(
    p_product_id INTEGER
)
RETURNS VOID
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE product
    SET counter = COALESCE(counter, 0) + 1
    WHERE id = p_product_id;
END;
$$;

-- =============================================
-- Function: fn_calculate_product_rating
-- Description: Calculate average product rating
-- =============================================
CREATE OR REPLACE FUNCTION fn_calculate_product_rating(
    p_product_id INTEGER
)
RETURNS INTEGER
LANGUAGE plpgsql
AS $$
DECLARE
    v_avg_rating NUMERIC;
BEGIN
    SELECT AVG(rate)
    INTO v_avg_rating
    FROM product_review
    WHERE product_id = p_product_id
      AND active = true;
    
    RETURN COALESCE(ROUND(v_avg_rating), 0);
END;
$$;

-- =============================================
-- Trigger: Update product rating on review change
-- =============================================
CREATE OR REPLACE FUNCTION trg_update_product_rating()
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
    IF TG_OP = 'DELETE' THEN
        UPDATE product
        SET rate = fn_calculate_product_rating(OLD.product_id)
        WHERE id = OLD.product_id;
        RETURN OLD;
    ELSE
        UPDATE product
        SET rate = fn_calculate_product_rating(NEW.product_id)
        WHERE id = NEW.product_id;
        RETURN NEW;
    END IF;
END;
$$;

DROP TRIGGER IF EXISTS trg_product_review_rating ON product_review;
CREATE TRIGGER trg_product_review_rating
AFTER INSERT OR UPDATE OR DELETE ON product_review
FOR EACH ROW
EXECUTE FUNCTION trg_update_product_rating();

-- =============================================
-- Function: fn_generate_order_code
-- Description: Generate unique order code
-- =============================================
CREATE OR REPLACE FUNCTION fn_generate_order_code()
RETURNS VARCHAR(50)
LANGUAGE plpgsql
AS $$
DECLARE
    v_code VARCHAR(50);
    v_exists BOOLEAN;
BEGIN
    LOOP
        -- Generate code: DH + YYYYMMDD + 6 random digits
        v_code := 'DH' || TO_CHAR(NOW(), 'YYYYMMDD') || LPAD(FLOOR(RANDOM() * 1000000)::TEXT, 6, '0');
        
        -- Check if code exists
        SELECT EXISTS(SELECT 1 FROM product_order WHERE order_code = v_code) INTO v_exists;
        
        EXIT WHEN NOT v_exists;
    END LOOP;
    
    RETURN v_code;
END;
$$;

-- =============================================
-- Trigger: Auto-generate order code
-- =============================================
CREATE OR REPLACE FUNCTION trg_generate_order_code()
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
    IF NEW.order_code IS NULL OR NEW.order_code = '' THEN
        NEW.order_code := fn_generate_order_code();
    END IF;
    RETURN NEW;
END;
$$;

DROP TRIGGER IF EXISTS trg_product_order_code ON product_order;
CREATE TRIGGER trg_product_order_code
BEFORE INSERT ON product_order
FOR EACH ROW
EXECUTE FUNCTION trg_generate_order_code();

-- =============================================
-- Function: fn_update_order_totals
-- Description: Recalculate order totals
-- =============================================
CREATE OR REPLACE FUNCTION fn_update_order_totals(
    p_order_id INTEGER
)
RETURNS VOID
LANGUAGE plpgsql
AS $$
DECLARE
    v_sub_total DECIMAL(19,4);
BEGIN
    -- Calculate subtotal from order details
    SELECT COALESCE(SUM(total), 0)
    INTO v_sub_total
    FROM product_order_detail
    WHERE product_order_id = p_order_id;
    
    -- Update order totals
    UPDATE product_order
    SET sub_total = v_sub_total,
        total = v_sub_total + COALESCE(shipping_fee, 0) - COALESCE(discount, 0)
    WHERE id = p_order_id;
END;
$$;

-- =============================================
-- Trigger: Update order totals on detail change
-- =============================================
CREATE OR REPLACE FUNCTION trg_update_order_totals()
RETURNS TRIGGER
LANGUAGE plpgsql
AS $$
BEGIN
    IF TG_OP = 'DELETE' THEN
        PERFORM fn_update_order_totals(OLD.product_order_id);
        RETURN OLD;
    ELSE
        PERFORM fn_update_order_totals(NEW.product_order_id);
        RETURN NEW;
    END IF;
END;
$$;

DROP TRIGGER IF EXISTS trg_order_detail_totals ON product_order_detail;
CREATE TRIGGER trg_order_detail_totals
AFTER INSERT OR UPDATE OR DELETE ON product_order_detail
FOR EACH ROW
EXECUTE FUNCTION trg_update_order_totals();