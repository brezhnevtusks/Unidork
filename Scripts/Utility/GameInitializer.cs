using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

namespace Unidork.Utility
{
    public class GameInitializer : MonoBehaviour
    {
        #region Properties
        
        public static bool IsInitialized { get; private set; }
        
        #endregion
        
        #region Init
        
        private async Task Start()
        {
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
            await InitAsync();
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