using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unidork.Tweens
{
	/// <summary>
	/// Stores data about a tween: its duration, ease, etc.
	/// </summary>
	[System.Serializable]
	public class TweenSettings
	{
		#region Enums

		/// <summary>
		/// Type of tween's time parameter like duration or delay.
		/// </summary>
		private enum TimeType
		{
			/// <summary>
			/// Constant time value.
			/// </summary>
			Constant,

			/// <summary>
			/// Time parameter's value is random between min and max.
			/// </summary>
			Range
		}

		#endregion

		#region Properties

		/// <summary>
		/// Duration of the tween.
		/// </summary>
		public float Duration
		{
			get
			{
				return durationType switch
				{
					TimeType.Constant => duration,
					TimeType.Range => Random.Range(durationRange.x, durationRange.y),
					_ => throw new ArgumentOutOfRangeException()
				};
			}
		}

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
		public float Delay
		{
			get
			{
				return delayType switch
				{
					TimeType.Constant => delay,
					TimeType.Range => Random.Range(delayRange.x, delayRange.y),
					_ => throw new ArgumentOutOfRangeException()
				};
			}
		}

		#endregion

		#region Fields

		/// <summary>
		/// Tween's duration type.
		/// </summary>
		[Tooltip("Tween's duration type." +
		         "Constant - constant tween duration" +
		         "Random - random between min and max. ")]
		[SerializeField]
		private TimeType durationType;

		/// <summary>
		/// Duration of the tween.
		/// </summary>
		[ShowIf("@this.durationType", TimeType.Constant)] [Tooltip("Duration of the tween.")] [SerializeField]
		private float duration = 1f;

		/// <summary>
		/// Min and max possible values for tween's random duration.
		/// </summary>
		[ShowIf("@this.durationType", TimeType.Range)]
		[Tooltip("Min and max possible values for tween's random duration.")]
		[SerializeField]
		private Vector2 durationRange = new(0f, 1f);

		/// <summary>
		/// Should a custom ease curve be used?
		/// </summary>
		[Tooltip("Should a custom ease curve be used?")] [SerializeField]
		private bool useCustomEaseCurve;

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
		private AnimationCurve customEaseCurve = new();

		/// <summary>
		/// Tween's delay type.
		/// </summary>
		[Tooltip("Tween's delay type." +
		         "Constant - constant delay duration" +
		         "Random - random between min and max. ")]
		[SerializeField]
		private TimeType delayType;

		/// <summary>
		/// Delay for the tween.
		/// </summary>
		[ShowIf("@this.delayType", TimeType.Constant)]
		[Tooltip("Delay for the tween.")]
		[SerializeField]
		private float delay;
		
		/// <summary>
		/// Min and max values for tween's delay.
		/// </summary>
		[ShowIf("@this.delayType", TimeType.Range)]
		[Tooltip("Min and max values for tween's delay.")]
		[SerializeField]
		private Vector2 delayRange = Vector2.zero;
		
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

        #region Duration

		

        #endregion
    }
}