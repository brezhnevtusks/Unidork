using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Unidork.Attributes;
using UnityEngine;

namespace Unidork.DeviceUtility
{
	/// <summary>
	/// Calculates mobile device tier for user's device.
	/// </summary>
	public abstract class MobileDeviceTierCalculator : MonoBehaviour
    {
		#region Properties

		/// <summary>
		/// Tier of user's mobile device.
		/// </summary>
		public static MobileDeviceTier DeviceTier { get; private set; }

		#endregion

		#region Fields

		/// <summary>
		/// Should a specific device tier be used?		
		/// </summary>
		[Space, SettingsHeader, Space]
		[Tooltip("Should a specific device tier be used?")]
		[SerializeField]
		private bool forceDeviceTier = false;

		/// <summary>
		/// Mobile device tier to force.
		/// </summary>
		[ShowIf("@this.forceDeviceTier == true")]
		[Tooltip("Mobile device tier to force.")]
		[SerializeField]
		private MobileDeviceTier deviceTierToForce = MobileDeviceTier.Low;

	    /// <summary>
	    /// Instance of this component.
	    /// </summary>
	    [UsedImplicitly]
	    private static MobileDeviceTierCalculator instance;

	    /// <summary>
	    /// Minimum SDK version for a device for it to be considered mid-tier.
	    /// </summary>
	    [Space, SettingsHeader, Space]
	    [Tooltip("Minimum SDK version for a device for it to be considered mid-tier.")]
	    [SerializeField]
	    private int minSdkVersionForMidTier = 26;

	    /// <summary>
	    /// Minimum SDK version for a device for it to be considered high-tier.
	    /// </summary>		
	    [Tooltip("Minimum SDK version for a device for it to be considered high-tier.")]
	    [SerializeField]
	    private int minSdkVersionForHighTier = 28;
	    
		#endregion

		#region Init

		private void Awake()
		{
			instance = this;
			SetMobileDeviceTier();
		}

		/// <summary>
		/// Sets the mobile device tier stored in <see cref="DeviceTier"/> based on current platform and settings in this component.
		/// </summary>
		private void SetMobileDeviceTier()
		{
			if (forceDeviceTier)
			{
				DeviceTier = deviceTierToForce;
				return;
			}

#if UNITY_EDITOR
			DeviceTier = MobileDeviceTier.High;
#elif UNITY_ANDROID
			DeviceTier = instance.GetAndroidDeviceTier();
#elif UNITY_IOS
			DeviceTier = instance.GetiOSDeviceTier();
#else
			DeviceTier = MobileDeviceTier.Low;
#endif
		}

		#endregion

		#region Tier

#if UNITY_ANDROID


		/// <summary>
		/// Gets tier of an Android device
		/// </summary>
		/// <returns><see cref="MobileDeviceTier"/> for user's device.</returns>
		protected abstract MobileDeviceTier GetAndroidDeviceTier();		

		/// <summary>
		/// Gets maximum allowed tier of an Android device based on SDK version.
		/// </summary>
		/// <returns>
		/// Maximum <see cref="MobileDeviceTier"/> for user's device based on Android SDK version.
		/// </returns>
		protected virtual MobileDeviceTier GetMaxAndroidDeviceTier()
		{
			var buildInfo = new AndroidJavaClass("android.os.Build$VERSION");
			var sdkVersion = buildInfo.GetStatic<int>("SDK_INT");

			if (sdkVersion < minSdkVersionForMidTier)
			{
				return MobileDeviceTier.Low;
			}
			
			return sdkVersion < minSdkVersionForHighTier ? MobileDeviceTier.Mid : MobileDeviceTier.High;
		}
#endif

#if UNITY_IOS

		/// <summary>
		/// Gets tier of an iOS device.
		/// </summary>
		/// <returns><see cref="MobileDeviceTier"/> for user's device.</returns>
		protected abstract MobileDeviceTier GetiOSDeviceTier();

		/// <summary>
		/// Gets maximum allowed tier of an iOS device.
		/// </summary>
		/// <remarks>We assume that all iOS devices are high-tier, although it is not entirely accurate,
		/// my Apple-wielding friends.</remarks>
		/// <returns>
		/// A <see cref="MobileDeviceTier"/> calcualted for user's device.
		/// </returns>
		protected virtual MobileDeviceTier GetMaxiOSDeviceTier() => MobileDeviceTier.High;

#endif

		#endregion
	}
}