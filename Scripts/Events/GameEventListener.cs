using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnderdorkStudios.UnderTools.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Unidork.Events
{
    /// <summary>
    /// Stores a list of events to listen to and respective responses.
    /// </summary>
    [System.Serializable]
#if UNITY_EDITOR
	[ExecuteAlways]
#endif
	public class GameEventListener : MonoBehaviour
    {
		#region Properties

		/// <summary>
		/// Array of event-response pairs belonging to this listener.
		/// </summary>
		/// <value>
		/// Gets the value of the field eventResponsePairs.
		/// </value>
		public EventResponsePair[] EventReponsePairs => (EventResponsePair[])eventResponsePairs.Clone();

		#endregion

		#region Fields

		/// <summary>
		/// Array of event-response pairs belonging to this listener.
		/// </summary>
		[Tooltip("Array of event-response pairs belonging to this listener.")]
        [SerializeField]
        private EventResponsePair[] eventResponsePairs = null;

#endregion

        #region Enable/disable

        private void Awake() => OnEnable();

        private void OnEnable()
        {
            if (!enabled)
            {
                return;
            }

            if (eventResponsePairs == null)
            {
                return;
            }

            foreach (EventResponsePair eventResponsePair in eventResponsePairs)
            {
                if (eventResponsePair.Response == null)
				{
                    continue;
				}

                if (eventResponsePair.Type == EventResponsePairType.SingleEvent)
				{
                    RegisterListener(eventResponsePair.Event);
                }
                else
				{
                    GameEvent[] events = eventResponsePair.GetGameEvents();

                    if (events.IsNullOrEmpty())
					{
                        continue;
					}

                    foreach (GameEvent @event in events)
					{
                        RegisterListener(@event);
					}
				}                
            }

            void RegisterListener(GameEvent @event)
			{
                if (@event == null || (@event.EventListeners != null && @event.EventListeners.Contains(this)))
                {
                    return;
				}

                @event.RegisterListener(this);
			}
        }

        private void OnDisable()
        {
            if (eventResponsePairs == null)
            {
                return;
            }

			foreach (EventResponsePair e in eventResponsePairs)
            {
                if (e.Event == null)
				{
                    continue;
				}

                e.Event.UnregisterListener(this);
            }
        }

        private void OnDestroy()
        {
	        if (eventResponsePairs == null)
	        {
		        return;
	        }

	        foreach (EventResponsePair e in eventResponsePairs)
	        {
		        if (e.Event == null)
		        {
			        continue;
		        }

		        e.Event.UnregisterListener(this);
	        }
        }

        /*#if UNITY_EDITOR

		private void OnDestroy()
		{
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
			{
				if (GameEventListenerWindow.IsShowing)
				{
					UnityEditor.EditorWindow.GetWindow<GameEventListenerWindow>().RefreshListenerList();
				}
			}
		}

#endif*/

		#endregion

		#region Event

		/// <summary>
		/// Invokes listeners if their event's name matches the name of the passed argument.
		/// </summary>
		/// <param name="e">Target event.</param>
		public void OnEventRaised(GameEvent e)
        {
#if UNITY_EDITOR
	        
#if UNITY_2021_1_OR_NEWER
	        UnityEditor.SceneManagement.PrefabStage prefabStage =
		        UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

	        if (prefabStage != null && prefabStage.IsPartOfPrefabContents(gameObject))
	        {
		        return;
	        }
#else
	        UnityEditor.Experimental.SceneManagement.PrefabStage prefabStage =
		        UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

	        if (prefabStage != null && prefabStage.IsPartOfPrefabContents(gameObject))
	        {
		        return;
	        }
#endif

#endif
            foreach (EventResponsePair eventResponsePair in eventResponsePairs)
            {
                if (eventResponsePair.Type == EventResponsePairType.SingleEvent)
				{
                    RaiseEvent(e, eventResponsePair.Event, eventResponsePair.Response);
				}
                else
				{
                    GameEvent[] gameEvents = eventResponsePair.GetGameEvents();

                    if (gameEvents.IsNullOrEmpty())
					{
                        continue;
					}

                    foreach (GameEvent gameEvent in gameEvents)
					{
                        RaiseEvent(e, gameEvent, eventResponsePair.Response);
					}
				}
            }

			void RaiseEvent(GameEvent raisedEvent, GameEvent @event, UnityEvent response)
			{
                if (raisedEvent.Equals(@event))
				{
                    if (@event.LogToConsole)
					{
						for (var i = 0; i < response.GetPersistentEventCount(); i++)
						{
							Debug.Log($"<b>{response.GetPersistentTarget(i).name}</b> is invoking <i>{response.GetPersistentMethodName(i)}</i> in response to <i>{@event.name}</i> event");	
						}
					}

                    response.Invoke();
				}
			}
        }

        #endregion

#if UNITY_EDITOR

        #region Editor

        /// <summary>
        /// Hides the content of all event-response pairs.
        /// </summary>
        [UsedImplicitly]
        [ButtonGroup]
        [Button("Collapse All", ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1f)]
        private void CollapseAllEventResponsePair()
		{
            foreach (EventResponsePair eventResponsePair in eventResponsePairs)
			{
                eventResponsePair.HideEventResponsePairContent();
			}
		}

        /// <summary>
        /// Shows the content of all event-response pairs.
        /// </summary>
        [UsedImplicitly]
        [ButtonGroup]
        [Button("Expand All", ButtonSizes.Large), GUIColor(0f, 1f, 0f)]
        private void ExpandAllEventResponsePairs()
		{
            foreach (EventResponsePair eventResponsePair in eventResponsePairs)
            {
                eventResponsePair.ShowEventResponsePairContent();
            }
        }

        #endregion

#endif
    }
}
