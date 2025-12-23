#!/usr/bin/env python3
"""
Comprehensive script to convert ALL int/int? ID fields to long/long? in entity files.
This includes both Primary Keys and Foreign Keys.
"""

import os
import re
import sys

# Directories to process
DIRS_TO_FIX = [
    "backend/CMS.API/Data/Entities",
    "backend/CMS.API/Models/DTOs",
    "backend/CMS.API/Models/Responses",
]

def convert_int_to_long(content):
    """Convert int/int? ID fields to long/long?"""
    modified = content
    changes = []
    
    # Pattern 1: public int Id { get; set; } -> public long Id { get; set; }
    # (For primary keys)
    pattern_pk = r'(public\s+)int(\s+Id\s*\{\s*get;\s*set;\s*\})'
    if re.search(pattern_pk, modified):
        modified = re.sub(pattern_pk, r'\1long\2', modified)
        changes.append("Changed Id: int -> long")
    
    # Pattern 2: public int? SomethingId { get; set; } -> public long? SomethingId { get; set; }
    # (For foreign keys ending with Id)
    pattern_fk = r'(public\s+)int\?(\s+\w+Id\s*\{\s*get;\s*set;\s*\})'
    matches = re.findall(pattern_fk, modified)
    if matches:
        modified = re.sub(pattern_fk, r'\1long?\2', modified)
        changes.append(f"Changed {len(matches)} FK fields: int? -> long?")
    
    # Pattern 3: public int SomethingId { get; set; } (non-nullable FK) -> public long
    pattern_fk_nonnull = r'(public\s+)int(\s+\w+Id\s*\{\s*get;\s*set;\s*\})'
    matches2 = re.findall(pattern_fk_nonnull, modified)
    if matches2:
        # Be careful not to match "Id" which was already converted
        # Only match XxxId patterns
        pattern_fk_nonnull_specific = r'(public\s+)int(\s+[A-Z]\w+Id\s*\{\s*get;\s*set;\s*\})'
        if re.search(pattern_fk_nonnull_specific, modified):
            modified = re.sub(pattern_fk_nonnull_specific, r'\1long\2', modified)
            changes.append("Changed non-nullable FK fields: int -> long")
    
    return modified, changes

def process_file(filepath):
    """Process a single file."""
    print(f"\n📄 Processing: {filepath}")
    
    try:
        with open(filepath, 'r', encoding='utf-8') as f:
            content = f.read()
    except Exception as e:
        print(f"  ❌ Error reading file: {e}")
        return 0
    
    modified, changes = convert_int_to_long(content)
    
    if changes:
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(modified)
        print(f"  ✅ Modified")
        for change in changes:
            print(f"    - {change}")
        return len(changes)
    else:
        print("  ⏭️  No changes needed")
        return 0

def process_directory(directory):
    """Process all .cs files in directory recursively."""
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
    print("=" * 70)
    print("🔧 COMPREHENSIVE INT TO LONG CONVERTER")
    print("   Converts ALL int/int? ID fields to long/long?")
    print("   (Both Primary Keys and Foreign Keys)")
    print("=" * 70)
    
    total_files = 0
    total_changes = 0
    
    for directory in DIRS_TO_FIX:
        print(f"\n{'='*70}")
        print(f"📁 Processing Directory: {directory}")
        print("="*70)
        files, changes = process_directory(directory)
        total_files += files
        total_changes += changes
    
    print("\n" + "=" * 70)
    print("📊 SUMMARY")
    print("=" * 70)
    print(f"  Files modified: {total_files}")
    print(f"  Total changes:  {total_changes}")
    print("=" * 70)
    
    if total_changes > 0:
        print("\n⚠️  IMPORTANT: After running this script:")
        print("   1. cd backend/CMS.API && dotnet build")
        print("   2. Fix any remaining compile errors in Services")
        print("   3. Restart the backend: dotnet watch run")

if __name__ == "__main__":
    main()
