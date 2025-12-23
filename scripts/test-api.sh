#!/bin/bash

# =====================================================================================
# COMPREHENSIVE CMS API TEST SUITE
# Tests ALL API endpoints with full coverage
# =====================================================================================

BASE_URL="http://localhost:5000/api"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m'

PASSED=0
FAILED=0
TOTAL=0

# Test function for public endpoints
test_endpoint() {
    local name="$1"
    local method="$2"
    local endpoint="$3"
    local data="$4"
    local expected_status="$5"
    
    TOTAL=$((TOTAL + 1))
    
    printf "${CYAN}▶${NC} %-55s " "$name"
    
    local curl_cmd="curl -s -w '%{http_code}' -X $method -H 'Content-Type: application/json'"
    
    if [ -n "$data" ]; then
        curl_cmd="$curl_cmd -d '$data'"
    fi
    
    curl_cmd="$curl_cmd '${BASE_URL}${endpoint}'"
    
    local response=$(eval $curl_cmd 2>/dev/null)
    local status_code="${response: -3}"
    
    if [ "$status_code" == "$expected_status" ]; then
        echo -e "${GREEN}✓ PASS${NC} ($status_code)"
        PASSED=$((PASSED + 1))
        return 0
    else
        echo -e "${RED}✗ FAIL${NC} (Expected: $expected_status, Got: $status_code)"
        FAILED=$((FAILED + 1))
        return 1
    fi
}

# Test function for authenticated endpoints
test_auth_endpoint() {
    local name="$1"
    local method="$2"
    local endpoint="$3"
    local data="$4"
    local token="$5"
    local expected_status="$6"
    
    TOTAL=$((TOTAL + 1))
    
    printf "${CYAN}▶${NC} %-55s " "$name"
    
    if [ -z "$token" ]; then
        echo -e "${YELLOW}⏭ SKIP${NC} (No token)"
        return 1
    fi
    
    local curl_cmd="curl -s -w '%{http_code}' -X $method"
    curl_cmd="$curl_cmd -H 'Authorization: Bearer $token'"
    curl_cmd="$curl_cmd -H 'Content-Type: application/json'"
    
    if [ -n "$data" ]; then
        curl_cmd="$curl_cmd -d '$data'"
    fi
    
    curl_cmd="$curl_cmd '${BASE_URL}${endpoint}'"
    
    local response=$(eval $curl_cmd 2>/dev/null)
    local status_code="${response: -3}"
    
    if [ "$status_code" == "$expected_status" ]; then
        echo -e "${GREEN}✓ PASS${NC} ($status_code)"
        PASSED=$((PASSED + 1))
        return 0
    else
        echo -e "${RED}✗ FAIL${NC} (Expected: $expected_status, Got: $status_code)"
        FAILED=$((FAILED + 1))
        return 1
    fi
}

# Header
echo ""
echo -e "${CYAN}╔══════════════════════════════════════════════════════════════════════════════════╗${NC}"
echo -e "${CYAN}║                     COMPREHENSIVE CMS API TEST SUITE                              ║${NC}"
echo -e "${CYAN}║                     Testing ALL API Endpoints                                     ║${NC}"
echo -e "${CYAN}║                     Base URL: $BASE_URL                                   ║${NC}"
echo -e "${CYAN}╚══════════════════════════════════════════════════════════════════════════════════╝${NC}"
echo ""

# ═══════════════════════════════════════════════════════════════════════════════════
# HEALTH CHECK
# ═══════════════════════════════════════════════════════════════════════════════════
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${YELLOW}  ❤️  HEALTH CHECK${NC}"
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"

test_endpoint "Health Check" "GET" "/../health" "" "200"
test_endpoint "Swagger JSON" "GET" "/../swagger/v1/swagger.json" "" "200"

# ═══════════════════════════════════════════════════════════════════════════════════
# AUTH API (11 endpoints)
# ═══════════════════════════════════════════════════════════════════════════════════
echo ""
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${YELLOW}  🔐 AUTH API TESTS (11 endpoints)${NC}"
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"

# Generate unique test user
TEST_EMAIL="apitest_$(date +%s)@example.com"
TEST_PASSWORD="Test@123456"

# 1. Register
REGISTER_DATA="{\"email\":\"$TEST_EMAIL\",\"password\":\"$TEST_PASSWORD\",\"confirmPassword\":\"$TEST_PASSWORD\",\"fullName\":\"API Test User\"}"
test_endpoint "POST /auth/register" "POST" "/auth/register" "$REGISTER_DATA" "200"

# 2. Login and capture token
printf "${CYAN}▶${NC} %-55s " "POST /auth/login"
LOGIN_DATA="{\"email\":\"$TEST_EMAIL\",\"password\":\"$TEST_PASSWORD\"}"
LOGIN_RESPONSE=$(curl -s -X POST "${BASE_URL}/auth/login" -H "Content-Type: application/json" -d "$LOGIN_DATA")
ACCESS_TOKEN=$(echo "$LOGIN_RESPONSE" | jq -r '.data.token // empty')
REFRESH_TOKEN=$(echo "$LOGIN_RESPONSE" | jq -r '.data.refreshToken // empty')
TOTAL=$((TOTAL + 1))
if [ -n "$ACCESS_TOKEN" ] && [ "$ACCESS_TOKEN" != "null" ]; then
    echo -e "${GREEN}✓ PASS${NC} (200)"
    PASSED=$((PASSED + 1))
else
    echo -e "${RED}✗ FAIL${NC} (Login failed)"
    FAILED=$((FAILED + 1))
    ACCESS_TOKEN=""
fi

# 3. Login with wrong credentials
BAD_LOGIN="{\"email\":\"wrong@email.com\",\"password\":\"wrongpass\"}"
test_endpoint "POST /auth/login (invalid)" "POST" "/auth/login" "$BAD_LOGIN" "401"

# 4. Get current user
test_auth_endpoint "GET /auth/me" "GET" "/auth/me" "" "$ACCESS_TOKEN" "200"

# 5. Validate token
test_auth_endpoint "GET /auth/validate" "GET" "/auth/validate" "" "$ACCESS_TOKEN" "200"

# 6. Refresh token
if [ -n "$REFRESH_TOKEN" ] && [ "$REFRESH_TOKEN" != "null" ] && [ -n "$ACCESS_TOKEN" ]; then
    REFRESH_DATA="{\"token\":\"$ACCESS_TOKEN\",\"refreshToken\":\"$REFRESH_TOKEN\"}"
    test_endpoint "POST /auth/refresh" "POST" "/auth/refresh" "$REFRESH_DATA" "200"
else
    printf "${CYAN}▶${NC} %-55s ${YELLOW}⏭ SKIP${NC} (No refresh token)\n" "POST /auth/refresh-token"
fi

# 7. Forgot password
FORGOT_DATA="{\"email\":\"$TEST_EMAIL\"}"
test_endpoint "POST /auth/forgot-password" "POST" "/auth/forgot-password" "$FORGOT_DATA" "200"

# 8. Change password (requires auth)
CHANGE_PWD_DATA="{\"currentPassword\":\"$TEST_PASSWORD\",\"newPassword\":\"NewTest@123456\",\"confirmPassword\":\"NewTest@123456\"}"
test_auth_endpoint "POST /auth/change-password" "POST" "/auth/change-password" "$CHANGE_PWD_DATA" "$ACCESS_TOKEN" "200"

# 9. Logout
test_auth_endpoint "POST /auth/logout" "POST" "/auth/logout" "" "$ACCESS_TOKEN" "200"

# 10. Confirm email (without valid token, expect error)
test_endpoint "GET /auth/confirm-email (no token)" "GET" "/auth/confirm-email?userId=test&token=invalid" "" "400"

# 11. Reset password (without valid token, expect error)  
RESET_DATA="{\"email\":\"$TEST_EMAIL\",\"token\":\"invalid\",\"newPassword\":\"Test@123456\",\"confirmPassword\":\"Test@123456\"}"
test_endpoint "POST /auth/reset-password (invalid)" "POST" "/auth/reset-password" "$RESET_DATA" "400"

# ═══════════════════════════════════════════════════════════════════════════════════
# ARTICLES API (14 endpoints)
# ═══════════════════════════════════════════════════════════════════════════════════
echo ""
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${YELLOW}  📰 ARTICLES API TESTS (14 endpoints)${NC}"
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"

# Re-login with original password (that was changed)
LOGIN_DATA="{\"email\":\"$TEST_EMAIL\",\"password\":\"NewTest@123456\"}"
LOGIN_RESPONSE=$(curl -s -X POST "${BASE_URL}/auth/login" -H "Content-Type: application/json" -d "$LOGIN_DATA")
ACCESS_TOKEN=$(echo "$LOGIN_RESPONSE" | jq -r '.data.token // empty')

# 1. Get all articles (public)
test_endpoint "GET /articles" "GET" "/articles" "" "200"

# 2. Get articles with pagination
test_endpoint "GET /articles?page=1&pageSize=5" "GET" "/articles?page=1&pageSize=5" "" "200"

# 3. Get top articles
test_endpoint "GET /articles/top" "GET" "/articles/top" "" "200"

# 4. Get top articles with count
test_endpoint "GET /articles/top?count=5" "GET" "/articles/top?count=5" "" "200"

# 5. Get recent articles
test_endpoint "GET /articles/recent" "GET" "/articles/recent" "" "200"

# 6. Get recent articles with count
test_endpoint "GET /articles/recent?count=5" "GET" "/articles/recent?count=5" "" "200"

# 7. Get article by ID (non-existent)
test_endpoint "GET /articles/999999 (not found)" "GET" "/articles/999999" "" "404"

# 8. Get article by URL (non-existent)
test_endpoint "GET /articles/url/non-existent (not found)" "GET" "/articles/url/non-existent-article-slug" "" "404"

# 9. Get article comments
test_endpoint "GET /articles/1/comments" "GET" "/articles/1/comments" "" "200"

# 10. Get related articles
test_endpoint "GET /articles/1/related" "GET" "/articles/1/related" "" "200"

# 11. Create article (requires Editor/Admin role - expect 403 for User role)
CREATE_ARTICLE="{\"name\":\"Test Article\",\"content\":\"Test content\",\"url\":\"test-article-$(date +%s)\"}"
test_auth_endpoint "POST /articles (create - needs Editor)" "POST" "/articles" "$CREATE_ARTICLE" "$ACCESS_TOKEN" "403"

# 12. Update article (requires Editor/Admin role - expect 403)
UPDATE_ARTICLE="{\"name\":\"Updated Article\",\"content\":\"Updated content\"}"
test_auth_endpoint "PUT /articles/999999 (needs Editor)" "PUT" "/articles/999999" "$UPDATE_ARTICLE" "$ACCESS_TOKEN" "403"

# 13. Delete article (requires Editor/Admin role - expect 403)
test_auth_endpoint "DELETE /articles/999999 (needs Editor)" "DELETE" "/articles/999999" "" "$ACCESS_TOKEN" "403"

# 14. Approve (requires admin - expect 403)
test_auth_endpoint "POST /articles/999999/approve (needs Admin)" "POST" "/articles/999999/approve" "" "$ACCESS_TOKEN" "403"

# ═══════════════════════════════════════════════════════════════════════════════════
# PRODUCTS API (17 endpoints)
# ═══════════════════════════════════════════════════════════════════════════════════
echo ""
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${YELLOW}  🛒 PRODUCTS API TESTS (17 endpoints)${NC}"
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"

# 1. Get all products (public)
test_endpoint "GET /products" "GET" "/products" "" "200"

# 2. Get products with pagination
test_endpoint "GET /products?page=1&pageSize=5" "GET" "/products?page=1&pageSize=5" "" "200"

# 3. Get products with filters
test_endpoint "GET /products?minPrice=100" "GET" "/products?minPrice=100" "" "200"

# 4. Get top products
test_endpoint "GET /products/top" "GET" "/products/top" "" "200"

# 5. Get top products with count
test_endpoint "GET /products/top?count=5" "GET" "/products/top?count=5" "" "200"

# 6. Get recent products
test_endpoint "GET /products/recent" "GET" "/products/recent" "" "200"

# 7. Get recent products with count  
test_endpoint "GET /products/recent?count=5" "GET" "/products/recent?count=5" "" "200"

# 8. Get best sellers
test_endpoint "GET /products/bestsellers" "GET" "/products/bestsellers" "" "200"

# 9. Get product by ID (non-existent)
test_endpoint "GET /products/999999 (not found)" "GET" "/products/999999" "" "404"

# 10. Get product by URL (non-existent)
test_endpoint "GET /products/url/non-existent" "GET" "/products/url/non-existent-product-slug" "" "404"

# 11. Get product by code (non-existent)
test_endpoint "GET /products/code/INVALID" "GET" "/products/code/INVALID-CODE-123" "" "404"

# 12. Get product comments
test_endpoint "GET /products/1/comments" "GET" "/products/1/comments" "" "200"

# 13. Get product reviews
test_endpoint "GET /products/1/reviews" "GET" "/products/1/reviews" "" "200"

# 14. Get related products
test_endpoint "GET /products/1/related" "GET" "/products/1/related" "" "200"

# 15. Create product (requires Editor/Admin role - expect 403)
CREATE_PRODUCT="{\"name\":\"Test Product\",\"code\":\"TEST$(date +%s)\",\"price\":100000,\"url\":\"test-product-$(date +%s)\"}"
test_auth_endpoint "POST /products (create - needs Editor)" "POST" "/products" "$CREATE_PRODUCT" "$ACCESS_TOKEN" "403"

# 16. Update product (requires Editor/Admin role - expect 403)
UPDATE_PRODUCT="{\"name\":\"Updated Product\",\"price\":150000}"
test_auth_endpoint "PUT /products/999999 (needs Editor)" "PUT" "/products/999999" "$UPDATE_PRODUCT" "$ACCESS_TOKEN" "403"

# 17. Delete product (requires Editor/Admin role - expect 403)
test_auth_endpoint "DELETE /products/999999 (needs Editor)" "DELETE" "/products/999999" "" "$ACCESS_TOKEN" "403"

# ═══════════════════════════════════════════════════════════════════════════════════
# ADVERTISING API (5 endpoints)
# ═══════════════════════════════════════════════════════════════════════════════════
echo ""
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${YELLOW}  📢 ADVERTISING API TESTS (5 endpoints)${NC}"
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"

# 1. Get all advertising blocks  
test_endpoint "GET /advertising/blocks" "GET" "/advertising/blocks" "" "200"

# 2. Get advertising block by ID (non-existent)
test_endpoint "GET /advertising/block/999999 (not found)" "GET" "/advertising/block/999999" "" "404"

# 3. Get advertisings by block ID
test_endpoint "GET /advertising/by-block/1" "GET" "/advertising/by-block/1" "" "200"

# 4. Get advertising by ID (non-existent)
test_endpoint "GET /advertising/999999 (not found)" "GET" "/advertising/999999" "" "404"

# 5. Track click (non-existent)
test_endpoint "POST /advertising/999999/click (not found)" "POST" "/advertising/999999/click" "" "404"

# ═══════════════════════════════════════════════════════════════════════════════════
# CONTACT API
# ═══════════════════════════════════════════════════════════════════════════════════
echo ""
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${YELLOW}  📧 CONTACT API TESTS${NC}"
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"

# 1. Submit contact form
CONTACT_DATA="{\"fullName\":\"Test User\",\"email\":\"contact_$(date +%s)@test.com\",\"phoneNumber\":\"0123456789\",\"descriptions\":\"Test message\"}"
test_endpoint "POST /contact" "POST" "/contact" "$CONTACT_DATA" "201"

# 2. Submit invalid contact form
INVALID_CONTACT="{\"fullName\":\"\",\"email\":\"invalid\"}"
test_endpoint "POST /contact (invalid)" "POST" "/contact" "$INVALID_CONTACT" "400"

# ═══════════════════════════════════════════════════════════════════════════════════
# NEWSLETTER API
# ═══════════════════════════════════════════════════════════════════════════════════
echo ""
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${YELLOW}  📬 NEWSLETTER API TESTS${NC}"
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"

# 1. Subscribe to newsletter
NEWSLETTER_DATA="{\"email\":\"newsletter_$(date +%s)@test.com\"}"
test_endpoint "POST /newsletter/subscribe" "POST" "/newsletter/subscribe" "$NEWSLETTER_DATA" "201"

# 2. Subscribe with invalid email
INVALID_NEWSLETTER="{\"email\":\"invalid-email\"}"
test_endpoint "POST /newsletter/subscribe (invalid)" "POST" "/newsletter/subscribe" "$INVALID_NEWSLETTER" "400"

# ═══════════════════════════════════════════════════════════════════════════════════
# ORDERS API (if applicable)
# ═══════════════════════════════════════════════════════════════════════════════════
echo ""
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${YELLOW}  🛍 ORDERS API TESTS${NC}"
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"

# 1. Get user orders (requires auth)
test_auth_endpoint "GET /orders/my-orders" "GET" "/orders/my-orders" "" "$ACCESS_TOKEN" "200"

# 2. Get order by ID (non-existent)  
test_auth_endpoint "GET /orders/999999 (not found)" "GET" "/orders/999999" "" "$ACCESS_TOKEN" "404"

# 3. Get order statuses
test_endpoint "GET /orders/statuses" "GET" "/orders/statuses" "" "200"

# 4. Get payment methods
test_endpoint "GET /orders/payment-methods" "GET" "/orders/payment-methods" "" "200"

# ═══════════════════════════════════════════════════════════════════════════════════
# DASHBOARD API (Admin - 8 endpoints)
# ═══════════════════════════════════════════════════════════════════════════════════
echo ""
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${YELLOW}  📊 DASHBOARD API TESTS (Admin - 8 endpoints)${NC}"
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"

# Note: Dashboard requires Editor/Admin role, testing with User role should return 403
test_auth_endpoint "GET /dashboard/summary (needs Editor)" "GET" "/dashboard/summary" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /dashboard/today (needs Editor)" "GET" "/dashboard/today" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /dashboard/order-stats (needs Editor)" "GET" "/dashboard/order-stats" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /dashboard/top-products (needs Editor)" "GET" "/dashboard/top-products" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /dashboard/recent-orders (needs Editor)" "GET" "/dashboard/recent-orders" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /dashboard/products-by-category (needs Editor)" "GET" "/dashboard/products-by-category" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /dashboard/period-stats (needs Editor)" "GET" "/dashboard/period-stats?startDate=2025-01-01&endDate=2025-12-31" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /dashboard/revenue-by-brand (needs Admin)" "GET" "/dashboard/revenue-by-brand" "" "$ACCESS_TOKEN" "403"

# ═══════════════════════════════════════════════════════════════════════════════════
# MASTER DATA API (25 endpoints)
# ═══════════════════════════════════════════════════════════════════════════════════
echo ""
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${YELLOW}  🗂 MASTER DATA API TESTS (25 endpoints)${NC}"
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"

# All MasterData endpoints require EditorOrAdmin role
test_auth_endpoint "GET /admin/master-data/units (needs Editor)" "GET" "/admin/master-data/units" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/master-data/units/999999 (needs Editor)" "GET" "/admin/master-data/units/999999" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/master-data/manufacturers (needs Editor)" "GET" "/admin/master-data/manufacturers" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/master-data/manufacturers/999999 (needs Editor)" "GET" "/admin/master-data/manufacturers/999999" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/master-data/order-statuses (needs Editor)" "GET" "/admin/master-data/order-statuses" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/master-data/payment-methods (needs Editor)" "GET" "/admin/master-data/payment-methods" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/master-data/payment-methods/999999 (needs Editor)" "GET" "/admin/master-data/payment-methods/999999" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/master-data/payment-statuses (needs Editor)" "GET" "/admin/master-data/payment-statuses" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/master-data/product-statuses (needs Editor)" "GET" "/admin/master-data/product-statuses" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/master-data/article-types (needs Editor)" "GET" "/admin/master-data/article-types" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/master-data/article-statuses (needs Editor)" "GET" "/admin/master-data/article-statuses" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/master-data/banks (needs Editor)" "GET" "/admin/master-data/banks" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/master-data/property-categories (needs Editor)" "GET" "/admin/master-data/property-categories" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/master-data/property-types (needs Editor)" "GET" "/admin/master-data/property-types" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/master-data/properties (needs Editor)" "GET" "/admin/master-data/properties" "" "$ACCESS_TOKEN" "403"


# Admin write operations (need Editor/Admin role)
test_auth_endpoint "POST /admin/master-data/units (needs Editor)" "POST" "/admin/master-data/units" "{\"name\":\"Test Unit\"}" "$ACCESS_TOKEN" "403"
test_auth_endpoint "PUT /admin/master-data/units/1 (needs Editor)" "PUT" "/admin/master-data/units/1" "{\"name\":\"Updated\"}" "$ACCESS_TOKEN" "403"
test_auth_endpoint "DELETE /admin/master-data/units/999999 (needs Editor)" "DELETE" "/admin/master-data/units/999999" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "POST /admin/master-data/manufacturers (needs Editor)" "POST" "/admin/master-data/manufacturers" "{\"name\":\"Test\"}" "$ACCESS_TOKEN" "403"
test_auth_endpoint "PUT /admin/master-data/manufacturers/1 (needs Editor)" "PUT" "/admin/master-data/manufacturers/1" "{\"name\":\"Updated\"}" "$ACCESS_TOKEN" "403"
test_auth_endpoint "DELETE /admin/master-data/manufacturers/999999 (needs Editor)" "DELETE" "/admin/master-data/manufacturers/999999" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "POST /admin/master-data/payment-methods (needs Editor)" "POST" "/admin/master-data/payment-methods" "{\"name\":\"Test\"}" "$ACCESS_TOKEN" "403"
test_auth_endpoint "PUT /admin/master-data/payment-methods/1 (needs Editor)" "PUT" "/admin/master-data/payment-methods/1" "{\"name\":\"Updated\"}" "$ACCESS_TOKEN" "403"
test_auth_endpoint "DELETE /admin/master-data/payment-methods/999999 (needs Editor)" "DELETE" "/admin/master-data/payment-methods/999999" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "PUT /admin/master-data/payment-methods/1/toggle (needs Editor)" "PUT" "/admin/master-data/payment-methods/1/toggle-status" "" "$ACCESS_TOKEN" "403"

# ═══════════════════════════════════════════════════════════════════════════════════
# BLOCKS API (Admin - 16 endpoints)
# ═══════════════════════════════════════════════════════════════════════════════════
echo ""
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${YELLOW}  📦 BLOCKS API TESTS (Admin - 16 endpoints)${NC}"
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"

# All Blocks endpoints require Editor/Admin role
test_auth_endpoint "GET /admin/blocks/articles (needs Editor)" "GET" "/admin/blocks/articles" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/blocks/articles/1 (needs Editor)" "GET" "/admin/blocks/articles/1" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "POST /admin/blocks/articles (needs Editor)" "POST" "/admin/blocks/articles" "{\"name\":\"Test Block\"}" "$ACCESS_TOKEN" "403"
test_auth_endpoint "PUT /admin/blocks/articles/1 (needs Editor)" "PUT" "/admin/blocks/articles/1" "{\"name\":\"Updated\"}" "$ACCESS_TOKEN" "403"
test_auth_endpoint "DELETE /admin/blocks/articles/999999 (needs Editor)" "DELETE" "/admin/blocks/articles/999999" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "PUT /admin/blocks/articles/1/toggle (needs Editor)" "PUT" "/admin/blocks/articles/1/toggle-status" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "POST /admin/blocks/articles/1/items (needs Editor)" "POST" "/admin/blocks/articles/1/items" "{\"articleIds\":[1]}" "$ACCESS_TOKEN" "403"
test_auth_endpoint "DELETE /admin/blocks/articles/1/items/1 (needs Editor)" "DELETE" "/admin/blocks/articles/1/items/1" "" "$ACCESS_TOKEN" "403"

test_auth_endpoint "GET /admin/blocks/products (needs Editor)" "GET" "/admin/blocks/products" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/blocks/products/1 (needs Editor)" "GET" "/admin/blocks/products/1" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "POST /admin/blocks/products (needs Editor)" "POST" "/admin/blocks/products" "{\"name\":\"Test Block\"}" "$ACCESS_TOKEN" "403"
test_auth_endpoint "PUT /admin/blocks/products/1 (needs Editor)" "PUT" "/admin/blocks/products/1" "{\"name\":\"Updated\"}" "$ACCESS_TOKEN" "403"
test_auth_endpoint "DELETE /admin/blocks/products/999999 (needs Editor)" "DELETE" "/admin/blocks/products/999999" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "PUT /admin/blocks/products/1/toggle (needs Editor)" "PUT" "/admin/blocks/products/1/toggle-status" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "POST /admin/blocks/products/1/items (needs Editor)" "POST" "/admin/blocks/products/1/items" "{\"productIds\":[1]}" "$ACCESS_TOKEN" "403"
test_auth_endpoint "DELETE /admin/blocks/products/1/items/1 (needs Editor)" "DELETE" "/admin/blocks/products/1/items/1" "" "$ACCESS_TOKEN" "403"

# ═══════════════════════════════════════════════════════════════════════════════════
# ADVERTISING ADMIN API (Admin - 14 endpoints)
# ═══════════════════════════════════════════════════════════════════════════════════
echo ""
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${YELLOW}  📺 ADVERTISING ADMIN API TESTS (Admin - 14 endpoints)${NC}"
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"

# All Admin Advertising endpoints require Editor/Admin role
test_auth_endpoint "GET /admin/advertising/blocks (needs Editor)" "GET" "/admin/advertising/blocks" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/advertising/blocks/1 (needs Editor)" "GET" "/admin/advertising/blocks/1" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "POST /admin/advertising/blocks (needs Editor)" "POST" "/admin/advertising/blocks" "{\"name\":\"Test Block\"}" "$ACCESS_TOKEN" "403"
test_auth_endpoint "PUT /admin/advertising/blocks/1 (needs Editor)" "PUT" "/admin/advertising/blocks/1" "{\"name\":\"Updated\"}" "$ACCESS_TOKEN" "403"
test_auth_endpoint "DELETE /admin/advertising/blocks/999999 (needs Editor)" "DELETE" "/admin/advertising/blocks/999999" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "PUT /admin/advertising/blocks/1/toggle (needs Editor)" "PUT" "/admin/advertising/blocks/1/toggle-status" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/advertising/types (needs Editor)" "GET" "/admin/advertising/types" "" "$ACCESS_TOKEN" "403"

test_auth_endpoint "GET /admin/advertising (needs Editor)" "GET" "/admin/advertising" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "GET /admin/advertising/1 (needs Editor)" "GET" "/admin/advertising/1" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "POST /admin/advertising (needs Editor)" "POST" "/admin/advertising" "{\"name\":\"Test Ad\"}" "$ACCESS_TOKEN" "403"
test_auth_endpoint "PUT /admin/advertising/1 (needs Editor)" "PUT" "/admin/advertising/1" "{\"name\":\"Updated\"}" "$ACCESS_TOKEN" "403"
test_auth_endpoint "DELETE /admin/advertising/999999 (needs Editor)" "DELETE" "/admin/advertising/999999" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "PUT /admin/advertising/1/toggle (needs Editor)" "PUT" "/admin/advertising/1/toggle-status" "" "$ACCESS_TOKEN" "403"
test_auth_endpoint "PUT /admin/advertising/sort-order (needs Editor)" "PUT" "/admin/advertising/sort-order" "[]" "$ACCESS_TOKEN" "403"


# ═══════════════════════════════════════════════════════════════════════════════════
# SUMMARY
# ═══════════════════════════════════════════════════════════════════════════════════
echo ""
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${YELLOW}  📊 COMPREHENSIVE TEST SUMMARY${NC}"
echo -e "${YELLOW}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo ""
echo -e "  Total Tests:   ${CYAN}$TOTAL${NC}"
echo -e "  Passed:        ${GREEN}$PASSED${NC}"
echo -e "  Failed:        ${RED}$FAILED${NC}"
echo ""

# Calculate percentage
if [ $TOTAL -gt 0 ]; then
    PERCENTAGE=$((PASSED * 100 / TOTAL))
    echo -e "  Success Rate:  ${CYAN}${PERCENTAGE}%${NC}"
fi

echo ""
if [ $FAILED -eq 0 ]; then
    echo -e "  ${GREEN}🎉 ALL TESTS PASSED! API is fully functional.${NC}"
    exit 0
else
    echo -e "  ${YELLOW}⚠ Some tests failed. Check output above for details.${NC}"
    exit 1
fi
