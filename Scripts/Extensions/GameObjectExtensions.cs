using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Unidork.Extensions
{
	public static class GameObjectExtensions
	{
		#region Tweens

		#region Position

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOMove"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose position will be tweened.</param>
	    /// <param name="targetPosition">Target position.</param>
	    /// <param name="duration">Tween duration.</param>
	    /// <param name="snapping">When set to True, will snap all values to integers.</param>
	    public static TweenerCore<Vector3, Vector3, VectorOptions> DOMove(this GameObject target, Vector3 targetPosition, float duration, bool snapping = false)
		{
			return target.transform.DOMove(targetPosition, duration, snapping);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOMoveX"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose position will be tweened.</param>
		/// <param name="targetX">Target X position.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="snapping">When set to True, will snap all values to integers.</param>
	    public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveX(this GameObject target, float targetX, float duration, bool snapping = false)
	    {
		    return target.transform.DOMoveX(targetX, duration, snapping);
	    }

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOMoveY"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose position will be tweened.</param>
		/// <param name="targetY">Target Y position.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="snapping">When set to True, will snap all values to integers.</param>
	    public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveY(this GameObject target, float targetY, float duration, bool snapping = false)
	    {
		    return target.transform.DOMoveY(targetY, duration, snapping);
	    }

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOMoveZ"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose position will be tweened.</param>
		/// <param name="targetZ">Target Z position.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="snapping">When set to True, will snap all values to integers.</param>
	    public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveZ(this GameObject target, float targetZ, float duration, bool snapping = false)
	    {
		    return target.transform.DOMoveZ(targetZ, duration, snapping);
	    }

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOLocalMove"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose position will be tweened.</param>
		/// <param name="targetPosition">Target position.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="snapping">When set to True, will snap all values to integers.</param>
	    public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMove(this GameObject target, Vector3 targetPosition, float duration, bool snapping = false)
		{
			return target.transform.DOLocalMove(targetPosition, duration, snapping);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOLocalMoveX"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose position will be tweened.</param>
		/// <param name="targetX">Target X position.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="snapping">When set to True, will snap all values to integers.</param>
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMoveX(this GameObject target, float targetX, float duration, bool snapping = false)
		{
			return target.transform.DOLocalMoveX(targetX, duration, snapping);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOLocalMoveY"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose position will be tweened.</param>
		/// <param name="targetY">Target Y position.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="snapping">When set to True, will snap all values to integers.</param>
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMoveY(this GameObject target, float targetY, float duration, bool snapping = false)
		{
			return target.transform.DOLocalMoveY(targetY, duration, snapping);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOLocalMoveZ"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose position will be tweened.</param>
		/// <param name="targetZ">Target Z position.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="snapping">When set to True, will snap all values to integers.</param>
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMoveZ(this GameObject target, float targetZ, float duration, bool snapping = false)
		{
			return target.transform.DOLocalMoveZ(targetZ, duration, snapping);
		}

		#endregion

		#region Rotation

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DORotate"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetRotation">Target rotation.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="mode">Rotation mode.</param>
	    public static TweenerCore<Quaternion, Vector3, QuaternionOptions> DORotate(this GameObject target, Vector3 targetRotation, float duration,
																				   RotateMode mode = RotateMode.Fast)
		{
			return target.transform.DORotate(targetRotation, duration, mode);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DORotateQuaternion"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetRotation">Target rotation.</param>
		/// <param name="duration">Tween duration.</param>
		public static TweenerCore<Quaternion, Quaternion, NoOptions> DORotateQuaternion(this GameObject target, Quaternion targetRotation, float duration)
		{
			return target.transform.DORotateQuaternion(targetRotation, duration);
		}
		
		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOLocalRotate"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetRotation">Target rotation.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="mode">Rotation mode.</param>
		public static TweenerCore<Quaternion, Vector3, QuaternionOptions> DOLocalRotate(this GameObject target, Vector3 targetRotation, float duration,
			RotateMode mode = RotateMode.Fast)
		{
			return target.transform.DOLocalRotate(targetRotation, duration, mode);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOLocalRotateQuaternion"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetRotation">Target rotation.</param>
		/// <param name="duration">Tween duration.</param>
		public static TweenerCore<Quaternion, Quaternion, NoOptions> DOLocalRotateQuaternion(this GameObject target, Quaternion targetRotation, float duration)
		{
			return target.transform.DOLocalRotateQuaternion(targetRotation, duration);
		}

		#endregion

		#region Scale

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOScale(Transform, Vector3, float)"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetScale">Target scale.</param>
		/// <param name="duration">Tween duration.</param>
	    public static TweenerCore<Vector3, Vector3, VectorOptions> DOScale(this GameObject target, Vector3 targetScale, float duration)
		{
			return target.transform.DOScale(targetScale, duration);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOScale(Transform, float, float)"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetScale">Target scale.</param>
		/// <param name="duration">Tween duration.</param>
	    public static TweenerCore<Vector3, Vector3, VectorOptions> DOScale(this GameObject target, float targetScale, float duration)
		{
			return target.transform.DOScale(targetScale, duration);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOScaleX"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetXScale">Target X scale.</param>
		/// <param name="duration">Tween duration.</param>
	    public static TweenerCore<Vector3, Vector3, VectorOptions> DOScaleX(this GameObject target, float targetXScale, float duration)
		{
			return target.transform.DOScaleX(targetXScale, duration);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOScaleY"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetYScale">Target Y scale.</param>
		/// <param name="duration">Tween duration.</param>
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOScaleY(this GameObject target, float targetYScale, float duration)
		{
			return target.transform.DOScaleY(targetYScale, duration);
		}
		
		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOScaleZ"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetZScale">Target Z scale.</param>
		/// <param name="duration">Tween duration.</param>
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOScaleZ(this GameObject target, float targetZScale, float duration)
		{
			return target.transform.DOScaleZ(targetZScale, duration);
		}

		#endregion

		#endregion
	}
}