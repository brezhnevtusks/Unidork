using Unidork.Attributes;
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
		public static string SaveVersion => instance.saveVersion;

		#endregion
		
		#region Fields
		
		/// <summary>
		/// Singleton instance of this manager.
		/// </summary>
		protected static BaseSerializationManager instance;

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

		/// <summary>
		/// Saves data to a file using Easy Save.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="data">Save data.</param>
		/// <param name="filePath">Save file path.</param>
		/// <param name="settings">Settings.</param>
		/// <typeparam name="T">Type of save data.</typeparam>
		public static void Save<T>(string key, T data, string filePath, ES3Settings settings = null) where T : BaseSaveData
		{
			ES3.Save(key, data, filePath, settings);
		}

		/// <summary>
		/// Loads data from a file using Easy Save.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="filePath">File path.</param>
		/// <param name="settings">Load settings.</param>
		/// <typeparam name="T">Type of save data.</typeparam>
		/// <returns>
		/// An instance of <see cref="T"/> if load succeeds. Otherwise Easy Save will throw an exception.
		/// </returns>
		public static T Load<T>(string key, string filePath, T defaultValue = default, ES3Settings settings = null) where T : BaseSaveData
		{
			if (!ES3.FileExists(filePath, settings) || !ES3.KeyExists(key, filePath, settings))
			{
				ES3.Save(key, defaultValue, filePath, settings);
				return defaultValue;
			}
			
			return ES3.Load<T>(key, filePath, settings);
		}

		#endregion
	}
}