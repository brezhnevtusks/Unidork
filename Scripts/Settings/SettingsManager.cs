using Lofelt.NiceVibrations;
using System.Collections.Generic;
using Unidork.Attributes;
using Unidork.Serialization;
using UnityEngine;

namespace Unidork.Settings
{
	/// <summary>
	/// Base class for managers that handle operations with basic game settings: sound, music, vibrations.
	/// </summary>
    public class SettingsManager : MonoBehaviour
    {
		#region Properties

		/// <summary>
		/// Is sound currently enabled?
		/// </summary>
		public bool SoundEnabled => IsSettingOfTypeEnabled(SettingsType.Sound);

		/// <summary>
		/// Is music currently enabled?
		/// </summary>
		public bool MusicEnabled => IsSettingOfTypeEnabled(SettingsType.Music);

		/// <summary>
		/// Are vibrations currently enabled?
		/// </summary>
		public bool VibrationsEnabled => IsSettingOfTypeEnabled(SettingsType.Vibrations);		

		#endregion

		#region Fields

		/// <summary>
		/// Current version of save data.
		/// </summary>
		protected string saveVersion;

		/// <summary>
		/// Path to the persistent data directory.
		/// </summary>
		protected string persistentDataPath;

		/// <summary>
		/// Component that is responsible for serializing and deserializing save data.
		/// </summary>
		protected BaseSerializationManager serializationManager;

		/// <summary>
		/// Path of the settings save data file relative to <see cref="Application.persistentDataPath"/>.
		/// </summary>
		[Space, SettingsHeader, Space]
		[Tooltip("Path of the settings save data file relative to Application.persistentDataPath.")]
		[SerializeField]
		private string settingsSaveDataRelativePath = "/SettingsSaveData.json";

		/// <summary>
		/// Dictionary storing settings data where key is settings type and value is a Boolean
		/// specifying whether that setting is currently enabled.
		/// </summary>
		private Dictionary<SettingsType, bool> settingsDictionary;

		/// <summary>
		/// Object that stores settings save data.
		/// </summary>
		private SettingsSaveData settingsSaveData;

		#endregion

		#region Init

		/// <summary>
		/// Loads settings save data or create a new instance of settings if that data is null.
		/// </summary>
		protected virtual void LoadSaveData()
		{
			saveVersion = serializationManager.SaveVersion;
			persistentDataPath = Application.persistentDataPath;

			string settingsSaveDataPath = Application.persistentDataPath + settingsSaveDataRelativePath;

			settingsSaveData = BaseSerializationManager.DeserializeSaveDataFromFile<SettingsSaveData>(settingsSaveDataPath);

			if (settingsSaveData == null)
			{
				ResetSaveData();
			}

			settingsDictionary.Add(SettingsType.Sound, settingsSaveData.SoundEnabled);
			settingsDictionary.Add(SettingsType.Music, settingsSaveData.MusicEnabled);
			settingsDictionary.Add(SettingsType.Vibrations, settingsSaveData.VibrationsEnabled);

			foreach (KeyValuePair<SettingsType, bool> kvp in settingsDictionary) 
			{
				HandleSettingsToggle(kvp.Key);
			}
		}

		protected virtual void Start()
		{
			serializationManager = FindObjectOfType<BaseSerializationManager>();
			settingsDictionary = new Dictionary<SettingsType, bool>();

			LoadSaveData();			
		}

		#endregion

		#region Settings

		/// <summary>
		/// Sets the value of the dicrionary entry in <see cref="settingsDictionary"/> for 
		/// the passed settings type to the opposite Boolean value.
		/// </summary>
		/// <param name="settingsType">Settings type.</param>
		public void ToggleSettingsOfType(SettingsType settingsType)
		{
			foreach (KeyValuePair<SettingsType, bool> kvp in settingsDictionary)
			{
				SettingsType key = kvp.Key;

				if (key == settingsType)
				{
					settingsDictionary[key] = !settingsDictionary[key];
					HandleSettingsToggle(settingsType);
					SerializeSettingOfType(settingsType);
					break;
				}
			}
		}

		/// <summary>
		/// Checks whether a setting of passed type is enabled.
		/// </summary>
		/// <param name="settingsType"></param>
		/// <returns>True if setting is enabled, False otherwise.</returns>
		public bool IsSettingOfTypeEnabled(SettingsType settingsType)
		{
			foreach (KeyValuePair<SettingsType, bool> kvp in settingsDictionary)
			{
				if (kvp.Key == settingsType)
				{
					return kvp.Value;
				}
			}

			return false;
		}

		/// <summary>
		/// Performs necessary operations after a certain in-game setting is toggled
		/// </summary>
		/// <param name="settingsType">Settings type.</param>
		private void HandleSettingsToggle(SettingsType settingsType)
		{
			switch (settingsType)
			{
				case SettingsType.Sound:
					break;
				case SettingsType.Music:
					break;
				case SettingsType.Vibrations:
					HapticController.hapticsEnabled = VibrationsEnabled;
					break;
				default:
					break;
			}			
		}

		#endregion

		#region Serialization

		/// <summary>
		/// Creates a new instance of settings data and writes it to disk.
		/// </summary>
		public void ResetSaveData()
		{
			string settingsSaveDataPath = Application.persistentDataPath + settingsSaveDataRelativePath;

			settingsSaveData = new SettingsSaveData(saveVersion);
			BaseSerializationManager.SerializeSaveDataToFile(settingsSaveData, settingsSaveDataPath);
		}

		/// <summary>
		/// Writes the value of the setting of passed type to the save data object and serializes data to disk.
		/// </summary>
		/// <param name="settingsType">Settings type.</param>
		protected virtual void SerializeSettingOfType(SettingsType settingsType)
		{
			bool settingValue = settingsDictionary[settingsType];

			switch (settingsType)
			{
				case SettingsType.Sound:
					settingsSaveData.SoundEnabled = settingValue;
					break;
				case SettingsType.Music:
					settingsSaveData.MusicEnabled = settingValue;
					break;
				case SettingsType.Vibrations:
					settingsSaveData.VibrationsEnabled = settingValue;
					break;
				default:
					break;
			}

			string settingsSaveDataPath = Application.persistentDataPath + settingsSaveDataRelativePath;

			BaseSerializationManager.SerializeSaveDataToFile(settingsSaveData, settingsSaveDataPath);
		}

		#endregion
	}
}