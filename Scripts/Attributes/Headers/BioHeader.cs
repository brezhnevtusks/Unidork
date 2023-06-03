using Sirenix.OdinInspector;
using System;

namespace Unidork.Attributes
{
	[IncludeMyAttributes]
	[Title("BIO", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false)]
	[System.Diagnostics.Conditional("UNITY_EDITOR")]
	public class BioHeader : Attribute
	{
	}
}