using Unidork.Attributes;
using Unidork.Events;
using UnityEngine;

#if UNITY_EDITOR 
using Sirenix.OdinInspector;
using UnityEditor;
#endif

namespace Unidork.GameStates
{
	/// <summary>
	/// Scriptable object that stores data and handles operations with a game state.
	/// </summary>
    [CreateAssetMenu(fileName = "GS_", menuName = "Game States/New Game State", order = 0)]
    public class GameState : ScriptableObject
    {
		#region Properties

		/// <summary>
		/// Name of the game state.
		/// </summary>
		/// <value>
		/// Gets the value of the string field stateName.
		/// </value>
		public string StateName => stateName;

		#endregion

		#region Fields

		/// <summary>
		/// Name of the game state.
		/// </summary>
		[Space, SettingsHeader, Space]
		[Tooltip("Name of the game state.")]
		[SerializeField]
		private string stateName = null;

		/// <summary>
		/// Optional event to raise when the state is entered.
		/// </summary>
		[Space, EventsHeader, Space]
		[Tooltip("Optional event to raise when the state is entered.")]
		[SerializeField]
		private GameEvent eventToRaiseOnStateEntered = null;

		/// <summary>
		/// Optional event to raise when the state is exited.
		/// </summary>
		[Tooltip("Optional event to raise when the state is exited.")]
		[SerializeField]
		private GameEvent eventToRaiseOnStateExited = null;

#if UNITY_EDITOR
	    [Button("CREATE EVENTS", ButtonSizes.Medium), GUIColor(0, 1, 0)]
		private void CreateEvents()
		{
			string path = AssetDatabase.GetAssetPath(this);
			string[] splitPath = path.Split('.');
			string folderPath = splitPath[0].Replace(name, "");

			GameEvent onEnteredEvent = ScriptableObject.CreateInstance<GameEvent>();
			onEnteredEvent.name = $"EVT_OnEntered{stateName}GameState";
			eventToRaiseOnStateEntered = onEnteredEvent;
			AssetDatabase.CreateAsset(onEnteredEvent, folderPath + onEnteredEvent.name + ".asset");
			
			GameEvent onExitedEvent = CreateInstance<GameEvent>();
			onExitedEvent.name = $"EVT_OnExited{stateName}GameState";
			eventToRaiseOnStateExited = onExitedEvent;
			AssetDatabase.CreateAsset(onExitedEvent, folderPath + onExitedEvent.name + ".asset");
			
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
#endif
		
		#endregion

		#region State

		/// <summary>
		/// Called when the state is entered.
		/// </summary>
		public void OnStateEntered()
		{
			if (eventToRaiseOnStateEntered != null)
			{
				eventToRaiseOnStateEntered.Raise();
			}
		}

		/// <summary>
		/// Called when the state is exited.
		/// </summary>
		public void OnStateExited()
		{
			if (eventToRaiseOnStateExited != null)
			{
				eventToRaiseOnStateExited.Raise();
			}
		}

		#endregion
	}
}