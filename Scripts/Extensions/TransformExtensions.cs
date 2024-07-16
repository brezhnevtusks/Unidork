using DG.Tweening;
using Unidork.Tweens;
using UnityEngine;

namespace Unidork.Extensions
{
    public static class TransformExtensions
    {
        #region Tweens

        /// <summary>
        /// Creates a scale tween for this transform using the passed settings.
        /// </summary>
        /// <param name="transform">Transform</param>
        /// <param name="scaleTweenSettings">Scale tween settings.</param>
        /// <returns>A tween or null if settings are null.</returns>
        public static Tween DOScale(this Transform transform, FloatTweenSettings scaleTweenSettings)
        {
            return TweenUtility.CreateTransformScaleTween(transform, scaleTweenSettings);
        }
        
        /// <summary>
        /// Creates a scale tween for this transform using the passed settings.
        /// </summary>
        /// <param name="transform">Transform</param>
        /// <param name="scaleTweenSettings">Scale tween settings.</param>
        /// <returns>A tween or null if settings are null.</returns>
        public static Tween DOScale(this Transform transform, Vector3TweenSettings scaleTweenSettings)
        {
            return TweenUtility.CreateTransformScaleTween(transform, scaleTweenSettings);
        }

        /// <summary>
        /// Creates a punch scale tween for this transform using the passed settings.
        /// </summary>
        /// <param name="transform">Transform</param>
        /// <param name="scaleTweenSettings">Scale tween settings.</param>
        /// <param name="vibrato">Vibrato parameter of DoTween's punch scale tween.</param>
        /// <param name="elasticity">Elasticity parameter of DoTween's punch scale tween.</param>
        /// <returns>A tween or null if settings are null.</returns>
        public static Tween DOPunchScale(this Transform transform, FloatTweenSettings scaleTweenSettings, int vibrato = 10, float elasticity = 1f)
        {
            return TweenUtility.CreateTransformPunchScaleTween(transform, scaleTweenSettings, vibrato, elasticity);
        }
        
        /// <summary>
        /// Creates a punch scale tween for this transform using the passed settings.
        /// </summary>
        /// <param name="transform">Transform</param>
        /// <param name="scaleTweenSettings">Scale tween settings.</param>
        /// <param name="vibrato">Vibrato parameter of DoTween's punch scale tween.</param>
        /// <param name="elasticity">Elasticity parameter of DoTween's punch scale tween.</param>
        /// <returns>A tween or null if settings are null.</returns>
        public static Tween DOPunchScale(this Transform transform, Vector3TweenSettings scaleTweenSettings, int vibrato = 10, float elasticity = 1f)
        {
            return TweenUtility.CreateTransformPunchScaleTween(transform, scaleTweenSettings, vibrato, elasticity);
        }
        
        #endregion
    }
}