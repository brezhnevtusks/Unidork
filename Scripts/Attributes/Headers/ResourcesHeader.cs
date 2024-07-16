using Sirenix.OdinInspector;
using System;

namespace Unidork.Attributes
{
    [IncludeMyAttributes]
    [Title("RESOURCES", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false)]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public class ResourcesHeader : Attribute
    {
    }
}