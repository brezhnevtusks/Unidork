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
	    /// Reactive property that other objects can subscribe to track current game state changes.
	    /// </summary>
	    public static ReactivePropertyWithOldValue<GameState> GameState { get; private set; }

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
			GameState = new ReactivePropertyWithOldValue<GameState>(null);
			GameState
				.Subscribe(OnStateSwitched)
				.AddTo(this);
			
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
            GameState.Value = newGameState;

            if (logStateChangesToConsole && newGameState != null)
            {
	            Debug.Log($"Switched to state: {newGameState.StateName}");
            }
		}

		private void OnStateSwitched(GameState newGameState)
		{
			if (newGameState != null)
			{
				newGameState.OnStateEntered();
			}
			else
			{
				Debug.LogError("Trying to switch to a null GameState!");
			}

			GameState oldGameState = GameState.OldValue;

			if (oldGameState != null)
			{
				oldGameState.OnStateExited();
			}
		}

		#endregion
	}
}