using Sirenix.OdinInspector;
using Unidork.Attributes;
using Unidork.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unidork.StateMachine
{
	/// <summary>
	/// Represents a state in a finite state machine.
	/// </summary>
	public abstract class FSMState : ScriptableObject
	{
		#region Properties

		/// <summary>
		/// State name.
		/// </summary>
		/// <value>
		/// Gets the value of the string field stateName.
		/// </value>
		public string StateName => stateName;

		/// <summary>
		/// Should transitions from this state to itself be allowed?
		/// </summary>
		/// <value>
		/// Gets the value of the boolean field allowTransitionsFromSameState.
		/// </value>
		public bool AllowSelfTransition => allowSelfTransition;

#if UNITY_EDITOR
		public Color SceneGizmoColor => sceneGizmoColor;
#endif

		#endregion

		#region Fields

#if UNITY_EDITOR
		[Space, DebugHeader, Space]
		[SerializeField]
		private Color sceneGizmoColor = Color.gray;
#endif

		/// <summary>
		/// Should transitions from this state to itself be allowed?
		/// </summary>
		[Space, GeneralHeader, Space]
		[Tooltip("Should transitions from this state to itself be allowed?")]
		[SerializeField]
		private bool allowSelfTransition = false;

		/// <summary>
		/// State name.
		/// </summary>
		[Tooltip("State name.")]
		[SerializeField]
		protected string stateName = "";

		/// <summary>
		/// Optional actions to perform when the state is entered.
		/// </summary>
		[Space, Title("ENTRY", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false), Space]
		[Tooltip("Optional actions to perform when the state is entered.")]
		[FormerlySerializedAs("enterActions")]
		[SerializeField]
		protected FSMAction[] entryActions = null;

		/// <summary>
		/// Optional transitions to check when the state is entered.
		/// </summary>
		[Tooltip("Optional transitions to check when the state is entered.")]
		[SerializeField]
		protected FSMTransition[] entryTransitions = null;

		/// <summary>
		/// Actions that can be performed while this state is the active one.
		/// </summary>
		[Space, Title("UPDATE", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false), Space]
		[Tooltip("Actions that can be performed while this state is the active one.")]
		[SerializeField]
		protected FSMAction[] actions = null;

		/// <summary>
		/// Transitions to other states to evaluate when this state is the active one.
		/// </summary>
		[Tooltip("Transitions to other states to evaluate when this state is the active one.")]
		[SerializeField]
		protected FSMTransition[] transitions = null;

		/// <summary>
		/// Optional actions to perform when the state is exited.
		/// </summary>
		[Space, Title("EXIT", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false), Space]
		[Tooltip("Optional actions to perform when the state is exited.")]
		[SerializeField]
		protected FSMAction[] exitActions = null;

		

		#endregion

		#region State

		/// <summary>
		/// Callback for entering the state.
		/// </summary>
		/// <param name="stateController">Controller that handles updating states of a finite state machine.</param>
		/// <param name="enteredViaPreviousStateDummy">Sometimes a state is entered via the PreviousState
		/// dummy, which is used in one state to return to a previous state when a decision is resolved.
		/// In that case we don't always need to fire all the state entry logic anew.
		/// Inheriting states can use this parameter to decide whether a state needs to be re-initialized
		/// in entry.</param>
		public virtual void OnStateEntered(FSMController stateController, bool enteredViaPreviousStateDummy)
		{
			if (stateController.LogStateChangesToConsole)
			{
				Debug.Log($"{stateController.gameObject} is entering state {name}", this);
			}

			if (!entryActions.IsNullOrEmpty())
			{
				foreach (FSMAction enterAction in entryActions)
				{
					enterAction.Perform(stateController);
				}
			}

			if (entryTransitions.IsNullOrEmpty())
			{
				return;
			}

			foreach (FSMTransition entryTransition in entryTransitions)
			{
				if (entryTransition.IsForcedTransition)
				{
					stateController.TransitionToState(entryTransition.ForcedState);
					break;
				}

				bool decisionPositive = entryTransition.Decision.Decide(stateController);

				if (decisionPositive)
				{
					stateController.TransitionToState(entryTransition.StateWhenTrue);
					break;
				}
				else
				{
					FSMState stateWhenFalse = entryTransition.StateWhenFalse;

					if (stateWhenFalse != null)
					{
						stateController.TransitionToState(stateWhenFalse);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Callback for exitting the state.
		/// </summary>
		/// <param name="stateController">Controller that handles updating states of a finite state machine.</param>
		public virtual void OnStateExited(FSMController stateController) 
		{
			if (stateController.LogStateChangesToConsole)
			{
				Debug.Log($"{stateController.gameObject} is exiting state {name}", this);
			}

			if (exitActions.IsNullOrEmpty())
			{
				return;
			}

			foreach (FSMAction exitAction in exitActions)
			{
				exitAction.Perform(stateController);
			}
		}

		/// <summary>
		/// Updates current state by first performing its actions and then checking whether a transition to another state should happen.
		/// </summary>
		/// <param name="stateController">State controller driving the behavior of owning object.</param>
		public void UpdateState(FSMController stateController)
		{
			PerformActions(stateController);
			CheckTransitions(stateController);
		}

		/// <summary>
		/// Performs actions assigned to this state.
		/// </summary>
		/// <param name="stateController">State controller driving the behavior of owning object.</param>
		protected void PerformActions(FSMController stateController)
		{
			if (actions.IsNullOrEmpty())
			{
				return;
			}

			foreach (FSMAction action in actions)
			{
				action.Perform(stateController);
			}
		}

		protected void CheckTransitions(FSMController stateController)
		{
			foreach (FSMTransition transition in transitions)
			{
				if (transition.IsForcedTransition)
				{
					stateController.TransitionToState(transition.ForcedState);
					break;
				}

				bool decisionPositive = transition.Decision.Decide(stateController);

				if (decisionPositive)
				{
					FSMState stateWhenTrue = transition.StateWhenTrue;

					if (stateWhenTrue == null)
					{
						continue;
					}

					stateController.TransitionToState(stateWhenTrue);
					break;
				}
				else
				{
					FSMState stateWhenFalse = transition.StateWhenFalse;

					if (stateWhenFalse == null)
					{
						continue;
					}

					stateController.TransitionToState(stateWhenFalse);
					break;
				}
			}
		}

		#endregion
	}
}