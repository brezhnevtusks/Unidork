using System.Collections.Generic;
using UnityEngine;

namespace Unidork.Utility
{
    /// <summary>
    /// Utility class that can be used to notify objects of the level destroy event. 
    /// </summary>
    public static class LevelDestroyNotifier
    {
        #region Fields

        /// <summary>
        /// Current listeners.
        /// </summary>
        private static readonly HashSet<ILevelDestroyListener> listeners;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        static LevelDestroyNotifier()
        {
            listeners = new HashSet<ILevelDestroyListener>();
        }

        #endregion

        #region Listeners

        /// <summary>
        /// Add a listener to the list.
        /// </summary>
        /// <param name="levelDestroyListener">Listener to add.</param>
        public static void RegisterListener(ILevelDestroyListener levelDestroyListener)
        {
            listeners.Add(levelDestroyListener);
        }

        
        /// <summary>
        /// Removes a listener from the list. Logs an error if the listener is null.
        /// </summary>
        /// <param name="levelDestroyListener">Listener to remove.</param>
        public static void UnregisterListener(ILevelDestroyListener levelDestroyListener)
        {
            if (levelDestroyListener == null)
            {
                Debug.LogError("Trying to unregister a null level object!");
                return;
            }
            
            listeners.Remove(levelDestroyListener);
        }

        /// <summary>
        /// Notifies all current listeners that level has been destroyed. Logs errors if any of the listeners have been destroyed without unregistering.
        /// </summary>
        public static void NotifyOfLevelDestroy()
        {
            foreach (ILevelDestroyListener levelObject in listeners)
            {
                if (levelObject == null)
                {
                    Debug.LogError("Level object manager is trying to notify a null object!");
                    continue;
                }
                
                levelObject.OnLevelDestroyed();
            }
            
            listeners.Clear();
        }

        #endregion
    }
}