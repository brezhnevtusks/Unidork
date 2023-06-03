using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Unidork.Events
{
    /// <summary>
    /// Holds a pair of event and respective response.
    /// </summary>
    [System.Serializable]
    public class EventResponsePair
    {
        #region Properties

        public EventResponsePairType Type => type;

        /// <summary>
        /// <see cref="GameEvent"/> to register a response with.
        /// </summary>
        /// <value>
        /// Gets the value of the field @event.
        /// </value>
        public GameEvent Event => @event;

        /// <summary>
        /// Response to invoke when <see cref="GameEvent"/> is raised.
        /// </summary>
        /// <value>
        /// Gets the value of the field response.
        /// </value>
        public UnityEvent Response => response;

#if UNITY_EDITOR

        /// <summary>
        /// Editor-only flag used by the custom property drawer to fold/unfol content of an event-response pair.
        /// </summary>
        /// <value>
        /// Gets the value of the boolean field showContents.
        /// </value>
        public bool ShowContent => showContent;
#endif

        #endregion

        #region Fields

        /// <summary>
        /// Type of event-response pair.
        /// </summary>
        [Tooltip("Type of event-response pair.\n\n" +
                 "SingleEvent - single event invokes a UnityEvent.\n\n" +
                 "MultipleEvents - multiple events invoke a UnityEvent.")]
        [SerializeField]
        private EventResponsePairType type = EventResponsePairType.SingleEvent;

        /// <summary>
        /// <see cref="GameEvent"/> to register a response with.
        /// </summary>
        [Tooltip("GameEvent to register a response with.")]
        [SerializeField]
        private GameEvent @event = null;

        /// <summary>
        /// Array of <see cref="GameEvent"/> to register a response with.
        /// </summary>
        [Tooltip("GameEvents to register a response with.")]
        [SerializeField]
        private GameEvent[] events = null;

        /// <summary>
        /// Response to invoke when <see cref="GameEvent"/> is raised.
        /// </summary>
        [Tooltip("Response to invoke when GameEvent is raised.")]
        [SerializeField]
        private UnityEvent response = null;

#if UNITY_EDITOR

        /// <summary>
        /// Editor-only flag used by the custom property drawer to fold/unfol content of an event-response pair.
        /// </summary>
        [UsedImplicitly]
        [SerializeField, HideInInspector]        
        private bool showContent = true;

#endif

        #endregion

        #region Get

        /// <summary>
        /// Gets game events associated with this event-response pair.
        /// </summary>
        /// <returns>
        /// An array of <see cref="GameEvent"/> objects, which is a clone of <see cref="events"/>.
        /// </returns>
        public GameEvent[] GetGameEvents() => (GameEvent[])events.Clone();

		#endregion

#if UNITY_EDITOR

	#region Editor

	/// <summary>
	/// Sets the showContent flag to True.
	/// </summary>
	public void ShowEventResponsePairContent() => showContent = true;

        /// <summary>
        /// Sets the showContent flag to False.
        /// </summary>
        public void HideEventResponsePairContent() => showContent = false;

        #endregion

#endif
    }
}

