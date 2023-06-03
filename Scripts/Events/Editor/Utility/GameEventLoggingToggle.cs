using Unidork.Extensions;
using UnityEditor;
using UnityEngine;

namespace Unidork.Events
{
    /// <summary>
    /// Contains utility methods that toggle game event logging. 
    /// </summary>
    public static class GameEventLoggingToggle
    {
        #region Logging
        
        /// <summary>
        /// Enables game event logging via a menu item.
        /// </summary>
        [MenuItem("Utility/Game Events/Enable Game Event Logging")]
        private static void EnableGameEventLogging()
        {
            GameEvent[] gameEvents = GetGameEventAssets();

            if (gameEvents == null)
            {
                Debug.LogWarning("There are no GameEvent assets in the project!");
                return;
            }

            ToggleGameEvents(gameEvents, true);
        }

        /// <summary>
        /// Disables game event logging via a menu item.
        /// </summary>
        [MenuItem("Utility/Game Events/Disable Game Event Logging")]
        private static void DisableGameEventLogging()
        {
            GameEvent[] gameEvents = GetGameEventAssets();

            if (gameEvents == null)
            {
                Debug.LogWarning("There are no GameEvent assets in the project!");
                return;
            }

            ToggleGameEvents(gameEvents, false);
        }

        /// <summary>
        /// Sets the value of the <see cref="GameEvent.LogToConsole"/> property on passed game events to the passed toggle value.
        /// </summary>
        /// <param name="toggleValue"></param>
        private static void ToggleGameEvents(GameEvent[] gameEvents, bool toggleValue)
        {
            foreach (GameEvent gameEvent in gameEvents)
            {
                gameEvent.LogToConsole = toggleValue;
                EditorUtility.SetDirty(gameEvent);
            }
            
            AssetDatabase.SaveAssets();
        }
        
        #endregion

        #region Misc

        /// <summary>
        /// Gets all assets of type <see cref="GameEvent"/> in the project.
        /// </summary>
        /// <returns>
        /// An array of <see cref="GameEvent"/> assets or null if no such assets exist in the project.
        /// </returns>
        private static GameEvent[] GetGameEventAssets()
        {
            string[] guids = AssetDatabase.FindAssets("t:gameevent");

            if (guids.IsNullOrEmpty())
            {
                return null;
            }
            
            var events = new GameEvent[guids.Length];

            for (int i = 0, count = events.Length; i < count; i++)
            {
                events[i] = AssetDatabase.LoadAssetAtPath<GameEvent>(AssetDatabase.GUIDToAssetPath(guids[i]));
            }

            return events;
        }

        #endregion
    }
}