using MoreMountains.Feedbacks;
using UnityEngine;

namespace Unidork.Feedbacks
{
    [AddComponentMenu("")]
    [FeedbackHelp("This feedback will stop and clear a particle system.")]
    [FeedbackPath("Particles/Particles Stop and Clear")]
    public class MMF_StopAndClearParticles : MMF_Feedback
    {
#if UNITY_EDITOR
        /// sets the inspector color for this feedback
        public override Color FeedbackColor => MMFeedbacksInspectorColors.ParticlesColor;
        public override bool EvaluateRequiresSetup() => boundParticleSystem == null;
        public override string RequiredTargetText => boundParticleSystem != null ? boundParticleSystem.name : "";
        public override string RequiresSetupText => "This feedback requires that a BoundParticleSystem be set to be able to work properly. You can set one below.";
#endif

        #region Fields

        /// <summary>
        /// Particle system to stop with this feedback.
        /// </summary>
        [MMFInspectorGroup("Bound Particles", true, 41, true)]
        [Tooltip("Particle system to stop with this feedback.")]
        [SerializeField] private ParticleSystem boundParticleSystem;

        #endregion

        #region Play
        
        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1)
        {
            if (boundParticleSystem != null)
            {
                boundParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
        
        #endregion
    }
}