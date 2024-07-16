using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Timeline;

namespace Unidork.Timeline
{
    /// <summary>
    /// Component that can be used to call unpause method on <see cref="TimelinePauseUtility"/>.
    /// </summary>
    public class TimelineUnpauseSignalSender : MonoBehaviour
    {
        /// <summary>
        /// Signal that unpauses a timeline.
        /// </summary>
        [SerializeField] private SignalAsset signal;

        /// <summary>
        /// Sends the signal.
        /// </summary>
        [Button("Send Signal")]
        public void Send()
        {
            TimelinePauseUtility.SendUnpauseSignal(signal);
        }
    }
}