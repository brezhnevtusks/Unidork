using System;
using Sirenix.OdinInspector;

namespace Unidork.Attributes
{
    [IncludeMyAttributes]
    [Title("INFO", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false)]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public class InfoHeader : Attribute
    {
    }
}