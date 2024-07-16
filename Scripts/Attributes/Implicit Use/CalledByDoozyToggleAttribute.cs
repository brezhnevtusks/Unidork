using System;
using JetBrains.Annotations;

namespace Unidork.Attributes
{
    /// <summary>
    /// Attribute to put on methods that are triggered from Doozy UI toggles.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    [MeansImplicitUse]
    public class CalledByDoozyToggleAttribute : Attribute
    {
        public string DoozyToggleCategory { get; }
        public string DoozyToggleName { get; }

        public CalledByDoozyToggleAttribute(string doozyToggleCategory, string doozyViewName)
        {
            DoozyToggleCategory = doozyToggleCategory ?? "";
            DoozyToggleName = doozyViewName ?? "";
        }
    }
}