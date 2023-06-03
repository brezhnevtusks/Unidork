using Unidork.Attributes;
using Unidork.DeviceUtility;
using UnityEngine;

namespace Unidork.Settings
{
    /// <summary>
    /// Base class for managers that handle quality settings for mobile devices. 
    /// Supposed to be inherited from to implement logic specific to each project.
    /// </summary>
    public abstract class MobileQualityManager : MonoBehaviour
    {
		#region Enums
		
		/// <summary>
		/// Initialization type of this manager.
		/// <para>Start - manager is initialized on start.</para>
		/// <para>OutsideCall - initialization method is called by another component or an event listener.</para>
		/// </summary>
		private enum InitializationType
		{
			Start,
			ExternalCall
		}

		#endregion

		#region Fields

		[Space, SettingsHeader, Space]
		[SerializeField]
		private InitializationType initType = InitializationType.Start;

		#endregion

		#region Init

		protected virtual void Start()
		{
			if (initType != InitializationType.Start)
			{
				return;
			}

			SetMobileQualitySettings();
		}

		#endregion

		#region Settings

		/// <summary>
		/// Sets the quality settings for user's mobile device.
		/// </summary>
		public virtual void SetMobileQualitySettings()
		{
			MobileDeviceTier mobileDeviceTier = MobileDeviceTierCalculator.DeviceTier;

			switch (mobileDeviceTier)
			{
				case MobileDeviceTier.Low:
					SetMobileQualitySettingsToLow();
					break;
				case MobileDeviceTier.Mid:
					SetMobileQualitySettingsToMedium();
					break;
				case MobileDeviceTier.High:
					SetMobileQualitySettingsToHigh();
					break;
			}
		}

		/// <summary>
		/// Sets the quality settings to low.
		/// </summary>
		protected abstract void SetMobileQualitySettingsToLow();

		/// <summary>
		/// Sets the quality settings to medium.
		/// </summary>
		protected abstract void SetMobileQualitySettingsToMedium();


		/// <summary>
		/// Sets the quality settings to high.
		/// </summary>
		protected abstract void SetMobileQualitySettingsToHigh();

		#endregion
	}
}