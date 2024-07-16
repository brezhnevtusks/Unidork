using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Unidork.ObjectPooling
{
	/// <summary>
	/// Contains settings for an <see cref="ObjectPooler"/>.
	/// </summary>
	[System.Serializable]
    public class ObjectPoolerSettings<T>
    {
        #region Properties

        /// <summary>
        /// Name of the pooler.
        /// </summary>
        /// <value>
        /// Gets the value of the string field name.
        /// </value>
        public string Name => name;

        /// <summary>
        /// Transform that serves as a holder for pooled objects.
        /// </summary>
        /// <value>
        /// Gets the value of the Transform field pooledObjectHolder.
        /// </value>
        public Transform PooledObjectHolder => pooledObjectHolder;

        #endregion

        #region Fields

        /// <summary>
        /// Name of the pooler.
        /// </summary>
        [Tooltip("Name of the pooler.")] 
        [SerializeField]
        private string name = "New Pooler";

        /// <summary>
        /// Transform that serves as a holder for pooled objects.
        /// </summary>
        [Tooltip("Transform that serves as a holder for pooled objects.")]
        [SerializeField]
        private Transform pooledObjectHolder = null;
        
        /// <summary>
        /// Array of objects storing settings for each type of item that needs to be put in a pool.
        /// </summary>
        [Tooltip("Array of objects storing settings for each type of item that needs to be put in a pool.")]
        [SerializeField]
        private PooledObjectSettings<T>[] pooledObjectSettings = null;

        #endregion

        #region Get

        /// <summary>
        /// Gets the pooled object settings.
        /// </summary>
        /// <returns>
        /// A clone of <see cref="pooledObjectSettings"/>.
        /// </returns>
        public PooledObjectSettings<T>[] GetPooledObjectSettings() => (PooledObjectSettings<T>[]) pooledObjectSettings.Clone();

        /// <summary>
        /// Gets settings for an object that matches the passed asset reference.
        /// </summary>
        /// <param name="assetReference">Asset address.</param>
        /// <returns>
        /// Settings that match the passed asset address, null otherwise.
        /// </returns>
        public PooledObjectSettings<T> GetSettingsByAssetReference(AssetReference assetReference)
        {
            foreach (PooledObjectSettings<T> settings in pooledObjectSettings)
            {
                if (settings.AssetReference == assetReference)
                {
                    return settings;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets settings for an object that matches the passed object type.
        /// </summary>
        /// <param name="objectType">Object type.</param>
        /// <returns>
        /// Settings that match the passed object type, null otherwise.
        /// </returns>
        public PooledObjectSettings<T> GetSettingsByObjectType(T objectType)
        {
            foreach (PooledObjectSettings<T> settings in pooledObjectSettings)
            {
                if (settings.ObjectType.Equals(objectType))
                {
                    return settings;
                }
            }

            return null;
        }

        #endregion
    }
}