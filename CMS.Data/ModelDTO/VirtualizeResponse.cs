using System;
using System.Collections.Generic;

namespace CMS.Data.ModelDTO
{
    public class VirtualizeResponse<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalSize { get; set; } = 0;
    }
    public class ComponentMetadata
    {
        public Type ComponentType { get; set; }
        public Dictionary<string, object> ComponentParameters { get; set; }
    }
    public class CommonDropdownBoolValue
    {
        public bool Id { get; set; }
        public string Name { get; set; }
    }
}
   