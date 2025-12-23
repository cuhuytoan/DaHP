-- PostgreSQL Seed Data
-- CMS Database Migration - Initial Data
-- Generated: 2024

-- =============================================
-- Article Status
-- =============================================
INSERT INTO article_status (id, name) VALUES
(0, 'Nháp'),
(1, 'Chờ duyệt'),
(2, 'Đã duyệt'),
(3, 'Từ chối'),
(4, 'Đã xuất bản')
ON CONFLICT (id) DO NOTHING;

-- =============================================
-- Product Status
-- =============================================
INSERT INTO product_status (id, name) VALUES
(0, 'Nháp'),
(1, 'Chờ duyệt'),
(2, 'Đã duyệt'),
(3, 'Từ chối'),
(4, 'Đang bán')
ON CONFLICT (id) DO NOTHING;

-- =============================================
-- Product Brand Status
-- =============================================
INSERT INTO product_brand_status (id, name) VALUES
(0, 'Chờ xác minh'),
(1, 'Đã xác minh'),
(2, 'Tạm khóa'),
(3, 'Đã khóa')
ON CONFLICT (id) DO NOTHING;

-- =============================================
-- Product Order Status
-- =============================================
INSERT INTO product_order_status (id, name, active, sort) VALUES
(1, 'Chờ xác nhận', true, 1),
(2, 'Đã xác nhận', true, 2),
(3, 'Đang giao hàng', true, 3),
(4, 'Đã giao hàng', true, 4),
(5, 'Đã hủy', true, 5),
(6, 'Hoàn trả', true, 6)
ON CONFLICT (id) DO NOTHING;

-- =============================================
-- Product Order Payment Status
-- =============================================
INSERT INTO product_order_payment_status (id, name, active, sort) VALUES
(1, 'Chưa thanh toán', true, 1),
(2, 'Đã thanh toán', true, 2),
(3, 'Thanh toán thất bại', true, 3),
(4, 'Hoàn tiền', true, 4)
ON CONFLICT (id) DO NOTHING;

-- =============================================
-- Product Order Payment Method
-- =============================================
INSERT INTO product_order_payment_method (id, name, active, sort) VALUES
(1, 'Thanh toán khi nhận hàng (COD)', true, 1),
(2, 'Chuyển khoản ngân hàng', true, 2),
(3, 'Ví điện tử', true, 3),
(4, 'Thẻ tín dụng/Ghi nợ', true, 4)
ON CONFLICT (id) DO NOTHING;

-- =============================================
-- Advertising Type
-- =============================================
INSERT INTO advertising_type (id, name) VALUES
(1, 'Văn bản'),
(2, 'Hình ảnh'),
(3, 'HTML Code')
ON CONFLICT (id) DO NOTHING;

-- =============================================
-- Country (Vietnam)
-- =============================================
INSERT INTO country (id, name, code, active) VALUES
(1, 'Việt Nam', 'VN', true)
ON CONFLICT (id) DO NOTHING;

-- =============================================
-- Default Settings
-- =============================================
INSERT INTO setting (id, setting_key, setting_value, setting_comment) VALUES
(1, 'SiteName', 'CMS Website', 'Tên website'),
(2, 'SiteDescription', 'Hệ thống quản lý nội dung', 'Mô tả website'),
(3, 'SiteKeywords', 'cms, website, content management', 'Từ khóa SEO'),
(4, 'SiteLogo', '/images/logo.png', 'Logo website'),
(5, 'SiteFavicon', '/images/favicon.ico', 'Favicon'),
(6, 'ContactEmail', 'contact@example.com', 'Email liên hệ'),
(7, 'ContactPhone', '0123456789', 'Số điện thoại liên hệ'),
(8, 'ContactAddress', 'Địa chỉ công ty', 'Địa chỉ liên hệ'),
(9, 'FacebookUrl', 'https://facebook.com', 'Link Facebook'),
(10, 'YoutubeUrl', 'https://youtube.com', 'Link Youtube'),
(11, 'ZaloUrl', '', 'Link Zalo'),
(12, 'GoogleAnalytics', '', 'Mã Google Analytics'),
(13, 'FacebookPixel', '', 'Mã Facebook Pixel'),
(14, 'SmtpHost', 'smtp.gmail.com', 'SMTP Host'),
(15, 'SmtpPort', '587', 'SMTP Port'),
(16, 'SmtpUser', '', 'SMTP Username'),
(17, 'SmtpPassword', '', 'SMTP Password'),
(18, 'SmtpFromEmail', '', 'Email gửi đi'),
(19, 'SmtpFromName', '', 'Tên người gửi'),
(20, 'PageSize', '20', 'Số item mỗi trang')
ON CONFLICT (id) DO NOTHING;

-- =============================================
-- Default Roles
-- =============================================
INSERT INTO asp_net_roles (id, name, normalized_name, concurrency_stamp) VALUES
('1', 'Admin', 'ADMIN', gen_random_uuid()::TEXT),
('2', 'Editor', 'EDITOR', gen_random_uuid()::TEXT),
('3', 'Author', 'AUTHOR', gen_random_uuid()::TEXT),
('4', 'Member', 'MEMBER', gen_random_uuid()::TEXT)
ON CONFLICT (id) DO NOTHING;

-- =============================================
-- Default Admin User (password: Admin@123)
-- =============================================
INSERT INTO asp_net_users (id, user_name, normalized_user_name, email, normalized_email, email_confirmed, password_hash, security_stamp, concurrency_stamp, phone_number, phone_number_confirmed, two_factor_enabled, lockout_enabled, access_failed_count) VALUES
('admin-user-id-001', 'admin@example.com', 'ADMIN@EXAMPLE.COM', 'admin@example.com', 'ADMIN@EXAMPLE.COM', true, 'AQAAAAEAACcQAAAAEL8xP8s5VbBfuU5VAKjP8wHwq8xqVxvJjOkOvqZxZxZxZxZxZxZxZxZxZxZxZxZxZw==', gen_random_uuid()::TEXT, gen_random_uuid()::TEXT, NULL, false, false, true, 0)
ON CONFLICT (id) DO NOTHING;

INSERT INTO asp_net_user_roles (user_id, role_id) VALUES
('admin-user-id-001', '1')
ON CONFLICT (user_id, role_id) DO NOTHING;

INSERT INTO asp_net_user_profiles (user_id, full_name, active, create_date) VALUES
('admin-user-id-001', 'Administrator', true, NOW())
ON CONFLICT DO NOTHING;

-- =============================================
-- Default Article Categories
-- =============================================
INSERT INTO article_category (id, parent_id, name, url, sort, active, can_delete, create_date) VALUES
(1, NULL, 'Tin tức', 'tin-tuc', 1, true, false, NOW()),
(2, NULL, 'Sự kiện', 'su-kien', 2, true, true, NOW()),
(3, NULL, 'Thông báo', 'thong-bao', 3, true, true, NOW())
ON CONFLICT (id) DO NOTHING;

-- Reset sequence
SELECT setval('article_category_id_seq', (SELECT MAX(id) FROM article_category));

-- =============================================
-- Default Product Categories
-- =============================================
INSERT INTO product_category (id, parent_id, name, url, sort, active, can_delete, create_date) VALUES
(1, NULL, 'Sản phẩm', 'san-pham', 1, true, false, NOW()),
(2, NULL, 'Dịch vụ', 'dich-vu', 2, true, true, NOW())
ON CONFLICT (id) DO NOTHING;

-- Reset sequence
SELECT setval('product_category_id_seq', (SELECT MAX(id) FROM product_category));

-- =============================================
-- Default Units
-- =============================================
INSERT INTO unit (id, name, active, create_date) VALUES
(1, 'Cái', true, NOW()),
(2, 'Chiếc', true, NOW()),
(3, 'Bộ', true, NOW()),
(4, 'Hộp', true, NOW()),
(5, 'Kg', true, NOW()),
(6, 'Lít', true, NOW()),
(7, 'Mét', true, NOW()),
(8, 'Gói', true, NOW())
ON CONFLICT (id) DO NOTHING;

-- Reset sequence
SELECT setval('unit_id_seq', (SELECT MAX(id) FROM unit));

-- =============================================
-- Vietnamese Character Replacement
-- =============================================
INSERT INTO replace_char (char_from, char_to) VALUES
('à', 'a'), ('á', 'a'), ('ạ', 'a'), ('ả', 'a'), ('ã', 'a'),
('â', 'a'), ('ầ', 'a'), ('ấ', 'a'), ('ậ', 'a'), ('ẩ', 'a'), ('ẫ', 'a'),
('ă', 'a'), ('ằ', 'a'), ('ắ', 'a'), ('ặ', 'a'), ('ẳ', 'a'), ('ẵ', 'a'),
('è', 'e'), ('é', 'e'), ('ẹ', 'e'), ('ẻ', 'e'), ('ẽ', 'e'),
('ê', 'e'), ('ề', 'e'), ('ế', 'e'), ('ệ', 'e'), ('ể', 'e'), ('ễ', 'e'),
('ì', 'i'), ('í', 'i'), ('ị', 'i'), ('ỉ', 'i'), ('ĩ', 'i'),
('ò', 'o'), ('ó', 'o'), ('ọ', 'o'), ('ỏ', 'o'), ('õ', 'o'),
('ô', 'o'), ('ồ', 'o'), ('ố', 'o'), ('ộ', 'o'), ('ổ', 'o'), ('ỗ', 'o'),
('ơ', 'o'), ('ờ', 'o'), ('ớ', 'o'), ('ợ', 'o'), ('ở', 'o'), ('ỡ', 'o'),
('ù', 'u'), ('ú', 'u'), ('ụ', 'u'), ('ủ', 'u'), ('ũ', 'u'),
('ư', 'u'), ('ừ', 'u'), ('ứ', 'u'), ('ự', 'u'), ('ử', 'u'), ('ữ', 'u'),
('ỳ', 'y'), ('ý', 'y'), ('ỵ', 'y'), ('ỷ', 'y'), ('ỹ', 'y'),
('đ', 'd'),
('À', 'A'), ('Á', 'A'), ('Ạ', 'A'), ('Ả', 'A'), ('Ã', 'A'),
('Â', 'A'), ('Ầ', 'A'), ('Ấ', 'A'), ('Ậ', 'A'), ('Ẩ', 'A'), ('Ẫ', 'A'),
('Ă', 'A'), ('Ằ', 'A'), ('Ắ', 'A'), ('Ặ', 'A'), ('Ẳ', 'A'), ('Ẵ', 'A'),
('È', 'E'), ('É', 'E'), ('Ẹ', 'E'), ('Ẻ', 'E'), ('Ẽ', 'E'),
('Ê', 'E'), ('Ề', 'E'), ('Ế', 'E'), ('Ệ', 'E'), ('Ể', 'E'), ('Ễ', 'E'),
('Ì', 'I'), ('Í', 'I'), ('Ị', 'I'), ('Ỉ', 'I'), ('Ĩ', 'I'),
('Ò', 'O'), ('Ó', 'O'), ('Ọ', 'O'), ('Ỏ', 'O'), ('Õ', 'O'),
('Ô', 'O'), ('Ồ', 'O'), ('Ố', 'O'), ('Ộ', 'O'), ('Ổ', 'O'), ('Ỗ', 'O'),
('Ơ', 'O'), ('Ờ', 'O'), ('Ớ', 'O'), ('Ợ', 'O'), ('Ở', 'O'), ('Ỡ', 'O'),
('Ù', 'U'), ('Ú', 'U'), ('Ụ', 'U'), ('Ủ', 'U'), ('Ũ', 'U'),
('Ư', 'U'), ('Ừ', 'U'), ('Ứ', 'U'), ('Ự', 'U'), ('Ử', 'U'), ('Ữ', 'U'),
('Ỳ', 'Y'), ('Ý', 'Y'), ('Ỵ', 'Y'), ('Ỷ', 'Y'), ('Ỹ', 'Y'),
('Đ', 'D')
ON CONFLICT DO NOTHING;