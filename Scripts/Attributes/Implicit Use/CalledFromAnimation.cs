using JetBrains.Annotations;
using System;

namespace Unidork.Attributes
{
	/// <summary>
	/// Attribute to put on methods that are called from a Unity animation.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [MeansImplicitUse]
    public class CalledFromAnimation : Attribute
    {
        public string GameObject { get; }        
        public string AnimatorState { get; }
        public string Animation { get; }

        public CalledFromAnimation(string gameObject, string animatorState, string animation)
		{
            GameObject = gameObject ?? "";
            AnimatorState = animatorState ?? "";
            Animation = animation ?? "";
		}
    }
}