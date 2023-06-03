using Sirenix.OdinInspector;
using Unidork.Attributes;
using UniRx;
using UnityEngine;

namespace Unidork.StateMachine
{
	/// <summary>
	/// Controller that handles updating states of a finite state machine.
	/// </summary>
	public class FSMController : MonoBehaviour
	{
		#region Properties

		/// <summary>
		/// UniRx reactive property that stores changes in current state that other classes can subscribe to.
		/// </summary>
		public ReactiveProperty<FSMState> ReactiveCurrentState { get; private set; }

		/// <summary>
		/// Controller's current state.
		/// </summary>
		public FSMState CurrentState
		{
			get => currentState;

			private set
			{
				bool enteringNullState = value == null;

				var enteredViaPreviousStateDummy = false;

				bool enteringSameState = false;

				if (previousState == currentState && previousState != null)
				{
					enteringSameState = true;
				}

				if (!enteringNullState && value.StateName.Equals("PreviousState"))
				{
					value = previousState;
					enteredViaPreviousStateDummy = true;
				}

				if (currentState != null)
				{
					previousState = currentState;
					currentState.OnStateExited(this);
				}
				
				currentState = value;

				if (enteringSameState)
				{
					if (previousState.AllowSelfTransition)
					{
						ReactiveCurrentState.SetValueAndForceNotify(currentState);
					}
				}
				else
				{
					ReactiveCurrentState.Value = currentState;
				}				

				if (enteringNullState)
				{
					return;
				}	

				if (enteringSameState)
				{
					if (previousState.AllowSelfTransition)
					{
						currentState.OnStateEntered(this, enteredViaPreviousStateDummy);
					}
				}
				else
				{
					currentState.OnStateEntered(this, enteredViaPreviousStateDummy);
				}				
			}
		}

		/// <summary>
		/// Controller's prevoius state.
		/// </summary>
		public FSMState PreviousState => previousState;

		/// <summary>
		/// Object that holds data used by the state controller.
		/// </summary>
		public FSMControllerData StateControllerData { get; set; }

		/// <summary>
		/// Should state changes like entering and exiting be logged to console?
		/// </summary>
		/// <value>
		/// Gets the value of the boolean field logStateChangesToConsole.
		/// </value>
		public bool LogStateChangesToConsole => logStateChangesToConsole;

#endregion

		#region Fields	

		/// <summary>
		/// Current state.
		/// </summary>
		[Space, DebugHeader, Space]
		[SerializeField, ReadOnly]
		private FSMState currentState;

		/// <summary>
		/// Should state changes like entering and exiting be logged to console?
		/// </summary>
		[Tooltip("Should state changes like entering and exiting be logged to console?")]
		[SerializeField]
		private bool logStateChangesToConsole = false;

		/// <summary>
		/// Previous state.
		/// </summary>
		private FSMState previousState;

		#endregion

		#region Data

		/// <summary>
		/// Gets state controller data. Used by derived classes to get specific types of controller data.
		/// </summary>
		/// <typeparam name="T">Type of data to get.</typeparam>
		/// <returns>
		/// <see cref="StateControllerData"/> cast to the specified type.
		/// </returns>
		public T GetStateControllerData<T>() where T : FSMControllerData
		{
			return (T)StateControllerData;
		}

		#endregion

		#region Init

		/// <summary>
		/// Sets the state of the controller.
		/// </summary>
		/// <param name="startState">New state.</param>
		public void SetState(FSMState startState)
		{
			CurrentState = startState;			
		}

		private void Awake()
		{
			ReactiveCurrentState = new ReactiveProperty<FSMState>(null);
		}

		#endregion

		#region Enable/disable

		/// <summary>
		/// Enables the state controller.
		/// </summary>
		public void Enable() => enabled = true;

		/// <summary>
		/// Disables the state controller.
		/// </summary>
		public void Disable() => enabled = false;

		#endregion

		#region Update

		private void Update()
		{
			if (currentState == null)
			{
				return;
			}

			currentState.UpdateState(this);
		}

		#endregion

		#region Transitions

		/// <summary>
		/// Transitions to a new state passed to the method.
		/// </summary>
		/// <param name="newState">New state.</param>
		public void TransitionToState(FSMState newState)
		{
			CurrentState = newState;
		}

		#endregion

		#region Editor

#if UNITY_EDITOR

		private void OnDrawGizmos()
		{
			if (currentState != null)
			{
				Color oldColor = Gizmos.color;

				Gizmos.color = currentState.SceneGizmoColor;
				Gizmos.DrawSphere(transform.position + Vector3.up * 2f, 0.5f);

				Gizmos.color = oldColor;
			}
		}
#endif

		#endregion
	}
}