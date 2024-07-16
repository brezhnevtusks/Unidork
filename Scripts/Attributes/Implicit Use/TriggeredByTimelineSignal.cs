using System;
using JetBrains.Annotations;

namespace Unidork.Attributes
{
    /// <summary>
    /// Attribute to put on methods that are triggered via Timeline signals.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [MeansImplicitUse]
    public class TriggeredByTimelineSignalAttribute : Attribute
    {
        public string SignalName1 { get; }
        public string SignalName2 { get; }
        public string SignalName3 { get; }
        public string SignalName4 { get; }
        public string SignalName5 { get; }
        public string SignalName6 { get; }
        public string SignalName7 { get; }
        public string SignalName8 { get; }
        public string SignalName9 { get; }
        public string SignalName10 { get; }

        public TriggeredByTimelineSignalAttribute(string signalName1, string signalName2 = "", string signalName3 = "", string signalName4 = "", string signalName5 = "",
                                         string signalName6 = "", string signalName7 = "", string signalName8 = "", string signalName9 = "",
                                         string signalName10 = "")
        {
            SignalName1 = signalName1 ?? "";
            SignalName2 = signalName2 ?? "";
            SignalName3 = signalName3 ?? "";
            SignalName4 = signalName4 ?? "";
            SignalName5 = signalName5 ?? "";
            SignalName6 = signalName6 ?? "";
            SignalName7 = signalName7 ?? "";
            SignalName8 = signalName8 ?? "";
            SignalName9 = signalName9 ?? "";
            SignalName10 = signalName10 ?? "";
        }
    }
}
