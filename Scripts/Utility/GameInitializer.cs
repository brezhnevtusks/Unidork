using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using Unidork.Attributes;
#if ADDRESSABLES
using Unidork.SceneManagement;
#endif
using UnityEngine;

namespace Unidork.Utility
{
    public class GameInitializer : MonoBehaviour
    {
        #region Properties
        
        public static bool IsInitialized { get; private set; }
        
        #endregion
        
        #region Fields

#if ADDRESSABLES
        [Space, BaseHeader, Space]
        [SerializeField] private StartSceneLoader startSceneLoader;
#endif

        #endregion
        
        #region Init
        
        private async Task Start()
        {
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
            await InitAsync();
#if ADDRESSABLES
            await startSceneLoader.LoadSceneAsync();
#endif
            await PostSceneLoadAsync();
            IsInitialized = true;
            Debug.Log("Game initialized");
        }

        protected virtual async UniTask InitAsync()
        {
        }

        protected virtual async UniTask PostSceneLoadAsync()
        {
        }
        
        #endregion
    }
}