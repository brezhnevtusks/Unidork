using UnityEngine;

namespace Unidork.StateMachine
{
	/// <summary>
	/// Base class for scriptable objects that represents finite state machine actions.
	/// </summary>
	public abstract class FSMAction : ScriptableObject
	{
		/// <summary>
		/// Performs the action.
		/// </summary>
		/// <param name="stateController">Controller that handles updating states of a finite state machine.</param>
		public abstract void Perform(FSMController stateController);
	}
}
