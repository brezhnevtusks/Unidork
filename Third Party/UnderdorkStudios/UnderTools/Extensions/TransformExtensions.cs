using System.Collections.Generic;
using UnityEngine;

namespace UnderdorkStudios.UnderTools.Extensions
{
    public static class TransformExtensions
    {
        #region General

        /// <summary>
        /// Sets the position of a transform to match another transform.
        /// </summary>
        /// <param name="transform">Transform.</param>
        /// <param name="other">Transform to match.</param>
        public static void CopyPosition(this Transform transform, Transform other)
        {
            transform.position = other.position;
        }

        /// <summary>
        /// Sets the position and rotation of a transform to match another transform.
        /// </summary>
        /// <param name="transform">Transform.</param>
        /// <param name="other">Transform to match.</param>
        public static void CopyPositionAndRotation(this Transform transform, Transform other)
        {
            transform.position = other.position;
            transform.rotation = other.rotation;
        }

        /// <summary>
        /// Sets the position, rotation, and scale of a transform to match another transform.
        /// </summary>
        /// <param name="transform">Transform.</param>
        /// <param name="other">Transform to match.</param>
        public static void CopyPositionRotationAndScale(this Transform transform, Transform other)
        {
            transform.position = other.position;
            transform.rotation = other.rotation;
            transform.localScale = other.localScale;
        }
        
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
        /// Performs a breadth-first deep search of a transform's hierarchy for an object with specified name.
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
                Transform trans = transformQueue.Dequeue();

                if (trans != transform)
                {
                    children.Add(trans);
                }

                foreach (Transform childTransform in trans)
                {
                    transformQueue.Enqueue(childTransform);
                }
            }

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
            return transform.childCount == 0 ? null : transform.GetChild(0);
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
        /// Checks whether one transform is a direct child or a descendant of another transform.
        /// </summary>
        /// <param name="transform">Transform.</param>
        /// <param name="otherTransform">Other transform.</param>
        /// <returns>
        /// True if this transform is one of the children of the other transform, False otherwise.
        /// </returns>
        public static bool IsDescendantOf(this Transform transform, Transform otherTransform)
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
    }
}