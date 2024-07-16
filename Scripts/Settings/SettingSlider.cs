using Cysharp.Threading.Tasks;
using Unidork.Attributes;
using UnityEngine;

namespace Unidork.Settings
{
    public class SettingSlider : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Type of setting to toggle.
        /// </summary>
        [Space, BaseHeader, Space]
        [SerializeField]
        protected SettingType settingType;

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
        /// Sets the value of the specified setting on <see cref="SettingsManager"/>.
        /// </summary>
        public virtual void SetValue(float value) => SettingsManager.SetValue(settingType, value);

        #endregion
    }
}