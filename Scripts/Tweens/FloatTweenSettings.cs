using DG.Tweening;
using UnityEngine;

namespace Unidork.Tweens
{
	/// <summary>
	/// Stores data about a tween that animates a float to a specified target value: its duration, ease, etc.
	/// </summary>
	[System.Serializable]
	public class FloatTweenSettings : TweenSettings
	{
		#region Properties

		/// <summary>
		/// Target float value.
		/// </summary>
		/// <value>
		/// Gets the value of the float field targetValue.
		/// </value>
		public float TargetValue => targetValue;

		#endregion

		#region Fields

		/// <summary>
		/// Target float value.
		/// </summary>
		[Tooltip("Target float value.")]
		[SerializeField]
		private float targetValue = 0f;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates an instance of a linear float tween settings with a 1 second duration and target value of 1f.
		/// </summary>
		/// <returns>
		/// A <see cref="FloatTweenSettings"/>.
		/// </returns>
		public static FloatTweenSettings CreateLinearFloatTweenSettings()
		{
			return new FloatTweenSettings(1f, 1f, 0f, false, Ease.Linear, new AnimationCurve());
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="targetValue">Target float value.</param>
		/// <param name="duration">Duration of the tween.</param>
		/// <param name="delay">Delay for the tween.</param>
		/// <param name="useCustomEaseCurve">Should a custom ease curve be used?</param>
		/// <param name="easeFunction">Ease function to use when animating the float value.</param>
		/// <param name="customEaseCurve">Animation curve to pass to DoTween's SetEase function in case we
		/// don't want to use any built-in easing functions.</param>
		public FloatTweenSettings(float targetValue, float duration, float delay, bool useCustomEaseCurve, Ease easeFunction, AnimationCurve customEaseCurve) : 
			base(duration, delay, useCustomEaseCurve, easeFunction, customEaseCurve)
		{
			this.targetValue = targetValue;
		}

		#endregion

	}
}