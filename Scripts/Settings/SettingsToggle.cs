using System;
using Unidork.Attributes;
using UnityEngine;

namespace Unidork.Settings
{
	/// <summary>
	/// Tells the <see cref="SettingsManager"/> to toggle one of the in-game settings.
	/// </summary>
	public class SettingsToggle : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Component that handles operations with basic game settings: sound, music, vibrations.
        /// </summary>
        protected static SettingsManager settingsManager;

        /// <summary>
        /// Type of setting to toggle.
        /// </summary>
        [Space, SettingsHeader, Space]
        [SerializeField]
        protected SettingsType settingsType = default;

		#endregion

		#region Init

		/// <summary>
		/// Sets up the initial state of the toggle.
		/// </summary>
		protected virtual void SetUpToggle()
		{			
		}

		private void Awake()
		{
			if (settingsManager != null)
			{
				return;
			}

			settingsManager = FindObjectOfType<SettingsManager>();
		}

		private void Start()
		{
			SetUpToggle();
		}		

		#endregion

		#region Toggle

		/// <summary>
		/// Tells the <see cref="SettingsManager"/> to toggle the setting of type assigned in <see cref="settingsType"/>.
		/// </summary>
		public virtual void ToggleSetting() => settingsManager.ToggleSettingsOfType(settingsType);

		#endregion
	}
}