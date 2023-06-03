using System;
using JetBrains.Annotations;

namespace Unidork.Attributes
{
	/// <summary>
	/// Attribute to put on methods that are called by Doozy UI buttons.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	[MeansImplicitUse]
	public class CalledByDoozyButtonAttribute : Attribute
	{
		public string DoozyButtonCategory { get;  }
		public string DoozyButtonName { get; }

		public CalledByDoozyButtonAttribute(string doozyButtonCategory, string doozyButtonName)
		{
			DoozyButtonCategory = doozyButtonCategory ?? "";
			DoozyButtonName = doozyButtonName ?? "";
		}
	}
}