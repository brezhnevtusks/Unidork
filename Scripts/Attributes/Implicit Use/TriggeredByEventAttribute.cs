using System;
using JetBrains.Annotations;

namespace Unidork.Attributes
{
	/// <summary>
	/// Attribute to put on methods that are triggered via game event listeners.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	[MeansImplicitUse]
	public class TriggeredByEventAttribute : Attribute
	{
		public string EventName1 { get; }
		public string EventName2 { get; }
		public string EventName3 { get; }
		public string EventName4 { get; }
		public string EventName5 { get; }
		public string EventName6 { get; }
		public string EventName7 { get; }
		public string EventName8 { get; }
		public string EventName9 { get; }
		public string EventName10 { get; }

		public TriggeredByEventAttribute(string eventName1, string eventName2 = "", string eventName3 = "", string eventName4 = "", string eventName5 = "",
										 string eventName6 = "", string eventName7 = "", string eventName8 = "", string eventName9 = "",
		                                 string eventName10 = "")
		{
			EventName1 = eventName1 ?? "";
			EventName2 = eventName2 ?? "";
			EventName3 = eventName3 ?? "";
			EventName4 = eventName4 ?? "";
			EventName5 = eventName5 ?? "";
			EventName6 = eventName6 ?? "";
			EventName7 = eventName7 ?? "";
			EventName8 = eventName8 ?? "";
			EventName9 = eventName9 ?? "";
			EventName10 = eventName10 ?? "";
		}
	}
}