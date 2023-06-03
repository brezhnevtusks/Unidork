using UnityEngine;

namespace Unidork.ObjectPooling
{
    /// <summary>
    /// Interface for objects that can be pooled by an <see cref="ObjectPooler"/>.
    /// </summary>
    public interface IPooledObject
    {
        /// <summary>
        /// Is the pooled object currently active?
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Sets up the pooled object's transform, settings its position, rotation, scale, and parent.
        /// </summary>
        /// <param name="parent">Parent.</param>
        /// <param name="position">Position.</param>
        /// <param name="rotation">Rotation.</param>
        /// <param name="overrideScale">Should start scale be overriden?</param>
        /// <param name="scale">Scale.</param>
        void SetUpTransform(Transform parent, Vector3 position, Quaternion rotation, bool overrideScale = false, Vector3 scale = default);

        /// <summary>
        /// Sets the pooled object's parent.
        /// </summary>
        /// <param name="parent">Parent.</param>
        void SetParent(Transform parent);
        
        /// <summary>
        /// Activates the pooled object.
        /// </summary>
        void Activate();

        /// <summary>
        /// Deactivates the pooled object.
        /// </summary>
        /// <param name="deactivateOnStart">Is the pooled object being deactivated on start?</param>
        void Deactivate(bool deactivateOnStart = false);

        /// <summary>
        /// Destroys the pooled object.
        /// </summary>
        void Destroy();
    }
}