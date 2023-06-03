using DG.Tweening;
using Unidork.Tweens;
using UnityEngine;

namespace Unidork.TransformUtility
{
	/// <summary>
	/// Performs various common operations with a transform that include moving, scaling, rotation it, etc.
	/// </summary>
	public class TransformController : MonoBehaviour
    {
		#region Fields
      
        [SerializeField]
        private Vector3 movePositionOffset = Vector3.zero;
        
        [SerializeField]
        private TweenSettings moveTweenSettings = TweenSettings.CreateLinearTweenSettings();
        
        private Vector3 defaultPosition;

        private Tween moveTween;

        #endregion

        #region Move
   
        public void MoveToOffsetPosition()
        {
            moveTween?.Kill();
            moveTween = transform.DOMove(defaultPosition + movePositionOffset,
                                         moveTweenSettings.Duration);
            moveTween.SetEase(moveTweenSettings.EaseFunction);
        }

        public void MoveToDefaultPosition()
        {
            if (defaultPosition == Vector3.zero)
            {
                defaultPosition = transform.position;
            }

            moveTween?.Kill();
            moveTween = transform.DOMove(defaultPosition,
                                         moveTweenSettings.Duration);
            moveTween.SetEase(moveTweenSettings.EaseFunction);

        }

        #endregion
    }
}