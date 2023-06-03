using Sirenix.OdinInspector;
using Unidork.Attributes;
using UnityEngine;

namespace Unidork.StateMachine
{
	/// <summary>
	/// Represents a transition between states in a finite state machine.
	/// </summary>
	/// <typeparam name="T">Type of data used in the finite state machine.</typeparam>
	[System.Serializable]
	public class FSMTransition
	{
		#region Properties

		/// <summary>
		/// When set to True, this transition will be forced to happen regardless of any decisions.
		/// </summary>
		/// <value>
		/// Gets the value of the boolean field isForcedTransition.
		/// </value>
		public bool IsForcedTransition => isForcedTransition;

		/// <summary>
		/// State to transition to when isForcedTransition is set to True.
		/// </summary>
		/// <value>
		/// Gets the value of the field forced state.
		/// </value>
		public FSMState ForcedState => forcedState;

		/// <summary>
		/// Decision to use to decide the outcome of the transition.
		/// </summary>
		/// <value>
		/// Gets the value of the field decision.
		/// </value>
		public FSMDecision Decision => decision;

		/// <summary>
		///  State to go to when decision returns a positive evaluation.
		/// </summary>
		/// <value>
		/// Gets the value of the field stateWhenTrue.
		/// </value>
		public FSMState StateWhenTrue => stateWhenTrue;

		/// <summary>
		///  State to go to when decision returns a negative evaluation.
		/// </summary>
		/// <value>
		/// Gets the value of the field stateWhenFalse.
		/// </value>
		public FSMState StateWhenFalse => stateWhenFalse;

		#endregion

		#region Fields

		/// <summary>
		/// When set to True, this transition will be forced to happen regardless of any decisions.
		/// </summary>
		[Tooltip("When set to True, this transition will be forced to happen regardless of any decisions.")]
		[SerializeField]
		private bool isForcedTransition = false;

		/// <summary>
		/// State to transition to when isForcedTransition is set to True.
		/// </summary>
		[ShowIf("@this.isForcedTransition == true")]
		[Tooltip("State to transition to when isForcedTransition is set to True.")]
		[SerializeField]
		private FSMState forcedState = null;

		/// <summary>
		/// Decision to use to decide the outcome of the transition.
		/// </summary>
		[HideIf("@this.isForcedTransition == true")]
		[Tooltip("Decision to use to decide the outcome of the transition.")]
		[SerializeField]
		protected FSMDecision decision = null;

		/// <summary>
		/// State to go to when decision returns a positive evaluation.
		/// </summary>
		[HideIf("@this.isForcedTransition == true")]
		[Tooltip("State to go to when decision returns a positive evaluation.")]
		[SerializeField]
		protected FSMState stateWhenTrue = null;

		/// <summary>
		/// State to go to when decision returns a negative evaluation.
		/// </summary>
		[HideIf("@this.isForcedTransition == true")]
		[Tooltip("State to go to when decision returns a positive evaluation.")]
		[SerializeField]
		protected FSMState stateWhenFalse = null;

		#endregion
	}
}