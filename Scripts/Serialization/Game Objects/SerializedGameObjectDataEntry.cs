using System.Collections.Generic;
using UnityEngine;

namespace Unidork.Serialization
{
    /// <summary>
    /// Stores a single entry in a <see cref="SerializedGameObjectData"/> object.
    /// An entry consists of an address of an Addressable asset and a list of <see cref="SerializedGameObjectDataJsons"/>
    /// objects that store data about all serialized instances of that asset.
    /// </summary>
    [System.Serializable]
    public class SerializedGameObjectDataEntry
    {
        #region Properties

        /// <summary>
        /// Addressable asset address.
        /// </summary>
        /// <value>
        /// Gets the value of the string field assetAddress.
        /// </value>
        public string AssetAddress => assetAddress;        

        /// <summary>
        /// List storing all serialized game object data Jsons that match the asset address.
        /// </summary>
        /// <value>
        /// Gets the value of the field serializedGameObjectData.
        /// </value>
        public List<string> SerializedGameObjectDataJsons => serializedGameObjectDataJsons;

        #endregion

        #region Fields

        /// <summary>
        /// Addressable asset address.
        /// </summary>
        [SerializeField]
        private string assetAddress;

        /// <summary>
        /// List storing all serialized game object data Jsons that match the asset address.
        /// </summary>
        [SerializeField]
        private List<string> serializedGameObjectDataJsons;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="assetAddress">Addressable asset address.</param>
        /// <param name="serializedGameObjectDataJsons">List storing all serialized game object data Jsons that matche the asset address.</param>
        public SerializedGameObjectDataEntry(string assetAddress, List<string> serializedGameObjectDataJsons)
		{
            this.assetAddress = assetAddress;
            this.serializedGameObjectDataJsons = serializedGameObjectDataJsons;
		}

		#endregion
	}
}