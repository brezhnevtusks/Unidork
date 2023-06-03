using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Unidork.ObjectPooling
{
    /// <summary>
    /// Stores data about an object that needs to be spawned. 
	/// Supposed to be extended to store data like object types/etc.
    /// </summary>
	[System.Serializable]
    public abstract class SpawnedObjectData
    {
		#region Properties

		/// <summary>
		/// Reference to the addressable asset.
		/// </summary>
		/// <value>
		/// Gets the value of the field assetAddress.
		/// </value>
		public AssetReference AssetReference => assetReference;

		#endregion

		#region Fields

		/// <summary>
		/// Reference to the addressable asset.
		/// </summary>
		[Tooltip("Reference to the addressable asset.")]
		[SerializeField]
        private AssetReference assetReference = null;

		#endregion
	}
}