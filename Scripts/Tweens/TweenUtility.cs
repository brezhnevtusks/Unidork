using DG.Tweening;
using UnityEngine;

namespace Unidork.Tweens
{
    public static class TweenUtility
    {
        /// <summary>
        /// Creates a scale tween for a transform using passed settings.
        /// </summary>
        /// <param name="transform">Transform</param>
        /// <param name="scaleTweenSettings">Settings.</param>
        /// <returns>A tween or null if either transform or settings are null.</returns>
        public static Tween CreateTransformScaleTween(Transform transform, FloatTweenSettings scaleTweenSettings)
        {
            if (transform == null)
            {
                Debug.LogError("Transform is null!");
                return null;
            }

            if (scaleTweenSettings == null)
            {
                Debug.LogError("Settings are null!");
                return null; 
            }

            Tween scaleTween = transform.DOScale(scaleTweenSettings.TargetValue, scaleTweenSettings.Duration)
                                        .SetDelay(scaleTweenSettings.Delay);

            return scaleTweenSettings.UseCustomEaseCurve ? scaleTween.SetEase(scaleTweenSettings.CustomEaseCurve) : scaleTween.SetEase(scaleTweenSettings.EaseFunction);
        }

        /// <summary>
        ///  Creates a scale tween for a transform using passed settings.
        /// </summary>
        /// <param name="transform">Transform</param>
        /// <param name="scaleTweenSettings">Settings.</param>
        /// <returns>A tween or null if either transform or settings are null.</returns>
        public static Tween CreateTransformScaleTween(Transform transform, Vector3TweenSettings scaleTweenSettings)
        {
            if (transform == null)
            {
                Debug.LogError("Transform is null!");
                return null;
            }

            if (scaleTweenSettings == null)
            {
                Debug.LogError("Settings are null!");
                return null; 
            }

            Tween scaleTween = transform.DOScale(scaleTweenSettings.TargetValue, scaleTweenSettings.Duration)
                                        .SetDelay(scaleTweenSettings.Delay);

            return scaleTweenSettings.UseCustomEaseCurve ? scaleTween.SetEase(scaleTweenSettings.CustomEaseCurve) : scaleTween.SetEase(scaleTweenSettings.EaseFunction); 
        }
    } 
}