using Sirenix.OdinInspector;
using Unidork.Attributes;
using UnityEngine;

namespace Unidork.Utility 
{
    /// <summary>
    /// Locks allowed screen orientations based on the settings.
    /// </summary>
    [DefaultExecutionOrder(-10000)]
    public class ScreenOrientationLocker : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Should both portrait and landscape orientation be allowed?
        /// </summary>
        [Space, SettingsHeader, Space]
        [Tooltip("Should both portrait and landscape orientation be allowed?")]
        [SerializeField]
        private bool allowBothPortraitAndLandscape = false;
        
        /// <summary>
        /// Screen orientation in case only portrait or only landscape is allowed.
        /// </summary>
        [ShowIf("@this.allowBothPortraitAndLandscape == false")]
        [Tooltip("Screen orientation in case only portrait or only landscape is allowed.")]
        [SerializeField] 
        private ScreenOrientation screenOrientation = ScreenOrientation.Portrait;

        #endregion

        #region Init
        
        private void Awake()
        {
            Screen.orientation = UnityEngine.ScreenOrientation.AutoRotation;

            if (allowBothPortraitAndLandscape)
            {
                Screen.autorotateToLandscapeLeft = true;
                Screen.autorotateToLandscapeRight = true;
                Screen.autorotateToPortrait = true;
                Screen.autorotateToPortraitUpsideDown = true;
            }
            else
            {
                if (screenOrientation == ScreenOrientation.Landscape)
                {
                    Screen.autorotateToLandscapeLeft = true;
                    Screen.autorotateToLandscapeRight = true;
                    Screen.autorotateToPortrait = false;
                    Screen.autorotateToPortraitUpsideDown = false;                    
                }
                else
                {
                    Screen.autorotateToLandscapeLeft = false;
                    Screen.autorotateToLandscapeRight = false;
                    Screen.autorotateToPortrait = true;
                    Screen.autorotateToPortraitUpsideDown = true;
                }
            }
        }
        
        #endregion
    }
}