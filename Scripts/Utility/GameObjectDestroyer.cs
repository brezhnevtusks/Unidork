using Unidork.Attributes;
using UnityEngine;

namespace Unidork.Utility
{
    /// <summary>
    /// Destroys a game object.
    /// </summary>
    public class GameObjectDestroyer : MonoBehaviour
    {
        #region Enums

        /// <summary>
        /// Type of object's destruction.
        /// <para>Awake - destroy on Awake.</para>
        /// <para>Start - destroy on Start.</para>
        /// <para>MethodCall - destroy when Destroy method is called.</para>
        /// </summary> 
        private enum DestroyType
        {
            Awake,
            Start,
            MethodCall
        }

        #endregion
        
        #region Fields

        /// <summary>
        /// Defines how object should be destroyed.
        /// </summary>
        [Space, SettingsHeader, Space] 
        [Tooltip("Defines how object should be destroyed.")]
        [SerializeField]
        private DestroyType destroyType = default;

        #endregion

        #region Destroy

        private void Awake()
        {
            if (destroyType == DestroyType.Awake)
            {
                DestroyGameObject();
            }
        }

        private void Start()
        {
            if (destroyType == DestroyType.Start)
            {
                DestroyGameObject();
            }
        }

        public void DestroyGameObject() => Destroy(gameObject);

        #endregion
    }
}