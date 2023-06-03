using Sirenix.OdinInspector;
using Unidork.Attributes;
using Unidork.SceneManagement;
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
	    /// Reactive property that other objects can subscribe to to track current game state changes.
	    /// </summary>
	    public ReactiveProperty<GameState> CurrentGameStateReactive { get; } = new(null);

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
        /// Array storing all in-game states.
        /// </summary>
        [Space, Title("STATES", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false), Space]
        [Tooltip("Array storing all in-game states.")]
        [SerializeField]
        private GameState[] gameStates;

	    /// <summary>
	    /// Game state to set on start. If left blank, will default to game state with name "Splash".
	    /// </summary>
	    [Tooltip(("Game state to set on start. If left blank, will default to game state with name \"Splash\"."))]
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
			if (!SplashSceneLoader.AlreadyLoadedSplash)
			{
				SwitchToState("Splash");
				return;
			}
			
			if (startState != null)
		    {
			    SwitchToState(startState);
			    return;
		    }
		    
		    SwitchToState("Splash");
	    }

	    #endregion

		#region Switch

		/// <summary>
		/// Switches the current game state to the passed value.
		/// </summary>
		/// <param name="newGameState">New game state.</param>
		public void SwitchToState(GameState newGameState)
		{
            CurrentGameState = newGameState;

			if (!logStateChangesToConsole || newGameState == null)
			{
				return;
			}
			
			Debug.Log($"SWITCHED TO STATE: {newGameState.StateName}");
		}

        /// <summary>
        /// Switches current game state to the state with the passed name.
        /// </summary>
        /// <param name="stateName">State name.</param>
        public void SwitchToState(string stateName)
		{
            SwitchToState(GetStateWithName(stateName));
		}

		#endregion

		#region States

        /// <summary>
        /// Gets the state with the specified name.
        /// </summary>
        /// <param name="gameStateName">State name.</param>
        /// <returns>
        /// Game state from the <see cref="gameStates"/> array whose
        /// name matches the passed value or null if suck state doesn't exist.s
        /// </returns>
        protected GameState GetStateWithName(string gameStateName)
		{
            foreach (GameState gameState in gameStates)
			{
                if (gameState.StateName == gameStateName)
				{
                    return gameState;
				}
			}

            return null;
		}

		#endregion
	}
}