using Sirenix.OdinInspector;
using Unidork.Attributes;
using Unidork.UniRx;
#if ADDRESSABLES
using Unidork.SceneManagement;
#endif
using UniRx;
using UnityEngine;

namespace Unidork.GameStates
{
    /// <summary>
    /// Switches game states in response to in-game events.
    /// </summary>
    public class GameStateSwitcher : MonoBehaviour
    {
	    #region Properties

	    /// <summary>
	    /// Current game state object.
	    /// </summary>
	    /// <value>
	    /// Gets and sets the value of the field value.
	    /// </value>
	    public GameState CurrentGameState
	    {
		    get => currentGameState;

		    set
		    {
			    previousGameState = currentGameState;
			    
			    bool switchingToSameState = currentGameState == value;

			    if (currentGameState != null && !switchingToSameState)
			    {
				    currentGameState.OnStateExited();
			    }

			    currentGameState = value;

			    if (currentGameState != null && !switchingToSameState)
			    {
				    currentGameState.OnStateEntered();
			    }

			    CurrentGameStateReactive.Value = value;
		    }
	    }

	    /// <summary>
	    /// Reactive property that other objects can subscribe to track current game state changes.
	    /// </summary>
	    public static ReactivePropertyWithOldValue<GameState> CurrentGameStateReactive { get; private set; }

	    /// <summary>
	    /// Previous game state.
	    /// </summary>
	    /// <returns>
	    /// Gets the value of the field previousGameState.
	    /// </returns>
	    public GameState PreviousGameState => previousGameState;

	    #endregion
	    
        #region Fields        

	    /// <summary>
	    /// Game state to set on start. If left blank, will default to game state with name "Splash".
	    /// </summary>
	    [Space, SettingsHeader, Space]
	    [Tooltip("Game state to set on start. If left blank, will default to game state with name \"Splash\".")]
	    [SerializeField]
	    private GameState startState;

        /// <summary>
        /// Current game state.
        /// </summary>
        [ShowInInspector, ReadOnly]
        private GameState currentGameState;

        /// <summary>
        /// Previous game state.
        /// </summary>
        [ShowInInspector, ReadOnly]
        private GameState previousGameState;
        
	    /// <summary>
	    /// Should state changes be logged to console?
	    /// </summary>
	    [Space, DebugHeader, Space] 
	    [Tooltip("Should state changes be logged to console?")]
	    [SerializeField]
	    private bool logStateChangesToConsole;

		#endregion

		#region Init

		protected virtual void Awake()
		{
#if ADDRESSABLES
			if (!SplashSceneLoader.AlreadyLoadedSplash)
			{
				return;
			}
#endif
			CurrentGameStateReactive = new ReactivePropertyWithOldValue<GameState>(null);
			
			SwitchToState(startState);

			if (transform.parent == null)
			{ 
				DontDestroyOnLoad(gameObject);
			}
		}

	    #endregion

		#region Switch

		/// <summary>
		/// Switches the current game state to the passed value.
		/// </summary>
		/// <param name="newGameState">New game state.</param>
		public virtual void SwitchToState(GameState newGameState)
		{
            CurrentGameState = newGameState;

			if (!logStateChangesToConsole || newGameState == null)
			{
				return;
			}
			
			Debug.Log($"Switched to state: {newGameState.StateName}");
		}

		#endregion
	}
}