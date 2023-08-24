using System.IO;
using Unidork.Attributes;
using Unidork.FileUtility;
using UnityEngine;

namespace Unidork.Serialization
{
	/// <summary>
	/// Base class for serialization managers.
	/// </summary>
	public class BaseSerializationManager : MonoBehaviour
	{
		#region Properties

		/// <summary>
		/// Current version of save data.
		/// </summary>
		/// <value>
		/// Gets the value of the string field saveVersion.
		/// </value>
		public string SaveVersion => saveVersion;

		#endregion
		
		#region Fields
		
		/// <summary>
		/// Singleton instance of this manager.
		/// </summary>
		private static BaseSerializationManager instance;

		/// <summary>
		/// Current version of save data.
		/// </summary>
		[Space, SettingsHeader, Space]
		[Tooltip("Current version of save data.")] 
		[SerializeField]
		private string saveVersion = "0.1";

		#endregion

		#region Init

		protected virtual void Awake()
		{
			if (instance != null) 
			{
				Destroy(gameObject);
				return;
			}

			instance = this;
		}

		#endregion

		#region Serialize

		#region Json

		/// <summary>
		/// Serializes save data to a json and writes it to disk at specified path.
		/// </summary>
		/// <param name="saveData">Save data to serialize.</param>
		/// <param name="savePath">Save file path.</param>
		public static void SerializeSaveDataToFile(BaseSaveData saveData, string savePath)
		{
			string saveJson = SerializeSaveDataToJson(saveData);
			File.WriteAllText(savePath, saveJson);
		}
		
		/// <summary>
		/// Serializes a <see cref="BaseSaveData"/> object to Json.
		/// </summary>
		/// <param name="saveData">Save data.</param>
		/// <returns>
		/// A Json representation of the passed save data object.
		/// </returns>
		protected static string SerializeSaveDataToJson(BaseSaveData saveData) => JsonUtility.ToJson(saveData);

		#endregion

		#endregion

		#region Deserialize

		#region Json

		/// <summary>
		/// Deserializes data at specified file path into an object of specified type.
		/// </summary>
		/// <param name="filePath">File path.</param>
		/// <typeparam name="T">Type of object to deserialize.</typeparam>
		/// <returns>
		/// An instance of <typeparamref name="T"/>.
		/// </returns>
		public static T DeserializeSaveDataFromFile<T>(string filePath) where T : BaseSaveData
		{
			string saveJson = FileUtils.GetByteStringFromFile(filePath);

			if (string.IsNullOrEmpty(saveJson))
			{
				return null;
			}
			
			return DeserializeSaveDataFromJson<T>(saveJson);
		}
		
		/// <summary>
		/// Deserializes the passed Json string into an object of specified type.
		/// </summary>
		/// <param name="json">Json object.</param>
		/// <typeparam name="T">Type of object to deserialize.</typeparam>
		/// <returns>
		/// An instance of <typeparamref name="T"/>.
		/// </returns>
		protected static T DeserializeSaveDataFromJson<T>(string json) where T : BaseSaveData => JsonUtility.FromJson<T>(json);

		#endregion

		#endregion
	}
}