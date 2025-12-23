#!/usr/bin/env python3
"""
Script to fix Entity Framework FK type mismatches.
Changes int? foreign keys to long? where they reference entities with long primary keys.
"""

import os
import re
import sys

# Directory containing entity files
ENTITIES_DIR = "backend/CMS.API/Data/Entities"

# List of entities that have long Id as primary key
# These are entities where FK references should use long? instead of int?
LONG_PK_ENTITIES = [
    "Article",
    "ArticleCategory", 
    "ArticleBlock",
    "ArticleType",
    "ArticleComment",
    "ArticleCommentStaff",
    "ArticleAttachFile",
    "ArticleCategoryArticle",
    "ArticleBlockArticle",
    "ArticleCategoryAssign",
    "ArticleRelationArticle",
    "ArticleTop",
    "Product",
    "ProductCategory",
    "ProductType",
    "ProductBrand",
    "ProductBrandType",
    "ProductBrandLevel",
    "ProductBrandCategory",
    "ProductManufacture",
    "ProductComment",
    "ProductCommentStaff",
    "ProductBlock",
    "ProductBlockProduct",
    "ProductCategoryProduct",
    "ProductReview",
    "ProductRelationProduct",
    "ProductPicture",
    "ProductAttachFile",
    "ProductTop",
    "ProductLike",
    "ProductPropertyValue",
    "AdvertisingBlock",
    "Advertising",
    "Location",
    "District", 
    "Ward",
    "Bank",
    "Country",
    "Unit",
    "Menu",
    "MenuCategory"
]

# FK fields to convert from int? to long? (field name patterns)
FK_PATTERNS = [
    r"ArticleTypeId",
    r"ArticleStatusId",
    r"ArticleCategoryId",
    r"ArticleBlockId",
    r"ArticleId",
    r"ProductTypeId",
    r"ProductBrandId",
    r"ProductManufactureId",
    r"ProductCategoryId",
    r"ProductBlockId",
    r"ProductId",
    r"ProductBrandCategoryId",
    r"ProductBrandTypeId",
    r"ProductBrandLevelId",
    r"AdvertisingBlockId",
    r"AdvertisingTypeId",
    r"LocationId",
    r"DistrictId",
    r"WardId",
    r"BankId",
    r"CountryId",
    r"UnitId",
    r"ParentId",
    r"MenuId",
    r"MenuCategoryId",
    r"CustomerId",
]

def fix_fk_types(content):
    """Fix FK types from int? to long? for specified patterns."""
    modified = content
    changes = []
    
    for pattern in FK_PATTERNS:
        # Match: public int? FieldName { get; set; }
        regex = rf'(public\s+)int\?(\s+{pattern}\s*\{{\s*get;\s*set;\s*\}})'
        matches = re.findall(regex, modified)
        if matches:
            modified = re.sub(regex, r'\1long?\2', modified)
            changes.append(f"  Changed {pattern}: int? -> long?")
    
    return modified, changes

def process_file(filepath):
    """Process a single file and fix FK types."""
    print(f"\n📄 Processing: {filepath}")
    
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()
    
    modified, changes = fix_fk_types(content)
    
    if changes:
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(modified)
        print(f"  ✅ Modified ({len(changes)} changes)")
        for change in changes:
            print(change)
        return len(changes)
    else:
        print("  ⏭️  No changes needed")
        return 0

def main():
    print("=" * 60)
    print("🔧 EF Core FK Type Mismatch Fixer")
    print("=" * 60)
    
    if not os.path.exists(ENTITIES_DIR):
        print(f"❌ Error: Directory not found: {ENTITIES_DIR}")
        sys.exit(1)
    
    total_changes = 0
    files_modified = 0
    
    # Process all .cs files in the Entities directory
    for filename in os.listdir(ENTITIES_DIR):
        if filename.endswith('.cs'):
            filepath = os.path.join(ENTITIES_DIR, filename)
            changes = process_file(filepath)
            if changes > 0:
                files_modified += 1
                total_changes += changes
    
    print("\n" + "=" * 60)
    print("📊 SUMMARY")
    print("=" * 60)
    print(f"  Files modified: {files_modified}")
    print(f"  Total changes:  {total_changes}")
    print("=" * 60)
    
    if total_changes > 0:
        print("\n⚠️  Please rebuild the project:")
        print("   cd backend/CMS.API && dotnet build")

if __name__ == "__main__":
    main()
