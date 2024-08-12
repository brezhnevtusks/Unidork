using UnderdorkStudios.UnderMessages;
using UnderdorkStudios.UnderTags;
using Unidork.Attributes;
using UnityEngine;

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
		/// Gets the value of the <see cref="UnderTag"/> field stateName.
		/// </value>
		public UnderTag StateName => stateName;

		/// <summary>
		/// Optional message to send when the state is entered.
		/// </summary>
		public UnderTag MessageToSendOnStateEntered => messageToSendOnStateEntered;

		/// <summary>
		/// Optional message to send when the state is exited.
		/// </summary>
		public UnderTag MessageToSendOnStateExited => messageToSendOnStateExited;

		#endregion

		#region Constants

		public const string Game_State_Channel = "Message.GameState";

		#endregion

		#region Fields

		/// <summary>
		/// Name of the game state.
		/// </summary>
		[Space, SettingsHeader, Space]
		[Tooltip("Name of the game state.")]
		[SerializeField]
		private UnderTag stateName = null;

		/// <summary>
		/// Optional message to send when the state is entered.
		/// </summary>
		[Space, EventsHeader, Space]
		[Tooltip("Optional message to send when the state is entered.")]
		[SerializeField]
		private UnderTag messageToSendOnStateEntered = null;

		/// <summary>
		/// Optional message to send when the state is exited.
		/// </summary>
		[Tooltip("Optional message to send when the state is exited.")]
		[SerializeField]
		private UnderTag messageToSendOnStateExited = null;
		
		#endregion

		#region State

		/// <summary>
		/// Called when the state is entered.
		/// </summary>
		public void OnStateEntered()
		{
			if (messageToSendOnStateEntered.IsValid())
			{
				UnderMessageSystem.SendMessage(channelName: Game_State_Channel, messageName:messageToSendOnStateEntered);
			}
		}

		/// <summary>
		/// Called when the state is exited.
		/// </summary>
		public void OnStateExited()
		{
			if (messageToSendOnStateExited.IsValid())
			{
				UnderMessageSystem.SendMessage(channelName: Game_State_Channel, messageName: messageToSendOnStateExited);
			}
		}

		#endregion
	}
}