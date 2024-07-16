using Sirenix.OdinInspector;
using System;

namespace Unidork.Attributes
{
    [IncludeMyAttributes]
    [Title("SKILLS", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false)]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public class SkillsHeader : Attribute
    {
    }
}