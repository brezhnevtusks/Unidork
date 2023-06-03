using UnityEngine;

namespace Unidork.Serialization
{
	[System.Serializable]
	public class SettingsSaveData : BaseSaveData
	{
		#region Properties

		/// <summary>
		/// Is sound enabled in the settings menu?
		/// </summary>
		/// <value>
		/// Gets the value of the boolean field soundEnabled.
		/// </value>
		public bool SoundEnabled { get => soundEnabled; set => soundEnabled = value; }

		/// <summary>
		/// Is music enabled in the settings menu?
		/// </summary>
		/// <value>
		/// Gets the value of the boolean field musicEnabled.
		/// </value>
		public bool MusicEnabled { get => musicEnabled; set => musicEnabled = value; }

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
		private bool soundEnabled;
		
		/// <summary>
		/// Is music enabled in the settings menu?
		/// </summary>
		[SerializeField]
		private bool musicEnabled;

		/// <summary>
		/// Are vibrations enabled in the settings menu?
		/// </summary>
		[SerializeField]
		private bool vibrationsEnabled;

		#endregion
		
		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="saveVersion">Save version for this save data.</param>
		/// <param name="soundEnabled">Is sound enabled in the settings menu?</param>
		/// <param name="musicEnabled">Is music enabled in the settings menu?</param>
		/// <param name="vibrationsEnabled">Are vibrations enabled in the settings menu?</param>
		public SettingsSaveData( string saveVersion, bool soundEnabled = true, bool musicEnabled= true, bool vibrationsEnabled = true) : base(saveVersion)
		{
			this.soundEnabled = soundEnabled;
			this.musicEnabled = musicEnabled;
			this.vibrationsEnabled = vibrationsEnabled;
		}

		#endregion
	}
}