using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Unidork.ObjectPooling
{
    /// <summary>
    /// Stores settings for an item that is part of an object pool.
    /// </summary>
    [System.Serializable]
    public class ObjectPoolItemSettings
    {
        #region Properties

        /// <summary>
        /// Reference to the addressable asset that contains the object prefab.
        /// </summary>
        /// <value>
        /// Gets the value of the field assetReference.
        /// </value>
        public AssetReference AssetReference => assetReference;

        /// <summary>
        /// Number of objects to put in the pool.
        /// </summary>
        /// <value>
        /// Gets the value of the int field numberToPool.
        /// </value>
        public int NumberToPool => numberToPool;

        /// <summary>
        /// If the pool runs out of objects of this type, is it allowed to add more?
        /// </summary>
        /// <value>
        /// Gets the value of the bool field poolCanAddMore.
        /// </value>
        public bool PoolCanExpand => poolCanExpand;

        #endregion

        #region Fields

        /// <summary>
        /// Reference to the addressable asset that contains the object prefab.
        /// </summary>    
        [Tooltip("Reference to the addressable asset that contains the object prefab.")]
        [SerializeField]
        private AssetReference assetReference = null;

        /// <summary>
        /// Number of objects to put in the pool.
        /// </summary>
        [Tooltip("Number of objects to put in the pool.")]
        [SerializeField]
        private int numberToPool = 10;

        /// <summary>
        /// If the pool runs out of objects of this type, is it allowed to add more?
        /// </summary>
        [Tooltip("If the pool runs out of objects of this type, is it allowed to add more?")]
        [SerializeField]
        private bool poolCanExpand = true;

        #endregion
    }
}