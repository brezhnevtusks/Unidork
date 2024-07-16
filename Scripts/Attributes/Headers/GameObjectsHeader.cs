using Sirenix.OdinInspector;
using System;

namespace Unidork.Attributes
{
    [IncludeMyAttributes]
    [Title("GAME OBJECTS", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false)]
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public class GameObjectsHeader : Attribute
    {
    }
}