using System;
using JetBrains.Annotations;

namespace Unidork.Attributes
{
	/// <summary>
	/// Attribute to put on methods that are triggered from Doozy UI views.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	[MeansImplicitUse]
	public class CalledByDoozyViewAttribute : Attribute
	{
		public string DoozyViewCategory { get; }
		public string DoozyViewName { get; }

		public CalledByDoozyViewAttribute(string doozyViewCategory, string doozyViewName)
		{
			DoozyViewCategory = doozyViewCategory ?? "";
			DoozyViewName = doozyViewName ?? "";
		}
	}
}
