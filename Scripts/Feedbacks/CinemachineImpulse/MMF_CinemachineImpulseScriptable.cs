using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.FeedbacksForThirdParty;
using MoreMountains.Tools;
#if CINEMACHINE
using Unity.Cinemachine;
#endif

namespace Unidork.Feedbacks
{
   [AddComponentMenu("")]
#if CINEMACHINE
	[FeedbackPath("Camera/Cinemachine Impulse - Scriptable")]
#endif
	[FeedbackHelp("This feedback lets you trigger a Cinemachine Impulse event. You'll need a Cinemachine Impulse Listener on your camera for this to work.")]
	public class MMF_CinemachineImpulseScriptable : MMF_Feedback
	{
		/// a static bool used to disable all feedbacks of this type at once
		public static bool FeedbackTypeAuthorized = true;
		/// sets the inspector color for this feedback
#if UNITY_EDITOR
		public override Color FeedbackColor { get { return MMFeedbacksInspectorColors.CameraColor; } }
		public override bool HasCustomInspectors => true;
		public override bool HasAutomaticShakerSetup => true;
		public override bool EvaluateRequiresSetup() => impulseDefinitionRef == null;
		public override string RequiredTargetText => impulseDefinitionRef != null ? impulseDefinitionRef.name : "";
		public override string RequiresSetupText => "This feedback requires a CinemachineImpulseDefinitionRef to be set to be able to work properly. You can set one below.";
#endif
		public override bool HasRandomness => true;


#if CINEMACHINE
		[MMFInspectorGroup("Cinemachine Impulse", true, 28)]
		/// the impulse definition to broadcast
		[Tooltip("the impulse definition to broadcast")]
		public CinemachineImpulseDefinitionRef impulseDefinitionRef;
		/// whether or not to clear impulses (stopping camera shakes) when the Stop method is called on that feedback
		[Tooltip("whether or not to clear impulses (stopping camera shakes) when the Stop method is called on that feedback")]
		public bool ClearImpulseOnStop = false;
#endif
		
		[Header("Gizmos")]
		/// whether or not to draw gizmos to showcase the various distance properties of this feedback, when applicable. Dissipation distance in blue, impact radius in yellow.
		[Tooltip("whether or not to draw gizmos to showcase the various distance properties of this feedback, when applicable. Dissipation distance in blue, impact radius in yellow.")]
		public bool DrawGizmos = false;
		
#if CINEMACHINE
		/// the duration of this feedback is the duration of the impulse
		public override float FeedbackDuration => impulseDefinitionRef != null && impulseDefinitionRef.Value != null 
			? impulseDefinitionRef.Value.TimeEnvelope.Duration 
			: 0f;
#endif

		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1.0f)
		{
			if (!Active || !FeedbackTypeAuthorized)
			{
				return;
			}

#if CINEMACHINE
			CinemachineImpulseManager.Instance.IgnoreTimeScale = !InScaledTimescaleMode;
			float intensityMultiplier = ComputeIntensity(feedbacksIntensity, position);
			impulseDefinitionRef.Value.CreateEvent(position, impulseDefinitionRef.Velocity * intensityMultiplier);
#endif
		}

		/// <summary>
		/// Stops the animation if needed
		/// </summary>
		/// <param name="position"></param>
		/// <param name="feedbacksIntensity"></param>
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1)
		{
#if CINEMACHINE
			if (!Active || !FeedbackTypeAuthorized || !ClearImpulseOnStop)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			CinemachineImpulseManager.Instance.Clear();
#endif
		}

		/// <summary>
		/// When adding the feedback we initialize its cinemachine impulse definition
		/// </summary>
		public override void OnAddFeedback()
		{
#if CINEMACHINE
			if (impulseDefinitionRef == null)
			{
				return;
			}
			
			// sets the feedback properties
			impulseDefinitionRef.Value ??= new CinemachineImpulseDefinition();
			impulseDefinitionRef.Value.RawSignal = Resources.Load<NoiseSettings>("MM_6D_Shake");
			impulseDefinitionRef.Velocity = new Vector3(5f, 5f, 5f);
#endif
		}

		/// <summary>
		/// Draws dissipation distance and impact distance gizmos if necessary
		/// </summary>
		public override void OnDrawGizmosSelectedHandler()
		{
			if (!DrawGizmos)
			{
				return;
			}
#if CINEMACHINE
			CinemachineImpulseDefinition impulseDefinition = impulseDefinitionRef.Value;
			
			if (impulseDefinition != null)
			{
				if ( impulseDefinition.ImpulseType is CinemachineImpulseDefinition.ImpulseTypes.Dissipating or
				    CinemachineImpulseDefinition.ImpulseTypes.Propagating or CinemachineImpulseDefinition.ImpulseTypes.Legacy )
				{
					Gizmos.color = MMColors.Aqua;
					Gizmos.DrawWireSphere(Owner.transform.position, impulseDefinition.DissipationDistance);
				}
				if (impulseDefinition.ImpulseType == CinemachineImpulseDefinition.ImpulseTypes.Legacy)
				{
					Gizmos.color = MMColors.ReunoYellow;
					Gizmos.DrawWireSphere(Owner.transform.position, impulseDefinition.ImpactRadius);
				}
			}
#endif
		}
		
		/// <summary>
		/// Automatically adds a Cinemachine Impulse Listener to the camera
		/// </summary>
		public override void AutomaticShakerSetup()
		{
			MMCinemachineHelpers.AutomaticCinemachineShakersSetup(Owner, "CinemachineImpulse");
		}
	}
}