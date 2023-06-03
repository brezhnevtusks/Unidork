using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Unidork.SceneManagement
{
    /// <summary>
    /// Utility script that can be used to load and unload the splash scene to properly initialize the game when it's run in the editor.
    /// </summary>
    public class SplashSceneLoader : MonoBehaviour
    {        
        #region Properties

        public static bool AlreadyLoadedSplash { get; set; }

        public static AssetReference OverrideSceneAssetReference { get; private set; }

        #endregion

        #region Fields

        /// <summary>
        /// Override scene to load when starting the game in the editor from the scene where this component is located."
        /// When left empty, default scene is loaded instead.
        /// </summary>
        [Tooltip("Override scene to load when starting the game in the editor from the scene where this component is located." +
                 "When left empty, default scene is loaded instead.")]
        [SerializeField] 
        private AssetReference overrideSceneAssetReference;

        /// <summary>
        /// Delay before starting the unload of the splash scene.
        /// </summary>
        [Tooltip("Delay before starting the unload of the splash scene.")]
        [SerializeField]
        private float splashSceneUnloadDelay = 0f;
        
        #endregion

        #region Load

        private void Awake()
        {
#if UNITY_EDITOR
            if (AlreadyLoadedSplash)
            {
                return;
            }
            
            OverrideSceneAssetReference = overrideSceneAssetReference;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Splash");
#endif
        }

        /// <summary>
        /// Tells the <see cref="SceneLoader"/> to unload the splash scene.
        /// </summary>
        public void UnloadSplashScene()
        {
            FindObjectOfType<SceneLoader>().StartSplashSceneUnload(0f);
        }

        #endregion
    } 
}