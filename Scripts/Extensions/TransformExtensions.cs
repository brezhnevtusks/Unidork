using System.Collections.Generic;
using DG.Tweening;
using Unidork.Tweens;
using UnityEngine;

namespace Unidork.Extensions
{
    public static class TransformExtensions
    {
        #region General

        /// <summary>
        /// Sets transform's position, rotation and scale.
        /// </summary>
        /// <param name="transform">Target transform.</param>
        /// <param name="position">Position to set.</param>
        /// <param name="rotation">Rotation to set.</param>
        /// <param name="scale">Scale to set.</param>
        /// <param name="setWorldPosition">Should world position be set or local?</param>
        /// <param name="setWorldRotation">Should world rotation be set or local?</param>
        public static void SetPositionRotationAndScale(this Transform transform, Vector3 position, Quaternion rotation, Vector3 scale,
                                                       bool setWorldPosition = true, bool setWorldRotation = true)
        {
            if (setWorldPosition)
            {
                transform.position = position;
            }
            else
            {
                transform.localPosition = position;
            }

            if (setWorldRotation)
            {
                transform.rotation = rotation;
            }
            else
            {
                transform.localRotation = rotation;
            }

            transform.localScale = scale;
        }

        /// <summary>
        /// Sets transform's position, rotation and scale.
        /// </summary>
        /// <param name="transform">Target transform.</param>
        /// <param name="position">Position to set.</param>
        /// <param name="eulerAngles">Euler angles that represent rotation to set.</param>
        /// <param name="scale">Scale to set.</param>
        /// <param name="setWorldPosition">Should world position be set or local?</param>
        /// <param name="setWorldRotation">Should world rotation be set or local?</param>
        public static void SetPositionRotationAndScale(this Transform transform, Vector3 position, Vector3 eulerAngles, Vector3 scale,
                                                       bool setWorldPosition = true, bool setWorldRotation = true)
        {
            SetPositionRotationAndScale(transform, position, Quaternion.Euler(eulerAngles), scale, setWorldPosition, setWorldRotation);
        }

        /// <summary>
        /// Resets a transform's position, rotation, and scale to their default values.
        /// </summary>
        /// <param name="transform">Transform to reset.</param>
        /// <param name="resetLocally">Should transform be reset locally?</param>
        public static void Reset(this Transform transform, bool resetLocally = true)
		{            
            SetPositionRotationAndScale(transform, Vector3.zero, Quaternion.identity, Vector3.one, !resetLocally, !resetLocally);
		}

        #endregion

        #region Children

        /// <summary>
        /// Performs a breadht-first deep search of a transform's hierarchy for an object with specified name.
        /// </summary>
        /// <param name="transform">Transform.</param>
        /// <param name="childName">Name of object to search for.</param>
        /// <returns>
        /// A transform whose name matches <paramref name="childName"/> or null if no such object exists.
        /// </returns>
        public static Transform FindDeepChild(this Transform transform, string childName)
        {
            var transformQueue = new Queue<Transform>();
            transformQueue.Enqueue(transform);

            while (transformQueue.Count > 0)
            {
                var childTransform = transformQueue.Dequeue();

                if (childTransform.name == childName)
                {
                    return childTransform;
                }                    

                foreach (Transform grandChildTransform in childTransform)
                {
                    transformQueue.Enqueue(grandChildTransform);
                }                    
            }
            return null;
        }

        /// <summary>
        /// Gets immediate children of a transform.
        /// </summary>
        /// <param name="transform">Transform.</param>
        /// <returns>
        /// A list of Transforms that are immediate children of the passed transform.
        /// </returns>
        public static List<Transform> GetImmediateChildren(this Transform transform)
        {
            var immediateChildren = new List<Transform>();

            for (var i = 0; i < transform.childCount; i++)
            {
                immediateChildren.Add(transform.GetChild(i));
            }

            return immediateChildren;
        }
        
        /// <summary>
        /// Gets all children in transform's hierarchy.
        /// </summary>
        /// <param name="transform">Transform.</param>
        /// <returns>
        /// A list of transforms that are children of the target transform.
        /// </returns>
        public static List<Transform> GetAllChildren(this Transform transform)
        {
            var children = new List<Transform>();
            var transformQueue = new Queue<Transform>();
            transformQueue.Enqueue(transform);

            while (transformQueue.Count > 0)
            {
                var childTransform = transformQueue.Dequeue();
                children.Add(childTransform);

                foreach (Transform grandChildTransform in childTransform)
                {
                    transformQueue.Enqueue(grandChildTransform);
                }
            }

            children.Remove(transform);

            return children;
        }

        /// <summary>
        /// Gets all direct children in transform's hierarchy.
        /// </summary>
        /// <param name="transform">Transform.</param>
        /// <returns>
        /// A list of transforms that are direct children of the target transform.
        /// </returns>
        public static List<Transform> GetDirectChildren(this Transform transform)
        {
            var children = new List<Transform>();

            int childCount = transform.childCount;

            for (var i = 0; i < childCount; i++)
            {
                children.Add(transform.GetChild(i));
            }

            return children;
        }

        /// <summary>
        /// Gets the first child of a transform.
        /// </summary>
        /// <param name="transform">Transform.</param>
        /// <returns>Transform of this transform's first child or null if transform has no children.</returns>
        public static Transform GetFirstChild(this Transform transform)
        {
            int childCount = transform.childCount;
            return childCount == 0 ? null : transform.GetChild(0);
        }
        
        /// <summary>
        /// Gets the last child of a transform.
        /// </summary>
        /// <param name="transform">Transform.</param>
        /// <returns>Transform of this transform's last child or null if transform has no children.</returns>
        public static Transform GetLastChild(this Transform transform)
        {
            int childCount = transform.childCount;
            return childCount == 0 ? null : transform.GetChild(childCount - 1);
        }

        /// <summary>
        /// Checks whether one transform is one of the parents of another transform.
        /// </summary>
        /// <param name="transform">Transform.</param>
        /// <param name="otherTransform">Other transform.</param>
        /// <returns>
        /// True if this transform is one of the parents of the other transform, False otherwise.
        /// </returns>
        public static bool IsOneOfParentsOf(this Transform transform, Transform otherTransform)
		{
            Transform currentParent = otherTransform.parent;

            if (currentParent == null)
			{
                return false;
			}

            while (currentParent != null)
			{
                if (transform == currentParent)
				{
                    return true;
				}

                currentParent = currentParent.parent;
			}

            return false;
		}

        /// <summary>
        /// Checks whether one transform is one of the children of another transform.
        /// </summary>
        /// <param name="transform">Transform.</param>
        /// <param name="otherTransform">Other transform.</param>
        /// <returns>
        /// True if this transform is one of the children of the other transform, False otherwise.
        /// </returns>
        public static bool IsOneOfChildrenOf(this Transform transform, Transform otherTransform)
		{
            List<Transform> children = otherTransform.GetAllChildren();
            return children.Contains(transform);
		}

        #endregion

        #region Transform point

        /// <summary>
        /// Transforms a point, ignoring object's scale.
        /// </summary>
        /// <param name="transform">Transform.</param>
        /// <param name="position">Point to transform.</param>
        /// <returns>
        /// A Vector3 representing a transformed position, unaffected by object's scale.
        /// </returns>
        public static Vector3 TransformPointUnscaled(this Transform transform, Vector3 position)
        {
            var localToWorldMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            return localToWorldMatrix.MultiplyPoint3x4(position);
        }

        /// <summary>
        /// Inverse-transforms a point, ignoring object's scale.
        /// </summary>
        /// <param name="transform">Transform.</param>
        /// <param name="position">Point to transform.</param>
        /// <returns>
        /// A Vector3 representing an inversed transformed position, unaffected by object's scale.
        /// </returns>
        public static Vector3 InverseTransformPointUnscaled(this Transform transform, Vector3 position)
        {
            var worldToLocalMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one).inverse;
            return worldToLocalMatrix.MultiplyPoint3x4(position);
        }


        #endregion
        
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
        
        #endregion
    }
}