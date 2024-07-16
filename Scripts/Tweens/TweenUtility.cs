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

            Tween scaleTween = transform
                .DOScale(scaleTweenSettings.TargetValue, scaleTweenSettings.Duration)
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

            Tween scaleTween = transform
                .DOScale(scaleTweenSettings.TargetValue, scaleTweenSettings.Duration)
                .SetDelay(scaleTweenSettings.Delay);

            return scaleTweenSettings.UseCustomEaseCurve ? scaleTween.SetEase(scaleTweenSettings.CustomEaseCurve) : scaleTween.SetEase(scaleTweenSettings.EaseFunction); 
        }

        /// <summary>
        /// Creates a punch scale tween for a transform using passed settings.
        /// </summary>
        /// <param name="transform">Transform</param>
        /// <param name="scaleTweenSettings">Settings.</param>
        /// <param name="vibrato">Vibrato parameter of DoTween's punch scale tween.</param>
        /// <param name="elasticity">Elasticity parameter of DoTween's punch scale tween.</param>
        /// <returns>A tween or null if either transform or settings are null.</returns>
        public static Tween CreateTransformPunchScaleTween(Transform transform, FloatTweenSettings scaleTweenSettings, int vibrato = 10, float elasticity = 1f)
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

            Tween scaleTween = transform
                .DOPunchScale(Vector3.one * scaleTweenSettings.TargetValue, scaleTweenSettings.Duration, vibrato, elasticity)
                .SetDelay(scaleTweenSettings.Delay);

            return scaleTweenSettings.UseCustomEaseCurve ? scaleTween.SetEase(scaleTweenSettings.CustomEaseCurve) : scaleTween.SetEase(scaleTweenSettings.EaseFunction);
        }
        
        /// <summary>
        ///  Creates a punch scale tween for a transform using passed settings.
        /// </summary>
        /// <param name="transform">Transform</param>
        /// <param name="scaleTweenSettings">Settings.</param>
        /// <param name="vibrato">Vibrato parameter of DoTween's punch scale tween.</param>
        /// <param name="elasticity">Elasticity parameter of DoTween's punch scale tween.</param>
        /// <returns>A tween or null if either transform or settings are null.</returns>
        public static Tween CreateTransformPunchScaleTween(Transform transform, Vector3TweenSettings scaleTweenSettings, int vibrato = 10, float elasticity = 1f)
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

            Tween scaleTween = transform
                .DOPunchScale(scaleTweenSettings.TargetValue, scaleTweenSettings.Duration, vibrato, elasticity)
                .SetDelay(scaleTweenSettings.Delay);

            return scaleTweenSettings.UseCustomEaseCurve ? scaleTween.SetEase(scaleTweenSettings.CustomEaseCurve) : scaleTween.SetEase(scaleTweenSettings.EaseFunction); 
        }
    } 
}