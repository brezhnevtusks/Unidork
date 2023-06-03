using System.Collections.Generic;
#if UNITY_EDITOR && ADDRESSABLES
using Unidork.AddressableAssetsUtility.Editor;
#endif
using UnityEditor;
using UnityEngine;

namespace Unidork.Serialization
{
	/// <summary>
	/// Stores a list of <see cref="SerializedGameObjectDataEntry"/> objects.
	/// </summary>
	public class SerializedGameObjectData : ScriptableObject
    {
		#region Properties

		// <summary>
		/// Entries storing an Addressable asset address and all serialized game object data that corresponds with it.
		/// </summary>
		/// <value>
		/// Gets the value of the field entries.
		/// </value>
		public List<SerializedGameObjectDataEntry> Entries => entries;

		#endregion

		#region Fields

		/// <summary>
		/// Entries storing an Addressable asset address and all serialized game object data that corresponds with it.
		/// </summary>
		[SerializeField]
		private List<SerializedGameObjectDataEntry> entries = new List<SerializedGameObjectDataEntry>();

		#endregion

		#region Add

#if UNITY_EDITOR && ADDRESSABLES

		/// <summary>
		/// Adds serialized game object data to the respective entry.
		/// </summary>
		/// <param name="prefab">Prefab of the serialized game object.</param>
		/// <param name="serializedGameObjectJson">Json storing serialized game object..</param>
		public void AddSerializedGameObjectData(GameObject prefab, string serializedGameObjectJson)
		{
			string prefabAddress = AddressablesEditorUtility.GetAddressableAddressFromPrefab(prefab);

			if (string.IsNullOrEmpty(prefabAddress))
			{
				return;
			}

			SerializedGameObjectDataEntry entry = GetEntryForAssetAddress(prefabAddress);

			entry.SerializedGameObjectDataJsons.Add(serializedGameObjectJson);
		}

#endif

#endregion

		#region Entries

		/// <summary>
		/// Gets an entry for the passed Addressable asset address.
		/// </summary>
		/// <param name="assetAddress"></param>
		/// <returns></returns>
		private SerializedGameObjectDataEntry GetEntryForAssetAddress(string assetAddress)
		{
			foreach (SerializedGameObjectDataEntry entry in entries)
			{
				if (entry.AssetAddress.Equals(assetAddress))
				{
					return entry;
				}
			}

			SerializedGameObjectDataEntry newEntry =
				new SerializedGameObjectDataEntry(assetAddress, new List<string>());

			entries.Add(newEntry);

			return newEntry;
		}

		#endregion
	}
}