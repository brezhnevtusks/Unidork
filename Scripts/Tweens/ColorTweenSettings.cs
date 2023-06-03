using DG.Tweening;
using Unidork.Attributes;
using UnityEngine;

namespace Unidork.Tweens
{
	/// <summary>
	/// Stores data about a tween that animates a color value: its duration, ease, etc.
	/// </summary>
	[System.Serializable]
	public class ColorTweenSettings : TweenSettings
    {
		#region Properties

		/// <summary>
		/// Target color.
		/// </summary>
		/// <value>
		/// Gets and sets the value of the float field targetColor.
		/// </value>
		public Color TargetColor { get => targetColor; set => targetColor = value; }

		#endregion

		#region Fields

		/// <summary>
		/// Target color.
		/// </summary>
		[Tooltip("Target color.")]
		[SerializeField]
		private Color targetColor = Color.white;

		#endregion

		#region Constructor

		/// <summary>
		/// Creates an instance of a linear float tween data with a 1 second duration.
		/// </summary>
		/// <returns>
		/// A <see cref="ColorTweenSettings"/>.
		/// </returns>
		public static ColorTweenSettings CreateLinearColorTweenSettings()
		{
			return new ColorTweenSettings(Color.white, 1f, 0f, false, Ease.Linear, new AnimationCurve());
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="targetColor">Target color.</param>
		/// <param name="duration">Duration of the tween.</param>
		/// <param name="delay">Delay for the tween.</param>
		/// <param name="useCustomEaseCurve">Should a custom ease curve be used?</param>
		/// <param name="easeFunction">Ease function to use when animating the float value.</param>
		/// <param name="customEaseCurve">Animation curve to pass to DoTween's SetEase function in case we
		/// don't want to use any built-in easing functions.</param>
		public ColorTweenSettings(Color targetColor, float duration, float delay, bool useCustomEaseCurve, Ease easeFunction, AnimationCurve customEaseCurve) : base(duration, delay, useCustomEaseCurve, easeFunction, customEaseCurve)
		{
			this.targetColor = targetColor;
		}

		#endregion
	}
}