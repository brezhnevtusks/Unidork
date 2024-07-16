using System.Collections.Generic;
using UnityEngine;

namespace Unidork.Utility
{
	/// <summary>
	/// Class that handles pausing and unpausing <see cref="IPausable"/> objects.
	/// Objects have to register and unregister themselves from the manager.
	/// Methods can be overriden for additional pause logic.
	/// </summary>
    public class PauseManager : MonoBehaviour
    {
		#region Properties

        /// <summary>
		/// Are objects currently paused?
		/// </summary>
		public static bool ObjectsPaused { get; private set; }

		#endregion

	    #region Fields

	    /// <summary>
	    /// Instance of this component to make sure we don't have multiple managers pausing/unpausing our objects.
	    /// </summary>
	    protected static PauseManager instance;
	    
	    /// <summary>
	    /// List of all pausable objects that registered themselves with this manager.
	    /// </summary>
	    private static readonly HashSet<IPausable> pausableObjects;

	    #endregion

	    #region Constructor

	    /// <summary>
	    /// Constructor.
	    /// </summary>
	    static PauseManager()
	    {
		    pausableObjects = new HashSet<IPausable>();
	    }

	    #endregion

	    #region Init

	    private void Awake()
	    {
		    if (instance != null)
		    {
			    Destroy(gameObject);
			    return;
		    }

		    instance = this;
	    }

	    #endregion

	    #region Pausable objects

	    /// <summary>
	    /// Registers the passed pausable object with the manager.
	    /// </summary>
	    /// <param name="pausableObject">Pausable object.</param>
	    public static void RegisterPausable(IPausable pausableObject)
	    {
		    if (pausableObject == null)
		    {
			    Debug.LogError("Trying to register a pausable object that is null!");
			    return;
		    }
		    _ = pausableObjects.Add(pausableObject);
	    }

	    /// <summary>
	    /// Unregisters the passed pausable object from the manager.
	    /// </summary>
	    /// <param name="pausableObject">Pausable object.</param>
	    public static void UnregisterPausable(IPausable pausableObject)
	    {
		    if (pausableObject == null)
		    {
			    Debug.LogError("Trying to unregister a pausable object that is null!");
			    return;
		    }

		    _ = pausableObjects.Remove(pausableObject);
	    }

	    #endregion

	    #region Pause

	    /// <summary>
	    /// Calls <see cref="IPausable.Pause"/> on all currently registered objects.
	    /// </summary>
	    public static void PauseStatic()
	    {
		    instance.Pause();
	    }
	    
	    /// <summary>
	    /// Calls <see cref="IPausable.Unpause"/> on all currently registered objects.
	    /// </summary>
	    public static void UnpauseStatic()
	    {
		    instance.Unpause();
	    }

	    /// <summary>
	    /// Calls <see cref="IPausable.Pause"/> on all currently registered objects.
	    /// </summary>
	    /// <remarks>Non-static method to be used with events listeners, UnityEvents, etc</remarks>
	    public virtual void Pause()
	    {
		    foreach (IPausable pausableObject in pausableObjects)
		    {
			    pausableObject.Pause();
		    }
			 
			ObjectsPaused = true;
	    }

	    /// <summary>
	    /// Calls <see cref="IPausable.Unpause"/> on all currently registered objects.
	    /// </summary>
	    /// /// <remarks>Non-static method to be used with events listeners, UnityEvents, etc</remarks>
	    public virtual void Unpause()
	    {
		    foreach (IPausable pausableObject in pausableObjects)
		    {
			    pausableObject.Unpause();
		    }
		    
			ObjectsPaused = false;
	    }

	    #endregion
	}
}