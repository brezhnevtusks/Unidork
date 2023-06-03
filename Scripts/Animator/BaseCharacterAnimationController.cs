using Sirenix.OdinInspector;
using Unidork.Attributes;
using Unidork.Constants;
using Unidork.Extensions;
using Unidork.StateMachine;
using UniRx;
using UnityEngine;

namespace Unidork.Animations
{
	/// <summary>
	/// Class that can be used as a base for character animation controllers.
	/// </summary>
	public class BaseCharacterAnimationController : MonoBehaviour
    {
	    #region Properties
	    
	    /// <summary>
	    /// Component that controls the Mechanim animation system.
	    /// </summary>
	    /// <value>
	    /// Gets the value of the field animator.
	    /// </value>
	    public Animator Animator => animator;

	    #endregion
	    
		#region Fields

		/// <summary>
		/// Component that controls the Mechanim animation system.
		/// </summary>
		[Space, BaseHeader, Space]
		[Tooltip("Component that controls the Mechanim animation system.")]
		[SerializeField]
		protected Animator animator = null;

		/// <summary>
		/// Should this animation controller change animations following 
		/// state changes in a connected FSMController?
		/// </summary>
		[Tooltip("Should this animation controller change animations following " +
				 "state changes in a connected FSMController?")]
		[SerializeField]
		protected bool subscribeToFsmController = false;

		/// <summary>
		/// FSM controller that this animation controller tracks.
		/// Whenever a new FSM state is entered, animator controllers triggers a new animation.
		/// </summary>
		[ShowIf("@this.subscribeToFsmController == true")]
		[Tooltip("FSM controller that this animation controller tracks." +
				 "Whenever a new FSM state is entered, animator controllers triggers a new animation.")]
		[SerializeField]
		protected FSMController fsmController = null;

		#endregion

		#region Init

		private void Awake()
		{
			if (animator != null)
			{
				return;
			}

			animator = gameObject.GetComponentInChildrenNonAlloc<Animator>();
		}

		protected virtual void Start()
		{
			if (!subscribeToFsmController)
			{
				return;
			}

			fsmController.ReactiveCurrentState.Subscribe(newState => PlayAnimationOnFSMStateChange(newState));
		}

		#endregion

		#region Animations		

		/// <summary>
		/// Sets the idle trigger on the animator if idle animation isn't already running.
		/// </summary>
		public virtual void PlayIdleAnimation()
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsTag(AnimatorConstants.IdleAnimatorTag))
			{
				return;
			}

			animator.SetTrigger(AnimatorConstants.IdleAnimatorHash);
		}

		/// <summary>
		/// Sets the stand trigger on the animator if stand animation isn't already running.
		/// </summary>
		public virtual void PlayStandAnimation()
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsTag(AnimatorConstants.StandAnimatorTag))
			{
				return;
			}

			animator.SetTrigger(AnimatorConstants.StandAnimatorHash);
		}

		/// <summary>
		/// Sets the walk trigger on the animator if walk animation isn't already running.
		/// </summary>
		public virtual void PlayWalkAnimation()
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsTag(AnimatorConstants.WalkAnimatorTag))
			{
				return;
			}

			animator.SetTrigger(AnimatorConstants.WalkAnimatorHash);
		}

		/// <summary>
		/// Sets the run trigger on the animator if run animation isn't already running.
		/// </summary>
		public virtual void PlayRunAnimation()
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsTag(AnimatorConstants.RunAnimatorTag))
			{
				return;
			}

			animator.SetTrigger(AnimatorConstants.RunAnimatorHash);
		}

		/// <summary>
		/// Sets the sneak trigger on the animator if sneak animation isn't already running.
		/// </summary>
		public virtual void PlaySneakAnimation()
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsTag(AnimatorConstants.SneakAnimatorTag))
			{
				return;
			}

			animator.SetTrigger(AnimatorConstants.SneakAnimatorHash);
		}

		/// <summary>
		/// Sets the crouch trigger on the animator if crouch animation isn't already running.
		/// </summary>
		public virtual void PlayCrouchAnimation()
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsTag(AnimatorConstants.CrouchAnimatorTag))
			{
				return;
			}

			animator.SetTrigger(AnimatorConstants.CrouchAnimatorHash);
		}

		/// <summary>
		/// Sets the jump trigger on the animator if jump animation isn't already running.
		/// </summary>
		public virtual void PlayJumpAnimation()
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsTag(AnimatorConstants.JumpAnimatorTag))
			{
				return;
			}

			animator.SetTrigger(AnimatorConstants.JumpAnimatorHash);
		}

		/// <summary>
		/// Sets the aim trigger on the animator if aim animation isn't already running.
		/// </summary>
		public virtual void PlayAimAnimation()
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsTag(AnimatorConstants.AimAnimatorTag))
			{
				return;
			}

			animator.SetTrigger(AnimatorConstants.AimAnimatorHash);
		}

		/// <summary>
		/// Sets the attack trigger on the animator if attack animation isn't already running.
		/// </summary>
		public virtual void PlayAttackAnimation()
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsTag(AnimatorConstants.AttackAnimatorTag))
			{
				return;
			}

			animator.SetTrigger(AnimatorConstants.AttackAnimatorHash);
		}

		/// <summary>
		/// Sets the die trigger on the animator if die animation isn't already running.
		/// </summary>
		public virtual void PlayDieAnimation()
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsTag(AnimatorConstants.DieAnimatorTag))
			{
				return;
			}

			animator.SetTrigger(AnimatorConstants.DieAnimatorHash);
		}

		/// <summary>
		/// Sets the dance trigger on the animator if dance animation isn't already running.
		/// </summary>
		public virtual void PlayDanceAnimation()
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsTag(AnimatorConstants.DanceAnimatorTag))
			{
				return;
			}

			animator.SetTrigger(AnimatorConstants.DanceAnimatorHash);
		}

		/// <summary>
		/// Sets the celebrate trigger on the animator if celebrate animation isn't already running.
		/// </summary>
		public virtual void PlayCelebrateAnimation()
		{
			if (animator.GetCurrentAnimatorStateInfo(0).IsTag(AnimatorConstants.CelebrateAnimatorTag))
			{
				return;
			}

			animator.SetTrigger(AnimatorConstants.CelebrateAnimatorHash);
		}

		/// <summary>
		/// Plays an animation that corresponds with the passed FSM state.
		/// </summary>
		/// <param name="newState">New FSM state.</param>
		protected virtual void PlayAnimationOnFSMStateChange(FSMState newState)
		{
			if (newState == null)
			{
				return;				
			}

			switch (newState.StateName)
			{
				case "Idle":
					PlayIdleAnimation();
					break;
				case "Stand":
					PlayStandAnimation();
					break;
				case "Walk":
					PlayWalkAnimation();
					break;
				case "Run":
					PlayRunAnimation();
					break;
				case "Sneak":
					PlaySneakAnimation();
					break;
				case "Crouch":
					PlayCrouchAnimation();
					break;
				case "Jump":
					PlayJumpAnimation();
					break;
				case "Aim":
					PlayAimAnimation();
					break;
				case "Attack":
					PlayAttackAnimation();
					break;
				case "Die":
					PlayDieAnimation();
					break;
				case "Dance":
					PlayDanceAnimation();
					break;
				case "Celebrate":
					PlayCelebrateAnimation();
					break;
				default:
					break;
			}
		}

		#endregion

	    #region State Info
	    
	    /// <summary>
	    /// Checks whether the animator is in a specific state.
	    /// </summary>
	    /// <param name="stateName">State name.</param>
	    /// <param name="animatorLayerIndex">Index of the animator layer to check.</param>
	    /// <returns>True if the animator is in a state with the passed name, False otherwise.</returns>
	    public bool IsInState(string stateName, int animatorLayerIndex = 0) => animator.IsInState(stateName, animatorLayerIndex);

	    /// <summary>
	    /// Checks whether the animator is in a specific state.
	    /// </summary>
	    /// <param name="stateNameHash">Hash of the state name.</param>
	    /// <param name="animatorLayerIndex">Index of the animator layer to check.</param>
	    /// <returns>True if the animator is in a state with the passed hash, False otherwise.</returns>
	    public bool IsInState(int stateNameHash, int animatorLayerIndex = 0) => animator.IsInState(stateNameHash, animatorLayerIndex);

	    /// <summary>
	    /// Checks whether the animator is in a state that has a specific tag.
	    /// </summary>
	    /// <param name="tag">Tag name.</param>
	    /// <param name="animatorLayerIndex">Index of the animator layer to check.</param>
	    /// <returns>True if the animator is in a state with the passed tag name, False otherwise.</returns>
	    public bool IsInStateWithTag(string tag, int animatorLayerIndex = 0) => animator.IsInStateWithTag(tag, animatorLayerIndex);

	    /// <summary>
	    /// Checks whether the animator is in a state that has a specific tag.
	    /// </summary>
	    /// <param name="tagHash">Tag hash.</param>
	    /// <param name="animatorLayerIndex">Index of the animator layer to check.</param>
	    /// <returns>True if the animator is in a state with the passed tag hash, False otherwise.</returns>
	    public bool IsInStateWithTag(int tagHash, int animatorLayerIndex) => animator.IsInStateWithTag(tagHash, animatorLayerIndex);

	    #endregion

		#region Speed

		/// <summary>
		/// Sets the animator speed to the passed value.
		/// </summary>
		/// <param name="speed">Speed.</param>
		public virtual void SetAnimatorSpeed(float speed) => animator.speed = speed;

		#endregion

		#region Masks

		/// <summary>
		/// Sets the weight of an animator layer to the passed value.
		/// </summary>
		/// <param name="layerName">Layer name.</param>
		/// <param name="weight">Weight value.</param>
		public void SetLayerWeight(string layerName, float weight)
		{
			animator.SetLayerWeight(animator.GetLayerIndex(layerName), weight);
		}

		/// <summary>
		/// Sets the weight of an animator layer to the passed value.
		/// </summary>
		/// <param name="layerIndex">Layer index.</param>
		/// <param name="weight">Weight value.</param>
		public void SetLayerWeight(int layerIndex, float weight)
		{
			animator.SetLayerWeight(layerIndex, weight);
		}

		#endregion

		#region Pause

		/// <summary>
		/// Pauses the animator by settings its speed to 0.
		/// </summary>
		public void PauseAnimator() => animator.speed = 0f;

		/// <summary>
		/// Unpauses the animator by settings its speed to 0.
		/// </summary>
		public void UnpauseAnimator() => animator.speed = 1f;

		#endregion
	}
}