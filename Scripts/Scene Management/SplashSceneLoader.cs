using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Unidork.SceneManagement
{
    /// <summary>
    /// Utility script that can be used to load and unload the splash scene to properly initialize the game when it's run in the editor.
    /// </summary>
    public class SplashSceneLoader : MonoBehaviour
    {        
        #region Properties

        public static bool AlreadyLoadedSplash { get; set; }

        #endregion

        #region Fields

        /// <summary>
        /// Delay before starting the unload of the splash scene.
        /// </summary>
        [Tooltip("Delay before starting the unload of the splash scene.")]
        [SerializeField]
        private float splashSceneUnloadDelay = 0f;
        
        #endregion

        #region Load

#if UNITY_EDITOR
        private void Awake()
        {
            if (AlreadyLoadedSplash)
            {
                return;
            }
            
            UnityEngine.SceneManagement.SceneManager.LoadScene("SCN_Splash");
        }
#endif

        /// <summary>
        /// Starts the splash scene unload.
        /// </summary>
        public void UnloadSplashScene()
        {
            UnloadSplashSceneAsync(splashSceneUnloadDelay).Forget();
        }
        
        /// <summary>
        /// Unloads the splash scene asynchronously after a delay.
        /// </summary>
        /// <returns>
        /// <see cref="UniTaskVoid"/>.
        /// </returns>
        private async UniTaskVoid UnloadSplashSceneAsync(float delay)
        {
            await UniTask.Delay(new TimeSpan(0, 0, 0, 0, (int)(delay * 1000f)));
            await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(0).ToUniTask();
        }

        #endregion
    } 
}