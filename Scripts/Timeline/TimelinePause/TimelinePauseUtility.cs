using UnityEngine.Timeline;

namespace Unidork.Timeline
{
    /// <summary>
    /// Utility class that can be used by various components to unpause a paused timeline.
    /// </summary>
    public static class TimelinePauseUtility
    {
        /// <summary>
        /// Is the timeline currently paused?
        /// </summary>
        public static bool IsPaused { get; set; }
        
        public delegate void OnUnpauseSignalReceivedEvent(SignalAsset signal);
        /// <summary>
        /// Event that is raised to unpause a timeline.
        /// </summary>
        public static OnUnpauseSignalReceivedEvent OnUnpauseSignalReceived;

        /// <summary>
        /// Sends the unpause signal.
        /// </summary>
        /// <param name="signal">Signal.</param>
        public static void SendUnpauseSignal(SignalAsset signal)
        {
            OnUnpauseSignalReceived?.Invoke(signal);
        }
    }
}