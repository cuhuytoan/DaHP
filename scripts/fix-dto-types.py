#!/usr/bin/env python3
"""
Script to fix type mismatches in DTOs and Services after entity FK changes.
Changes int? to long? in DTOs for affected fields.
"""

import os
import re
import sys

# Directories to process
DIRS_TO_FIX = [
    "backend/CMS.API/Models/DTOs",
    "backend/CMS.API/Models/Responses",
    "backend/CMS.API/Services",
]

# FK field patterns that should be long? instead of int?
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
    r"ProductBrandStatusId",
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
    r"DepartmentManId",
    r"ProductReviewTypeId",
    r"ProductStatusId",
]

def fix_dto_types(content):
    """Fix DTO types from int? to long? for specified patterns."""
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
    """Process a single file and fix types."""
    print(f"\n📄 Processing: {filepath}")
    
    try:
        with open(filepath, 'r', encoding='utf-8') as f:
            content = f.read()
    except Exception as e:
        print(f"  ❌ Error reading file: {e}")
        return 0
    
    modified, changes = fix_dto_types(content)
    
    if changes:
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(modified)
        print(f"  ✅ Modified ({len(changes)} changes)")
        for change in changes[:5]:  # Show max 5 changes
            print(change)
        if len(changes) > 5:
            print(f"  ... and {len(changes) - 5} more")
        return len(changes)
    else:
        print("  ⏭️  No changes needed")
        return 0

def process_directory(directory):
    """Process all .cs files in directory."""
    if not os.path.exists(directory):
        print(f"⚠️  Directory not found: {directory}")
        return 0, 0
    
    total_changes = 0
    files_modified = 0
    
    for root, dirs, files in os.walk(directory):
        for filename in files:
            if filename.endswith('.cs'):
                filepath = os.path.join(root, filename)
                changes = process_file(filepath)
                if changes > 0:
                    files_modified += 1
                    total_changes += changes
    
    return files_modified, total_changes

def main():
    print("=" * 60)
    print("🔧 DTO & Service FK Type Fixer")
    print("=" * 60)
    
    total_files = 0
    total_changes = 0
    
    for directory in DIRS_TO_FIX:
        print(f"\n📁 Directory: {directory}")
        files, changes = process_directory(directory)
        total_files += files
        total_changes += changes
    
    print("\n" + "=" * 60)
    print("📊 SUMMARY")
    print("=" * 60)
    print(f"  Files modified: {total_files}")
    print(f"  Total changes:  {total_changes}")
    print("=" * 60)
    
    if total_changes > 0:
        print("\n⚠️  Please rebuild the project:")
        print("   cd backend/CMS.API && dotnet build")

if __name__ == "__main__":
    main()
