using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unidork.Serialization
{
	[Serializable]
	public class SettingsSaveData : BaseSaveData
	{
		#region Properties

		/// <summary>
		/// Is sound enabled in the settings menu?
		/// </summary>
		/// <value>
		/// Gets and sets the value of the boolean field soundEnabled.
		/// </value>
		public bool SoundEffectsEnabled { get => soundEffectsEnabled; set => soundEffectsEnabled = value; }

		/// <summary>
		/// Master sound volume.
		/// </summary>
		/// <returns>
		/// Gets and sets the value of the float field soundVolume.
		/// </returns>
		public float SoundVolume { get => soundVolume; set => soundVolume = value; }

		/// <summary>
		/// Is music enabled in the settings menu?
		/// </summary>
		/// <value>
		/// Gets and sets the value of the boolean field musicEnabled.
		/// </value>
		public bool MusicEnabled { get => musicEnabled; set => musicEnabled = value; }

		/// <summary>
		/// Music volume.
		/// </summary>
		/// <returns>
		/// Gets and sets the value of the float field musicVolume.
		/// </returns>
		public float MusicVolume { get => musicVolume; set => musicVolume = value; }

		/// <summary>
		/// Are vibrations enabled in the settings menu?
		/// </summary>
		/// <value>
		/// Gets the value of the boolean field vibrationsEnabled.
		/// </value>
		public bool VibrationsEnabled { get => vibrationsEnabled; set => vibrationsEnabled = value; }

		#endregion
		
		#region Fields

		/// <summary>
		/// Is sound enabled in the settings menu?
		/// </summary>
		[SerializeField]
		private bool soundEffectsEnabled;

		/// <summary>
		/// Master sound volume.
		/// </summary>
		[SerializeField] 
		private float soundVolume;
		
		/// <summary>
		/// Is music enabled in the settings menu?
		/// </summary>
		[SerializeField]
		private bool musicEnabled;

		/// <summary>
		/// Music volume.
		/// </summary>
		[SerializeField]
		private float musicVolume;

		/// <summary>
		/// Are vibrations enabled in the settings menu?
		/// </summary>
		[SerializeField]
		private bool vibrationsEnabled;

		#endregion
		
		#region Constructor

		/// <summary>
		/// Creates settings save data with default values.
		/// </summary>
		/// <returns>
		/// An instance of <see cref="SettingsSaveData"/>.
		/// </returns>
		public static SettingsSaveData CreateDefault()
		{
			return new SettingsSaveData(BaseSerializationManager.SaveVersion, DateTime.Now);
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="saveVersion">Save version for this save data.</param>
		/// <param name="timestamp">Save data timestamp.</param>
		/// <param name="soundEffectsEnabled">Is sound enabled in the settings menu?</param>
		/// <param name="soundVolume">Master sound volume.</param>
		/// <param name="musicEnabled">Is music enabled in the settings menu?</param>
		/// <param name="musicVolume">Music volume.</param>
		/// <param name="vibrationsEnabled">Are vibrations enabled in the settings menu?</param>
		public SettingsSaveData(string saveVersion, DateTime timestamp, bool soundEffectsEnabled = true, float soundVolume = 1f,
		                        bool musicEnabled = true, float musicVolume = 1f, bool vibrationsEnabled = true) : base(saveVersion, timestamp)
		{
			this.soundEffectsEnabled = soundEffectsEnabled;
			this.soundVolume = soundVolume;
			this.musicEnabled = musicEnabled;
			this.musicVolume = musicVolume;
			this.vibrationsEnabled = vibrationsEnabled;
		}

		#endregion
	}
}