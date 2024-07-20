using System;

namespace UnderdorkStudios.UnderTags.Editor
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public class UnderTagDropdownAttribute : Attribute
    {
        public string ParentTag { get; }
        public bool IncludeParent { get; }
        public bool DirectChildrenOnly { get; }
        
        public UnderTagDropdownAttribute(string parentTag, bool includeParent = false, bool directChildrenOnly = true)
        {
            ParentTag = parentTag;
            IncludeParent = includeParent;
            DirectChildrenOnly = directChildrenOnly;
        }
    }
}