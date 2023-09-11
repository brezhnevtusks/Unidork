using Sirenix.OdinInspector;
using System.Collections.Generic;
#if DOOZY_UIMANAGER && DOOZY_SIGNALS
using Doozy.Runtime.Signals;
#endif
using UnityEngine;

namespace Unidork.Events
{
    /// <summary>
    /// A scriptable object that holds a unique game event type.
    /// </summary>
    [CreateAssetMenu(fileName = "EVT_", menuName = "Scriptable Objects/Events/Event", order = 2)]
    public class GameEvent : ScriptableObject
    {
        #region Properties

        public string Name => name;

        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        public List<GameEventListener> EventListeners { get => eventListeners; set => eventListeners = value; }

        /// <summary>
        /// When set to True, will log each invocation connected to this event.
        /// </summary>
        /// <value>
        /// Gets the value of the boolean field logToConsole.
        /// </value>
        public bool LogToConsole { get => logToConsole; set => logToConsole = value; }

        #endregion

        #region Fields

        /// <summary>
        /// When set to True, will log each invokation connected to this event.
        /// </summary>
        [UnityEngine.Tooltip("When set to True, will log each invokation connected to this event.")]
        [SerializeField]
        private bool logToConsole = true;
        
#if DOOZY_UIMANAGER && DOOZY_SIGNALS
        /// <summary>
        /// Should a Doozy UI signal be sent as well?
        /// </summary>
        /// <remarks>For Doozy UI Manager version 4.</remarks>
        [Tooltip("hould a Doozy UI signal be sent as well?")]
        [SerializeField] 
        private bool raiseDoozySignal = false;
#endif
        
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        [Tooltip("The list of listeners that this event will notify if it is raised.")]
        [SerializeField, ReadOnly]
        private List<GameEventListener> eventListeners;

        #endregion

        #region Constants

        /// <summary>
        /// Name of the Doozy signal category that the game events belong to.
        /// </summary>
        public const string GameEventSignalCategoryName = "GameEvents";

        #endregion

        #region Event

        /// <summary>
        /// Raises the event, notifying all listeners.
        /// </summary>
        public void Raise()
        {
            for (int index = EventListeners.Count - 1; index >= 0; index--)
            {
                GameEventListener currentListener = eventListeners[index];
                
                if (currentListener == null)
                {
                    eventListeners.RemoveAt(index);
                    continue;
                }
                
                currentListener.OnEventRaised(this);
            }
            
#if DOOZY_UIMANAGER && DOOZY_SIGNALS
            if (raiseDoozySignal)
            {
                _ = Signal.Send(GameEventSignalCategoryName, name.Replace('_'.ToString(), string.Empty));
            }
#endif
        }

        #endregion

        #region Listeners

        /// <summary>
        /// Registers a new listeners.
        /// </summary>
        /// <param name="listener">Listener to register.</param>
        public void RegisterListener(GameEventListener listener)
        {
            if (eventListeners == null)
            {
                eventListeners = new List<GameEventListener>();
            }

            for (var i = eventListeners.Count - 1; i >= 0; i--)
            {
                if (eventListeners[i] != null)
                {
                    continue;
                }
                
                eventListeners.RemoveAt(i);
            }

            if (!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }

        /// <summary>
        /// Unregisters an existing listeners.
        /// </summary>
        /// <param name="listener">Listener to unregister.</param>
        public void UnregisterListener(GameEventListener listener)
        {
            if (eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);
            }
        }

        #endregion

        private void OnEnable()
        {
            eventListeners = new List<GameEventListener>();
        }
    }
}
