#if ADDRESSABLES

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using static UnityEngine.AddressableAssets.Addressables;

namespace Unidork.AddressableAssetsUtility
{
	/// <summary>
	/// Utility methods for Unity's Addressables system.
	/// </summary>
	public static class AddressablesUtility
	{
		/// <summary>
		/// Loads an Addressable game object and gets a component of specified type on that game object.
		/// </summary>
		/// <param name="assetReference">Addressable asset reference.</param>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <returns>
		/// An async operation handle that stores a component of type <see cref="T"/> as its result.
		/// </returns>
		public static AsyncOperationHandle<T> LoadComponentAsync<T>(AssetReference assetReference) where T : Component
		{
			var componentAsyncLoadOperation = new ComponentAsyncLoadOperation<T>(assetReference);
			AsyncOperationHandle<T> loadHandle = ResourceManager.StartOperation(componentAsyncLoadOperation, default);
			return loadHandle;
		}
		
		/// <summary>
		/// Gets an Addressable resource location from an asset reference.
		/// </summary>
		/// <param name="assetReference">Asset Reference.</param>
		/// <returns>
		/// An <see cref="IResourceLocation"/> or null if the asset reference is invalid.
		/// </returns>
		public static IResourceLocation GetResourceLocationFromAssetReference(AssetReference assetReference)
		{
			if (assetReference == null || !assetReference.RuntimeKeyIsValid())
			{
				Debug.LogError($"Invalid asset reference: {assetReference?.AssetGUID})");
				return null;
			}

			foreach (IResourceLocator resourceLocator in ResourceLocators)
			{
				if (!resourceLocator.Locate(assetReference.AssetGUID, typeof(object), out IList<IResourceLocation> resourceLocations))
				{
					continue;
				}

				if (resourceLocations.Count > 0)
				{
					return resourceLocations[0];
				}
			}

			Debug.LogError($"Failed to locate asset: {assetReference.AssetGUID}");

			return null;
		}
	}
}

#endif