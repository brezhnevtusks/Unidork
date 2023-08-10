using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unidork.Tweens
{
    /// <summary>
    /// Stores data about a tween: its duration, ease, etc.
    /// </summary>
    [System.Serializable]
    public class TweenSettings
    {
		#region Properties

		/// <summary>
		/// Duration of the tween.
		/// </summary>
		/// <value>
		/// Gets and sets the value of the float field duration.
		/// </value>
		public float Duration { get => duration; set => duration = value; }

		/// <summary>
		/// Should a custom ease curve be used?
		/// </summary>
		/// <value>
		/// Gets the value of the boolean field useCustomEaseCurved.
		/// </value>
		public bool UseCustomEaseCurve => useCustomEaseCurve;

        /// <summary>
        /// Ease function to use when animating the x coordinate.
        /// </summary>
        /// <value>
        /// Gets the value of the field easeFunction.
        /// </value>
        public Ease EaseFunction => easeFunction;

        /// <summary>
        /// Animation curve to pass to DoTween's SetEase function in case we don't want to use any built-in easing functions.
        /// </summary>
        /// <value>
        /// Gets the value of the field customEaseCurve.
        /// </value>
        public AnimationCurve CustomEaseCurve => customEaseCurve;

		/// <summary>
		/// Delay for the tween.
		/// </summary>
		/// <value>
		/// Gets and sets the value of the float field delay.
		/// </value>
		public float Delay { get => delay; set => delay = value; }

		#endregion

		#region Fields

		/// <summary>
		/// Duration of the tween.
		/// </summary>
        [Tooltip("Duration of the tween.")]
        [SerializeField]
        private float duration = 1f;
        
        /// <summary>
        /// Should a custom ease curve be used?
        /// </summary>
        [Tooltip("Should a custom ease curve be used?")]
        [SerializeField]
        private bool useCustomEaseCurve = false;
        
        /// <summary>
        /// Ease function to use when animating values.
        /// </summary>
        [ShowIf("@this.useCustomEaseCurve == false")]
        [Tooltip("Ease function to use when animating values.")]
        [SerializeField]
        private Ease easeFunction = Ease.Linear;

        /// <summary>
        /// Animation curve to pass to DoTween's SetEase function in case we don't want to use any built-in easing functions.
        /// </summary>
        [ShowIf("@this.useCustomEaseCurve == true")]
        [Tooltip("Animation curve to pass to DoTween's SetEase function in case we don't want to use any built-in easing functions.")]
        [SerializeField]
        private AnimationCurve customEaseCurve = new AnimationCurve();

        /// <summary>
        /// Delay for the tween.
        /// </summary>
        [Tooltip("Delay for the tween.")]
        [SerializeField]
        private float delay = 0f;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of a linear tween data with a 1 second duration.
        /// </summary>
        /// <returns>
        /// A <see cref="TweenSettings"/>.
        /// </returns>
        public static TweenSettings CreateLinearTweenSettings()
		{
            return new TweenSettings(1f, 0f, false, Ease.Linear, new AnimationCurve());
		}

        /// <summary>
        /// Creates an instance of a zero duration tween to use for special cases like editor buttons.
        /// </summary>
        /// <returns>
        /// A <see cref="TweenSettings"/>.
        /// </returns>
        public static TweenSettings CreateInstantTweenSettings()
		{
            return new TweenSettings(0f, 0f, false, Ease.Linear, new AnimationCurve());
        }
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="duration">Duration of the tween.</param>
        /// <param name="delay">Delay for the tween.</param>
        /// <param name="useCustomEaseCurve">Should a custom ease curve be used?</param>
        /// <param name="easeFunction">Ease function to use when animating values.</param>
        /// <param name="customEaseCurve">Animation curve to pass to DoTween's SetEase function in case we
        /// don't want to use any built-in easing functions.</param>
        public TweenSettings(float duration, float delay, bool useCustomEaseCurve, Ease easeFunction, AnimationCurve customEaseCurve)
        {
            this.duration = duration;
            this.delay = delay;
            this.useCustomEaseCurve = useCustomEaseCurve;
            this.easeFunction = easeFunction;
            this.customEaseCurve = customEaseCurve;
        }

        #endregion
    }
}