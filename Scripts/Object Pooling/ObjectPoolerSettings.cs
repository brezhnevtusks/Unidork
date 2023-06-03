using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Unidork.ObjectPooling
{
	/// <summary>
	/// Contains settings for an <see cref="ObjectPooler"/>.
	/// </summary>
	[System.Serializable]
    public class ObjectPoolerSettings
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
        private ObjectPoolItemSettings[] itemSettings = null;

        #endregion

        #region Get

        /// <summary>
        /// Gets the item settings,
        /// </summary>
        /// <returns>
        /// A clone of <see cref="itemSettings"/>.
        /// </returns>
        public ObjectPoolItemSettings[] GetPoolItemSettings() => (ObjectPoolItemSettings[]) itemSettings.Clone();

        /// <summary>
        /// Gets settings for an item that matches the passed addressable asset reference.
        /// </summary>
        /// <param name="assetReference">Addressable asset reference.</param>
        /// <returns>
        /// The result of calling <see cref="GetSettingsForItem(string)"/>.
        /// </returns>
        public ObjectPoolItemSettings GetSettingsForItem(AssetReference assetReference)
		{
            return GetSettingsForItem(assetReference.AssetGUID);
		}

        /// <summary>
        /// Gets settings for an item that matches the passed asset address.
        /// </summary>
        /// <param name="assetAddress">Asset address.</param>
        /// <returns>
        /// Settings that match the passed asset address or null if such settings don't exist.
        /// </returns>
        public ObjectPoolItemSettings GetSettingsForItem(string assetAddress)
        {
            foreach (ObjectPoolItemSettings settings in itemSettings)
            {
                if (settings.AssetReference.AssetGUID == assetAddress)
                {
                    return settings;
                }
            }

            return null;
        }

        #endregion
    }
}