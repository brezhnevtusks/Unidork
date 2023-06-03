#if ADDRESSABLES

using UnityEditor;
using UnityEngine;

namespace Unidork.AddressableAssetsUtility.Editor
{
    /// <summary>
    /// Editor-only utility methods for the Addressables system.
    /// </summary>
    public class AddressablesEditorUtility : MonoBehaviour
    {
        #region Address

        /// <summary>
        /// Gets the passed prefab asset's Addressable address.
        /// </summary>
        /// <param name="prefab">Prefab.</param>
        /// <returns>
        /// A string storing the prefab's Addressable address or en empty string if the prefab asset is not addressable.
        /// </returns>
        public static string GetAddressableAddressFromPrefab(Object prefab)
        {
            string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefab);
            string prefabGuid = AssetDatabase.AssetPathToGUID(prefabPath);

            //Debug.Log($"Prefab path: {prefabPath}. Guid: {prefabGuid}");

            var assetEntry = UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(
                prefabGuid, includeImplicit: true);
            
            if (assetEntry == null)
			{
                Debug.LogError($"Prefab {prefab.name} is not an Addressable asset!");
                return "";
			}

            //Debug.Log($"Asset entry address: {assetEntry.address}");

            return assetEntry.address;
        }

        #endregion
    }
}

#endif