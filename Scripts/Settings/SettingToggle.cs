using Cysharp.Threading.Tasks;
using Unidork.Attributes;
using UnityEngine;

namespace Unidork.Settings
{
	/// <summary>
	/// Toggles one of the in-game settings <see cref="SettingsManager"/>.
	/// </summary>
	public class SettingToggle : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Type of setting to toggle.
        /// </summary>
        [Space, BaseHeader, Space]
        [SerializeField]
        protected SettingType settingType = default;

		#endregion

		#region Init
		
		/// <summary>
		/// Can be used to set up the initial state of the toggle.
		/// </summary>
		protected virtual void SetUp()
		{			
		}

		/// <summary>
		/// Can be used to set up the initial state of the toggle.
		/// </summary>
		private async UniTaskVoid SetUpAsync()
		{
			await UniTask.WaitUntil(() => SettingsManager.IsInitialized);
			SetUp();
		}

		private void Start()
		{
			SetUpAsync().Forget();
		}		

		#endregion

		#region Toggle

		/// <summary>
		/// Tells the <see cref="SettingsManager"/> to toggle the setting of type assigned in <see cref="settingType"/>.
		/// </summary>
		public virtual void ToggleSetting() => SettingsManager.ToggleSetting(settingType);

		#endregion
	}
}