using System;
using Unidork.Extensions;
using Unidork.Serialization;
using UniRx;
using UnityEngine;

namespace Unidork.Settings
{
	/// <summary>
	/// Base class for managers that handle operations with basic game settings: sound, music, vibrations.
	/// </summary>
    public abstract class SettingsManager : MonoBehaviour
    {
		#region Properties

		/// <summary>
		/// Has the manager been initialized?
		/// </summary>
		public static bool IsInitialized => SoundEffectsEnabled != null;
		
		/// <summary>
		/// Are sound effects enabled?
		/// </summary>
		public static ReactiveProperty<bool> SoundEffectsEnabled { get; private set; }
		
		/// <summary>
		/// Is music enabled?
		/// </summary>
		public static ReactiveProperty<bool> MusicEnabled { get; private set; }

		/// <summary>
		/// Are vibrations enabled?
		/// </summary>
		public static ReactiveProperty<bool> VibrationsEnabled { get; private set; }

		/// <summary>
		/// Current sound volume.
		/// </summary>
		public static ReactiveProperty<float> SoundEffectsVolume { get; private set; }

		/// <summary>
		/// Current music volume.
		/// </summary>
		public static ReactiveProperty<float> MusicVolume { get; private set; }

		#endregion

		#region Fields

		/// <summary>
		/// Current version of save data.
		/// </summary>
		protected string saveVersion;

	    /// <summary>
	    /// Settings save data.
	    /// </summary>
		private SettingsSaveData settingsSaveData;

		#endregion
		
		#region Constants
		
		/// <summary>
		/// Key to use when saving/loading with Easy Save.
		/// </summary>
		private const string SettingsKeyName = "Settings";
		
		/// <summary>
		/// Relative file path to use when saving/loading with Easy Save.
		/// </summary>
		private const string SettingsSavePath = "Settings.und";
		
		#endregion

		#region Init

		protected void Start()
		{
			Init();	
		}
		
		/// <summary>
		/// Loads settings save data and initialized the manager.
		/// </summary>
		protected virtual void Init()
		{
			saveVersion = BaseSerializationManager.SaveVersion;

			settingsSaveData = BaseSerializationManager.Load(SettingsKeyName, SettingsSavePath, SettingsSaveData.CreateDefault());

			SoundEffectsEnabled = new ReactiveProperty<bool>(settingsSaveData.SoundEffectsEnabled);
			MusicEnabled = new ReactiveProperty<bool>(settingsSaveData.MusicEnabled);
			VibrationsEnabled = new ReactiveProperty<bool>(settingsSaveData.VibrationsEnabled);

			SoundEffectsVolume = new ReactiveProperty<float>(settingsSaveData.SoundVolume);
			MusicVolume = new ReactiveProperty<float>(settingsSaveData.MusicVolume);
		}

		#endregion

		#region Settings

		/// <summary>
		/// Toggles the boolean value of a setting of passed type.
		/// </summary>
		/// <param name="settingType">Settings type.</param>
		public static void ToggleSetting(SettingType settingType)
		{
			switch (settingType)
			{
				case SettingType.SoundEffects:
				{
					SoundEffectsEnabled.Invert();
					break;
				}
				case SettingType.Music:
				{
					MusicEnabled.Invert();
					break;
				}
				case SettingType.Vibrations:
				{
					VibrationsEnabled.Invert();
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException(nameof(settingType), settingType, null);
				}
			}
		}

		/// <summary>
		/// Sets the value of the specified setting.
		/// </summary>
		/// <param name="settingType">Setting type.</param>
		/// <param name="value">Value to set.</param>
		public static void SetValue(SettingType settingType, float value)
		{
			switch (settingType)
			{
				case SettingType.SoundEffects:
				{
					SoundEffectsVolume.Value = value;
					break;
				}
				case SettingType.Music:
				{
					MusicVolume.Value = value;
					break;
				}
				case SettingType.Vibrations:
				{
					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException(nameof(settingType), settingType, null);
				}
			}
		}

		#endregion

		#region Serialization

		/// <summary>
		/// Saves all settings.
		/// </summary>
		public void SaveSettings()
		{
			settingsSaveData.SoundEffectsEnabled = SoundEffectsEnabled.Value;
			settingsSaveData.SoundVolume = SoundEffectsVolume.Value;

			settingsSaveData.MusicEnabled = MusicEnabled.Value;
			settingsSaveData.MusicVolume = MusicVolume.Value;

			settingsSaveData.VibrationsEnabled = VibrationsEnabled.Value;
			
			BaseSerializationManager.Save(SettingsKeyName, settingsSaveData, SettingsSavePath);
		}
		
		/// <summary>
		/// Creates a new instance of settings data and writes it to disk.
		/// </summary>
		public void ResetSaveData()
		{
			settingsSaveData = new SettingsSaveData(saveVersion, DateTime.Now);
			BaseSerializationManager.Save(SettingsKeyName, settingsSaveData, SettingsSavePath);
		}

		#endregion
	}
}