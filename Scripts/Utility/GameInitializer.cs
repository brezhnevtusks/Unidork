using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.LowLevel;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Unidork.Utility
{
    public class GameInitializer : MonoBehaviour
    {
        #region Properties
        
        public static bool SaveLoaded { get; protected set; }
        public static bool IsInitialized { get; private set; }
        
        #endregion
        
        #region Init

        protected virtual void Awake()
        {
            PlayerLoopSystem playerLoopSystem = PlayerLoop.GetCurrentPlayerLoop();
            PlayerLoopHelper.Initialize(ref playerLoopSystem);
        }
        
        private async Task Start()
        {
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
            try
            {
                await InitAsync();
            }
            catch(System.Exception e)
            {
                Debug.LogError($"Game initialization failed! {e}");
            }
            IsInitialized = true;
            Debug.Log("Game initialized!");
        }
        
        protected virtual async UniTask InitAsync()
        {
        }
        
        #endregion
    }
}