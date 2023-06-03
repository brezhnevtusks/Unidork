using UnityEngine;

namespace Unidork.StateMachine
{
	/// <summary>
	/// Represents a decision made by a finite state machine.
	/// </summary>
	public abstract class FSMDecision : ScriptableObject
	{
		/// <summary>
		/// Evaluates a decision that a state machine has to make.
		/// </summary>
		/// <param name="stateController">State controller driving the behavior of owning object.</param>
		/// <returns>
		/// True if a positive decision is made when evaluating conditions, False otherwise.
		/// </returns>
		public abstract bool Decide(FSMController stateController);
	}
}