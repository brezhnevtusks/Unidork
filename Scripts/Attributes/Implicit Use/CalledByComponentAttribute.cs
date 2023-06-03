using JetBrains.Annotations;
using System;

namespace Unidork.Attributes
{
    /// <summary>
	/// Attribute to put on methods that are called by a specific component via a Unity Event.
	/// </summary>    
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	[MeansImplicitUse]
	public class CalledByComponentAttribute : Attribute
    {
		public string CallerGameObject { get; }
		public string CallerComponent { get; }

		public CalledByComponentAttribute(string callerGameObject, string callerComponent)
		{
			CallerGameObject = callerGameObject ?? "";
			CallerComponent = callerComponent ?? "";
		}
	}
}