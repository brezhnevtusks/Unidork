using DG.Tweening;
using UnityEngine;

namespace Unidork.Tweens
{
    /// <summary>
    /// Stores data about a tween that animates a Vector3 to a specified target value: its duration, ease, etc.
    /// </summary>
    [System.Serializable]
    public class Vector3TweenSettings : TweenSettings
    {
        #region Properties

        /// <summary>
        /// Target Vector3 value.
        /// </summary>
        /// <value>
        /// Gets the value of the Vector3 field targetValue.
        /// </value>
        public Vector3 TargetValue => targetValue;

        #endregion

        #region Fields

        /// <summary>
        /// Target Vector3 value.
        /// </summary>
        [Tooltip("Target Vector3 value.")]
        [SerializeField]
        private Vector3 targetValue = Vector3.zero;

        #endregion
       
        #region Constructor
        
        // <summary>
        /// Creates an instance of a linear Vector3 tween settings with a 1 second duration and target Value of Vector3.one.
        /// </summary>
        /// <returns>
        /// A <see cref="FloatTweenSettings"/>.
        /// </returns>
        public static Vector3TweenSettings CreateLinearVector3TweenSettings()
        {
            return new Vector3TweenSettings(Vector3.one, 1f, 0f, false, Ease.Linear, new AnimationCurve());
        } 
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="targetValue">Target Vector3 value.</param>
        /// <param name="duration">Duration of the tween.</param>
        /// <param name="delay">Delay for the tween.</param>
        /// <param name="useCustomEaseCurve">Should a custom ease curve be used?</param>
        /// <param name="easeFunction">Ease function to use when animating the float value.</param>
        /// <param name="customEaseCurve">Animation curve to pass to DoTween's SetEase function in case we
        /// don't want to use any built-in easing functions.</param>
        public Vector3TweenSettings(Vector3 targetValue, float duration, float delay, bool useCustomEaseCurve, Ease easeFunction, AnimationCurve customEaseCurve) : 
            base(duration, delay, useCustomEaseCurve, easeFunction, customEaseCurve)
        {
            this.targetValue = targetValue;
        }
        
        #endregion
    }
}